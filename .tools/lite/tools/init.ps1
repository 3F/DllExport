param($installPath, $toolsPath, $package, $project)

# DllExport.Lite: run by Visual Studio's NuGet when the package is installed and each time the solution opens.
# For every C# project in the solution that references DllExport.Lite, it runs Convert-ToDllExportLite.ps1,
# which removes UnmanagedExports or the plugin template's NppPlugin.DllExport and adds ARM64. Projects that
# are already converted are left alone (a quick text check). Nothing is downloaded or started.

if (-not $dte -or -not $dte.Solution -or -not $dte.Solution.FullName) { return }

$converter = Join-Path $toolsPath 'Convert-ToDllExportLite.ps1'
$solution = $dte.Solution.FullName

function Get-SolutionProjects($projects) {
    foreach ($p in $projects) {
        if (-not $p) { continue }
        if ($p.Kind -eq '{66A26720-8FB5-11D2-AA7E-00C04F688DDE}') {   # solution folder
            if ($p.ProjectItems) { Get-SolutionProjects ($p.ProjectItems | ForEach-Object { $_.SubProject }) }
        }
        elseif ($p.FullName -and $p.FullName.EndsWith('.csproj', [System.StringComparison]::OrdinalIgnoreCase)) { $p }
    }
}

function Test-NeedsConversion([string] $file) {
    $dir = Split-Path $file
    $text = [System.IO.File]::ReadAllText($file)
    $pc = Join-Path $dir 'packages.config'
    $pcText = if (Test-Path $pc) { [System.IO.File]::ReadAllText($pc) } else { '' }

    $usesLite = ($text -match 'DllExport\.Lite') -or ($pcText -match 'id="DllExport\.Lite"')
    if (-not $usesLite) { return $false }

    $oldTool = ($text -match 'RGiesecke\.DllExport\.(Metadata|targets)|NppPlugin\.DllExport\.targets|DllExport\\DllExportAttribute\.cs') -or
               ($pcText -match 'id="UnmanagedExports"') -or (Test-Path (Join-Path $dir 'PluginInfrastructure\DllExport'))
    $hasX64 = $text -match "(?i)\|x64'|'x64'"
    $hasArm64 = $text -match "(?i)\|ARM64'|'ARM64'|<Platforms>[^<]*ARM64"
    # a project <Reference> to the metalib (the package's targets reference it themselves)
    $staleRef = $text -match '(?s)<Reference\s+Include="DllExport,[^"]*">(?:(?!</Reference>).)*DllExport\.Lite(?:(?!</Reference>).)*</Reference>'
    return $oldTool -or ($hasX64 -and -not $hasArm64) -or $staleRef
}

# A PackageReference install adds <PackageReference Include="DllExport.Lite"> to VS's in-memory project
# but doesn't save it before running this script, so the file on disk doesn't mention the package yet.
function Test-UsesLiteInMemory($p) {
    try {
        $refs = $p.Object.PackageReferences   # VSLangProj150.PackageReferences, PackageReference projects only
        if ($refs) { foreach ($name in $refs.InstalledPackages) { if ($name -eq 'DllExport.Lite') { return $true } } }
    }
    catch { }
    return $false
}

$projects = @(Get-SolutionProjects $dte.Solution.Projects)
foreach ($p in $projects) {
    if (([System.IO.File]::ReadAllText($p.FullName) -notmatch 'DllExport\.Lite') -and
        ((Test-UsesLiteInMemory $p) -or ($p.Saved -eq $false))) {
        try { $p.Save() } catch { }
    }
}

$todo = @($projects | Where-Object { Test-NeedsConversion $_.FullName })
if ($todo.Count -eq 0) { return }

try {
    # Let VS write its pending project changes (such as the reference NuGet just added) before the files are
    # edited. The solution isn't saved: VS would rewrite the whole .sln in its own format.
    foreach ($p in $todo) { $p.Save() }

    foreach ($p in $todo) { & $converter -Project $p.FullName -Solution $solution }

    Write-Host ''
    Write-Host 'DllExport.Lite: project files were updated on disk. If Visual Studio asks, choose "Reload All".'
}
catch {
    Write-Warning "DllExport.Lite: conversion failed: $($_.Exception.Message)"
    Write-Warning "You can run it by hand: powershell -ExecutionPolicy Bypass -File `"$converter`" -Project <your .csproj>"
}
