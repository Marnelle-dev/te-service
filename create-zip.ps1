# Script PowerShell pour creer le ZIP de deploiement pour Portainer
# Executez ce script depuis le repertoire racine du projet

Write-Host "Creation du package ZIP pour Portainer..." -ForegroundColor Green

# Supprimer l'ancien ZIP s'il existe
if (Test-Path "portainer-deploy.zip") {
    Remove-Item "portainer-deploy.zip" -Force
    Write-Host "Ancien ZIP supprime" -ForegroundColor Yellow
}

# Creer un dossier temporaire
$tempDir = "portainer-temp"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

Write-Host "Copie des fichiers..." -ForegroundColor Cyan

# Copier le Dockerfile
Copy-Item "Dockerfile" -Destination $tempDir -Force

# Copier .dockerignore
Copy-Item ".dockerignore" -Destination $tempDir -Force

# Copier docker-compose.yml (utiliser directement docker-compose.yml)
Copy-Item "docker-compose.yml" -Destination "$tempDir\docker-compose.yml" -Force

# Copier le dossier TransportTeService.Api (sans bin/obj)
Write-Host "Copie de TransportTeService.Api..." -ForegroundColor Cyan
$apiDir = "$tempDir\TransportTeService.Api"
New-Item -ItemType Directory -Path $apiDir | Out-Null

# Copier les fichiers et dossiers necessaires (exclure bin, obj)
Get-ChildItem -Path "TransportTeService.Api" -Exclude "bin", "obj" | ForEach-Object {
    if ($_.PSIsContainer) {
        Copy-Item $_.FullName -Destination $apiDir -Recurse -Force
    } else {
        Copy-Item $_.FullName -Destination $apiDir -Force
    }
}

# Creer le ZIP
Write-Host "Creation du ZIP..." -ForegroundColor Cyan
Compress-Archive -Path "$tempDir\*" -DestinationPath "portainer-deploy.zip" -Force

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host ""
Write-Host "ZIP cree avec succes : portainer-deploy.zip" -ForegroundColor Green
Write-Host ""
Write-Host "Prochaines etapes :" -ForegroundColor Yellow
Write-Host "1. Dans Portainer, choisissez Upload au lieu de Web editor" -ForegroundColor White
Write-Host "2. Uploadez le fichier portainer-deploy.zip" -ForegroundColor White
Write-Host "3. Configurez la variable d'environnement CONNECTION_STRING" -ForegroundColor White
Write-Host "4. Deployez la stack" -ForegroundColor White
