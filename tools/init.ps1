param($installPath, $toolsPath, $package, $project)

$manager = "DllExport.bat"
Copy-Item "$installPath\\$manager" "$PWD" -Force

$project.Save($project.FullPath)
Start-Process -FilePath ".\\$manager" -WorkingDirectory "$PWD"