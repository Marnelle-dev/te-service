# Solution finale pour déployer sur Portainer

## 🔍 Problème identifié

Vous avez deux options pour déployer sur Portainer distant :

1. **Repository Git** (ne nécessite pas Docker local) ✅
2. **Image pré-construite** (nécessite Docker local) ❌ (Docker n'est pas accessible)

## ✅ Solution recommandée : Repository Git

Puisque Docker n'est pas accessible localement, la meilleure solution est d'utiliser un dépôt Git.

### Étape 1 : Créer un dépôt Git

1. **Créez un compte** sur GitHub, GitLab, ou Bitbucket (si vous n'en avez pas)
2. **Créez un nouveau dépôt** (ex: `transport-te-service`)
3. **Ne l'initialisez pas** avec un README (gardez-le vide)

### Étape 2 : Préparer votre projet pour Git

Assurez-vous d'avoir un fichier `.gitignore` à la racine :

```gitignore
# Build results
bin/
obj/
*.user
*.suo
*.cache

# Docker
portainer-*.zip
portainer-temp*/

# IDE
.vs/
.vscode/
.idea/

# OS
.DS_Store
Thumbs.db
```

### Étape 3 : Pousser votre projet sur Git

```bash
# Initialiser Git (si pas déjà fait)
git init

# Ajouter tous les fichiers
git add .

# Commit
git commit -m "Initial commit - Transport TE Service"

# Ajouter le remote (remplacez par votre URL)
git remote add origin https://github.com/votre-username/transport-te-service.git

# Pousser
git push -u origin main
```

### Étape 4 : Dans Portainer

1. **Allez dans "Stacks"** → **"Add stack"**
2. **Choisissez "Repository"**
3. **Configurez** :
   - **Repository URL** : `https://github.com/votre-username/transport-te-service.git`
   - **Repository reference** : `main` (ou `master`)
   - **Compose path** : `docker-compose.yml`
   - **Auto-update** : Optionnel
4. **Variables d'environnement** :
   - **Name** : `CONNECTION_STRING`
   - **Value** : `Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=VotreMotDePasse;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;`
5. **Cliquez sur "Deploy the stack"**

Portainer va :
- Cloner le dépôt Git
- Trouver le docker-compose.yml
- Trouver le Dockerfile
- Construire l'image
- Démarrer le conteneur

## 🔄 Alternative : Si vous n'avez pas de compte Git

Si vous ne pouvez pas utiliser Git, vous pouvez :

1. **Demander à un collègue** de créer le dépôt et vous donner l'accès
2. **Utiliser un Git interne** à votre entreprise
3. **Transférer les fichiers manuellement** sur le serveur Portainer via SSH/SCP

## 📝 Fichiers nécessaires dans le dépôt Git

Assurez-vous que ces fichiers sont dans votre dépôt :

```
transport-te-service/
├── Dockerfile
├── .dockerignore
├── docker-compose.yml
└── TransportTeService.Api/
    ├── TransportTeService.Api.csproj
    ├── Program.cs
    ├── appsettings.json
    └── ... (tous les autres fichiers)
```

## ⚠️ Important

- **Ne commitez JAMAIS** les mots de passe dans Git
- Utilisez les **variables d'environnement** de Portainer pour les secrets
- Le fichier `docker-compose.yml` doit être à la racine du dépôt

## 🎯 Prochaines étapes

1. Créez le dépôt Git
2. Poussez votre code
3. Configurez Portainer avec l'URL du dépôt
4. Déployez !

Cette méthode est la plus fiable et ne nécessite pas Docker local.

