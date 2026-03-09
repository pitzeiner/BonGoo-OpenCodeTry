using Microsoft.EntityFrameworkCore;
using BonGoo.Web.API.Entities;

namespace BonGoo.Web.API.Data;

public class BonGooDbContext : DbContext
{
    public BonGooDbContext(DbContextOptions<BonGooDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Veranstalter> Veranstalter => Set<Veranstalter>();
    public DbSet<Veranstaltung> Veranstaltungen => Set<Veranstaltung>();
    public DbSet<Abgabestelle> Abgabestellen => Set<Abgabestelle>();
    public DbSet<Bedienung> Bedienungen => Set<Bedienung>();
    public DbSet<Produkt> Produkte => Set<Produkt>();
    public DbSet<CounterProdukt> CounterProdukte => Set<CounterProdukt>();
    public DbSet<Bestellung> Bestellungen => Set<Bestellung>();
    public DbSet<Bon> Bons => Set<Bon>();
    public DbSet<Sammelrechnung> Sammelrechnungen => Set<Sammelrechnung>();
    public DbSet<Festführer> Festführer => Set<Festführer>();
    public DbSet<Fremdverpflegung> Fremdverpflegungen => Set<Fremdverpflegung>();
    public DbSet<EinAuszahlung> EinAuszahlungen => Set<EinAuszahlung>();
    public DbSet<BedienungBarmittel> BedienungenBarmittel => Set<BedienungBarmittel>();
    public DbSet<QrLoginToken> QrLoginTokens => Set<QrLoginToken>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<KassaCode> KassaCodes => Set<KassaCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BonGooDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}