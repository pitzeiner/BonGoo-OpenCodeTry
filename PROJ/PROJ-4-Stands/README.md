# PROJ-4: Stands (Abgabestellen, Bedienungen, Produkte)

## Status: ⏳ Offen

### Abgabestellen

Ausgabestellen wie Bar, Küche, etc. pro Veranstaltung.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/veranstaltungen/{eventId}/abgabestellen` | GET | Alle Stände |
| `/api/veranstaltungen/{eventId}/abgabestellen/{id}` | GET | Einzelner Stand |
| `/api/veranstaltungen/{eventId}/abgabestellen` | POST | Erstellen |
| `/api/veranstaltungen/{eventId}/abgabestellen/{id}` | PUT | Bearbeiten |
| `/api/veranstaltungen/{eventId}/abgabestellen/{id}` | DELETE | Löschen |

### Bedienungen

Service-Stände/Mitarbeiter pro Veranstaltung.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/veranstaltungen/{eventId}/bedienungen` | GET | Alle Bedienungen |
| `/api/veranstaltungen/{eventId}/bedienungen/{id}` | GET | Einzelne Bedienung |
| `/api/veranstaltungen/{eventId}/bedienungen` | POST | Erstellen |
| `/api/veranstaltungen/{eventId}/bedienungen/{id}` | PUT | Bearbeiten |
| `/api/veranstaltungen/{eventId}/bedienungen/{id}` | DELETE | Löschen |

### Produkte

Produkte pro Abgabestelle.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/abgabestellen/{standId}/produkte` | GET | Alle Produkte |
| `/api/abgabestellen/{standId}/produkte/{id}` | GET | Einzelnes Produkt |
| `/api/abgabestellen/{standId}/produkte` | POST | Erstellen |
| `/api/abgabestellen/{standId}/produkte/{id}` | PUT | Bearbeiten |
| `/api/abgabestellen/{standId}/produkte/{id}` | DELETE | Löschen |

### CounterProdukte

Zähler-Produkte (optional pro Stand).

### Datenmodell

```csharp
public class Abgabestelle
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; }  // "Bar 1", "Küche", etc.
    public bool Einzeldruck { get; set; }
    public bool Kassastelle { get; set; }
    public bool TakeAway { get; set; }
    public string? Drucker { get; set; }
    public Guid VeranstaltungId { get; set; }
}

public class Bedienung
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid VeranstaltungId { get; set; }
}

public class Produkt
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

### Skills-Relevanz

- **Architecture**: Nested Resources (Veranstaltung → Abgabestelle → Produkt)
- **QA**: CRUD-Tests, Validierung

### Referenzen

- `.skills/architecture.md` - API Design
- `.skills/qa.md` - Integration Tests