# Script pour creer un ZIP avec UN SEUL fichier docker-compose.yml
Write-Host "Creation du package ZIP avec un seul docker-compose.yml..." -ForegroundColor Green

if (Test-Path "portainer-deploy-single.zip") {
    Remove-Item "portainer-deploy-single.zip" -Force
}

$tempDir = "portainer-temp-single"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

# Copier les fichiers (PAS les autres fichiers YAML)
Copy-Item "Dockerfile" -Destination $tempDir -Force
Copy-Item ".dockerignore" -Destination $tempDir -Force

# Creer UN SEUL docker-compose.yml
$composeContent = "version: '3.8'`nservices:`n  transport-te-api:`n    build:`n      context: .`n      dockerfile: Dockerfile`n    container_name: transport-te-api-prod`n    ports:`n      - `"8400:8080`"`n    environment:`n      - ASPNETCORE_ENVIRONMENT=Production`n      - ASPNETCORE_URLS=http://+:8080`n      - ConnectionStrings__chaine=`${CONNECTION_STRING}`n    restart: unless-stopped"
[System.IO.File]::WriteAllText("$tempDir\docker-compose.yml", $composeContent, [System.Text.Encoding]::UTF8)

# Copier le dossier API (exclure bin, obj, et TOUS les fichiers .yml/.yaml)
$excludeDirs = @('bin', 'obj')
$excludeFiles = @('*.yml', '*.yaml')
Get-ChildItem -Path "TransportTeService.Api" | Where-Object { 
    $_.Name -notin $excludeDirs -and $_.Extension -notin @('.yml', '.yaml')
} | ForEach-Object {
    if ($_.PSIsContainer) {
        Copy-Item $_.FullName -Destination $tempDir -Recurse -Force -Exclude $excludeFiles
    } else {
        Copy-Item $_.FullName -Destination $tempDir -Force
    }
}

# Creer le ZIP
$currentDir = Get-Location
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "$currentDir\portainer-deploy-single.zip" -Force
Set-Location $currentDir

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host "ZIP cree : portainer-deploy-single.zip" -ForegroundColor Green
Write-Host "Ce ZIP contient UN SEUL fichier docker-compose.yml" -ForegroundColor Yellow

