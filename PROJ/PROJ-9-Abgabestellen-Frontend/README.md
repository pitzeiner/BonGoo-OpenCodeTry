# PROJ-9: Frontend - Abgabestellen

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Web.Stands` |
| Framework | Blazor WebAssembly |
| Zielgruppe | VeranstalterAdmin |
| Typ | Verwaltungskomponente |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Abgabestellen-Liste | Alle Stände pro Veranstaltung |
| Abgabestelle erstellen | Bar, Küche, etc. |
| Abgabestelle bearbeiten | Einstellungen, Drucker |
| Bedienungen | Service-Mitarbeiter verwalten |
| Produkte | Produkte pro Stand |
| CounterProdukte | Zähler-Produkte |
| QR-Code Generierung | Für Abgabestellen-Login |
| Kassa-Code Verwaltung | 6-stellige Codes generieren |

### Seitenstruktur

```
/veranstaltungen/{eventId}/abgabestellen           → Liste
/veranstaltungen/{eventId}/abgabestellen/new       → Erstellen
/veranstaltungen/{eventId}/abgabestellen/{id}      → Detail
/veranstaltungen/{eventId}/abgabestellen/{id}/edit → Bearbeiten

/veranstaltungen/{eventId}/bedienungen             → Liste
/veranstaltungen/{eventId}/bedienungen/{id}/qr     → QR-Code

/abgabestellen/{standId}/produkte                  → Produkte
/abgabestellen/{standId}/produkte/new              → Produkt erstellen
/abgabestellen/{standId}/counter                   → Counter-Produkte
```

### Komponenten

```
Pages/
├── Abgabestellen/
│   ├── Index.razor
│   ├── New.razor
│   ├── Detail.razor
│   └── Edit.razor
├── Bedienungen/
│   ├── Index.razor
│   └── QrCode.razor
└── Produkte/
    ├── Index.razor
    ├── New.razor
    └── Edit.razor

Components/
├── StandCard.razor
├── StandForm.razor
├── BedienungList.razor
├── ProductCard.razor
├── ProductForm.razor
├── QrCodeDisplay.razor
└── KassaCodeGenerator.razor
```

### Services

```csharp
public interface IAbgabestelleService
{
    Task<IReadOnlyList<AbgabestelleDto>> GetAllAsync(Guid eventId);
    Task<AbgabestelleDto> GetByIdAsync(Guid id);
    Task<AbgabestelleDto> CreateAsync(Guid eventId, CreateAbgabestelleRequest request);
    Task UpdateAsync(Guid id, UpdateAbgabestelleRequest request);
    Task DeleteAsync(Guid id);
}

public interface IBedienungService
{
    Task<IReadOnlyList<BedienungDto>> GetAllAsync(Guid eventId);
    Task<string> GenerateQrCodeAsync(Guid id);
    Task GenerateKassaCodeAsync(Guid eventId);
}

public interface IProduktService
{
    Task<IReadOnlyList<ProduktDto>> GetByStandAsync(Guid standId);
    Task<ProduktDto> CreateAsync(Guid standId, CreateProduktRequest request);
    Task UpdateAsync(Guid id, UpdateProduktRequest request);
    Task DeleteAsync(Guid id);
    Task SetAusverkauftAsync(Guid id, bool ausverkauft);
}
```

### Datenmodell

```csharp
public class AbgabestelleDto
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; }
    public bool Einzeldruck { get; set; }
    public bool Kassastelle { get; set; }
    public bool TakeAway { get; set; }
    public string? Drucker { get; set; }
    public Guid VeranstaltungId { get; set; }
}

public class ProduktDto
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; }
    public int Reihenfolge { get; set; }
    public bool Ausverkauft { get; set; }
    public bool HatCounter { get; set; }
    public decimal? Preis { get; set; }
    public Guid AbgabestelleId { get; set; }
}
```

### Referenzen

- `.skills/frontend.md` - Frontend Guidelines
- `PROJ/PROJ-4-Stands/` - Backend Stands API
- `BonGoo.Shared/DTOs/Abgabestelle/` - DTOs
- `BonGoo.Shared/DTOs/Produkt/` - DTOs
