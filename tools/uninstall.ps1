param($installPath, $toolsPath, $package, $project)

$namespaceProp  = 'DllExportNamespace';
$targetFileName = 'net.r_eg.DllExport.targets'
$targetFileName = [System.IO.Path]::Combine($toolsPath, $targetFileName)
$targetUri = New-Object Uri($targetFileName, [UriKind]::Absolute)

$projects = Get-DllExportMsBuildProjectsByFullName($project.FullName)

return $projects |  % {
    $currentProject = $_

    $currentProject.Xml.Imports | ? {
        "net.r_eg.DllExport.targets" -ieq [System.IO.Path]::GetFileName($_.Project)
    }  | % {  
        $currentProject.Xml.RemoveChild($_)
    }

    # NS
    $prop = $currentProject.GetProperty($namespaceProp);
    if($prop -ne $Null -and $prop -ne ''){
        Write-Host "Remove NS property: '$prop'"
        $currentProject.RemoveProperty($prop);
        $project.Save()
    }
}