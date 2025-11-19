using Microsoft.EntityFrameworkCore;
using TransportTeService.Api.Models;

namespace TransportTeService.Api.Data;

public class TransportDbContext : DbContext
{
    public TransportDbContext(DbContextOptions<TransportDbContext> options) : base(options)
    {
    }

    public DbSet<Transport> Transports { get; set; }
    public DbSet<TransportEval> TransportsEval { get; set; }
    public DbSet<LigneTransport> LignesTransports { get; set; }
    public DbSet<LigneTransportEval> LignesTransportsEval { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration Transport
        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.NoTransport).IsUnique().HasFilter("[NoTransport] IS NOT NULL");
            entity.HasOne(e => e.TransportEval)
                  .WithOne(e => e.Transport)
                  .HasForeignKey<TransportEval>(e => e.TransportId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration TransportEval
        modelBuilder.Entity<TransportEval>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.TransportId).IsUnique();
            
            // Configuration des précisions pour les types decimal
            entity.Property(e => e.FretDevise).HasPrecision(18, 5);
            entity.Property(e => e.FretFCFA).HasPrecision(18, 5);
            entity.Property(e => e.AssuranceDevise).HasPrecision(18, 5);
            entity.Property(e => e.AssuranceFCFA).HasPrecision(18, 5);
            entity.Property(e => e.AutresChargesDevise).HasPrecision(18, 5);
            entity.Property(e => e.AutresChargesFCFA).HasPrecision(18, 5);
            entity.Property(e => e.MasseBrute).HasPrecision(18, 5);
            entity.Property(e => e.ValeurDevise).HasPrecision(18, 5);
        });

        // Configuration LigneTransport
        modelBuilder.Entity<LigneTransport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasOne(e => e.Transport)
                  .WithMany(e => e.LignesTransport)
                  .HasForeignKey(e => e.TransportId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.LigneTransportEval)
                  .WithOne(e => e.LigneTransport)
                  .HasForeignKey<LigneTransportEval>(e => e.LigneTransportId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration LigneTransportEval
        modelBuilder.Entity<LigneTransportEval>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.LigneTransportId).IsUnique();
        });
    }
}

