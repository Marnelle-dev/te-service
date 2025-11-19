# Script pour creer un ZIP avec indentation parfaite
Write-Host "Creation du package ZIP avec indentation parfaite..." -ForegroundColor Green

if (Test-Path "portainer-deploy-perfect.zip") {
    Remove-Item "portainer-deploy-perfect.zip" -Force
}

$tempDir = "portainer-temp-perfect"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

# Copier Dockerfile
Copy-Item "Dockerfile" -Destination $tempDir -Force

# Copier .dockerignore
Copy-Item ".dockerignore" -Destination $tempDir -Force

# Creer docker-compose.yml avec indentation parfaite (2 espaces, pas de tabs)
$composeYaml = "version: '3.8'`n`nservices:`n  transport-te-api:`n    build:`n      context: .`n      dockerfile: Dockerfile`n    container_name: transport-te-api-prod`n    ports:`n      - `"8400:8080`"`n    environment:`n      - ASPNETCORE_ENVIRONMENT=Production`n      - ASPNETCORE_URLS=http://+:8080`n      - ConnectionStrings__chaine=`${CONNECTION_STRING}`n    restart: unless-stopped`n"

# Ecrire avec UTF8 sans BOM
$utf8NoBom = New-Object System.Text.UTF8Encoding $false
[System.IO.File]::WriteAllText("$tempDir\docker-compose.yml", $composeYaml, $utf8NoBom)

Write-Host "docker-compose.yml cree avec indentation parfaite" -ForegroundColor Green

# Copier le dossier API
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

# Creer le ZIP
$currentDir = Get-Location
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "$currentDir\portainer-deploy-perfect.zip" -Force
Set-Location $currentDir

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host ""
Write-Host "ZIP cree : portainer-deploy-perfect.zip" -ForegroundColor Green
Write-Host "Indentation: 2 espaces (pas de tabs)" -ForegroundColor Yellow

