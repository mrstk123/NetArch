# Find and delete all .keep files in the current directory and subdirectories
Get-ChildItem -Path . -Recurse -Filter ".keep" | Remove-Item -Force

Write-Host "Cleaned all .keep files."
