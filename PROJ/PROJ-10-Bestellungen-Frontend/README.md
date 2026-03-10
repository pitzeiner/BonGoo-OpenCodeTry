# PROJ-10: Frontend - Bestellungen & Statistiken

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Web.Orders` |
| Framework | Blazor WebAssembly |
| Zielgruppe | VeranstalterAdmin |
| Typ | Verwaltungs- & Anzeigekomponente |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Bestellungs-Übersicht | Alle Bestellungen pro Veranstaltung |
| Bestellung-Detail | Bons anzeigen |
| Bons verwalten | Status ändern, stornieren |
| Statistiken | Umsatz, Produkte, Stunden |
| Berichte | Tages-, Wochen-, Monatsberichte |
| Export | CSV, PDF Export |

### Seitenstruktur

```
/veranstaltungen/{eventId}/bestellungen           → Liste
/veranstaltungen/{eventId}/bestellungen/{id}      → Detail
/veranstaltungen/{eventId}/bons/{id}              → Bon bearbeiten

/veranstaltungen/{eventId}/statistiken            → Dashboard
/veranstaltungen/{eventId}/berichte               → Berichte
/veranstaltungen/{eventId}/export                 → Export
```

### Komponenten

```
Pages/
├── Bestellungen/
│   ├── Index.razor
│   └── Detail.razor
├── Statistik/
│   └── Dashboard.razor
└── Berichte/
    └── Index.razor

Components/
├── BestellungList.razor
├── BestellungCard.razor
├── BonList.razor
├── BonItem.razor
├── StatistikChart.razor
├── UmsatzCard.razor
└── ExportButton.razor
```

### Services

```csharp
public interface IBestellungService
{
    Task<IReadOnlyList<BestellungDto>> GetAllAsync(Guid eventId);
    Task<BestellungDto> GetByIdAsync(Guid id);
    Task<BestellungDto> CreateAsync(Guid eventId, CreateBestellungRequest request);
    Task DeleteAsync(Guid id);
}

public interface IBonService
{
    Task<IReadOnlyList<BonDto>> GetByBestellungAsync(Guid bestellungId);
    Task<BonDto> UpdateAsync(Guid id, UpdateBonRequest request);
    Task KassierenAsync(Guid id);
    Task DruckenAsync(Guid id);
    Task ZurueckstellenAsync(Guid id);
    Task DeleteAsync(Guid id);
}

public interface IStatistikService
{
    Task<StatistikDto> GetTagesstatistikAsync(Guid eventId, DateTime date);
    Task<IReadOnlyList<StundenStatistikDto>> GetStundenStatistikAsync(Guid eventId, DateTime date);
    Task<IReadOnlyList<ProduktStatistikDto>> GetProduktStatistikAsync(Guid eventId, DateTime? from, DateTime? to);
}
```

### Datenmodell

```csharp
public class BestellungDto
{
    public Guid Id { get; set; }
    public int BestellNr { get; set; }
    public string TischNr { get; set; }
    public Guid? BedienungId { get; set; }
    public Guid VeranstaltungId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Gesamtbetrag { get; set; }
    public List<BonDto> Bons { get; set; }
}

public class BonDto
{
    public Guid Id { get; set; }
    public string? Anmerkung { get; set; }
    public bool Kassiert { get; set; }
    public bool Druck { get; set; }
    public bool Zurückgestellt { get; set; }
    public int Menge { get; set; }
    public Guid ProduktId { get; set; }
    public string ProduktName { get; set; }
    public decimal Preis { get; set; }
}
```

### Statistik-Dashboard

| Widget | Daten |
|--------|-------|
| Heutiger Umsatz | Summe aller Bons |
| Anzahl Bestellungen | Anzahl Bestellungen |
| Top Produkte | Meistverkaufte Produkte |
| Stundenverteilung | Umsatz pro Stunde |
| Zahlungsarten | Bar vs. Karte |

### Referenzen

- `.skills/frontend.md` - Frontend Guidelines
- `PROJ/PROJ-5-Orders/` - Backend Orders API
- `BonGoo.Shared/DTOs/Bestellung/` - DTOs
