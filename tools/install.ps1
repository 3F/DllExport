param($installPath, $toolsPath, $package, $project)

$namespaceProp  = 'DllExportNamespace';
$targetFileName = 'net.r_eg.DllExport.targets'
$assemblyFName  = 'DllExport' # $package.AssemblyReferences[0].Name
$publicKeyToken = '8337224C9AD9E356';
$defaultNS      = 'System.Runtime.InteropServices';
$targetFileName = [IO.Path]::Combine($toolsPath, $targetFileName)
$targetUri      = New-Object Uri -ArgumentList $targetFileName, [UriKind]::Absolute

$msBuildV4Name  = 'Microsoft.Build'; #, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a';
$msBuildV4      = [System.Reflection.Assembly]::LoadWithPartialName($msBuildV4Name)          # obsolete
$msvb           = [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.VisualBasic') # obsolete

if(!$msBuildV4) {
    throw New-Object System.IO.FileNotFoundException("Could not load $msBuildV4Name.");
}

$projectCollection = $msBuildV4.GetType('Microsoft.Build.Evaluation.ProjectCollection')
$projects          =  $projectCollection::GlobalProjectCollection.GetLoadedProjects($project.FullName)


# Define or check the DllExportNamespace property

$vNamespace = '';
$userNS     = '';

$projects |  % {
    $mbeProject = $_

    $vNamespace = $mbeProject.GetPropertyValue($namespaceProp);
    if([String]::IsNullOrEmpty($vNamespace))
    {
        #$pFName = [System.IO.Path]::GetFileName($mbeProject.FullPath);
        $userNS = [Microsoft.VisualBasic.Interaction]::InputBox(
                                    "Select a DllExport namespace. `nIt overrides prev. if exists. `n`nHow about 'System.Runtime.InteropServices' ? `nor:", 
                                    "DllExport namespace", 
                                    "$($mbeProject.GetPropertyValue('RootNamespace'))")

        $mbeProject.SetProperty($namespaceProp, $userNS)
        $project.Save()

        $vNamespace = $userNS;
    }
}

if([String]::IsNullOrWhiteSpace($vNamespace)) {
    $vNamespace = $defaultNS; # inc. the 'cancel' button by user
}

# override new NS for all projects that are contains this library in references
# TODO: another way

if(![String]::IsNullOrEmpty($vNamespace)) # -And ![String]::IsNullOrEmpty($userNS)
{
    foreach($dtePrj in $project.Collection)
    {
        # https://msdn.microsoft.com/en-us/library/vslangproj.reference.aspx
        $dtePrj.Object.References | ? { 
            $_.Name -ieq $assemblyFName -And $_.PublicKeyToken -ieq $publicKeyToken
        } | % {  

            $ePrjs = $projectCollection::GlobalProjectCollection.GetLoadedProjects($dtePrj.FullName);
            foreach($ePrj in $ePrjs) {
                if(!$ePrj.FullPath.EndsWith(".user", [System.StringComparison]::OrdinalIgnoreCase)) {
                    $ePrj.SetProperty($namespaceProp, $vNamespace);
                }
            }
            $dtePrj.Save($dtePrj.FullName); # save it with EnvDTE to avoid 'modified outside the environment'

        }
    }
}

# binary modifications of assembly

. "nsbin.ps1"
defNS $([System.IO.Path]::Combine($installPath, 'lib\net', $assemblyFName + '.dll'))  $vNamespace


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