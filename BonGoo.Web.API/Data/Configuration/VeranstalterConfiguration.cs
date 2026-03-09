using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BonGoo.Web.API.Entities;

namespace BonGoo.Web.API.Data.Configuration;

public class VeranstalterConfiguration : IEntityTypeConfiguration<Veranstalter>
{
    public void Configure(EntityTypeBuilder<Veranstalter> builder)
    {
        builder.ToTable("Veranstalter");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Beschreibung)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(x => x.Logo)
            .HasColumnType("varbinary(max)");
        
        builder.Property(x => x.Plz)
            .HasMaxLength(20);
        
        builder.Property(x => x.Ort)
            .HasMaxLength(100);
        
        builder.Property(x => x.Strasse)
            .HasMaxLength(100);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}