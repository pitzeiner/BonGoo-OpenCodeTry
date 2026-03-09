namespace BonGoo.Web.API.Entities;

public class Veranstalter : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string? Beschreibung { get; set; }
    public byte[]? Logo { get; set; }
    public string? Plz { get; set; }
    public string? Ort { get; set; }
    public string? Strasse { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<Veranstaltung> Veranstaltungen { get; set; } = new List<Veranstaltung>();
}