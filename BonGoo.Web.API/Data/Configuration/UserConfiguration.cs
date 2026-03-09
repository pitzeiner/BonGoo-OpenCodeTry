using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BonGoo.Web.API.Entities;

namespace BonGoo.Web.API.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Phone)
            .HasMaxLength(25);
        
        builder.Property(x => x.Street)
            .HasMaxLength(100);
        
        builder.Property(x => x.PostalCode)
            .HasMaxLength(10);
        
        builder.Property(x => x.City)
            .HasMaxLength(100);
        
        builder.Property(x => x.Country)
            .HasMaxLength(100);
        
        builder.Property(x => x.VerificationToken)
            .HasMaxLength(500);
        
        builder.Property(x => x.PasswordResetToken)
            .HasMaxLength(500);
        
        builder.Property(x => x.Role)
            .HasConversion<int>();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.HasOne(x => x.Veranstalter)
            .WithMany()
            .HasForeignKey(x => x.VeranstalterId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}