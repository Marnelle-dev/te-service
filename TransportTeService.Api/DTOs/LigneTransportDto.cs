namespace TransportTeService.Api.DTOs;

public class LigneTransportDto
{
    public Guid? Id { get; set; }
    public string? NoLigne { get; set; }
    public Guid TransportId { get; set; }
    public Guid? LigneCommande { get; set; }
    public bool LivraisonPartielle { get; set; }
    public string? Declaration { get; set; }
}

public class LigneTransportEvalDto
{
    public Guid? Partenaire { get; set; }
    public Guid? PositionTarifaire { get; set; }
    public string? Designation { get; set; }
    public string? Marque { get; set; }
    public string? Colisage { get; set; }
    public decimal? MasseBrute { get; set; }
    public decimal? MasseNette { get; set; }
    public decimal? Volume { get; set; }
    public decimal? Quantite { get; set; }
    public Guid? Devise { get; set; }
    public decimal? PrixUnitaire { get; set; }
    public decimal? ValeurDevise { get; set; }
    public Guid? UniteStatistique { get; set; }
    public Guid? ModeConditionnement { get; set; }
    public int? Nombre { get; set; }
    public string? NumerosCollis { get; set; }
    public string? MarquesCollis { get; set; }
    public string? Commentaires { get; set; }
    public Guid? PaysOrigine { get; set; }
    public long? ValeurFCFA { get; set; }
}

public class CreateLigneTransportRequest
{
    public LigneTransportDto? LigneTransport { get; set; }
    public LigneTransportEvalDto? LigneTransportEval { get; set; }
}

public class UpdateLigneTransportRequest
{
    public LigneTransportDto? LigneTransport { get; set; }
    public LigneTransportEvalDto? LigneTransportEval { get; set; }
}

public class LigneTransportResponse
{
    public LigneTransportDto? LigneTransport { get; set; }
    public LigneTransportEvalDto? LigneTransportEval { get; set; }
}

