# Déploiement Docker - Transport TE Service

Ce guide explique comment déployer le microservice Transport TE sur Docker et Portainer.

## Prérequis

- Docker Desktop ou Docker Engine
- Portainer (optionnel, pour l'interface graphique)
- SQL Server (local ou distant)

## Structure des fichiers Docker

- `Dockerfile` : Configuration pour construire l'image de l'API
- `docker-compose.yml` : Configuration complète avec API et SQL Server local
- `docker-compose.prod.yml` : Configuration pour production (sans SQL Server local)
- `.dockerignore` : Fichiers exclus de la construction Docker

## Options de déploiement

### Option 1 : Déploiement avec SQL Server externe (Production)

C'est la configuration recommandée si vous utilisez un serveur SQL Server externe (comme `DSI-SAGDDI-P18`).

1. **Créer un fichier `.env`** (optionnel, pour les variables d'environnement) :
```env
CONNECTION_STRING=Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=VotreMotDePasse;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;
```

2. **Construire et démarrer** :
```bash
docker-compose -f docker-compose.prod.yml up -d --build
```

### Option 2 : Déploiement complet avec SQL Server local (Développement)

Cette option crée un conteneur SQL Server local pour les tests.

1. **Construire et démarrer** :
```bash
docker-compose up -d --build
```

2. **Appliquer les migrations** (une fois la base de données prête) :
```bash
docker exec -it transport-te-api dotnet ef database update --project /app/TransportTeService.Api.csproj
```

**Note**: Pour utiliser cette option, vous devez modifier la chaîne de connexion dans `docker-compose.yml` pour pointer vers `db` au lieu de `DSI-SAGDDI-P18` :
```yaml
ConnectionStrings__chaine=Server=db;Database=GUOT_TE_PROD;User Id=sa;Password=Marnelle@1234567890;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;
```

## Déploiement sur Portainer

### Méthode 1 : Via Stack (Recommandé)

1. **Connectez-vous à Portainer**
2. **Allez dans "Stacks"** (menu de gauche)
3. **Cliquez sur "Add stack"**
4. **Nommez votre stack** : `transport-te-service`
5. **Copiez le contenu de `docker-compose.prod.yml`** dans l'éditeur
6. **Configurez les variables d'environnement** :
   - `CONNECTION_STRING` : Votre chaîne de connexion SQL Server
7. **Cliquez sur "Deploy the stack"**

### Méthode 2 : Via Docker Compose

1. **Uploadez les fichiers** sur le serveur Portainer :
   - `Dockerfile`
   - `docker-compose.prod.yml`
   - Tous les fichiers du projet

2. **Connectez-vous au serveur via SSH** ou utilisez la console Portainer

3. **Naviguez vers le dossier du projet**

4. **Construisez et démarrez** :
```bash
docker-compose -f docker-compose.prod.yml up -d --build
```

## Configuration

### Variables d'environnement

Les principales variables d'environnement configurées dans `docker-compose.yml` :

- `ASPNETCORE_ENVIRONMENT` : `Production`
- `ASPNETCORE_URLS` : `http://+:8080`
- `ConnectionStrings__chaine` : Chaîne de connexion SQL Server

### Ports

- **API** : `8080` (peut être modifié dans `docker-compose.yml`)
- **SQL Server** (si utilisé localement) : `1433`

### Health Checks

L'API inclut un health check qui vérifie la disponibilité de Swagger :
- Intervalle : 30 secondes
- Timeout : 10 secondes
- Période de démarrage : 40 secondes
- Tentatives : 3

## Application des migrations

Les migrations EF Core doivent être appliquées manuellement après le premier démarrage :

```bash
# Si l'API est en conteneur
docker exec -it transport-te-api dotnet ef database update --project /app/TransportTeService.Api.csproj

# Ou depuis l'extérieur du conteneur
docker exec -it transport-te-api bash
cd /app
dotnet ef database update
```

## Vérification du déploiement

1. **Vérifier les conteneurs** :
```bash
docker ps
```

2. **Vérifier les logs** :
```bash
docker logs transport-te-api
```

3. **Accéder à Swagger** :
   - Ouvrez votre navigateur : `http://votre-serveur:8080`
   - Vous devriez voir l'interface Swagger

4. **Tester un endpoint** :
   - Essayez `GET /api/v1/transports` depuis Swagger

## Troubleshooting

### L'API ne démarre pas

- Vérifiez les logs : `docker logs transport-te-api`
- Vérifiez que la base de données est accessible depuis le conteneur
- Vérifiez les variables d'environnement

### Erreur de connexion à la base de données

- Vérifiez que SQL Server accepte les connexions depuis Docker
- Vérifiez la chaîne de connexion
- Vérifiez les pare-feu et règles de réseau

### Les migrations ne s'appliquent pas

- Exécutez manuellement les migrations (voir section ci-dessus)
- Vérifiez que les outils EF Core sont installés dans le conteneur

## Maintenance

### Mise à jour de l'API

1. **Arrêtez les conteneurs** :
```bash
docker-compose down
```

2. **Reconstruisez** :
```bash
docker-compose -f docker-compose.prod.yml up -d --build
```

### Sauvegarde de la base de données

Si vous utilisez SQL Server local :
```bash
docker exec transport-te-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P VotreMotDePasse -Q "BACKUP DATABASE GUOT_TE_PROD TO DISK='/var/opt/mssql/backup/backup.bak'"
```

### Logs

Consulter les logs en temps réel :
```bash
docker logs -f transport-te-api
```

## Sécurité

⚠️ **Important** : Pour la production, considérez :

1. **Ne pas exposer les mots de passe** dans les fichiers Docker Compose
2. **Utiliser Docker Secrets** ou des variables d'environnement sécurisées
3. **Configurer HTTPS** si nécessaire
4. **Restreindre les politiques CORS**
5. **Ajouter une authentification** à l'API
6. **Configurer des limites de ressources** pour les conteneurs

## Support

Pour toute question ou problème, consultez les logs ou contactez l'équipe de développement.

