# Guide Portainer - Déploiement Web Editor

## 🎯 Méthode recommandée : Web Editor + Variables d'environnement

### Étape 1 : Dans l'éditeur Web de Portainer

Collez ce contenu dans l'éditeur :

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

### Étape 2 : Configurer la variable d'environnement

Dans la section **"Environment variables"** :

1. Cliquez sur **"Switch to advanced mode"** si vous voulez copier plusieurs variables
2. Ajoutez une nouvelle variable :
   - **Name** : `CONNECTION_STRING`
   - **Value** : `Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=Marnelle@1234567890;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;`

⚠️ **Important** : Remplacez `Marnelle@1234567890` par votre vrai mot de passe SQL Server.

### Étape 3 : Uploader les fichiers sources

**Le problème** : Portainer avec Web editor nécessite que les fichiers sources (Dockerfile, code, etc.) soient disponibles.

**Solutions** :

#### Solution A : Si Portainer a une option "Upload files"

Cherchez un bouton "Upload files", "Additional files" ou "Build context" dans l'interface et uploadez :

- `Dockerfile` (à la racine)
- `.dockerignore` (à la racine)
- Tout le dossier `TransportTeService.Api/`

#### Solution B : Utiliser la méthode "Upload" avec ZIP

1. **Créez un ZIP** avec cette structure :
```
transport-te-service.zip
├── Dockerfile
├── .dockerignore
├── docker-compose.yml  (renommez docker-compose.prod.yml)
└── TransportTeService.Api/
    ├── TransportTeService.Api.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Production.json
    ├── Data/
    │   └── TransportDbContext.cs
    ├── DTOs/
    │   ├── TransportDto.cs
    │   └── LigneTransportDto.cs
    ├── Models/
    │   ├── Transport.cs
    │   ├── TransportEval.cs
    │   ├── LigneTransport.cs
    │   └── LigneTransportEval.cs
    ├── Services/
    │   ├── ITransportService.cs
    │   ├── TransportService.cs
    │   ├── ILigneTransportService.cs
    │   └── LigneTransportService.cs
    └── Migrations/
        └── (tous les fichiers de migrations)
```

2. Dans Portainer :
   - Choisissez **"Upload"** au lieu de "Web editor"
   - Uploadez le ZIP
   - Portainer le décompressera automatiquement

#### Solution C : Utiliser un dépôt Git (si disponible)

1. Poussez votre projet sur GitHub/GitLab/Git
2. Dans Portainer, choisissez **"Repository"**
3. Entrez l'URL de votre dépôt
4. Spécifiez la branche (généralement `main` ou `master`)
5. Spécifiez le chemin vers `docker-compose.prod.yml`
6. Configurez les variables d'environnement comme indiqué ci-dessus

## 📋 Checklist avant déploiement

- [ ] Docker-compose collé dans l'éditeur
- [ ] Variable `CONNECTION_STRING` définie dans "Environment variables"
- [ ] Fichiers sources uploadés (Dockerfile, .dockerignore, TransportTeService.Api/)
- [ ] Nom de la stack défini (ex: `transport-te-service`)

## 🚀 Déploiement

1. Vérifiez que tout est en place
2. Cliquez sur **"Deploy the stack"**
3. Attendez que le build se termine (peut prendre 5-10 minutes la première fois)
4. Vérifiez les logs pour voir si l'API démarre correctement

## ✅ Après le déploiement

### 1. Appliquer les migrations

Une fois l'API démarrée, appliquez les migrations :

```bash
# Dans Portainer :
# 1. Allez dans "Containers"
# 2. Trouvez "transport-te-api-prod"
# 3. Cliquez sur "Console" ou "Exec"
# 4. Exécutez :

dotnet ef database update --project /app/TransportTeService.Api.csproj
```

**OU** depuis votre machine locale :

```bash
dotnet ef database update --project TransportTeService.Api/TransportTeService.Api.csproj
```

### 2. Vérifier que l'API fonctionne

1. Accédez à Swagger : `http://votre-serveur-portainer:8080`
2. Testez un endpoint : `GET /api/v1/transports`

## 🐛 Problèmes courants

### Erreur : "Dockerfile not found"
- Vérifiez que le Dockerfile est bien uploadé à la racine
- Vérifiez que le chemin dans `dockerfile: Dockerfile` est correct

### Erreur : "context: ." not found
- Tous les fichiers sources doivent être disponibles
- Vérifiez que `TransportTeService.Api/` est bien présent

### Erreur de connexion à la base de données
- Vérifiez que la variable `CONNECTION_STRING` est bien définie
- Vérifiez que SQL Server accepte les connexions depuis Docker
- Vérifiez les règles de pare-feu

## 📝 Notes

- Le build prendra du temps la première fois (téléchargement des images .NET)
- Les logs sont visibles dans Portainer → Containers → Logs
- Vous pouvez redémarrer le conteneur depuis Portainer si nécessaire

