# Diagnostic du problème Portainer

## Problème identifié

L'erreur `failed to read dockerfile: open Dockerfile: no such file or directory` signifie que Portainer ne trouve pas le Dockerfile dans le contexte de build.

## Causes possibles

1. **Le ZIP n'est pas décompressé au bon endroit** par Portainer
2. **Le contexte de build (`context: .`) ne pointe pas vers le bon répertoire**
3. **Portainer cherche le Dockerfile dans un répertoire différent**

## Solution : Vérifier la structure du ZIP

Le ZIP doit avoir cette structure EXACTE :

```
portainer-deploy.zip
├── Dockerfile                    ← À la racine
├── .dockerignore                 ← À la racine
├── docker-compose.yml            ← À la racine
└── TransportTeService.Api/       ← Dossier à la racine
    ├── TransportTeService.Api.csproj
    ├── Program.cs
    └── ...
```

## Vérification manuelle

1. **Décompressez le ZIP localement** pour vérifier la structure
2. **Vérifiez que le Dockerfile est à la racine** (pas dans un sous-dossier)
3. **Vérifiez que docker-compose.yml est à la racine**

## Solution alternative : Utiliser un chemin absolu

Si Portainer décompresse dans un répertoire spécifique, vous pouvez essayer de modifier le `context` dans docker-compose.yml.

Mais d'abord, testons si le problème vient de la structure du ZIP.

