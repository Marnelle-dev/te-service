# Script de diagnostic pour Portainer
Write-Host "=== DIAGNOSTIC PORTAINER ===" -ForegroundColor Cyan
Write-Host ""

# 1. Verifier que le Dockerfile existe
Write-Host "1. Verification du Dockerfile..." -ForegroundColor Yellow
if (Test-Path "Dockerfile") {
    Write-Host "   ✓ Dockerfile trouve" -ForegroundColor Green
    $dockerfileContent = Get-Content "Dockerfile" -TotalCount 5
    Write-Host "   Premiere ligne: $($dockerfileContent[0])" -ForegroundColor Gray
} else {
    Write-Host "   ✗ Dockerfile NON TROUVE" -ForegroundColor Red
}

Write-Host ""

# 2. Verifier docker-compose.yml
Write-Host "2. Verification de docker-compose.yml..." -ForegroundColor Yellow
if (Test-Path "docker-compose.yml") {
    Write-Host "   ✓ docker-compose.yml trouve" -ForegroundColor Green
    $composeContent = Get-Content "docker-compose.yml" -Raw
    if ($composeContent -match "context:\s*\.") {
        Write-Host "   ✓ context: . trouve" -ForegroundColor Green
    } else {
        Write-Host "   ✗ context: . NON TROUVE" -ForegroundColor Red
    }
    if ($composeContent -match "dockerfile:\s*Dockerfile") {
        Write-Host "   ✓ dockerfile: Dockerfile trouve" -ForegroundColor Green
    } else {
        Write-Host "   ✗ dockerfile: Dockerfile NON TROUVE" -ForegroundColor Red
    }
} else {
    Write-Host "   ✗ docker-compose.yml NON TROUVE" -ForegroundColor Red
}

Write-Host ""

# 3. Verifier le contenu du ZIP
Write-Host "3. Verification du contenu du ZIP..." -ForegroundColor Yellow
if (Test-Path "portainer-deploy.zip") {
    Write-Host "   ✓ portainer-deploy.zip trouve" -ForegroundColor Green
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead("portainer-deploy.zip")
    
    $hasDockerfile = $zip.Entries | Where-Object { $_.FullName -eq "Dockerfile" }
    $hasCompose = $zip.Entries | Where-Object { $_.FullName -eq "docker-compose.yml" }
    $hasApi = $zip.Entries | Where-Object { $_.FullName -like "TransportTeService.Api/*" }
    
    if ($hasDockerfile) {
        Write-Host "   ✓ Dockerfile dans le ZIP" -ForegroundColor Green
    } else {
        Write-Host "   ✗ Dockerfile MANQUANT dans le ZIP" -ForegroundColor Red
    }
    
    if ($hasCompose) {
        Write-Host "   ✓ docker-compose.yml dans le ZIP" -ForegroundColor Green
    } else {
        Write-Host "   ✗ docker-compose.yml MANQUANT dans le ZIP" -ForegroundColor Red
    }
    
    if ($hasApi) {
        Write-Host "   ✓ TransportTeService.Api dans le ZIP ($(($zip.Entries | Where-Object { $_.FullName -like 'TransportTeService.Api/*' }).Count) fichiers)" -ForegroundColor Green
    } else {
        Write-Host "   ✗ TransportTeService.Api MANQUANT dans le ZIP" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "   Structure du ZIP (fichiers a la racine):" -ForegroundColor Cyan
    $rootFiles = $zip.Entries | Where-Object { $_.FullName -notlike "*/*" -and $_.FullName -ne "" }
    foreach ($file in $rootFiles) {
        Write-Host "     - $($file.FullName)" -ForegroundColor Gray
    }
    
    $zip.Dispose()
} else {
    Write-Host "   ✗ portainer-deploy.zip NON TROUVE" -ForegroundColor Red
}

Write-Host ""

# 4. Test de validation docker-compose
Write-Host "4. Test de validation docker-compose..." -ForegroundColor Yellow
$dockerComposeTest = docker-compose -f docker-compose.yml config 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✓ docker-compose.yml est valide" -ForegroundColor Green
} else {
    Write-Host "   ✗ docker-compose.yml a des erreurs:" -ForegroundColor Red
    Write-Host $dockerComposeTest -ForegroundColor Red
}

Write-Host ""
Write-Host "=== FIN DU DIAGNOSTIC ===" -ForegroundColor Cyan

