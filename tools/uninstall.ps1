param($installPath, $toolsPath, $package, $project)

$assemblyFName      = 'DllExport'
$targetFileName     = 'net.r_eg.DllExport.targets'
$escInstallPath     = $installPath -replace ' ', '` '
$escToolsPath       = $toolsPath -replace ' ', '` '
$metaLib            = $([System.IO.Path]::Combine($escInstallPath, 'lib\net20', $assemblyFName + '.dll'));
$gpc                = Get-MBEGlobalProjectCollection
$projects           = $gpc.GetLoadedProjects($project.FullName)

# Configurator

$dllConf = Get-TempPathToConfiguratorIfNotLoaded 'net.r_eg.DllExport.Configurator.dll' $escToolsPath
if($dllConf) {
    Import-Module $dllConf; 
}

Reset-Configuration -MetaLib $metaLib -InstallPath $escInstallPath -ToolsPath $escToolsPath -ProjectDTE $project -ProjectsMBE $gpc;

#

return $projects |  % {
    $currentProject = $_

    $currentProject.Xml.Imports | ? {
        $targetFileName -ieq [System.IO.Path]::GetFileName($_.Project)
    }  | % {  
        $currentProject.Xml.RemoveChild($_)
    }
}