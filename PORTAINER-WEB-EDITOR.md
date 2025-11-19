# Solution : Utiliser Web Editor dans Portainer

## ✅ Méthode recommandée : Web Editor + Upload fichiers séparés

### Étape 1 : Dans Portainer Web Editor

1. **Allez dans Portainer** → **Stacks** → **Add stack**
2. **Nom de la stack** : `transport-te-service`
3. **Choisissez "Web editor"** (pas "Upload")
4. **Collez ce contenu EXACT** dans l'éditeur :

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
```

### Étape 2 : Configurer la variable d'environnement

Dans la section **"Environment variables"** :

- **Name** : `CONNECTION_STRING`
- **Value** : 
  ```
  Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=VotreMotDePasse;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;
  ```

### Étape 3 : Uploader les fichiers sources

**Option A : Si Portainer a une section "Additional files"**

Cherchez une section "Additional files", "Build context", ou "Upload files" dans l'interface Portainer et uploadez :
- `Dockerfile`
- `.dockerignore`  
- Le dossier `TransportTeService.Api/` (ou un ZIP de ce dossier)

**Option B : Si Portainer n'a pas cette option**

Vous devrez uploader les fichiers sur le serveur Portainer via SSH/SCP, puis modifier le `context` dans docker-compose.yml pour pointer vers le chemin absolu.

### Étape 4 : Déployer

Cliquez sur **"Deploy the stack"**

---

## 🔄 Alternative : Utiliser Repository Git

Si le Web Editor ne fonctionne pas, utilisez Git :

1. **Créez un dépôt Git** (GitHub, GitLab, etc.)
2. **Poussez votre projet** sur le dépôt
3. **Dans Portainer** → **Stacks** → **Add stack**
4. **Choisissez "Repository"**
5. **Configurez** :
   - **Repository URL** : URL de votre dépôt
   - **Repository reference** : `main` ou `master`
   - **Compose path** : `docker-compose.yml`
6. **Variables d'environnement** : Ajoutez `CONNECTION_STRING`
7. **Déployez**

---

## 📝 Fichier docker-compose.yml à utiliser

Le fichier `docker-compose.yml` dans le projet est maintenant simplifié et devrait fonctionner. Il a été testé localement et est valide.

Si vous continuez à avoir des erreurs avec l'upload ZIP, utilisez le **Web Editor** ou **Repository Git**.

