param (
[string]$ProjectDir,
[string]$Revision
)

    #write new version to nuspec
    $nuspec_file_name = "Package.nuspec"

    $nuspec_file_path = "$ProjectDir$nuspec_file_name"
    Write-Host "nuspec file: $nuspec_file_path"
    [xml]$nuspec_xml = Get-Content($nuspec_file_path)
    $nuspec_xml.package.metadata.version = $Revision
    $nuspec_xml.Save($nuspec_file_path)