# Solution directe : Upload des fichiers sur le serveur Portainer

## 🎯 Objectif
Déployer directement sur Portainer sans passer par Git ou Docker Hub.

## ✅ Solution : Transférer les fichiers sur le serveur Portainer

### Étape 1 : Préparer les fichiers

Créez un dossier avec tous les fichiers nécessaires :

```
deploy-direct/
├── Dockerfile
├── .dockerignore
├── docker-compose.yml
└── TransportTeService.Api/
    └── (tous les fichiers)
```

### Étape 2 : Transférer sur le serveur Portainer

**Option A : Via SCP (si vous avez un accès SSH)**

```bash
# Depuis votre machine Windows (PowerShell ou Git Bash)
scp -r deploy-direct/ user@serveur-portainer:/chemin/vers/deploy/
```

**Option B : Via l'interface de Portainer (si disponible)**

1. Connectez-vous à Portainer
2. Cherchez une option "Files" ou "Volumes"
3. Uploadez les fichiers

**Option C : Via RDP/Remote Desktop (si Windows Server)**

1. Connectez-vous au serveur via Remote Desktop
2. Copiez les fichiers directement
3. Placez-les dans un dossier accessible (ex: `C:\docker\transport-te-service\`)

### Étape 3 : Modifier docker-compose.yml pour utiliser le chemin absolu

Une fois les fichiers sur le serveur, modifiez le docker-compose.yml pour pointer vers le chemin absolu :

```yaml
version: '3.8'

services:
  transport-te-api:
    build:
      context: /chemin/vers/deploy-direct  # Chemin absolu sur le serveur
      dockerfile: Dockerfile
    container_name: transport-te-api-prod
    ports:
      - "8400:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__chaine=${CONNECTION_STRING}
    restart: unless-stopped
```

### Étape 4 : Dans Portainer

1. **Allez dans "Stacks"** → **"Add stack"**
2. **Choisissez "Web editor"**
3. **Collez le docker-compose.yml modifié** (avec le chemin absolu)
4. **Variables d'environnement** : Ajoutez `CONNECTION_STRING`
5. **Déployez**

## 🔄 Alternative : Utiliser un volume partagé

Si vous avez un volume partagé accessible depuis Portainer :

1. Placez les fichiers sur le volume partagé
2. Utilisez le chemin du volume dans docker-compose.yml

## 📝 Script pour préparer les fichiers

Je peux créer un script qui prépare tous les fichiers dans un dossier prêt à transférer.

