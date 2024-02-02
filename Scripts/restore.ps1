$backupItems = Get-ChildItem -Path "..\Backups"

$selected = $backupItems | Out-GridView -Title "Welches Backup willst du wiederherstellen?" -OutputMode Single

if ($null -ne $selected) {
    mongorestore --host localhost --port 27017 --dir "..\Backups\$selected" --drop --db skiservice --username superadmin --password superadmin --authenticationDatabase admin --restoreDbUsersAndRoles
}

if ($?) {
    Write-Host "Backup successfully restored. Congratulation."
    Read-Host
}
else {
    Write-Host "An Error happened, please try again."
    Read-Host
}