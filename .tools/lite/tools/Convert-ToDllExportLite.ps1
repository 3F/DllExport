<#
.SYNOPSIS
    Converts a C# project from UnmanagedExports or the NotepadPlusPlusPluginPack.Net template's
    bundled NppPlugin.DllExport to DllExport.Lite, and adds the ARM64 platform.

.DESCRIPTION
    Part of the DllExport.Lite package (https://github.com/3F/DllExport). Plain, readable PowerShell:
    it only edits local project files, and never downloads or starts anything.

    It runs automatically from the package's init.ps1 when the package is installed in Visual Studio
    (and, as a quick no-op check, whenever the solution is opened). You can also run it by hand:

        powershell -ExecutionPolicy Bypass -File Convert-ToDllExportLite.ps1 -Project MyPlugin.csproj -AddPackage -Version 1.9.0

    Each step checks whether it's already done, so running it again changes nothing.
    Files are written with their original encoding, BOM and line endings. Outside a git work tree,
    originals are copied to <project folder>\DllExportLite.backup\ before the first change; inside
    one, git already has them (review with 'git diff', undo with 'git checkout').

    What it changes:
      1. Removes UnmanagedExports (packages.config entry or PackageReference, the
         RGiesecke.DllExport.Metadata reference and the RGiesecke.DllExport.targets import) and the
         *Undefined* DllExportTargetFrameworkVersion workaround.
      2. Removes the template's NppPlugin.DllExport.targets import and its own DllExportAttribute.cs,
         and changes 'using NppPlugin.DllExport;' to 'using RGiesecke.DllExport;' in the project's .cs files.
      3. With -AddPackage: adds DllExport.Lite (packages.config + Reference + Import, or PackageReference).
         Not needed when NuGet installed the package.
      4. Adds ARM64: copies every x64-conditioned property group or element in the project, and every
         x64 configuration in the solution, as ARM64.

.PARAMETER Project
    Path to the .csproj.
.PARAMETER Solution
    Path to the .sln. Default: the nearest .sln in the project folder or a parent folder that lists the project.
.PARAMETER AddPackage
    Also add the DllExport.Lite package reference (for converting without Visual Studio).
.PARAMETER Version
    DllExport.Lite version to reference with -AddPackage.
.PARAMETER PackagesDir
    packages.config projects only: the solution's packages folder. Default: <solution folder>\packages.
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true)][string] $Project,
    [string] $Solution,
    [switch] $AddPackage,
    [string] $Version,
    [string] $PackagesDir
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

$LiteId = 'DllExport.Lite'
$MetaLibToken = '8337224c9ad9e356'
$script:changes = New-Object System.Collections.Generic.List[string]

function Note([string] $msg) { $script:changes.Add($msg); Write-Host "  $msg" }

# ---------- text files with their original encoding, BOM and line endings ----------

function Read-TextFile([string] $path) {
    $bytes = [System.IO.File]::ReadAllBytes($path)
    $bom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $text = (New-Object System.Text.UTF8Encoding($false)).GetString($bytes, $(if ($bom) { 3 } else { 0 }), $bytes.Length - $(if ($bom) { 3 } else { 0 }))
    $nl = if ($text.Contains("`r`n")) { "`r`n" } else { "`n" }
    return [pscustomobject]@{ Path = $path; Text = $text; Bom = $bom; NewLine = $nl }
}

# Inside a git work tree, git already has the originals, so no backup copies are made.
function Test-InGit([string] $dir) {
    while ($dir) {
        if (Test-Path (Join-Path $dir '.git')) { return $true }
        $dir = Split-Path $dir
    }
    return $false
}

function Backup-File([string] $path) {
    if ($script:inGit) { return }
    $dir = Join-Path (Split-Path $script:projectPath) 'DllExportLite.backup'
    if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Path $dir | Out-Null }
    $dest = Join-Path $dir (Split-Path $path -Leaf)
    if (-not (Test-Path $dest)) { Copy-Item $path $dest }
}

