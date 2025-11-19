# Guide de déploiement rapide

## 🚀 Déploiement sur Portainer

### Étape 1 : Préparation

Assurez-vous d'avoir tous les fichiers suivants dans votre projet :
- `Dockerfile`
- `docker-compose.prod.yml`
- `.dockerignore`
- Tous les fichiers sources de l'API

### Étape 2 : Configuration dans Portainer

1. **Connectez-vous à Portainer**
2. **Allez dans "Stacks"** (menu de gauche)
3. **Cliquez sur "Add stack"**
4. **Donnez un nom** : `transport-te-service`
5. **Copiez le contenu de `docker-compose.prod.yml`** dans l'éditeur

### Étape 3 : Configuration des variables d'environnement

Dans l'éditeur Portainer, remplacez `${CONNECTION_STRING}` par votre chaîne de connexion réelle :

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ASPNETCORE_URLS=http://+:8080
  - ConnectionStrings__chaine=Server=DSI-SAGDDI-P18;Database=GUOT_TE_PROD;User Id=sa;Password=VotreMotDePasse;TrustServerCertificate=True;Encrypt=True;Connection Timeout=30;
```

### Étape 4 : Déploiement

1. **Cliquez sur "Deploy the stack"**
2. **Attendez que le build se termine** (peut prendre quelques minutes)
3. **Vérifiez les logs** pour voir si l'API démarre correctement

### Étape 5 : Application des migrations

Une fois l'API démarrée, appliquez les migrations :

```bash
# Dans Portainer, allez dans "Containers"
# Trouvez le conteneur "transport-te-api-prod"
# Cliquez sur "Console" ou "Exec" 
# Exécutez :

dotnet ef database update --project /app/TransportTeService.Api.csproj
```

**Note**: Si dotnet-ef n'est pas disponible, vous pouvez l'exécuter depuis votre machine locale en pointant vers la base de données distante.

### Étape 6 : Vérification

1. **Accédez à Swagger** : `http://votre-serveur:8080`
2. **Testez un endpoint** : Essayez `GET /api/v1/transports`

## 🔧 Déploiement local (test)

Pour tester localement avant de déployer sur Portainer :

```bash
# Construire l'image
docker build -t transport-te-api .

# Démarrer avec docker-compose
docker-compose -f docker-compose.prod.yml up -d --build

# Voir les logs
docker logs -f transport-te-api-prod

# Arrêter
docker-compose -f docker-compose.prod.yml down
```

## 📋 Checklist de déploiement

- [ ] Tous les fichiers Docker sont présents
- [ ] La chaîne de connexion SQL Server est correcte
- [ ] Les migrations ont été appliquées
- [ ] L'API répond sur le port 8080
- [ ] Swagger est accessible
- [ ] Les health checks passent

## ❓ Problèmes courants

### L'API ne démarre pas
- Vérifiez les logs : `docker logs transport-te-api-prod`
- Vérifiez que la base de données est accessible
- Vérifiez les variables d'environnement

### Erreur de connexion à la base de données
- Vérifiez la chaîne de connexion
- Vérifiez que SQL Server accepte les connexions depuis Docker
- Vérifiez les règles de pare-feu

### Les migrations ne s'appliquent pas
- Assurez-vous d'avoir les outils EF Core installés
- Exécutez les migrations manuellement depuis votre machine locale si nécessaire

