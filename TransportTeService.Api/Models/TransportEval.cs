using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportTeService.Api.Models;

[Table("TransportsEval")]
public class TransportEval
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TransportId { get; set; }

    [ForeignKey(nameof(TransportId))]
    public Transport Transport { get; set; } = null!;

    public Guid? Partenaire { get; set; }

    public Guid? Incoterm { get; set; }

    public int? Devise { get; set; }

    public Guid? ModeConditionnement { get; set; }

    [MaxLength(200)]
    public string? Transporteur { get; set; }

    [MaxLength(200)]
    public string? Expediteur { get; set; }

    [MaxLength(200)]
    public string? Consignee { get; set; }

    [MaxLength(200)]
    public string? Notifier { get; set; }

    [MaxLength(500)]
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

    public int? ModeTransport { get; set; } // 0 = Routier, 1 = Fluvial, 2 = Maritime, 3 = Aérien

    public int? PaysDestination { get; set; }

    [MaxLength(200)]
    public string? DestinationFinale { get; set; }

    public int? UniteDeChargement { get; set; }

    public int? NbreUnite { get; set; }

    public int? BureauDedouanement { get; set; }

    public int? Assurance { get; set; }

    // Audit
    public DateTime CreeLe { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string? CreePar { get; set; }

    public DateTime? ModifieLe { get; set; }

    [MaxLength(20)]
    public string? ModifiePar { get; set; }
}

