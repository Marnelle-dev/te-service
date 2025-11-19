# Script pour creer un ZIP ultra-propre
Write-Host "Creation du package ZIP ultra-propre..." -ForegroundColor Green

if (Test-Path "portainer-deploy-ultra-clean.zip") {
    Remove-Item "portainer-deploy-ultra-clean.zip" -Force
}

$tempDir = "portainer-temp-ultra-clean"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

# Copier les fichiers
Copy-Item "Dockerfile" -Destination $tempDir -Force
Copy-Item ".dockerignore" -Destination $tempDir -Force

# Creer docker-compose.yml propre dans le temp
$composeContent = @"
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
Set-Content -Path "$tempDir\docker-compose.yml" -Value $composeContent -Encoding UTF8 -NoNewline

# Copier le dossier API
$excludeDirs = @('bin', 'obj')
Get-ChildItem -Path "TransportTeService.Api" | Where-Object { 
    $_.Name -notin $excludeDirs 
} | ForEach-Object {
    if ($_.PSIsContainer) {
        Copy-Item $_.FullName -Destination $tempDir -Recurse -Force
    } else {
        Copy-Item $_.FullName -Destination $tempDir -Force
    }
}

# Creer le ZIP
$currentDir = Get-Location
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "$currentDir\portainer-deploy-ultra-clean.zip" -Force
Set-Location $currentDir

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host "ZIP cree : portainer-deploy-ultra-clean.zip" -ForegroundColor Green