function Write-TextFile($file, [string] $text) {
    if ($text -ceq $file.Text) { return }
    if (-not $PSCmdlet.ShouldProcess($file.Path, 'update')) { return }
    Backup-File $file.Path
    $enc = New-Object System.Text.UTF8Encoding($file.Bom)
    [System.IO.File]::WriteAllText($file.Path, $text, $enc)
}

# Remove whole lines (including their line break) that match a regex.
function Remove-Lines([string] $text, [string] $pattern) {
    return [regex]::Replace($text, "(?m)^[ \t]*$pattern[^\r\n]*\r?\n", '')
}

# Remove an XML element that may span several lines, starting at a line that matches $startPattern.
function Remove-Element([string] $text, [string] $startPattern, [string] $tag) {
    $rx = "(?ms)^[ \t]*$startPattern(?:[^\r\n]*/>|.*?</$tag>)[ \t]*\r?\n"
    return [regex]::Replace($text, $rx, '')
}

# ---------- locate files ----------

$script:projectPath = (Resolve-Path $Project).Path
$projDir = Split-Path $projectPath
$projName = Split-Path $projectPath -Leaf
$script:inGit = Test-InGit $projDir

if (-not $Solution) {
    $dir = $projDir
    while ($dir -and -not $Solution) {
        foreach ($s in Get-ChildItem -Path $dir -Filter *.sln -ErrorAction SilentlyContinue) {
            if ((Get-Content $s.FullName -Raw) -match [regex]::Escape($projName)) { $Solution = $s.FullName; break }
        }
        $dir = Split-Path $dir
    }
}

# .NET file APIs resolve relative paths against the process directory, not PowerShell's location.
if ($Solution) { $Solution = (Resolve-Path $Solution).Path }
if ($PackagesDir) { $PackagesDir = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($PackagesDir) }

Write-Host "DllExport.Lite: converting $projName"

$proj = Read-TextFile $projectPath
$t = $proj.Text
$nl = $proj.NewLine
$sdkStyle = $t -match '<Project\s+Sdk='
$pkgConfigPath = Join-Path $projDir 'packages.config'
$usesPackagesConfig = Test-Path $pkgConfigPath

# ---------- 1. UnmanagedExports ----------

$before = $t
$t = Remove-Element $t '<Reference\s+Include="RGiesecke\.DllExport\.Metadata' 'Reference'
$t = Remove-Lines $t '<Import\s+Project="[^"]*RGiesecke\.DllExport\.targets"'
$t = Remove-Element $t '<PackageReference\s+Include="UnmanagedExports"' 'PackageReference'
# the *Undefined* workaround, with the comment block just above it if it's about ildasm/ValidateToolPath
$t = [regex]::Replace($t, '(?ms)^[ \t]*\r?\n?[ \t]*<!--(?:(?!-->).)*?ValidateToolPath(?:(?!-->).)*?-->[ \t]*\r?\n(?=[ \t]*<DllExportTargetFrameworkVersion>\*Undefined\*)', '')
$t = Remove-Lines $t '<DllExportTargetFrameworkVersion>\*Undefined\*</DllExportTargetFrameworkVersion>'
if ($t -cne $before) { Note 'removed UnmanagedExports from the project' }

if ($usesPackagesConfig) {
    $pc = Read-TextFile $pkgConfigPath
    $pct = Remove-Lines $pc.Text '<package\s+id="UnmanagedExports"'
    if ($pct -cne $pc.Text) { Write-TextFile $pc $pct; $pc.Text = $pct; Note 'removed UnmanagedExports from packages.config' }
}

# ---------- 2. NotepadPlusPlusPluginPack.Net template ----------

$before = $t
$t = Remove-Lines $t '<Import\s+Project="[^"]*NppPlugin\.DllExport\.targets"'
$t = Remove-Element $t '<Compile\s+Include="[^"]*DllExport\\DllExportAttribute\.cs"' 'Compile'
if ($t -cne $before) { Note 'removed the template''s NppPlugin.DllExport tool from the project' }

