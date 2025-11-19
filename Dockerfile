# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY TransportTeService.Api/TransportTeService.Api.csproj TransportTeService.Api/
RUN dotnet restore TransportTeService.Api/TransportTeService.Api.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/TransportTeService.Api
RUN dotnet build TransportTeService.Api.csproj -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish TransportTeService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install dotnet-ef tools for migrations (accessible to all users)
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 9.0.0 && \
    mkdir -p /home/appuser/.dotnet/tools && \
    cp -r /root/.dotnet/tools/* /home/appuser/.dotnet/tools/ 2>/dev/null || true

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=publish /app/publish .

# Create a non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser && chown -R appuser:appuser /app

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check (using curl instead of wget)
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/swagger/index.html || exit 1

# Switch to non-root user
USER appuser

# Entry point
ENTRYPOINT ["dotnet", "TransportTeService.Api.dll"]

