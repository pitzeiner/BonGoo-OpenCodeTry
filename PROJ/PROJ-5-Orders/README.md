# PROJ-5: Orders (Bestellungen & Bons)

## Status: ⏳ Offen

### Bestellungen

Bestellungen pro Veranstaltung mit Tischnummer.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/veranstaltungen/{eventId}/bestellungen` | GET | Alle Bestellungen |
| `/api/veranstaltungen/{eventId}/bestellungen/{id}` | GET | Einzelne Bestellung |
| `/api/veranstaltungen/{eventId}/bestellungen` | POST | Erstellen |
| `/api/veranstaltungen/{eventId}/bestellungen/{id}` | PUT | Bearbeiten |
| `/api/veranstaltungen/{eventId}/bestellungen/{id}` | DELETE | Löschen |

### Bons

Positionen/Linien einer Bestellung.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/bestellungen/{orderId}/bons` | GET | Alle Bons der Bestellung |
| `/api/bestellungen/{orderId}/bons` | POST | Bon hinzufügen |
| `/api/bons/{id}` | PUT | Bearbeiten (Menge, Anmerkung) |
| `/api/bons/{id}/kassieren` | POST | Als kassiert markieren |
| `/api/bons/{id}/drucken` | POST | Als gedruckt markieren |
| `/api/bons/{id}/zurueckstellen` | POST | Zurückstellen |
| `/api/bons/{id}` | DELETE | Löschen |

### Zusatz-Features

| Feature | Beschreibung |
|---------|--------------|
| Sammelrechnungen | Gruppenrechnungen für Tische |
| Festführer | VIP-Billing mit Prozentsatz |
| Fremdverpflegung | Externe Bestellungen |
| EinAuszahlungen | Bargeld-Verwaltung |

### Datenmodell

```csharp
public class Bestellung
{
    public Guid Id { get; set; }
    public int BestellNr { get; set; }  // Auto-Inkrement pro Event
    public string TischNr { get; set; }
    public Guid? BedienungId { get; set; }
    public Guid VeranstaltungId { get; set; }
    public Guid? SammelrechnungId { get; set; }
}

public class Bon
{
    public Guid Id { get; set; }
    public string? Anmerkung { get; set; }
    public bool Abgerechnet { get; set; }
    public bool Fremdverpflegung { get; set; }
    public bool Eigenverbrauch { get; set; }
    public bool Einpacken { get; set; }
    public bool Kassiert { get; set; }
    public DateTime ErzeugtStamp { get; set; }
    public DateTime? AbgerechnetStamp { get; set; }
    public bool Druck { get; set; }
    public DateTime? DruckStamp { get; set; }
    public bool Zurückgestellt { get; set; }
    public bool Selbstabholung { get; set; }
    public int Menge { get; set; }
    public Guid BestellungId { get; set; }
    public Guid ProduktId { get; set; }
}
```

### Bon-Status

- `Kassiert` - Bezahlt
- `Abgerechnet` - Abgerechnet an Kassaterminal
- `Druck` - Gedruckt
- `Zurückgestellt` - Zurückgestellt
- `Fremdverpflegung` - Externe Verpflegung
- `Eigenverbrauch` - Für internen Gebrauch

### Skills-Relevanz

- **Architecture**: Vertical Slice für Bestellungen/Bons
- **QA**: Transaction-Tests, Status-Machine

### Referenzen

- `.skills/architecture.md` - API Design
- `.skills/qa.md` - Business Logic Tests