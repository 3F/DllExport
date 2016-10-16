function Remove-OldDllExportFolder {
    param($project)
    $defaultFiles = ('DllExportAttribute.cs',
                     'Mono.Cecil.dll',
                     'RGiesecke.DllExport.dll',
                     'RGiesecke.DllExport.pdb',
                     'RGiesecke.DllExport.MSBuild.dll',
                     'RGiesecke.DllExport.MSBuild.pdb',
                     'net.r_eg.DllExport.targets')

    $projectFile = New-Object 'System.IO.FileInfo'($project.FullName)

    $projectFile.Directory.GetDirectories("DllExport") | Select-Object -First 1 | % {
        $dllExportDir = $_
    
        if($dllExportDir.GetDirectories().Count -eq 0){
            $unknownFiles = $dllExportDir.GetFiles() | Select -ExpandProperty Name | ? { -not $defaultFiles -contains $_ }
    
            if(-not $unknownFiles){
                Write-Host "Removing 'DllExport' from " $project.Name
                $project.ProjectItems | ? {	$_.Name -ieq 'DllExport' } | % {
                    $_.Remove()
                }

                Write-Host "Deleting " $dllExportDir.FullName " ..."
                $dllExportDir.Delete($true)
            }
        }
    }
}

function Remove-OldDllExportFolders {
    Get-Project -all | % {
        Remove-OldDllExportFolder $_
    }
}

function Get-MBEGlobalProjectCollection {
    $msBuildV4Name = 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a';
    $msBuildV4 = [System.Reflection.Assembly]::LoadWithPartialName($msBuildV4Name)

    if(!$msBuildV4) {
        throw New-Object 'System.IO.FileNotFoundException'("Could not load $msBuildV4Name.")
    }

    $projectCollection = $msBuildV4.GetType('Microsoft.Build.Evaluation.ProjectCollection')

    return $projectCollection::GlobalProjectCollection
}

function Get-DllExportMsBuildProjectsByFullName([String] $fullName) {
    $gpc = Get-MBEGlobalProjectCollection

    return $gpc.GetLoadedProjects($fullName)
}

function Get-TempPathToDllTools([String] $toolsPath) {
    
    $tempRoot   = (Join-Path $([System.IO.Path]::GetTempPath()) '50ACAD2A-5AB3-4E6A-BA66-07F55672E91F') -replace ' ', '` '
    $tempFolder = $([System.Guid]::NewGuid());
    $delprefix  = '__del__';

    # rename for checking of lock / loaded assemblies
    Get-ChildItem -Recurse -Path $tempRoot | ?{ $_.PSIsContainer } | %{ 
        Rename-Item -ErrorAction SilentlyContinue -Path $_.FullName -NewName "$delprefix$($_.Name)" 
    }

    # now try to delete only this
    Get-ChildItem -Recurse -Path $tempRoot | ?{ $_.PSIsContainer -and $_.Name.StartsWith($delprefix) } | %{ 
        Remove-Item $_.FullName -Force -Recurse -ErrorAction SilentlyContinue
    }

    $tdll = (Join-Path $tempRoot $tempFolder);
    if(!(Test-Path -path $tdll)) {
        New-Item $tdll -Type Directory >$null
    }
    Copy-Item $toolsPath\*.dll -Destination $tdll >$null

    return $tdll
}

function Get-TempPathToConfiguratorIfNotLoaded([String] $asmFile, [String] $toolsPath) {
    
    $tdll = Get-TempPathToDllTools $toolsPath
    $mdll = (Join-Path $tdll $asmFile)
    
    if(!(Get-Module -Name $asmFile)) {
        # Import-Module $mdll;
        return $mdll
    }
    return $null
}

function Get-AllDllExportMsBuildProjects {
    (Get-Project -all | % {
        Get-DllExportMsBuildProjectsByFullName $_.FullName
    }) | ? {
        return ($_.Xml.Imports | ? {
               "net.r_eg.DllExport.targets" -ieq [System.IO.Path]::GetFileName($_.Project);
        }).Length -gt 0;
    }
}

function Set-NoDllExportsForAnyCpu([String] $projectName, [System.Nullable[bool]] $value) {
    $projects = Get-AllDllExportMsBuildProjects;
    
    [String] $asString = $value;

    if($projectName) {
        $projects = $projects | where { $_.Name -ieq $projectName };
    }
    $propertyName = 'NoDllExportsForAnyCpu';
    
    $projects = $projects | where { 
        $_.GetPropertyValue($propertyName) -ine $asString 
    } | % {
        $_.SetProperty($propertyName, $asString);
    }
}

Export-ModuleMember Set-NoDllExportsForAnyCpu
Export-ModuleMember Get-MBEGlobalProjectCollection
Export-ModuleMember Get-TempPathToDllTools
Export-ModuleMember Get-TempPathToConfiguratorIfNotLoaded
Export-ModuleMember Remove-OldDllExportFolder
Export-ModuleMember Remove-OldDllExportFolders
Export-ModuleMember Get-DllExportMsBuildProjectsByFullName
Export-ModuleMember Get-AllDllExportMsBuildProjects