using Microsoft.EntityFrameworkCore;
using TransportTeService.Api.Data;
using TransportTeService.Api.DTOs;
using TransportTeService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration de la base de données
var connectionString = builder.Configuration.GetConnectionString("chaine");
builder.Services.AddDbContext<TransportDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// Enregistrement des services
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<ILigneTransportService, LigneTransportService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger disponible en Development et Production (pour Docker)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport TE Service API v1");
    c.RoutePrefix = string.Empty; // Swagger UI à la racine
});

// HTTPS redirection uniquement si disponible
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors();

// ============================================
// ENDPOINTS TRANSPORTS
// ============================================

// GET /api/v1/transports
app.MapGet("/api/v1/transports", async (ITransportService transportService) =>
{
    try
    {
        var transports = await transportService.GetAllTransportsAsync();
        return Results.Ok(transports);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.ToString(),
            statusCode: 500,
            title: "Erreur lors de la récupération des transports"
        );
    }
})
.WithName("GetAllTransports")
.WithTags("Transports");

// GET /api/v1/transports/{id}
app.MapGet("/api/v1/transports/{id:guid}", async (Guid id, ITransportService transportService) =>
{
    var transport = await transportService.GetTransportByIdAsync(id);
    return transport != null ? Results.Ok(transport) : Results.NotFound();
})
.WithName("GetTransportById")
.WithTags("Transports");

// POST /api/v1/transports
app.MapPost("/api/v1/transports", async (CreateTransportRequest request, ITransportService transportService) =>
{
    try
    {
        var transport = await transportService.CreateTransportAsync(request);
        return Results.Created($"/api/v1/transports/{transport.Transport?.Id}", transport);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateTransport")
.WithTags("Transports");

// PUT /api/v1/transports/{id}
app.MapPut("/api/v1/transports/{id:guid}", async (Guid id, UpdateTransportRequest request, ITransportService transportService) =>
{
    var transport = await transportService.UpdateTransportAsync(id, request);
    return transport != null ? Results.Ok(transport) : Results.NotFound();
})
.WithName("UpdateTransport")
.WithTags("Transports");

// DELETE /api/v1/transports/{id}
app.MapDelete("/api/v1/transports/{id:guid}", async (Guid id, ITransportService transportService) =>
{
    var deleted = await transportService.DeleteTransportAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteTransport")
.WithTags("Transports");

// POST /api/v1/transports/{id}/submit
app.MapPost("/api/v1/transports/{id:guid}/submit", async (Guid id, ITransportService transportService) =>
{
    var submitted = await transportService.SubmitTransportAsync(id);
    if (!submitted)
    {
        return Results.BadRequest(new { error = "Le transport ne peut pas être soumis. Vérifiez qu'il est en statut 'Élaboré'." });
    }
    return Results.Ok(new { message = "Transport soumis avec succès", transportId = id });
})
.WithName("SubmitTransport")
.WithTags("Transports");

// ============================================
// ENDPOINTS LIGNES TRANSPORT
// ============================================

// GET /api/v1/lignes-transport
app.MapGet("/api/v1/lignes-transport", async (ILigneTransportService ligneTransportService) =>
{
    try
    {
        var lignes = await ligneTransportService.GetAllLignesTransportAsync();
        return Results.Ok(lignes);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.ToString(),
            statusCode: 500,
            title: "Erreur lors de la récupération des lignes de transport"
        );
    }
})
.WithName("GetAllLignesTransport")
.WithTags("LignesTransport");

// GET /api/v1/lignes-transport/{id}
app.MapGet("/api/v1/lignes-transport/{id:guid}", async (Guid id, ILigneTransportService ligneTransportService) =>
{
    var ligne = await ligneTransportService.GetLigneTransportByIdAsync(id);
    return ligne != null ? Results.Ok(ligne) : Results.NotFound();
})
.WithName("GetLigneTransportById")
.WithTags("LignesTransport");

// POST /api/v1/lignes-transport
app.MapPost("/api/v1/lignes-transport", async (CreateLigneTransportRequest request, ILigneTransportService ligneTransportService) =>
{
    try
    {
        var ligne = await ligneTransportService.CreateLigneTransportAsync(request);
        return Results.Created($"/api/v1/lignes-transport/{ligne.LigneTransport?.Id}", ligne);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateLigneTransport")
.WithTags("LignesTransport");

// PUT /api/v1/lignes-transport/{id}
app.MapPut("/api/v1/lignes-transport/{id:guid}", async (Guid id, UpdateLigneTransportRequest request, ILigneTransportService ligneTransportService) =>
{
    var ligne = await ligneTransportService.UpdateLigneTransportAsync(id, request);
    return ligne != null ? Results.Ok(ligne) : Results.NotFound();
})
.WithName("UpdateLigneTransport")
.WithTags("LignesTransport");

// DELETE /api/v1/lignes-transport/{id}
app.MapDelete("/api/v1/lignes-transport/{id:guid}", async (Guid id, ILigneTransportService ligneTransportService) =>
{
    var deleted = await ligneTransportService.DeleteLigneTransportAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteLigneTransport")
.WithTags("LignesTransport");

app.Run();
