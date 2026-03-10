# PROJ-9: Frontend - Bedienung (Mobile)

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Bedienung` |
| Framework | Blazor WebAssembly oder MAUI |
| Zielgruppe | Service-Mitarbeiter (mobil) |
| Auth | QR-Code Token |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Bestellung aufnehmen | Produkte scannen/auswählen |
| Kategorieseiten | Getränke, Speisen, etc. |
| Tischzuweisung | Bestellung zu Tisch |
| Laufzettel | Aktive Bestellungen |
| Nachbestellung | Schnell nachbestellen |
| Bons anzeigen | Aktuelle Bons ansehen |
| Umsatz anzeigen | Aktueller Umsatz |
| Barausgabe | Ein/Auszahlung erfassen |

### Technologie

- **Framework**: .NET MAUI (empfohlen) oder Blazor WASM als PWA
- **Mobile**: Touch-optimiert, große Buttons
- **Barcode-Scanner**: Für Produkte oder Tischzuweisung
- **Offline**: Lokale Queue wenn offline

### Auth-Flow

```
1. Admin generiert QR-Code im Setup
2. Bedienung scannt QR-Code
3. App tauscht Token gegen JWT
4. Token an Gerät oder Person gebunden
```

### Seitenstruktur

```
/login              → QR-Scanner
/dashboard          → Aktive Bestellungen, Schnellzugriff
/order/new          → Neue Bestellung
/products            → Produktkatalog
/orders             → Alle Bestellungen
/cash               → Barausgabe/Einlage
/profile            → Profil, Einstellungen
```

### Besonderheiten

- **Schnellzugriff**: Häufig bestellte Produkte oben
- **Laufzettel**: Alle aktiven Bestellungen mit Status
- **Timer**: Anzeige seit Bestellungserfassung
- **Bestätigung**: Akustisch bei neuem Auftrag an Abgabestelle

### Referenzen

- `.skills/architecture.md` - Vertikale Slice Architektur
- `.skills/frontend.md` - Frontend Guidelines
- `.skills/PROJ-5-Orders` - Bestellungen API
- `.skills/PROJ-2-Auth` - Auth mit QR-Code
- `Entities/Bestellung.cs` - Domain Model
- `Entities/BedienungBarmittel.cs` - Barkapital
