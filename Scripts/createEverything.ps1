Write-Host "Erase before starting initialization..."

mongosh --file .\erase.js --username superadmin --password superadmin --authenticationDatabase admin | Out-Null

if (!$?) {
    mongosh --file .\erase.js | Out-Null
}

Write-Host "initialize..."
Get-ChildItem -Path ".\DbCollections" | ForEach-Object {
    Write-Host "uploading collection $($_.Name)" & mongosh --file ".\DbCollections\$($_.Name)" | Out-Null
}

mongosh --file .\createUsersAndRoles.js | Out-Null