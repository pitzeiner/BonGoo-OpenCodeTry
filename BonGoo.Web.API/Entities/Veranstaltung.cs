namespace BonGoo.Web.API.Entities;

public class Veranstaltung : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string? Beschreibung { get; set; }
    public DateTime Von { get; set; }
    public DateTime? Bis { get; set; }
    public bool Aktiv { get; set; }
    
    public Guid VeranstalterId { get; set; }
    public Veranstalter Veranstalter { get; set; } = null!;
    
    public ICollection<Abgabestelle> Abgabestellen { get; set; } = new List<Abgabestelle>();
    public ICollection<Bedienung> Bedienungen { get; set; } = new List<Bedienung>();
    public ICollection<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
    public ICollection<Sammelrechnung> Sammelrechnungen { get; set; } = new List<Sammelrechnung>();
    public ICollection<Festführer> Festführer { get; set; } = new List<Festführer>();
    public ICollection<Fremdverpflegung> Fremdverpflegungen { get; set; } = new List<Fremdverpflegung>();
    public ICollection<EinAuszahlung> EinAuszahlungen { get; set; } = new List<EinAuszahlung>();
}