# Guide de déploiement sur Portainer distant

## 📋 Situation
- Portainer est installé sur un **serveur distant**
- Vous devez déployer l'API Transport TE Service
- La base de données SQL Server est accessible depuis le serveur Portainer

## 🎯 Solutions possibles

### Solution 1 : Upload ZIP (Recommandé si disponible)

**Avantages** : Simple, direct, pas besoin de Git

#### Étape 1 : Préparer le ZIP
Le fichier `portainer-deploy.zip` a été créé dans le répertoire racine du projet.

#### Étape 2 : Dans Portainer (serveur distant)

1. **Connectez-vous à Portainer**
2. **Allez dans "Stacks"** → **"Add stack"**
3. **Choisissez "Upload"** (pas "Web editor")
4. **Uploadez le fichier `portainer-deploy.zip`**
5. **Portainer décompressera automatiquement le ZIP**

#### Étape 3 : Configurer les variables d'environnement

Dans la section **"Environment variables"** :

- **Name** : `CONNECTION_STRING`
- **Value** : 
  ```
  Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=Marnelle@1234567890;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;
  ```
  ⚠️ **Remplacez le mot de passe par le vrai mot de passe SQL Server**

#### Étape 4 : Vérifier le docker-compose.yml

Portainer devrait détecter automatiquement le fichier `docker-compose.yml` dans le ZIP. Vérifiez qu'il contient :

```yaml
version: '3.8'

services:
  transport-te-api:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: transport-te-api-prod
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__chaine=${CONNECTION_STRING}
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/swagger/index.html"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
```

#### Étape 5 : Déployer

1. Cliquez sur **"Deploy the stack"**
2. Attendez que le build se termine (5-10 minutes la première fois)
3. Vérifiez les logs pour voir si l'API démarre correctement

---

### Solution 2 : Repository Git (Recommandé pour production)

**Avantages** : Versioning, mise à jour facile, meilleure pratique

#### Étape 1 : Préparer le dépôt Git

1. **Créez un dépôt Git** (GitHub, GitLab, ou autre)
2. **Poussez votre projet** sur le dépôt
3. **Assurez-vous que les fichiers suivants sont présents** :
   - `Dockerfile`
   - `.dockerignore`
   - `docker-compose.prod.yml`
   - `TransportTeService.Api/` (tout le dossier)

#### Étape 2 : Dans Portainer (serveur distant)

1. **Allez dans "Stacks"** → **"Add stack"**
2. **Choisissez "Repository"**
3. **Configurez le dépôt** :
   - **Repository URL** : URL de votre dépôt Git
   - **Repository reference** : `main` ou `master` (selon votre branche)
   - **Compose path** : `docker-compose.prod.yml`
   - **Auto-update** : Activez si vous voulez des mises à jour automatiques

#### Étape 3 : Configurer les variables d'environnement

Dans la section **"Environment variables"** :

- **Name** : `CONNECTION_STRING`
- **Value** : Votre chaîne de connexion SQL Server

#### Étape 4 : Déployer

1. Cliquez sur **"Deploy the stack"**
2. Portainer clonera le dépôt et construira l'image

---

### Solution 3 : Build local + Push vers Registry Docker

**Avantages** : Plus rapide, pas de build sur le serveur distant

#### Étape 1 : Construire l'image localement

```bash
# Construire l'image
docker build -t transport-te-api:latest .

# Taguer l'image pour votre registry (ex: Docker Hub)
docker tag transport-te-api:latest votre-username/transport-te-api:latest

# Pousser vers le registry
docker push votre-username/transport-te-api:latest
```

#### Étape 2 : Modifier docker-compose pour utiliser l'image

Créez un nouveau fichier `docker-compose.registry.yml` :

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
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/swagger/index.html"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
```

#### Étape 3 : Dans Portainer

1. **Utilisez "Web editor"** (plus besoin de fichiers sources)
2. **Collez le contenu de `docker-compose.registry.yml`**
3. **Configurez la variable `CONNECTION_STRING`**
4. **Déployez**

---

## ✅ Après le déploiement (toutes les solutions)

### 1. Appliquer les migrations

Une fois l'API démarrée, appliquez les migrations :

```bash
# Option A : Depuis Portainer (Console)
# 1. Allez dans "Containers"
# 2. Trouvez "transport-te-api-prod"
# 3. Cliquez sur "Console"
# 4. Exécutez :
dotnet ef database update --project /app/TransportTeService.Api.csproj

# Option B : Depuis votre machine locale
dotnet ef database update --project TransportTeService.Api/TransportTeService.Api.csproj
```

### 2. Vérifier que l'API fonctionne

1. **Accédez à Swagger** : `http://votre-serveur-portainer:8080`
2. **Testez un endpoint** : `GET /api/v1/transports`

### 3. Vérifier les logs

Dans Portainer :
- **Containers** → **transport-te-api-prod** → **Logs**
- Vérifiez qu'il n'y a pas d'erreurs

---

## 🐛 Problèmes courants

### Erreur : "Dockerfile not found"
- **Cause** : Les fichiers sources ne sont pas disponibles
- **Solution** : Utilisez la Solution 1 (Upload ZIP) ou Solution 2 (Repository Git)

### Erreur : "Cannot connect to SQL Server"
- **Cause** : La base de données n'est pas accessible depuis le conteneur
- **Solution** : 
  - Vérifiez que SQL Server accepte les connexions depuis Docker
  - Vérifiez les règles de pare-feu
  - Vérifiez la chaîne de connexion (nom du serveur, port, etc.)

### Erreur : "Build context not found"
- **Cause** : Le contexte de build n'est pas correct
- **Solution** : Assurez-vous que tous les fichiers sont présents dans le ZIP ou le dépôt Git

### L'API démarre mais ne répond pas
- **Cause** : Port non exposé ou mauvaise configuration
- **Solution** : 
  - Vérifiez que le port `8080` est bien exposé
  - Vérifiez que le port n'est pas utilisé par un autre service
  - Vérifiez les logs pour voir les erreurs

---

## 📝 Recommandations

1. **Pour le développement** : Utilisez la Solution 1 (Upload ZIP)
2. **Pour la production** : Utilisez la Solution 2 (Repository Git)
3. **Pour des builds rapides** : Utilisez la Solution 3 (Registry Docker)

## 🔒 Sécurité

⚠️ **Important** :
- Ne commitez jamais les mots de passe dans Git
- Utilisez les variables d'environnement de Portainer
- Considérez l'utilisation de Docker Secrets pour les mots de passe
- Activez HTTPS si nécessaire
- Restreignez l'accès à Portainer

---

## 📞 Support

Si vous rencontrez des problèmes :
1. Vérifiez les logs dans Portainer
2. Vérifiez que tous les fichiers sont présents
3. Vérifiez la configuration de la chaîne de connexion
4. Vérifiez que SQL Server est accessible depuis le serveur Portainer

