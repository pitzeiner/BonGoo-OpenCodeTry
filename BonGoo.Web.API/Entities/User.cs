namespace BonGoo.Web.API.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenExpires { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpires { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    public Guid? VeranstalterId { get; set; }
    public Veranstalter? Veranstalter { get; set; }
    
    public UserRole Role { get; set; } = UserRole.SetupUser;
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public enum UserRole
{
    VeranstalterAdmin = 1,
    SetupUser = 2,
    Kassaterminal = 3,
    Bedienung = 4,
    Abgabestelle = 5
}