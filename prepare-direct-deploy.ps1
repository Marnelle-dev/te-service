# Script pour preparer les fichiers pour deploiement direct
Write-Host "Preparation des fichiers pour deploiement direct..." -ForegroundColor Green

$deployDir = "deploy-direct"
if (Test-Path $deployDir) {
    Remove-Item $deployDir -Recurse -Force
}
New-Item -ItemType Directory -Path $deployDir | Out-Null

Write-Host "Copie des fichiers..." -ForegroundColor Cyan

# Copier Dockerfile
Copy-Item "Dockerfile" -Destination $deployDir -Force
Write-Host "  - Dockerfile" -ForegroundColor Gray

# Copier .dockerignore
Copy-Item ".dockerignore" -Destination $deployDir -Force
Write-Host "  - .dockerignore" -ForegroundColor Gray

# Creer docker-compose.yml avec chemin a modifier
$composeContent = @"
version: '3.8'

services:
  transport-te-api:
    build:
      context: /chemin/vers/deploy-direct
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
Set-Content -Path "$deployDir\docker-compose.yml" -Value $composeContent -Encoding UTF8
Write-Host "  - docker-compose.yml" -ForegroundColor Gray

# Copier le dossier API
Write-Host "Copie de TransportTeService.Api..." -ForegroundColor Cyan
$apiSource = "TransportTeService.Api"
$apiDest = "$deployDir\TransportTeService.Api"
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

Write-Host "  - TransportTeService.Api" -ForegroundColor Gray

Write-Host ""
Write-Host "Dossier prepare : $deployDir" -ForegroundColor Green
Write-Host ""
Write-Host "Prochaines etapes:" -ForegroundColor Yellow
Write-Host "1. Transferez le dossier deploy-direct sur le serveur Portainer" -ForegroundColor White
Write-Host "2. Notez le chemin absolu ou vous avez place les fichiers" -ForegroundColor White
Write-Host "3. Modifiez le chemin dans docker-compose.yml" -ForegroundColor White
Write-Host "4. Dans Portainer, utilisez Web Editor avec le docker-compose.yml" -ForegroundColor White
