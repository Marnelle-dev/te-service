# Solution pour l'erreur YAML dans Portainer

## Problème
```
YAMLSyntaxError: All collection items must start at the same column
```

## Solution 1 : Utiliser Web Editor (Recommandé)

Au lieu d'uploader le ZIP, utilisez le **Web Editor** dans Portainer :

1. **Dans Portainer** → **Stacks** → **Add stack**
2. **Choisissez "Web editor"** (pas "Upload")
3. **Collez ce contenu exact** :

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

4. **Dans "Environment variables"**, ajoutez :
   - **Name** : `CONNECTION_STRING`
   - **Value** : Votre chaîne de connexion SQL Server

5. **Important** : Vous devez ensuite uploader les fichiers sources séparément :
   - Cherchez une section "Additional files" ou "Upload files" dans Portainer
   - Uploadez :
     - `Dockerfile`
     - `.dockerignore`
     - Le dossier `TransportTeService.Api/` (ou créez un ZIP avec seulement ces fichiers)

## Solution 2 : Vérifier le fichier dans le ZIP

Le problème peut venir du fait que le fichier dans le ZIP a des caractères invisibles ou un mauvais encodage.

### Test local du YAML

Testez le fichier YAML localement avant de l'uploader :

```bash
# Sur Windows PowerShell
docker-compose -f docker-compose.yml config
```

Si cette commande fonctionne, le YAML est valide.

## Solution 3 : Créer un ZIP manuel

1. **Créez un nouveau dossier** : `portainer-deploy-manual`
2. **Copiez ces fichiers** dans ce dossier :
   - `Dockerfile`
   - `.dockerignore`
   - `docker-compose.yml` (la version simplifiée)
   - Tout le dossier `TransportTeService.Api/` (sans bin/obj)
3. **Créez un ZIP** de ce dossier
4. **Uploadez le ZIP** dans Portainer

## Solution 4 : Utiliser Repository Git

Si vous avez accès à Git, c'est la meilleure solution :

1. **Poussez votre projet** sur GitHub/GitLab
2. **Dans Portainer** → **Stacks** → **Add stack**
3. **Choisissez "Repository"**
4. **Entrez l'URL du dépôt**
5. **Compose path** : `docker-compose.yml`
6. **Configurez CONNECTION_STRING** dans les variables d'environnement

## Vérification de la syntaxe YAML

Le fichier `docker-compose.yml` doit :
- Utiliser uniquement des **espaces** (pas de tabs)
- Avoir une **indentation de 2 espaces** par niveau
- Tous les éléments de liste doivent être **alignés** à la même colonne

### Exemple correct :
```yaml
environment:
  - VAR1=value1
  - VAR2=value2
```

### Exemple incorrect :
```yaml
environment:
  - VAR1=value1
    - VAR2=value2  # ❌ Mauvaise indentation
```

## Fichier docker-compose.yml minimal (test)

Utilisez cette version ultra-minimale pour tester :

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

Si cette version fonctionne, vous pouvez ensuite ajouter le healthcheck.

