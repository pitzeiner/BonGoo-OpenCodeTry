namespace BonGoo.Web.API.Entities;

public class Abgabestelle : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public bool Einzeldruck { get; set; }
    public bool Kassastelle { get; set; }
    public bool TakeAway { get; set; }
    public string? Drucker { get; set; }
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
    
    public ICollection<Produkt> Produkte { get; set; } = new List<Produkt>();
    public ICollection<CounterProdukt> CounterProdukte { get; set; } = new List<CounterProdukt>();
    public ICollection<QrLoginToken> QrLoginTokens { get; set; } = new List<QrLoginToken>();
}

public class Bedienung : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public Guid VeranstaltungId { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
    
    public ICollection<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
    public ICollection<BedienungBarmittel> Barmittel { get; set; } = new List<BedienungBarmittel>();
    public ICollection<QrLoginToken> QrLoginTokens { get; set; } = new List<QrLoginToken>();
}

public class Produkt : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public int Reihenfolge { get; set; }
    public bool Ausverkauft { get; set; }
    public bool HatCounter { get; set; }
    public decimal? Preis { get; set; }
    
    public Guid AbgabestelleId { get; set; }
    public Abgabestelle Abgabestelle { get; set; } = null!;
    
    public Guid? CounterProduktId { get; set; }
    public CounterProdukt? CounterProdukt { get; set; }
    
    public ICollection<Bon> Bons { get; set; } = new List<Bon>();
}

public class CounterProdukt : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public int Menge { get; set; }
    public int Reihenfolge { get; set; }
    
    public Guid AbgabestelleId { get; set; }
    public Abgabestelle Abgabestelle { get; set; } = null!;
    
    public ICollection<Produkt> Produkte { get; set; } = new List<Produkt>();
}