using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BonGoo.Web.API.Entities;

namespace BonGoo.Web.API.Data.Configuration;

public class VeranstaltungConfiguration : IEntityTypeConfiguration<Veranstaltung>
{
    public void Configure(EntityTypeBuilder<Veranstaltung> builder)
    {
        builder.ToTable("Veranstaltungen");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Beschreibung)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(x => x.Von)
            .IsRequired();
        
        builder.HasOne(x => x.Veranstalter)
            .WithMany(x => x.Veranstaltungen)
            .HasForeignKey(x => x.VeranstalterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AbgabestelleConfiguration : IEntityTypeConfiguration<Abgabestelle>
{
    public void Configure(EntityTypeBuilder<Abgabestelle> builder)
    {
        builder.ToTable("Abgabestellen");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Drucker)
            .HasMaxLength(200);
        
        builder.HasOne(x => x.Veranstaltung)
            .WithMany(x => x.Abgabestellen)
            .HasForeignKey(x => x.VeranstaltungId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BedienungConfiguration : IEntityTypeConfiguration<Bedienung>
{
    public void Configure(EntityTypeBuilder<Bedienung> builder)
    {
        builder.ToTable("Bedienungen");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasOne(x => x.Veranstaltung)
            .WithMany(x => x.Bedienungen)
            .HasForeignKey(x => x.VeranstaltungId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProduktConfiguration : IEntityTypeConfiguration<Produkt>
{
    public void Configure(EntityTypeBuilder<Produkt> builder)
    {
        builder.ToTable("Produkte");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Preis)
            .HasPrecision(18, 2);
        
        builder.HasOne(x => x.Abgabestelle)
            .WithMany(x => x.Produkte)
            .HasForeignKey(x => x.AbgabestelleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.CounterProdukt)
            .WithMany(x => x.Produkte)
            .HasForeignKey(x => x.CounterProduktId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class CounterProduktConfiguration : IEntityTypeConfiguration<CounterProdukt>
{
    public void Configure(EntityTypeBuilder<CounterProdukt> builder)
    {
        builder.ToTable("CounterProdukte");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasOne(x => x.Abgabestelle)
            .WithMany(x => x.CounterProdukte)
            .HasForeignKey(x => x.AbgabestelleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BestellungConfiguration : IEntityTypeConfiguration<Bestellung>
{
    public void Configure(EntityTypeBuilder<Bestellung> builder)
    {
        builder.ToTable("Bestellungen");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.BestellNr)
            .IsRequired();
        
        builder.Property(x => x.TischNr)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasOne(x => x.Bedienung)
            .WithMany(x => x.Bestellungen)
            .HasForeignKey(x => x.BedienungId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.Veranstaltung)
            .WithMany(x => x.Bestellungen)
            .HasForeignKey(x => x.VeranstaltungId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Sammelrechnung)
            .WithMany(x => x.Bestellungen)
            .HasForeignKey(x => x.SammelrechnungId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class BonConfiguration : IEntityTypeConfiguration<Bon>
{
    public void Configure(EntityTypeBuilder<Bon> builder)
    {
        builder.ToTable("Bons");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Anmerkung)
            .HasColumnType("nvarchar(max)");
        
        builder.HasOne(x => x.Bestellung)
            .WithMany(x => x.Bons)
            .HasForeignKey(x => x.BestellungId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Produkt)
            .WithMany(x => x.Bons)
            .HasForeignKey(x => x.ProduktId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.IpAddress)
            .HasMaxLength(50);
        
        builder.Property(x => x.UserAgent)
            .HasMaxLength(500);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class QrLoginTokenConfiguration : IEntityTypeConfiguration<QrLoginToken>
{
    public void Configure(EntityTypeBuilder<QrLoginToken> builder)
    {
        builder.ToTable("QrLoginTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Abgabestelle)
            .WithMany(x => x.QrLoginTokens)
            .HasForeignKey(x => x.AbgabestelleId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.Bedienung)
            .WithMany(x => x.QrLoginTokens)
            .HasForeignKey(x => x.BedienungId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class KassaCodeConfiguration : IEntityTypeConfiguration<KassaCode>
{
    public void Configure(EntityTypeBuilder<KassaCode> builder)
    {
        builder.ToTable("KassaCodes");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(x => x.KassaId)
            .IsRequired()
            .HasMaxLength(100);
    }
}