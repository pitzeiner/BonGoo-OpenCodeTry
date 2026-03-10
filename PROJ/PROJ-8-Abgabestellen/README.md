# PROJ-8: Frontend - Abgabestellen (POS)

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Abgabestellen` |
| Framework | Blazor WebAssembly oder MAUI |
| Zielgruppe | Ausgabestellen (Bar, Küche, etc.) |
| Auth | QR-Code Token |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Bestellungsempfang | Echtzeit-Updates via SignalR |
| Produktstatus | Bereit, in Bearbeitung |
| Zubereitungszeit | Timer anzeigen |
| Benachrichtigung | Ton bei neuer Bestellung |
| Rückmeldung | "Bereit" setzen |
| Produktliste | Verfügbare Produkte anzeigen |
| Counter-Verwaltung | Mehrere Counter pro Abgabestelle |

### Technologie

- **Framework**: Blazor WebAssembly (.NET 9.0) oder .NET MAUI
- **Real-time**: SignalR Client für Bestellungs-Updates
- **UI**: Große Schrift, Farbcodierung nach Status
- **Audio**: Benachrichtigungstöne

### Auth-Flow

```
1. Admin generiert QR-Code im Setup
2. Abgabestelle scannt QR-Code
3. App tauscht Token gegen JWT
4. Token an Abgabestelle gebunden (1:1)
```

### Seitenstruktur

```
/login              → QR-Scanner
/dashboard          → Aktive Bestellungen
/order/{orderId}    → Bestellungs-Detail
/products           → Produktübersicht
/settings           → Konfiguration
```

### Bestellungs-Workflow

```
Kassa erstellt Bestellung
    ↓
SignalR sendet an Abgabestelle
    ↓
Anzeige mit Timer
    ↓
Mitarbeiter: "In Bearbeitung"
    ↓
Fertig: "Bereit"
    ↓
Kassa: "Abgeholt" → Bestellung geschlossen
```

### Referenzen

- `.skills/architecture.md` - Vertikale Slice Architektur
- `.skills/frontend.md` - Frontend Guidelines
- `.skills/PROJ-4-Stands` - Abgabestellen API
- `.skills/PROJ-5-Orders` - Bestellungen API
- `Entities/Abgabestelle.cs` - Domain Model
- `Entities/Bestellung.cs` - Domain Model