$tmplDir = Join-Path $projDir 'PluginInfrastructure\DllExport'
if (Test-Path $tmplDir) {
    if ($script:inGit) {
        if ($PSCmdlet.ShouldProcess($tmplDir, 'delete (git has the originals)')) {
            Remove-Item $tmplDir -Recurse -Force
            Note 'deleted PluginInfrastructure\DllExport\ (old tool and attribute)'
        }
    }
    elseif ($PSCmdlet.ShouldProcess($tmplDir, 'move to DllExportLite.backup')) {
        $dest = Join-Path $projDir 'DllExportLite.backup\PluginInfrastructure.DllExport'
        if (-not (Test-Path (Split-Path $dest))) { New-Item -ItemType Directory -Path (Split-Path $dest) | Out-Null }
        Move-Item $tmplDir $dest
        Note 'moved PluginInfrastructure\DllExport\ (old tool and attribute) to DllExportLite.backup\'
    }
}

# 'using NppPlugin.DllExport;' -> 'using RGiesecke.DllExport;' in the project's C# files
$csFiles = @()
foreach ($m in [regex]::Matches($t, '<Compile\s+Include="([^"]+\.cs)"')) { $csFiles += (Join-Path $projDir $m.Groups[1].Value) }
if ($sdkStyle) { $csFiles += @(Get-ChildItem -Path $projDir -Filter *.cs -Recurse | Where-Object { $_.FullName -notmatch '\\(obj|bin|DllExportLite\.backup)\\' } | ForEach-Object { $_.FullName }) }
foreach ($cs in ($csFiles | Sort-Object -Unique)) {
    if (-not (Test-Path $cs)) { continue }
    $f = Read-TextFile $cs
    $ct = [regex]::Replace($f.Text, '(?m)^(\s*)using\s+NppPlugin\.DllExport\s*;', '$1using RGiesecke.DllExport;')
    if ($ct -cne $f.Text) { Write-TextFile $f $ct; Note "changed 'using NppPlugin.DllExport' to 'using RGiesecke.DllExport' in $(Split-Path $cs -Leaf)" }
}

# ---------- 3. add DllExport.Lite (only without Visual Studio's NuGet) ----------

$hasLite = $t -match [regex]::Escape($LiteId)
if ($usesPackagesConfig) { $hasLite = $hasLite -or ((Get-Content $pkgConfigPath -Raw) -match [regex]::Escape("id=""$LiteId""")) }

