# PROJ-8: Frontend - Veranstaltungen

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Web.Veranstaltungen` |
| Framework | Blazor WebAssembly |
| Zielgruppe | VeranstalterAdmin |
| Typ | Verwaltungskomponente |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Veranstaltungs-Liste | Alle Events mit Status |
| Veranstaltung erstellen | Neues Event anlegen |
| Veranstaltung bearbeiten | Datum, Beschreibung, etc. |
| Veranstaltung aktivieren/deaktivieren | Aktiv-Status |
| Veranstaltung löschen | Mit Bestätigung |

### Seitenstruktur

```
/veranstaltungen              → Liste
/veranstaltungen/new          → Erstellen
/veranstaltungen/{id}         → Detail
/veranstaltungen/{id}/edit    → Bearbeiten
```

### Komponenten

```
Pages/
├── Index.razor
├── New.razor
├── Detail.razor
└── Edit.razor

Components/
├── VeranstaltungList.razor
├── VeranstaltungCard.razor
├── VeranstaltungForm.razor
└── DateRangePicker.razor
```

### Services

```csharp
public interface IVeranstaltungService
{
    Task<IReadOnlyList<VeranstaltungDto>> GetAllAsync();
    Task<IReadOnlyList<VeranstaltungDto>> GetActiveAsync();
    Task<VeranstaltungDto> GetByIdAsync(Guid id);
    Task<VeranstaltungDto> CreateAsync(CreateVeranstaltungRequest request);
    Task<VeranstaltungDto> UpdateAsync(Guid id, UpdateVeranstaltungRequest request);
    Task DeleteAsync(Guid id);
    Task SetActiveAsync(Guid id, bool active);
}
```

### Berechtigungen

| Aktion | VeranstalterAdmin | SetupUser |
|--------|-------------------|------------|
| Veranstaltung lesen | ✅ | ✅ |
| Veranstaltung erstellen | ✅ | ❌ |
| Veranstaltung ändern | ✅ | ❌ |
| Veranstaltung löschen | ✅ | ❌ |

### Datenmodell

```csharp
public class VeranstaltungDto
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; }
    public string? Beschreibung { get; set; }
    public DateTime Von { get; set; }
    public DateTime? Bis { get; set; }
    public bool Aktiv { get; set; }
    public Guid VeranstalterId { get; set; }
}
```

### Referenzen

- `.skills/frontend.md` - Frontend Guidelines
- `PROJ/PROJ-3-Events/` - Backend Veranstaltungen API
- `BonGoo.Shared/DTOs/Veranstaltung/` - DTOs
