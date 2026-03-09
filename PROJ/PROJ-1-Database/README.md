# PROJ-1: Database & Entities

## Status: ✅ Abgeschlossen

### Implementierte Entities

| Entity | Datei | Beschreibung |
|--------|-------|--------------|
| BaseEntity | `Entities/BaseEntity.cs` | Basisklasse mit Id, CreatedAt, UpdatedAt |
| User | `Entities/User.cs` | User mit Rolle, Veranstalter-Verknüpfung |
| Veranstalter | `Entities/Veranstalter.cs` | Vereine/Organisatoren |
| Veranstaltung | `Entities/Veranstaltung.cs` | Veranstaltungen mit Datum, Aktiv-Status |
| Abgabestelle | `Entities/Abgabestelle.cs` | Ausgabestellen (Bar, Küche, etc.) + Bedienung + CounterProdukt |
| Produkt | `Entities/Abgabestelle.cs` | Produkte mit Preis, Reihenfolge |
| CounterProdukt | `Entities/Abgabestelle.cs` | Counter-Produkte |
| Bestellung | `Entities/Bestellung.cs` | Bestellungen mit BestellNr, TischNr |
| Bon | `Entities/Bestellung.cs` | Bons/Positionen |
| Sammelrechnung | `Entities/Bestellung.cs` | Gruppenrechnungen |
| Festführer | `Entities/Bestellung.cs` | VIPs mit Prozentrechnung |
| Fremdverpflegung | `Entities/Bestellung.cs` | Externe Verpflegung |
| EinAuszahlung | `Entities/Bestellung.cs` | Bargeld Ein/Aus |
| BedienungBarmittel | `Entities/Bestellung.cs` | Barkapital der Bedienungen |
| QrLoginToken | `Entities/Auth.cs` | QR-Code Tokens |
| RefreshToken | `Entities/Auth.cs` | JWT Refresh Tokens |
| KassaCode | `Entities/Auth.cs` | 6-stellige Kassa-Codes |

### DbContext & Konfiguration

| Komponente | Datei | Beschreibung |
|------------|-------|--------------|
| BonGooDbContext | `Data/BonGooDbContext.cs` | EF Core DbContext mit auto-Timestamps |
| UserConfiguration | `Data/Configuration/UserConfiguration.cs` | FluentAPI für User |
| VeranstalterConfiguration | `Data/Configuration/VeranstalterConfiguration.cs` | FluentAPI für Veranstalter |
| EntityConfigurations | `Data/Configuration/EntityConfigurations.cs` | Alle anderen Entities |

### Konfiguration

- `appsettings.json` - Connection String, JWT, SMTP Einstellungen
- Target Framework: **.NET 9.0** (vorher 10.0)

### Skills-Relevanz

- **Architecture**: Vertikale Slice Struktur in `Entities/`
- **QA**: Entity-Konventionen (Guid PK, Timestamps)

### Erledigt ✅

- [x] Entities erstellen
- [x] DbContext erstellen
- [x] FluentAPI Konfigurationen
- [x] Connection String
- [x] **Migrationen getestet und erstellt**
- [x] **Korrektur: Veranstalter.UserId string → Guid**

### Korrekturen während des Tests

1. **Veranstalter.UserId**: War `string`, sollte `Guid` sein (weil User.Id Guid ist)
2. **BonGooDbContextFactory**: Design-time Factory hinzugefügt für Migrationen
3. **Migration erfolgreich**: Alle 17 Tabellen mit korrekten FKs und Indizes

### Noch offen

- [ ] Datenbank auf PostgreSQL erstellen (`dotnet ef database update`)

### Technische Details

```csharp
// Jede Entity erbt von BaseEntity
public class Veranstaltung : BaseEntity
{
    public string Bezeichnung { get; set; }
    public DateTime Von { get; set; }
    public DateTime? Bis { get; set; }
    public bool Aktiv { get; set; }
    public Guid VeranstalterId { get; set; }
    // Navigation Properties...
}
```

### Referenzen

- `.skills/architecture.md` - Entity Design
- `.skills/qa.md` - Naming Conventions
- `script.sql` - Datenbank-Schema Quelle