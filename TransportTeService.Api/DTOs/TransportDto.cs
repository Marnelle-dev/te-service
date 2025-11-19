namespace TransportTeService.Api.DTOs;

public class TransportDto
{
    public Guid? Id { get; set; }
    public string? NoTransport { get; set; }
    public string? NoBESC { get; set; }
    public string? NoConnaissement { get; set; }
    public int Etat { get; set; }
    public Guid? TransitaireId { get; set; }
    public string? TransporteurMaritime { get; set; }
    public string? Consignee { get; set; }
    public string? Notifier { get; set; }
    public string? ManifesteArrivee { get; set; }
    public bool IdentiqueCommande { get; set; }
    public string? ManifesteDepart { get; set; }
    public string? NoVoyage { get; set; }
    public string? NavireNom { get; set; }
    public DateTime? ETA { get; set; }
    public DateTime? ATA { get; set; }
    public int Escale { get; set; }
    public string? PosteAttribue { get; set; }
    public DateTime? DateApurement { get; set; }
    public string? NoAVE { get; set; }
    public string? Module { get; set; }
    public string? Assurance { get; set; }
}

public class TransportEvalDto
{
    public Guid? Partenaire { get; set; }
    public Guid? Incoterm { get; set; }
    public int? Devise { get; set; }
    public Guid? ModeConditionnement { get; set; }
    public string? Transporteur { get; set; }
    public string? Expediteur { get; set; }
    public string? Consignee { get; set; }
    public string? Notifier { get; set; }
    public string? Commentaires { get; set; }
    public decimal? FretDevise { get; set; }
    public decimal? FretFCFA { get; set; }
    public decimal? AssuranceDevise { get; set; }
    public decimal? AssuranceFCFA { get; set; }
    public decimal? AutresChargesDevise { get; set; }
    public decimal? AutresChargesFCFA { get; set; }
    public decimal? MasseBrute { get; set; }
    public long? ValeurFCFA { get; set; }
    public decimal? ValeurDevise { get; set; }
    public int? ModeTransport { get; set; }
    public int? PaysDestination { get; set; }
    public string? DestinationFinale { get; set; }
    public int? UniteDeChargement { get; set; }
    public int? NbreUnite { get; set; }
    public int? BureauDedouanement { get; set; }
    public int? Assurance { get; set; }
}

public class CreateTransportRequest
{
    public TransportDto? Transport { get; set; }
    public TransportEvalDto? TransportEval { get; set; }
}

public class UpdateTransportRequest
{
    public TransportDto? Transport { get; set; }
    public TransportEvalDto? TransportEval { get; set; }
}

public class TransportResponse
{
    public TransportDto? Transport { get; set; }
    public TransportEvalDto? TransportEval { get; set; }
}

