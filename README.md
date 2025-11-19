# TransportTeService - Microservice de Gestion des Transports à l'Exportation

## Description

Ce microservice gère les opérations de transport liées aux exportations dans le système SEG (Système d'Échange et de Gestion). Il permet de créer et gérer les Transports à l'Export (TE) à partir des déclarations d'exportation (DE).

## Fonctionnalités

- Créer et gérer les Transports à l'Export (TE) partiels ou totaux
- Gérer les lignes de transport associées
- Soumettre un transport pour évaluation
- Gérer les statuts de transport (Élaboré, AE, Ouvert, Rejeté)
- Support des transports multimodaux (routier, fluvial, maritime, aérien)

## Technologies

- .NET 9.0
- ASP.NET Core Minimal API
- Entity Framework Core 9.0
- SQL Server

## Structure du Projet

```
TransportTeService.Api/
├── Models/              # Entités de domaine
│   ├── Transport.cs
│   ├── TransportEval.cs
│   ├── LigneTransport.cs
│   └── LigneTransportEval.cs
├── Data/                # Accès aux données
│   └── TransportDbContext.cs
├── Services/            # Services métier
│   ├── ITransportService.cs
│   ├── TransportService.cs
│   ├── ILigneTransportService.cs
│   └── LigneTransportService.cs
├── DTOs/                # Objets de transfert de données
│   ├── TransportDto.cs
│   └── LigneTransportDto.cs
└── Program.cs           # Configuration et endpoints
```

## Configuration

### Base de données

La chaîne de connexion est configurée dans `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TransportTeServiceDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Migrations

Pour créer la base de données, exécutez :

```bash
dotnet ef database update
```

## Endpoints API

### Transports

- `GET /api/v1/transports` - Liste tous les transports
- `GET /api/v1/transports/{id}` - Récupère un transport par ID
- `POST /api/v1/transports` - Crée un nouveau transport
- `PUT /api/v1/transports/{id}` - Met à jour un transport
- `DELETE /api/v1/transports/{id}` - Supprime un transport
- `POST /api/v1/transports/{id}/submit` - Soumet un transport pour évaluation

### Lignes de Transport

- `GET /api/v1/lignes-transport` - Liste toutes les lignes de transport
- `GET /api/v1/lignes-transport/{id}` - Récupère une ligne par ID
- `POST /api/v1/lignes-transport` - Crée une nouvelle ligne
- `PUT /api/v1/lignes-transport/{id}` - Met à jour une ligne
- `DELETE /api/v1/lignes-transport/{id}` - Supprime une ligne

## Statuts de Transport

- **0 - Élaboré** : Transport en cours de création/modification
- **1 - AE (Attente Évaluation)** : Transport soumis, en attente d'évaluation
- **2 - Ouvert** : Transport approuvé
- **3 - Rejeté** : Transport refusé

## Exemples d'utilisation

### Créer un transport

```json
POST /api/v1/transports
{
  "transport": {
    "noTransport": "TE-2025-001",
    "etat": 0,
    "transitaireId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "consignee": "Consignataire ABC",
    "manifesteDepart": "MAN-001"
  },
  "transportEval": {
    "partenaire": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "modeTransport": 2,
    "valeurFCFA": 5000000,
    "masseBrute": 1500.50
  }
}
```

### Soumettre un transport

```json
POST /api/v1/transports/{id}/submit
```

## Développement

### Prérequis

- .NET 9.0 SDK
- SQL Server (ou LocalDB)
- Visual Studio 2022 ou VS Code

### Exécution

```bash
dotnet run --project TransportTeService.Api
```

L'API sera accessible sur `https://localhost:5001` ou `http://localhost:5000`.

### Documentation Swagger

Une fois l'application démarrée, accédez à `/openapi/v1.json` pour la documentation OpenAPI.

## Notes

- Ce microservice est basé sur le modèle du microservice de transport à l'importation (TI)
- Les transports TE peuvent être de consommation partielle ou totale
- La soumission d'un transport change automatiquement son statut de "Élaboré" à "AE"

