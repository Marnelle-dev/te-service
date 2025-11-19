# Script PowerShell pour creer le ZIP avec structure garantie
Write-Host "Creation du package ZIP pour Portainer (version fixee)..." -ForegroundColor Green

# Supprimer l'ancien ZIP s'il existe
if (Test-Path "portainer-deploy-fixed.zip") {
    Remove-Item "portainer-deploy-fixed.zip" -Force
}

# Creer un dossier temporaire
$tempDir = "portainer-temp-fixed"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

Write-Host "Copie des fichiers..." -ForegroundColor Cyan

# Copier les fichiers a la racine
Copy-Item "Dockerfile" -Destination $tempDir -Force
Copy-Item ".dockerignore" -Destination $tempDir -Force
Copy-Item "docker-compose.yml" -Destination $tempDir -Force

# Copier le dossier API (exclure bin et obj)
Write-Host "Copie de TransportTeService.Api..." -ForegroundColor Cyan
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

# Creer le ZIP depuis le CONTENU du dossier (important!)
Write-Host "Creation du ZIP..." -ForegroundColor Cyan
$currentDir = Get-Location
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "$currentDir\portainer-deploy-fixed.zip" -Force
Set-Location $currentDir

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host ""
Write-Host "ZIP cree avec succes : portainer-deploy-fixed.zip" -ForegroundColor Green
Write-Host ""
Write-Host "Ce ZIP a une structure garantie avec tous les fichiers a la racine" -ForegroundColor Yellow

