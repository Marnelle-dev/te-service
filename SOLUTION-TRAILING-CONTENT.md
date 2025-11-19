# Solution : Erreur "trailing content" dans Portainer

## 🔍 Problème

```
YAMLSyntaxError: Document contains trailing content not separated by a ... or --- line
```

Cette erreur signifie qu'il y a du contenu après le document YAML, ou plusieurs documents YAML dans le même fichier.

## ✅ Solutions

### Solution 1 : Utiliser le nouveau ZIP ultra-propre

J'ai créé `portainer-deploy-ultra-clean.zip` avec un fichier docker-compose.yml généré proprement.

**Essayez ce nouveau ZIP** dans Portainer.

### Solution 2 : Utiliser le Web Editor directement

Au lieu d'uploader un ZIP, utilisez le **Web Editor** dans Portainer :

1. **Dans Portainer** → **Stacks** → **Add stack**
2. **Choisissez "Web editor"**
3. **Collez ce contenu EXACT** (copiez-collez directement) :

```yaml
version: '3.8'
services:
  transport-te-api:
    build:
      context: .
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

4. **Important** : Assurez-vous qu'il n'y a **pas de lignes vides supplémentaires** à la fin
5. **Variables d'environnement** : Ajoutez `CONNECTION_STRING`
6. **Uploadez les fichiers sources** (Dockerfile, .dockerignore, TransportTeService.Api/) séparément si possible

### Solution 3 : Vérifier le contenu du ZIP

Le problème peut venir du fait qu'il y a plusieurs fichiers YAML dans le ZIP. Vérifiez :

1. **Décompressez le ZIP** localement
2. **Vérifiez qu'il n'y a qu'un seul fichier docker-compose.yml**
3. **Vérifiez qu'il n'y a pas de fichiers .yml ou .yaml supplémentaires**

### Solution 4 : Créer un ZIP minimal

Si le problème persiste, créez un ZIP avec seulement les fichiers essentiels :

1. Dockerfile
2. .dockerignore
3. docker-compose.yml (le fichier propre)
4. TransportTeService.Api/ (le dossier)

**Sans aucun autre fichier YAML**.

## 📝 Fichier docker-compose.yml propre

Le fichier doit être exactement comme ceci, **sans lignes vides à la fin** :

```yaml
version: '3.8'
services:
  transport-te-api:
    build:
      context: .
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

## 🎯 Recommandation

**Utilisez le Web Editor** dans Portainer et collez le contenu ci-dessus directement. C'est la méthode la plus fiable pour éviter les problèmes d'encodage ou de formatage.

Ensuite, vous devrez uploader les fichiers sources (Dockerfile, .dockerignore, TransportTeService.Api/) séparément, ou utiliser l'option "Repository" avec Git.

