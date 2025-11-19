# Solution : Dockerfile non trouvé dans Portainer

## Problème
```
Failed to deploy a stack: compose build operation failed: failed to solve: failed to read dockerfile: open Dockerfile: no such file or directory
```

Cela signifie que Portainer ne trouve pas le Dockerfile car les fichiers sources ne sont pas disponibles dans le contexte de build.

## ✅ Solution 1 : Utiliser Repository Git (Recommandé)

### Étape 1 : Préparer le dépôt Git

1. **Créez un dépôt Git** (GitHub, GitLab, Bitbucket, etc.)
2. **Poussez votre projet** sur le dépôt avec ces fichiers :
   - `Dockerfile`
   - `.dockerignore`
   - `docker-compose.yml`
   - Tout le dossier `TransportTeService.Api/`

### Étape 2 : Dans Portainer

1. **Allez dans "Stacks"** → **"Add stack"**
2. **Choisissez "Repository"**
3. **Configurez** :
   - **Repository URL** : `https://github.com/votre-username/votre-repo.git` (ou votre URL Git)
   - **Repository reference** : `main` ou `master`
   - **Compose path** : `docker-compose.yml`
   - **Auto-update** : Optionnel (activez si vous voulez des mises à jour automatiques)
4. **Variables d'environnement** :
   - **Name** : `CONNECTION_STRING`
   - **Value** : Votre chaîne de connexion SQL Server
5. **Cliquez sur "Deploy the stack"**

---

## ✅ Solution 2 : Construire l'image localement et utiliser un Registry

### Étape 1 : Construire et pousser l'image localement

```bash
# 1. Construire l'image
docker build -t transport-te-api:latest .

# 2. Taguer pour votre registry (ex: Docker Hub)
docker tag transport-te-api:latest votre-username/transport-te-api:latest

# 3. Se connecter à Docker Hub (ou votre registry)
docker login

# 4. Pousser l'image
docker push votre-username/transport-te-api:latest
```

### Étape 2 : Utiliser l'image dans Portainer

1. **Dans Portainer** → **Stacks** → **Add stack**
2. **Choisissez "Web editor"**
3. **Collez ce contenu** :

```yaml
version: '3.8'

services:
  transport-te-api:
    image: votre-username/transport-te-api:latest
    container_name: transport-te-api-prod
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__chaine=${CONNECTION_STRING}
    restart: unless-stopped
```

4. **Configurez `CONNECTION_STRING`** dans les variables d'environnement
5. **Déployez**

**Avantage** : Plus rapide, pas besoin de build sur le serveur distant

---

## ✅ Solution 3 : Upload des fichiers sur le serveur Portainer

Si vous avez un accès SSH au serveur Portainer :

### Étape 1 : Transférer les fichiers

```bash
# Via SCP (depuis votre machine)
scp -r TransportTeService.Api/ user@serveur-portainer:/chemin/vers/projet/
scp Dockerfile user@serveur-portainer:/chemin/vers/projet/
scp .dockerignore user@serveur-portainer:/chemin/vers/projet/
scp docker-compose.yml user@serveur-portainer:/chemin/vers/projet/
```

### Étape 2 : Modifier docker-compose.yml

Modifiez le `context` pour pointer vers le chemin absolu :

```yaml
version: '3.8'

services:
  transport-te-api:
    build:
      context: /chemin/vers/projet
      dockerfile: Dockerfile
    # ... reste de la config
```

### Étape 3 : Dans Portainer

Utilisez le Web Editor avec le docker-compose.yml modifié.

---

## 🎯 Recommandation

**Pour votre situation (Portainer distant)** :

1. **Solution la plus simple** : Utilisez **Repository Git** (Solution 1)
   - Créez un dépôt Git
   - Poussez votre code
   - Configurez Portainer pour cloner depuis Git

2. **Solution la plus rapide** : Construisez l'image localement et poussez-la vers un registry (Solution 2)
   - Build une fois localement
   - Push vers Docker Hub ou votre registry privé
   - Utilisez l'image dans Portainer

---

## 📝 Fichiers nécessaires pour Git

Assurez-vous que ces fichiers sont dans votre dépôt Git :

```
votre-repo/
├── Dockerfile
├── .dockerignore
├── docker-compose.yml
└── TransportTeService.Api/
    ├── TransportTeService.Api.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── Data/
    ├── DTOs/
    ├── Models/
    ├── Services/
    └── Migrations/
```

---

## ⚠️ Important

- Ne commitez **jamais** les mots de passe dans Git
- Utilisez les **variables d'environnement** de Portainer pour les secrets
- Le dossier `bin/` et `obj/` doivent être dans `.gitignore`

