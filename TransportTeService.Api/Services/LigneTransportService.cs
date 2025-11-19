using Microsoft.EntityFrameworkCore;
using TransportTeService.Api.Data;
using TransportTeService.Api.DTOs;
using TransportTeService.Api.Models;

namespace TransportTeService.Api.Services;

public class LigneTransportService : ILigneTransportService
{
    private readonly TransportDbContext _context;
    private readonly ILogger<LigneTransportService> _logger;

    public LigneTransportService(TransportDbContext context, ILogger<LigneTransportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<LigneTransportResponse>> GetAllLignesTransportAsync()
    {
        var lignes = await _context.LignesTransports
            .Include(l => l.LigneTransportEval)
            .ToListAsync();

        return lignes.Select(MapToResponse);
    }

    public async Task<LigneTransportResponse?> GetLigneTransportByIdAsync(Guid id)
    {
        var ligne = await _context.LignesTransports
            .Include(l => l.LigneTransportEval)
            .FirstOrDefaultAsync(l => l.Id == id);

        return ligne != null ? MapToResponse(ligne) : null;
    }

    public async Task<LigneTransportResponse> CreateLigneTransportAsync(CreateLigneTransportRequest request, string? createdBy = null)
    {
        if (request.LigneTransport == null)
        {
            throw new ArgumentNullException(nameof(request.LigneTransport), "La ligne de transport est requise.");
        }

        // Vérifier que le transport existe
        var transportExists = await _context.Transports.AnyAsync(t => t.Id == request.LigneTransport.TransportId);
        if (!transportExists)
        {
            throw new InvalidOperationException($"Le transport avec l'ID {request.LigneTransport.TransportId} n'existe pas.");
        }

        var ligne = new LigneTransport
        {
            Id = Guid.NewGuid(), // Génération automatique du GUID
            TransportId = request.LigneTransport.TransportId,
            NoLigne = request.LigneTransport.NoLigne,
            LigneCommande = request.LigneTransport.LigneCommande,
            LivraisonPartielle = request.LigneTransport.LivraisonPartielle,
            Declaration = request.LigneTransport.Declaration,
            CreeLe = DateTime.UtcNow,
            CreePar = createdBy
        };

        _context.LignesTransports.Add(ligne);
        // Le GUID est généré automatiquement par EF Core avec ValueGeneratedOnAdd()
        // On peut utiliser ligne.Id immédiatement

        if (request.LigneTransportEval != null)
        {
            ligne.LigneTransportEval = new LigneTransportEval
            {
                Id = Guid.NewGuid(), // Génération automatique du GUID
                LigneTransportId = ligne.Id,
                Partenaire = request.LigneTransportEval.Partenaire,
                PositionTarifaire = request.LigneTransportEval.PositionTarifaire,
                Designation = request.LigneTransportEval.Designation,
                Marque = request.LigneTransportEval.Marque,
                Colisage = request.LigneTransportEval.Colisage,
                MasseBrute = request.LigneTransportEval.MasseBrute,
                MasseNette = request.LigneTransportEval.MasseNette,
                Volume = request.LigneTransportEval.Volume,
                Quantite = request.LigneTransportEval.Quantite,
                Devise = request.LigneTransportEval.Devise,
                PrixUnitaire = request.LigneTransportEval.PrixUnitaire,
                ValeurDevise = request.LigneTransportEval.ValeurDevise,
                UniteStatistique = request.LigneTransportEval.UniteStatistique,
                ModeConditionnement = request.LigneTransportEval.ModeConditionnement,
                Nombre = request.LigneTransportEval.Nombre,
                NumerosCollis = request.LigneTransportEval.NumerosCollis,
                MarquesCollis = request.LigneTransportEval.MarquesCollis,
                Commentaires = request.LigneTransportEval.Commentaires,
                PaysOrigine = request.LigneTransportEval.PaysOrigine,
                ValeurFCFA = request.LigneTransportEval.ValeurFCFA,
                CreeLe = DateTime.UtcNow,
                CreePar = createdBy
            };
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Ligne de transport créée avec succès: {LigneId}", ligne.Id);

        return MapToResponse(ligne);
    }

    public async Task<LigneTransportResponse?> UpdateLigneTransportAsync(Guid id, UpdateLigneTransportRequest request, string? modifiedBy = null)
    {
        var ligne = await _context.LignesTransports
            .Include(l => l.LigneTransportEval)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (ligne == null)
            return null;

        // Mise à jour de la ligne
        if (request.LigneTransport != null)
        {
            ligne.NoLigne = request.LigneTransport.NoLigne ?? ligne.NoLigne;
            ligne.LigneCommande = request.LigneTransport.LigneCommande ?? ligne.LigneCommande;
            ligne.LivraisonPartielle = request.LigneTransport.LivraisonPartielle;
            ligne.Declaration = request.LigneTransport.Declaration ?? ligne.Declaration;
            ligne.ModifieLe = DateTime.UtcNow;
            ligne.ModifiePar = modifiedBy;
        }

        // Mise à jour ou création de LigneTransportEval
        if (request.LigneTransportEval != null)
        {
            if (ligne.LigneTransportEval == null)
            {
                ligne.LigneTransportEval = new LigneTransportEval
                {
                    Id = Guid.NewGuid(), // Génération automatique du GUID
                    LigneTransportId = ligne.Id,
                    CreeLe = DateTime.UtcNow,
                    CreePar = modifiedBy
                };
            }

            var eval = ligne.LigneTransportEval;
            eval.Partenaire = request.LigneTransportEval.Partenaire ?? eval.Partenaire;
            eval.PositionTarifaire = request.LigneTransportEval.PositionTarifaire ?? eval.PositionTarifaire;
            eval.Designation = request.LigneTransportEval.Designation ?? eval.Designation;
            eval.Marque = request.LigneTransportEval.Marque ?? eval.Marque;
            eval.Colisage = request.LigneTransportEval.Colisage ?? eval.Colisage;
            eval.MasseBrute = request.LigneTransportEval.MasseBrute ?? eval.MasseBrute;
            eval.MasseNette = request.LigneTransportEval.MasseNette ?? eval.MasseNette;
            eval.Volume = request.LigneTransportEval.Volume ?? eval.Volume;
            eval.Quantite = request.LigneTransportEval.Quantite ?? eval.Quantite;
            eval.Devise = request.LigneTransportEval.Devise ?? eval.Devise;
            eval.PrixUnitaire = request.LigneTransportEval.PrixUnitaire ?? eval.PrixUnitaire;
            eval.ValeurDevise = request.LigneTransportEval.ValeurDevise ?? eval.ValeurDevise;
            eval.UniteStatistique = request.LigneTransportEval.UniteStatistique ?? eval.UniteStatistique;
            eval.ModeConditionnement = request.LigneTransportEval.ModeConditionnement ?? eval.ModeConditionnement;
            eval.Nombre = request.LigneTransportEval.Nombre ?? eval.Nombre;
            eval.NumerosCollis = request.LigneTransportEval.NumerosCollis ?? eval.NumerosCollis;
            eval.MarquesCollis = request.LigneTransportEval.MarquesCollis ?? eval.MarquesCollis;
            eval.Commentaires = request.LigneTransportEval.Commentaires ?? eval.Commentaires;
            eval.PaysOrigine = request.LigneTransportEval.PaysOrigine ?? eval.PaysOrigine;
            eval.ValeurFCFA = request.LigneTransportEval.ValeurFCFA ?? eval.ValeurFCFA;
            eval.ModifieLe = DateTime.UtcNow;
            eval.ModifiePar = modifiedBy;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Ligne de transport mise à jour avec succès: {LigneId}", ligne.Id);

        return MapToResponse(ligne);
    }

    public async Task<bool> DeleteLigneTransportAsync(Guid id)
    {
        var ligne = await _context.LignesTransports.FindAsync(id);
        if (ligne == null)
            return false;

        _context.LignesTransports.Remove(ligne);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Ligne de transport supprimée: {LigneId}", id);

        return true;
    }

    private static LigneTransportResponse MapToResponse(LigneTransport ligne)
    {
        return new LigneTransportResponse
        {
            LigneTransport = new LigneTransportDto
            {
                Id = ligne.Id,
                NoLigne = ligne.NoLigne,
                TransportId = ligne.TransportId,
                LigneCommande = ligne.LigneCommande,
                LivraisonPartielle = ligne.LivraisonPartielle,
                Declaration = ligne.Declaration
            },
            LigneTransportEval = ligne.LigneTransportEval != null ? new LigneTransportEvalDto
            {
                Partenaire = ligne.LigneTransportEval.Partenaire,
                PositionTarifaire = ligne.LigneTransportEval.PositionTarifaire,
                Designation = ligne.LigneTransportEval.Designation,
                Marque = ligne.LigneTransportEval.Marque,
                Colisage = ligne.LigneTransportEval.Colisage,
                MasseBrute = ligne.LigneTransportEval.MasseBrute,
                MasseNette = ligne.LigneTransportEval.MasseNette,
                Volume = ligne.LigneTransportEval.Volume,
                Quantite = ligne.LigneTransportEval.Quantite,
                Devise = ligne.LigneTransportEval.Devise,
                PrixUnitaire = ligne.LigneTransportEval.PrixUnitaire,
                ValeurDevise = ligne.LigneTransportEval.ValeurDevise,
                UniteStatistique = ligne.LigneTransportEval.UniteStatistique,
                ModeConditionnement = ligne.LigneTransportEval.ModeConditionnement,
                Nombre = ligne.LigneTransportEval.Nombre,
                NumerosCollis = ligne.LigneTransportEval.NumerosCollis,
                MarquesCollis = ligne.LigneTransportEval.MarquesCollis,
                Commentaires = ligne.LigneTransportEval.Commentaires,
                PaysOrigine = ligne.LigneTransportEval.PaysOrigine,
                ValeurFCFA = ligne.LigneTransportEval.ValeurFCFA
            } : null
        };
    }
}

