param($installPath, $toolsPath, $package, $project)

$namespaceProp  = 'DllExportNamespace';
$targetFileName = 'net.r_eg.DllExport.targets'
$targetFileName = [System.IO.Path]::Combine($toolsPath, $targetFileName)
$targetUri = New-Object Uri($targetFileName, [UriKind]::Absolute)

$projects = Get-DllExportMsBuildProjectsByFullName($project.FullName)

return $projects |  % {
    $currentProject = $_

    $currentProject.RemoveProperty($currentProject.GetProperty($namespaceProp));
    $project.Save()

    $currentProject.Xml.Imports | ? {
        "net.r_eg.DllExport.targets" -ieq [System.IO.Path]::GetFileName($_.Project)
    }  | % {  
        $currentProject.Xml.RemoveChild($_)
    }
}