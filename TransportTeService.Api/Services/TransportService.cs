using Microsoft.EntityFrameworkCore;
using TransportTeService.Api.Data;
using TransportTeService.Api.DTOs;
using TransportTeService.Api.Models;

namespace TransportTeService.Api.Services;

public class TransportService : ITransportService
{
    private readonly TransportDbContext _context;
    private readonly ILogger<TransportService> _logger;

    public TransportService(TransportDbContext context, ILogger<TransportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TransportResponse>> GetAllTransportsAsync()
    {
        var transports = await _context.Transports
            .Include(t => t.TransportEval)
            .ToListAsync();

        return transports.Select(MapToResponse);
    }

    public async Task<TransportResponse?> GetTransportByIdAsync(Guid id)
    {
        var transport = await _context.Transports
            .Include(t => t.TransportEval)
            .FirstOrDefaultAsync(t => t.Id == id);

        return transport != null ? MapToResponse(transport) : null;
    }

    public async Task<TransportResponse> CreateTransportAsync(CreateTransportRequest request, string? createdBy = null)
    {
        var transport = new Transport
        {
            Id = Guid.NewGuid(), // Génération automatique du GUID
            NoTransport = request.Transport?.NoTransport,
            NoBESC = request.Transport?.NoBESC,
            NoConnaissement = request.Transport?.NoConnaissement,
            Etat = 0, // Élaboré par défaut
            TransitaireId = request.Transport?.TransitaireId,
            TransporteurMaritime = request.Transport?.TransporteurMaritime,
            Consignee = request.Transport?.Consignee,
            Notifier = request.Transport?.Notifier,
            ManifesteArrivee = request.Transport?.ManifesteArrivee,
            IdentiqueCommande = request.Transport?.IdentiqueCommande ?? false,
            ManifesteDepart = request.Transport?.ManifesteDepart,
            NoVoyage = request.Transport?.NoVoyage,
            NavireNom = request.Transport?.NavireNom,
            ETA = request.Transport?.ETA,
            ATA = request.Transport?.ATA,
            Escale = request.Transport?.Escale ?? 0,
            PosteAttribue = request.Transport?.PosteAttribue,
            DateApurement = request.Transport?.DateApurement,
            NoAVE = request.Transport?.NoAVE,
            Module = request.Transport?.Module,
            Assurance = request.Transport?.Assurance,
            CreeLe = DateTime.UtcNow,
            CreePar = createdBy
        };

        _context.Transports.Add(transport);
        // Le GUID est généré automatiquement par EF Core avec ValueGeneratedOnAdd()
        // On peut utiliser transport.Id immédiatement

        if (request.TransportEval != null)
        {
            transport.TransportEval = new TransportEval
            {
                Id = Guid.NewGuid(), // Génération automatique du GUID
                TransportId = transport.Id,
                Partenaire = request.TransportEval.Partenaire,
                Incoterm = request.TransportEval.Incoterm,
                Devise = request.TransportEval.Devise,
                ModeConditionnement = request.TransportEval.ModeConditionnement,
                Transporteur = request.TransportEval.Transporteur,
                Expediteur = request.TransportEval.Expediteur,
                Consignee = request.TransportEval.Consignee,
                Notifier = request.TransportEval.Notifier,
                Commentaires = request.TransportEval.Commentaires,
                FretDevise = request.TransportEval.FretDevise,
                FretFCFA = request.TransportEval.FretFCFA,
                AssuranceDevise = request.TransportEval.AssuranceDevise,
                AssuranceFCFA = request.TransportEval.AssuranceFCFA,
                AutresChargesDevise = request.TransportEval.AutresChargesDevise,
                AutresChargesFCFA = request.TransportEval.AutresChargesFCFA,
                MasseBrute = request.TransportEval.MasseBrute,
                ValeurFCFA = request.TransportEval.ValeurFCFA,
                ValeurDevise = request.TransportEval.ValeurDevise,
                ModeTransport = request.TransportEval.ModeTransport,
                PaysDestination = request.TransportEval.PaysDestination,
                DestinationFinale = request.TransportEval.DestinationFinale,
                UniteDeChargement = request.TransportEval.UniteDeChargement,
                NbreUnite = request.TransportEval.NbreUnite,
                BureauDedouanement = request.TransportEval.BureauDedouanement,
                Assurance = request.TransportEval.Assurance,
                CreeLe = DateTime.UtcNow,
                CreePar = createdBy
            };
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Transport créé avec succès: {TransportId}", transport.Id);

        return MapToResponse(transport);
    }

    public async Task<TransportResponse?> UpdateTransportAsync(Guid id, UpdateTransportRequest request, string? modifiedBy = null)
    {
        var transport = await _context.Transports
            .Include(t => t.TransportEval)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transport == null)
            return null;

        // Mise à jour du transport
        if (request.Transport != null)
        {
            transport.NoTransport = request.Transport.NoTransport ?? transport.NoTransport;
            transport.NoBESC = request.Transport.NoBESC ?? transport.NoBESC;
            transport.NoConnaissement = request.Transport.NoConnaissement ?? transport.NoConnaissement;
            transport.TransitaireId = request.Transport.TransitaireId ?? transport.TransitaireId;
            transport.TransporteurMaritime = request.Transport.TransporteurMaritime ?? transport.TransporteurMaritime;
            transport.Consignee = request.Transport.Consignee ?? transport.Consignee;
            transport.Notifier = request.Transport.Notifier ?? transport.Notifier;
            transport.ManifesteArrivee = request.Transport.ManifesteArrivee ?? transport.ManifesteArrivee;
            transport.IdentiqueCommande = request.Transport.IdentiqueCommande;
            transport.ManifesteDepart = request.Transport.ManifesteDepart ?? transport.ManifesteDepart;
            transport.NoVoyage = request.Transport.NoVoyage ?? transport.NoVoyage;
            transport.NavireNom = request.Transport.NavireNom ?? transport.NavireNom;
            transport.ETA = request.Transport.ETA ?? transport.ETA;
            transport.ATA = request.Transport.ATA ?? transport.ATA;
            transport.Escale = request.Transport.Escale;
            transport.PosteAttribue = request.Transport.PosteAttribue ?? transport.PosteAttribue;
            transport.DateApurement = request.Transport.DateApurement ?? transport.DateApurement;
            transport.NoAVE = request.Transport.NoAVE ?? transport.NoAVE;
            transport.Module = request.Transport.Module ?? transport.Module;
            transport.Assurance = request.Transport.Assurance ?? transport.Assurance;
            transport.ModifieLe = DateTime.UtcNow;
            transport.ModifiePar = modifiedBy;
        }

        // Mise à jour ou création de TransportEval
        if (request.TransportEval != null)
        {
            if (transport.TransportEval == null)
            {
                transport.TransportEval = new TransportEval
                {
                    Id = Guid.NewGuid(), // Génération automatique du GUID
                    TransportId = transport.Id,
                    CreeLe = DateTime.UtcNow,
                    CreePar = modifiedBy
                };
            }

            var eval = transport.TransportEval;
            eval.Partenaire = request.TransportEval.Partenaire ?? eval.Partenaire;
            eval.Incoterm = request.TransportEval.Incoterm ?? eval.Incoterm;
            eval.Devise = request.TransportEval.Devise ?? eval.Devise;
            eval.ModeConditionnement = request.TransportEval.ModeConditionnement ?? eval.ModeConditionnement;
            eval.Transporteur = request.TransportEval.Transporteur ?? eval.Transporteur;
            eval.Expediteur = request.TransportEval.Expediteur ?? eval.Expediteur;
            eval.Consignee = request.TransportEval.Consignee ?? eval.Consignee;
            eval.Notifier = request.TransportEval.Notifier ?? eval.Notifier;
            eval.Commentaires = request.TransportEval.Commentaires ?? eval.Commentaires;
            eval.FretDevise = request.TransportEval.FretDevise ?? eval.FretDevise;
            eval.FretFCFA = request.TransportEval.FretFCFA ?? eval.FretFCFA;
            eval.AssuranceDevise = request.TransportEval.AssuranceDevise ?? eval.AssuranceDevise;
            eval.AssuranceFCFA = request.TransportEval.AssuranceFCFA ?? eval.AssuranceFCFA;
            eval.AutresChargesDevise = request.TransportEval.AutresChargesDevise ?? eval.AutresChargesDevise;
            eval.AutresChargesFCFA = request.TransportEval.AutresChargesFCFA ?? eval.AutresChargesFCFA;
            eval.MasseBrute = request.TransportEval.MasseBrute ?? eval.MasseBrute;
            eval.ValeurFCFA = request.TransportEval.ValeurFCFA ?? eval.ValeurFCFA;
            eval.ValeurDevise = request.TransportEval.ValeurDevise ?? eval.ValeurDevise;
            eval.ModeTransport = request.TransportEval.ModeTransport ?? eval.ModeTransport;
            eval.PaysDestination = request.TransportEval.PaysDestination ?? eval.PaysDestination;
            eval.DestinationFinale = request.TransportEval.DestinationFinale ?? eval.DestinationFinale;
            eval.UniteDeChargement = request.TransportEval.UniteDeChargement ?? eval.UniteDeChargement;
            eval.NbreUnite = request.TransportEval.NbreUnite ?? eval.NbreUnite;
            eval.BureauDedouanement = request.TransportEval.BureauDedouanement ?? eval.BureauDedouanement;
            eval.Assurance = request.TransportEval.Assurance ?? eval.Assurance;
            eval.ModifieLe = DateTime.UtcNow;
            eval.ModifiePar = modifiedBy;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Transport mis à jour avec succès: {TransportId}", transport.Id);

        return MapToResponse(transport);
    }

    public async Task<bool> DeleteTransportAsync(Guid id)
    {
        var transport = await _context.Transports.FindAsync(id);
        if (transport == null)
            return false;

        _context.Transports.Remove(transport);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Transport supprimé: {TransportId}", id);

        return true;
    }

    public async Task<bool> SubmitTransportAsync(Guid id)
    {
        var transport = await _context.Transports.FindAsync(id);
        if (transport == null)
            return false;

        // Vérifier que le transport est en statut "Élaboré" (0)
        if (transport.Etat != 0)
        {
            _logger.LogWarning("Impossible de soumettre le transport {TransportId}: statut actuel = {Etat}", id, transport.Etat);
            return false;
        }

        // Changer le statut à "AE (Attente Évaluation)" (1)
        transport.Etat = 1;
        transport.ModifieLe = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Transport soumis avec succès: {TransportId}, nouveau statut: AE", id);

        // TODO: Publier un événement/notification pour informer les autres microservices
        // await _eventPublisher.PublishAsync(new TransportSubmittedEvent { TransportId = id });

        return true;
    }

    private static TransportResponse MapToResponse(Transport transport)
    {
        return new TransportResponse
        {
            Transport = new TransportDto
            {
                Id = transport.Id,
                NoTransport = transport.NoTransport,
                NoBESC = transport.NoBESC,
                NoConnaissement = transport.NoConnaissement,
                Etat = transport.Etat,
                TransitaireId = transport.TransitaireId,
                TransporteurMaritime = transport.TransporteurMaritime,
                Consignee = transport.Consignee,
                Notifier = transport.Notifier,
                ManifesteArrivee = transport.ManifesteArrivee,
                IdentiqueCommande = transport.IdentiqueCommande,
                ManifesteDepart = transport.ManifesteDepart,
                NoVoyage = transport.NoVoyage,
                NavireNom = transport.NavireNom,
                ETA = transport.ETA,
                ATA = transport.ATA,
                Escale = transport.Escale,
                PosteAttribue = transport.PosteAttribue,
                DateApurement = transport.DateApurement,
                NoAVE = transport.NoAVE,
                Module = transport.Module,
                Assurance = transport.Assurance
            },
            TransportEval = transport.TransportEval != null ? new TransportEvalDto
            {
                Partenaire = transport.TransportEval.Partenaire,
                Incoterm = transport.TransportEval.Incoterm,
                Devise = transport.TransportEval.Devise,
                ModeConditionnement = transport.TransportEval.ModeConditionnement,
                Transporteur = transport.TransportEval.Transporteur,
                Expediteur = transport.TransportEval.Expediteur,
                Consignee = transport.TransportEval.Consignee,
                Notifier = transport.TransportEval.Notifier,
                Commentaires = transport.TransportEval.Commentaires,
                FretDevise = transport.TransportEval.FretDevise,
                FretFCFA = transport.TransportEval.FretFCFA,
                AssuranceDevise = transport.TransportEval.AssuranceDevise,
                AssuranceFCFA = transport.TransportEval.AssuranceFCFA,
                AutresChargesDevise = transport.TransportEval.AutresChargesDevise,
                AutresChargesFCFA = transport.TransportEval.AutresChargesFCFA,
                MasseBrute = transport.TransportEval.MasseBrute,
                ValeurFCFA = transport.TransportEval.ValeurFCFA,
                ValeurDevise = transport.TransportEval.ValeurDevise,
                ModeTransport = transport.TransportEval.ModeTransport,
                PaysDestination = transport.TransportEval.PaysDestination,
                DestinationFinale = transport.TransportEval.DestinationFinale,
                UniteDeChargement = transport.TransportEval.UniteDeChargement,
                NbreUnite = transport.TransportEval.NbreUnite,
                BureauDedouanement = transport.TransportEval.BureauDedouanement,
                Assurance = transport.TransportEval.Assurance
            } : null
        };
    }
}

