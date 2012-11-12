$templateFile = $args[0]
$newFile = $args[1]
$fileToGetVersionFrom = $args[2]
$versionTextToReplace = "XXVERSIONXX"

Write-Host "Template file is " $templateFile
Write-Host "New file is " $newFile
Write-Host "Getting version from " $fileToGetVersionFrom

$fileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($fileToGetVersionFrom).FileVersion
Write-Host "Version of file " $fileVersion

Get-Content $templatefile | Foreach-Object {
    $_ -replace $versionTextToReplace, $fileVersion 
    } | Set-Content $newFile


