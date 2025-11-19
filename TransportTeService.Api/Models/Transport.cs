using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportTeService.Api.Models;

[Table("Transports")]
public class Transport
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string? NoTransport { get; set; }

    [MaxLength(50)]
    public string? NoBESC { get; set; }

    [MaxLength(50)]
    public string? NoConnaissement { get; set; }

    public int Etat { get; set; } // 0 = Élaboré, 1 = AE (Attente Évaluation), 2 = Ouvert, 3 = Rejeté

    public Guid? TransitaireId { get; set; }

    [MaxLength(200)]
    public string? TransporteurMaritime { get; set; }

    [MaxLength(200)]
    public string? Consignee { get; set; }

    [MaxLength(200)]
    public string? Notifier { get; set; }

    [MaxLength(100)]
    public string? ManifesteArrivee { get; set; }

    public bool IdentiqueCommande { get; set; }

    [MaxLength(100)]
    public string? ManifesteDepart { get; set; }

    [MaxLength(50)]
    public string? NoVoyage { get; set; }

    [MaxLength(100)]
    public string? NavireNom { get; set; }

    public DateTime? ETA { get; set; }

    public DateTime? ATA { get; set; }

    public int Escale { get; set; }

    [MaxLength(100)]
    public string? PosteAttribue { get; set; }

    public DateTime? DateApurement { get; set; }

    [MaxLength(50)]
    public string? NoAVE { get; set; }

    [MaxLength(50)]
    public string? Module { get; set; }

    [MaxLength(200)]
    public string? Assurance { get; set; }

    public DateTime? TLUPE { get; set; }

    public DateTime? BAD_Date { get; set; }

    [MaxLength(50)]
    public string? BAD_Ref { get; set; }

    [MaxLength(50)]
    public string? NoADV { get; set; }

    [MaxLength(50)]
    public string? NotifierSMS { get; set; }

    [MaxLength(500)]
    public string? MessageErreur { get; set; }

    // Relation avec TypeTransport
    public Guid? TypeId { get; set; }

    // Relation avec TransportEval
    public TransportEval? TransportEval { get; set; }

    // Relations avec les lignes de transport
    public ICollection<LigneTransport> LignesTransport { get; set; } = new List<LigneTransport>();

    // Relations avec les types spécialisés (si nécessaire)
    // public Routier? Routier { get; set; }
    // public Fluvial? Fluvial { get; set; }
    // public Maritime? Maritime { get; set; }
    // public Aerien? Aerien { get; set; }

    // Audit
    public DateTime CreeLe { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string? CreePar { get; set; }

    public DateTime? ModifieLe { get; set; }

    [MaxLength(20)]
    public string? ModifiePar { get; set; }
}