if ($AddPackage -and -not $hasLite) {
    if (-not $Version) { throw '-AddPackage needs -Version' }
    if ($usesPackagesConfig -and -not $sdkStyle) {
        if (-not $PackagesDir) { $PackagesDir = Join-Path $(if ($Solution) { Split-Path $Solution } else { $projDir }) 'packages' }
        $rel = $PackagesDir
        $uriFrom = New-Object System.Uri(($projDir.TrimEnd('\') + '\'))
        $rel = [System.Uri]::UnescapeDataString($uriFrom.MakeRelativeUri((New-Object System.Uri(($PackagesDir.TrimEnd('\') + '\')))).ToString()).Replace('/', '\').TrimEnd('\')
        $pkg = "$rel\$LiteId.$Version"

        $pc = Read-TextFile $pkgConfigPath
        $line = "  <package id=""$LiteId"" version=""$Version"" developmentDependency=""true"" targetFramework=""net40"" />"
        $pcNl = $pc.NewLine
        $pct = ([regex]'(\r?\n)?</packages>').Replace($pc.Text, { param($m) $pcNl + $line + $pcNl + '</packages>' }, 1)
        Write-TextFile $pc $pct

        # No <Reference>: the package's targets reference the metalib themselves.
        $imp = "  <Import Project=""$pkg\build\$LiteId.targets"" Condition=""Exists('$pkg\build\$LiteId.targets')"" />$nl"
        $t = ([regex]'(?m)^([ \t]*<Import\s+Project="[^"]*Microsoft\.CSharp\.targets"[^\r\n]*\r?\n)').Replace($t, { param($m) $m.Value + $imp }, 1)
        Note "added $LiteId $Version (packages.config, Import)"
    }
    else {
        $pr = "  <ItemGroup>$nl    <PackageReference Include=""$LiteId"" Version=""$Version"" />$nl  </ItemGroup>$nl"
        $t = ([regex]'(?m)^</Project>').Replace($t, { param($m) $pr + $m.Value }, 1)
        Note "added $LiteId $Version (PackageReference)"
    }
}

# ---------- 3b. no project reference to the metalib ----------
# The package's build targets reference its metalib (compile-only). A <Reference> to it in the project,
# left by hand or by an older DllExport.Lite version, would be a duplicate, so it's removed.

$before = $t
$refRx = [regex]'(?ms)^[ \t]*<Reference\s+Include="DllExport,[^"]*">(?:(?!</Reference>).)*?DllExport\.Lite(?:(?!</Reference>).)*?</Reference>[ \t]*\r?\n'
$t = $refRx.Replace($t, '')
if ($t -cne $before) { Note 'removed the project''s DllExport.Lite metalib <Reference> (the package''s targets add it)' }

# ---------- 4. ARM64 in the project ----------

if ($sdkStyle) {
    $m = [regex]::Match($t, '<Platforms>([^<]*)</Platforms>')
    if ($m.Success -and $m.Groups[1].Value -notmatch '(?i)\bARM64\b') {
        $t = $t.Remove($m.Index, $m.Length).Insert($m.Index, "<Platforms>$($m.Groups[1].Value);ARM64</Platforms>")
        Note 'added ARM64 to <Platforms>'
    }
}

# Every element (usually a PropertyGroup) whose Condition selects the x64 platform gets an ARM64 twin
# right after it, with x64 changed to ARM64 inside, unless an ARM64 one already exists.
$rx = New-Object System.Text.RegularExpressions.Regex('(?msi)^([ \t]*)<(\w+)\s+Condition="[^"]*(?:\|x64''|''x64'')[^"]*"[^>]*?(?:/>|>.*?</\2>)[ \t]*\r?\n')
$out = New-Object System.Text.StringBuilder
$pos = 0
$added = 0
foreach ($m in $rx.Matches($t)) {
    $block = $m.Value
    $twin = [regex]::Replace($block, '(?<![\w.])x64(?![\w])', 'ARM64')
    $twinCond = [regex]::Match($twin, 'Condition="[^"]*"').Value
    [void]$out.Append($t.Substring($pos, $m.Index + $m.Length - $pos))
    if ($t.IndexOf($twinCond) -lt 0) { [void]$out.Append($twin); $added++ }
    $pos = $m.Index + $m.Length
}
[void]$out.Append($t.Substring($pos))
$t = $out.ToString()
if ($added) { Note "added $added ARM64 property group(s) to the project, copied from x64" }

Write-TextFile $proj $t

# ---------- 4b. ARM64 in the solution ----------

if ($Solution -and (Test-Path $Solution)) {
    $sln = Read-TextFile $Solution
    $lines = $sln.Text -split "\r?\n", 0
    $result = New-Object System.Collections.Generic.List[string]
    $added = 0
    $existing = @{}
    foreach ($l in $lines) { $existing[$l.Trim()] = $true }
    foreach ($l in $lines) {
        $result.Add($l)
        # "Debug|x64 = Debug|x64" and "{GUID}.Debug|x64.ActiveCfg = Debug|x64"
        if ($l -match '^\s*(\{[0-9A-Fa-f-]+\}\.)?[^|=]+\|x64(\.\S+)?\s*=\s*[^|]+\|x64\s*$') {
            $twin = $l -replace '\|x64', '|ARM64'
            if (-not $existing.ContainsKey($twin.Trim())) { $result.Add($twin); $existing[$twin.Trim()] = $true; $added++ }
        }
    }
    if ($added) {
        Write-TextFile $sln ($result -join $sln.NewLine)
        Note "added $added ARM64 configuration line(s) to $(Split-Path $Solution -Leaf)"
    }
}
elseif (-not $Solution) { Write-Host '  (no .sln found that lists this project; add the ARM64 solution platform yourself)' }

if ($script:changes.Count -eq 0) { Write-Host '  already converted, nothing to do' }
else {
    Write-Host ''
    Write-Host '  Done. Build each platform with Visual Studio MSBuild, e.g.:'
    Write-Host "    msbuild `"$(if ($Solution) { Split-Path $Solution -Leaf } else { $projName })`" -restore -p:Configuration=Release -p:Platform=ARM64"
}
