# Configuration avec le port 8400

## ✅ Ports configurés

- **Port externe (hôte)** : `8400`
- **Port interne (conteneur)** : `8080`

Le mapping est : `8400:8080` (port externe : port interne)

## 📝 Fichiers mis à jour

Tous les fichiers docker-compose ont été mis à jour :
- `docker-compose.yml`
- `docker-compose.prod.yml`
- `docker-compose.registry.yml`

## 🚀 Utilisation dans Portainer

### Avec Web Editor

Collez ce contenu dans le Web Editor :

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

### Variables d'environnement

- **Name** : `CONNECTION_STRING`
- **Value** : Votre chaîne de connexion SQL Server

## 🌐 Accès à l'API

Une fois déployé, l'API sera accessible sur :

- **Swagger UI** : `http://votre-serveur:8400`
- **API** : `http://votre-serveur:8400/api/v1/transports`

## ⚠️ Important

- Le conteneur écoute toujours sur le port **8080** en interne
- Le port **8400** est le port externe exposé par Docker
- Assurez-vous que le port 8400 n'est pas utilisé par un autre service
- Vérifiez les règles de pare-feu si nécessaire

