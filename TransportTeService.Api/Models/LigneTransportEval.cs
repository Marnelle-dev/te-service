using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportTeService.Api.Models;

[Table("LignesTransportsEval")]
public class LigneTransportEval
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid LigneTransportId { get; set; }

    [ForeignKey(nameof(LigneTransportId))]
    public LigneTransport LigneTransport { get; set; } = null!;

    public Guid? Partenaire { get; set; }

    public Guid? PositionTarifaire { get; set; }

    [MaxLength(500)]
    public string? Designation { get; set; }

    [MaxLength(50)]
    public string? Marque { get; set; }

    [MaxLength(50)]
    public string? Colisage { get; set; }

    [Column(TypeName = "decimal(13,2)")]
    public decimal? MasseBrute { get; set; }

    [Column(TypeName = "decimal(13,2)")]
    public decimal? MasseNette { get; set; }

    [Column(TypeName = "decimal(15,3)")]
    public decimal? Volume { get; set; }

    [Column(TypeName = "decimal(15,3)")]
    public decimal? Quantite { get; set; }

    public Guid? Devise { get; set; }

    [Column(TypeName = "decimal(18,5)")]
    public decimal? PrixUnitaire { get; set; }

    [Column(TypeName = "decimal(18,5)")]
    public decimal? ValeurDevise { get; set; }

    public Guid? UniteStatistique { get; set; }

    public Guid? ModeConditionnement { get; set; }

    public int? Nombre { get; set; }

    [MaxLength(50)]
    public string? NumerosCollis { get; set; }

    [MaxLength(50)]
    public string? MarquesCollis { get; set; }

    [MaxLength(1000)]
    public string? Commentaires { get; set; }

    public Guid? PaysOrigine { get; set; }

    public long? ValeurFCFA { get; set; }

    // Audit
    public DateTime CreeLe { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string? CreePar { get; set; }

    public DateTime? ModifieLe { get; set; }

    [MaxLength(20)]
    public string? ModifiePar { get; set; }
}

