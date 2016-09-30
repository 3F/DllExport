param($installPath, $toolsPath, $package, $project)

$targetFileName = 'net.r_eg.DllExport.targets'
$assemblyFName  = 'DllExport' # $package.AssemblyReferences[0].Name
$publicKeyToken = '8337224C9AD9E356';
$tempRoot       = (Join-Path $([System.IO.Path]::GetTempPath()) '50ACAD2A-5AB3-4E6A-BA66-07F55672E91F') -replace ' ', '` '
$tempFolder     = $([System.Guid]::NewGuid());
$escInstallPath = $installPath -replace ' ', '` '
$escToolsPath   = $toolsPath -replace ' ', '` '
$metaLib        = $([System.IO.Path]::Combine($escInstallPath, 'lib\net20', $assemblyFName + '.dll'));
$guiAsmFile     = 'net.r_eg.DllExport.Configurator.dll';
$targetFileName = [IO.Path]::Combine($toolsPath, $targetFileName)
$targetUri      = New-Object Uri -ArgumentList $targetFileName, [UriKind]::Absolute

$msBuildV4Name  = 'Microsoft.Build'; #, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a';
$msBuildV4      = [System.Reflection.Assembly]::LoadWithPartialName($msBuildV4Name)          # obsolete

if(!$msBuildV4) {
    throw New-Object System.IO.FileNotFoundException("Could not load $msBuildV4Name.");
}

$projectCollection = $msBuildV4.GetType('Microsoft.Build.Evaluation.ProjectCollection')
$projects          =  $projectCollection::GlobalProjectCollection.GetLoadedProjects($project.FullName)

# GUI Configurator

# powershell -Command "Import-Module (Join-Path $escToolsPath Configurator.dll); Set-Configuration -Dll $asmpath"

Remove-Item -Path $tempRoot -Force -Recurse -ErrorAction SilentlyContinue

$tdll = (Join-Path $tempRoot $tempFolder);
if(!(Test-Path -path $tdll)) {
    New-Item $tdll -Type Directory >$null
}
Copy-Item $toolsPath\*.dll -Destination $tdll >$null

$dllGUI = (Join-Path $tdll $guiAsmFile)

if(!(Get-Module -Name $guiAsmFile)) {
    Import-Module $dllGUI; 
}

Set-Configuration -MetaLib $metaLib -InstallPath $installPath -ToolsPath $toolsPath -ProjectDTE $project -ProjectsMBE $projectCollection::GlobalProjectCollection;

# defNS $([System.IO.Path]::Combine($installPath, 'lib\net20', $assemblyFName + '.dll'))  $vNamespace


# change the reference to DllExport.dll to not be copied locally

$project.Object.References | ? { 
    $_.Name -ieq $assemblyFName -And $_.PublicKeyToken -ieq $publicKeyToken
} | % {
    if($_ | Get-Member | ? {$_.Name -eq "CopyLocal"}){
        $_.CopyLocal = $false
    }
}

$projects |  % {
    $currentProject = $_

    # remove imports of net.r_eg.DllExport.targets from this project 
    $currentProject.Xml.Imports | ? {
        return ($targetFileName -ieq [IO.Path]::GetFileName($_.Project))
    }  | % {  
        $currentProject.Xml.RemoveChild($_);
    }

    # remove the properties DllExportAttributeFullName and DllExportAttributeAssemblyName
    $currentProject.Xml.Properties | ? {
        $_.Name -eq "DllExportAttributeFullName" -or $_.Name -eq "DllExportAttributeAssemblyName"
    } | % {
        $_.Parent.RemoveChild($_)
    }

    $projectUri = New-Object Uri -ArgumentList $currentProject.FullPath, [UriKind]::Absolute
    $relativeUrl = $projectUri.MakeRelative($targetUri)
    $import = $currentProject.Xml.AddImport($relativeUrl)
    $import.Condition = "Exists('$relativeUrl')";
    
    # remove the old stuff in the DllExports folder from previous versions, (will check that only known files are in it)
    Remove-OldDllExportFolder $project
    Assert-PlatformTargetOfProject $project.FullName
}