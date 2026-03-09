# PROJ-3: Events (Veranstalter & Veranstaltungen)

## Status: ⏳ Offen

### Veranstalter

CRUD für Vereine/Organisatoren.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/veranstalter` | GET | Alle Veranstalter |
| `/api/veranstalter/{id}` | GET | Einzelner Veranstalter |
| `/api/veranstalter` | POST | Erstellen |
| `/api/veranstalter/{id}` | PUT | Bearbeiten |
| `/api/veranstalter/{id}` | DELETE | Löschen |

### Veranstaltungen

CRUD für Veranstaltungen eines Veranstalters.

| Endpoint | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/veranstaltungen` | GET | Alle Veranstaltungen |
| `/api/veranstaltungen/active` | GET | Aktive Veranstaltungen |
| `/api/veranstaltungen/{id}` | GET | Einzelne Veranstaltung |
| `/api/veranstaltungen` | POST | Erstellen (nur Admin) |
| `/api/veranstaltungen/{id}` | PUT | Bearbeiten |
| `/api/veranstaltungen/{id}` | DELETE | Löschen (nur Admin) |

### Skills-Relevanz

- **Architecture**: Vertical Slice pro Entity
- **QA**: CRUD-Tests, Validierung

### Datenmodell

```csharp
public class Veranstalter
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; }
    public string? Beschreibung { get; set; }
    public byte[]? Logo { get; set; }
    public string? Plz { get; set; }
    public string? Ort { get; set; }
    public string? Strasse { get; set; }
    public string UserId { get; set; }  // Owner
}

public class Veranstaltung
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

### Berechtigungen

| Aktion | VeranstalterAdmin | SetupUser |
|--------|-------------------|------------|
| Veranstalter lesen | ✅ | ✅ |
| Veranstalter schreiben | ✅ | ❌ |
| Veranstaltung lesen | ✅ | ✅ |
| Veranstaltung erstellen | ✅ | ❌ |
| Veranstaltung löschen | ✅ | ❌ |

### Referenzen

- `.skills/architecture.md` - API Design
- `.skills/qa.md` - CRUD Tests