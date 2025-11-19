#!/bin/bash
set -e

echo "Starting Transport TE Service..."

# Attendre que la base de données soit prête
echo "Waiting for database to be ready..."
until dotnet ef database update --no-build --project TransportTeService.Api/TransportTeService.Api.csproj || true; do
  echo "Database is unavailable - sleeping"
  sleep 2
done

echo "Database is ready!"
echo "Starting application..."

# Démarrer l'application
exec dotnet TransportTeService.Api.dll

