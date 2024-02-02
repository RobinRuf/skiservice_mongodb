$backupDirectory = "..\Backups\$(Get-Date -Format "yyyyMMddHHmmss")"

New-Item -Path "..\Backups" -ItemType Directory -Force 
mongodump --host localhost --port 27017 --db skiservice --dumpDbUsersAndRoles --out $backupDirectory --username superadmin --password superadmin --authenticationDatabase admin

if ($?) {
    Write-Host "Backup created at ${name} inside the Root of the project."
    Read-Host
}
else {
    Write-Host "An Error happened, please try again."
    Read-Host
}
