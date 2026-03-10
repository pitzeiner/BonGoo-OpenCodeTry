namespace BonGoo.Web.API.Entities;

public class QrLoginToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public int Type { get; set; }
    
    public Guid? AbgabestelleId { get; set; }
    public Abgabestelle? Abgabestelle { get; set; }
    
    public Guid? BedienungId { get; set; }
    public Bedienung? Bedienung { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public string? UsedByClientInfo { get; set; }
    public string? UsedByIpAddress { get; set; }
}

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class KassaCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string KassaId { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public string? VeranstaltungId { get; set; }
}