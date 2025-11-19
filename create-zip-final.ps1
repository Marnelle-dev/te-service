# Script final pour creer un ZIP avec UN SEUL fichier docker-compose.yml
Write-Host "Creation du package ZIP FINAL (un seul docker-compose.yml)..." -ForegroundColor Green

if (Test-Path "portainer-deploy-final.zip") {
    Remove-Item "portainer-deploy-final.zip" -Force
}

$tempDir = "portainer-temp-final"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

Write-Host "Copie des fichiers..." -ForegroundColor Cyan

# Copier Dockerfile
Copy-Item "Dockerfile" -Destination $tempDir -Force
Write-Host "  - Dockerfile copie" -ForegroundColor Gray

# Copier .dockerignore
Copy-Item ".dockerignore" -Destination $tempDir -Force
Write-Host "  - .dockerignore copie" -ForegroundColor Gray

# Creer UN SEUL docker-compose.yml (sans autres fichiers YAML)
$composeYaml = @"
version: '3.8'
services:
  transport-te-api:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: transport-te-api-prod
    ports:
      - "8400:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__chaine=`${CONNECTION_STRING}
    restart: unless-stopped
"@
[System.IO.File]::WriteAllText("$tempDir\docker-compose.yml", $composeYaml, [System.Text.Encoding]::UTF8)
Write-Host "  - docker-compose.yml cree" -ForegroundColor Gray

# Copier le dossier API (exclure TOUS les fichiers .yml/.yaml)
Write-Host "Copie de TransportTeService.Api..." -ForegroundColor Cyan
$apiSource = "TransportTeService.Api"
$apiDest = "$tempDir\TransportTeService.Api"
New-Item -ItemType Directory -Path $apiDest -Force | Out-Null

Get-ChildItem -Path $apiSource -Recurse | Where-Object {
    $_.FullName -notlike "*\bin\*" -and 
    $_.FullName -notlike "*\obj\*" -and
    $_.Extension -ne ".yml" -and 
    $_.Extension -ne ".yaml"
} | ForEach-Object {
    $relativePath = $_.FullName.Substring($apiSource.Length + 1)
    $destPath = Join-Path $apiDest $relativePath
    $destDir = Split-Path $destPath -Parent
    if (-not (Test-Path $destDir)) {
        New-Item -ItemType Directory -Path $destDir -Force | Out-Null
    }
    Copy-Item $_.FullName -Destination $destPath -Force
}

Write-Host "  - TransportTeService.Api copie" -ForegroundColor Gray

# Verifier qu'il n'y a qu'un seul fichier YAML
Write-Host ""
Write-Host "Verification des fichiers YAML dans le ZIP..." -ForegroundColor Yellow
$yamlFiles = Get-ChildItem -Path $tempDir -Recurse -Include *.yml,*.yaml
Write-Host "  Fichiers YAML trouves: $($yamlFiles.Count)" -ForegroundColor $(if ($yamlFiles.Count -eq 1) { "Green" } else { "Red" })
$yamlFiles | ForEach-Object { Write-Host "    - $($_.FullName)" -ForegroundColor Gray }

# Creer le ZIP
Write-Host ""
Write-Host "Creation du ZIP..." -ForegroundColor Cyan
$currentDir = Get-Location
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "$currentDir\portainer-deploy-final.zip" -Force
Set-Location $currentDir

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host ""
Write-Host "ZIP cree avec succes : portainer-deploy-final.zip" -ForegroundColor Green
Write-Host "Ce ZIP contient EXACTEMENT UN SEUL fichier docker-compose.yml" -ForegroundColor Yellow

