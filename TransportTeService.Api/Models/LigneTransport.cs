using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportTeService.Api.Models;

[Table("LignesTransports")]
public class LigneTransport
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TransportId { get; set; }

    [ForeignKey(nameof(TransportId))]
    public Transport Transport { get; set; } = null!;

    [MaxLength(50)]
    public string? NoLigne { get; set; }

    public Guid? LigneCommande { get; set; }

    public bool LivraisonPartielle { get; set; }

    [MaxLength(100)]
    public string? Declaration { get; set; } // Référence à la DE

    // Relation avec LigneTransportEval
    public LigneTransportEval? LigneTransportEval { get; set; }

    // Audit
    public DateTime CreeLe { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string? CreePar { get; set; }

    public DateTime? ModifieLe { get; set; }

    [MaxLength(20)]
    public string? ModifiePar { get; set; }
}

