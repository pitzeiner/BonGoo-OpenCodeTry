# PROJ-7: Frontend - Kassaterminal

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Kassaterminal` |
| Framework | Blazor WebAssembly oder MAUI |
| Zielgruppe | Kassierer an der Theke |
| Auth | 6-stelliger Kassa-Code |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Bestellungen erfassen | Schnelle Produktauswahl |
| Tischverwaltung | Tische anlegen, zuweisen |
| Produktkategorien | Getränke, Speisen, etc. |
| Bezahlung | Bar, Karte (Dummy) |
| Bons drucken | Druckvorschau |
| Storno | Bestellungen stornieren |
| Zwischensumme | Laufende Summe anzeigen |
| Schnelltasten | Beliebte Produkte |

### Technologie

- **Framework**: Blazor WebAssembly (.NET 9.0) oder .NET MAUI
- **UI**: Touch-optimiert, große Buttons
- **Offline**: Lokale Daten wenn API nicht erreichbar
- **API-Integration**: Bestellungen, Produkte, Abgabestellen

### Auth-Flow

```
1. Kassaterminal zeigt Code-Eingabe
2. Admin generiert 6-stelligen Code im Setup
3. Kassaterminal: Code eingeben → JWT Token
4. Token 5 Min gültig → automatisch refresh
```

### Seitenstruktur

```
/login              → Code-Eingabe
/dashboard          → Übersicht, aktive Tische
/order/{tischId}    → Bestellung erfassen
/payment/{orderId}  → Bezahlung
/history            → Letzte Bestellungen
```

### Referenzen

- `.skills/architecture.md` - Vertikale Slice Architektur
- `.skills/frontend.md` - Frontend Guidelines
- `.skills/PROJ-5-Orders` - Bestellungen API
- `Entities/Bestellung.cs` - Domain Model
