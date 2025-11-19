# Solution : Diagnostic du problème Dockerfile non trouvé

## 🔍 Diagnostic du problème

L'erreur `failed to read dockerfile: open Dockerfile: no such file or directory` indique que Portainer ne trouve pas le Dockerfile dans le contexte de build.

### Causes possibles :

1. **Portainer décompresse le ZIP dans un répertoire temporaire** et le contexte de build ne pointe pas vers le bon endroit
2. **La structure du ZIP n'est pas celle attendue** par Portainer
3. **Le contexte `context: .` ne résout pas correctement** dans l'environnement Portainer

## ✅ Solution 1 : Vérifier la structure du ZIP

### Étape 1 : Décompressez le ZIP localement

1. **Décompressez `portainer-deploy.zip`** dans un dossier de test
2. **Vérifiez la structure** - elle doit être :

```
dossier-decompresse/
├── Dockerfile                    ← Doit être ici
├── .dockerignore
├── docker-compose.yml
└── TransportTeService.Api/
    └── ...
```

### Étape 2 : Testez localement

Dans le dossier décompressé, testez :

```bash
docker-compose config
```

Si cela fonctionne, le problème vient de la façon dont Portainer gère le ZIP.

## ✅ Solution 2 : Utiliser le Web Editor avec fichiers séparés

### Dans Portainer :

1. **Web Editor** : Collez le docker-compose.yml
2. **Cherchez une option "Additional files"** ou "Upload files"
3. **Uploadez séparément** :
   - Dockerfile
   - .dockerignore
   - TransportTeService.Api/ (ou un ZIP de ce dossier)

## ✅ Solution 3 : Modifier le script de création du ZIP

Le problème peut venir de la façon dont le ZIP est créé. Créons un ZIP avec une structure garantie :

### Nouveau script (create-zip-fixed.ps1) :

```powershell
# Créer un dossier temporaire
$tempDir = "portainer-temp"
if (Test-Path $tempDir) {
    Remove-Item $tempDir -Recurse -Force
}
New-Item -ItemType Directory -Path $tempDir | Out-Null

# Copier TOUS les fichiers à la racine du dossier temp
Copy-Item "Dockerfile" -Destination $tempDir -Force
Copy-Item ".dockerignore" -Destination $tempDir -Force
Copy-Item "docker-compose.yml" -Destination $tempDir -Force

# Copier le dossier API
Copy-Item "TransportTeService.Api" -Destination $tempDir -Recurse -Force -Exclude "bin","obj"

# Créer le ZIP depuis le CONTENU du dossier (pas le dossier lui-même)
Set-Location $tempDir
Compress-Archive -Path * -DestinationPath "..\portainer-deploy-fixed.zip" -Force
Set-Location ..

# Nettoyer
Remove-Item $tempDir -Recurse -Force

Write-Host "ZIP cree : portainer-deploy-fixed.zip"
```

## ✅ Solution 4 : Utiliser un chemin explicite dans docker-compose

Essayez de modifier le docker-compose.yml pour utiliser un chemin explicite :

```yaml
version: '3.8'

services:
  transport-te-api:
    build:
      context: .
      dockerfile: ./Dockerfile  # Chemin explicite
    # ...
```

## 🎯 Solution recommandée : Test manuel

1. **Décompressez le ZIP** que vous avez créé
2. **Vérifiez que tous les fichiers sont présents**
3. **Testez `docker-compose config`** dans le dossier décompressé
4. **Si ça fonctionne**, le problème vient de Portainer
5. **Essayez d'uploader les fichiers individuellement** via le Web Editor

## 📝 Vérification dans Portainer

Après avoir uploadé le ZIP dans Portainer :

1. **Allez dans la stack créée**
2. **Vérifiez les logs de build**
3. **Regardez le chemin où Portainer cherche le Dockerfile**
4. **Vérifiez le contexte de build utilisé**

Cela vous donnera des indices sur où Portainer décompresse le ZIP.

