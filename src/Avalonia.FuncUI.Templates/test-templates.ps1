
$templates = Get-ChildItem ./content -Directory -Exclude "CrossPlatformMvuTemplate"

$errors = [System.Collections.Generic.List[PSObject]]::new()

foreach ($templateDir in $templates) {
    <# $templateDir is the current item #>
    Push-Location $templateDir.FullName;
    Write-Host "Building template: $($templateDir.Name)";
    dotnet build -c release

    if ($LASTEXITCODE -ne 0) {
        $errors.Add($templateDir);
    }
    Pop-Location;
}

if ($errors.Count -gt 0) {
    Write-Error -Message "The following templates failed to build:"
    $errors | Format-Table -Property Name;
    exit 1;
}

Write-Host "All templates built successfully."