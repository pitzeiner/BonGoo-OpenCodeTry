namespace BonGoo.Web.API.Entities;

public class Bestellung : BaseEntity
{
    public int BestellNr { get; set; }
    public string TischNr { get; set; } = string.Empty;
    
    public Guid? BedienungId { get; set; }
    public Bedienung? Bedienung { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
    
    public Guid? SammelrechnungId { get; set; }
    public Sammelrechnung? Sammelrechnung { get; set; }
    
    public ICollection<Bon> Bons { get; set; } = new List<Bon>();
}

public class Bon : BaseEntity
{
    public string? Anmerkung { get; set; }
    public bool Abgerechnet { get; set; }
    public bool Fremdverpflegung { get; set; }
    public bool Eigenverbrauch { get; set; }
    public bool Einpacken { get; set; }
    public bool Kassiert { get; set; }
    public DateTime ErzeugtStamp { get; set; } = DateTime.UtcNow;
    public DateTime? AbgerechnetStamp { get; set; }
    public bool Druck { get; set; }
    public DateTime? DruckStamp { get; set; }
    public bool Zurückgestellt { get; set; }
    public bool Selbstabholung { get; set; }
    public int Menge { get; set; } = 1;
    
    public Guid BestellungId { get; set; }
    public Bestellung Bestellung { get; set; } = null!;
    
    public Guid ProduktId { get; set; }
    public Produkt Produkt { get; set; } = null!;
}

public class Sammelrechnung : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public bool Aktiv { get; set; }
    public bool HatFestführer { get; set; }
    
    public Guid? FestführerId { get; set; }
    public Festführer? Festführer { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
    
    public ICollection<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
}

public class Festführer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Prozente { get; set; }
    public string? RechnungName { get; set; }
    public string? RechnungStrasseHnr { get; set; }
    public string? RechnungPLZOrt { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
    
    public ICollection<Sammelrechnung> Sammelrechnungen { get; set; } = new List<Sammelrechnung>();
}

public class Fremdverpflegung : BaseEntity
{
    public string Empfänger { get; set; } = string.Empty;
    public string BonText { get; set; } = string.Empty;
    public int Anzahl { get; set; }
    public bool Ausgedruckt { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
}

public class EinAuszahlung : BaseEntity
{
    public string Betroffener { get; set; } = string.Empty;
    public bool Einzahlung { get; set; }
    public bool Auszahlung { get; set; }
    public decimal Betrag { get; set; }
    public string? Beschreibung { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
}

public class BedienungBarmittel : BaseEntity
{
    public decimal Betrag { get; set; }
    public bool Wechselgeld { get; set; }
    public bool Abfuhr { get; set; }
    
    public Guid BedienungId { get; set; }
    public Bedienung Bedienung { get; set; } = null!;
}