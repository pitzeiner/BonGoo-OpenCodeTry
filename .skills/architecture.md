# Architecture Skill

Führende Richtlinien für Architekturentscheidungen im BonGoo-Projekt.

## Technologie-Stack

- **Backend**: ASP.NET Core 10, FastEndpoints, Entity Framework Core
- **Frontend**: Blazor WebAssembly
- **Datenbank**: PostgreSQL (Npgsql)
- **Auth**: Custom JWT mit Refresh Tokens

## Architektur-Prinzipien

### Vertical Slice Architecture

Jede Feature-Slice ist in einer einzigen Klasse implementiert:

```
Features/
└── Veranstaltungen/
    └── GetAllEndpoint.cs  # Endpoint + Request/Response + Handler
```

Alle zugehörigen Komponenten bleiben zusammen:
- Endpoint-Definition
- Request/Response DTOs
- Geschäftslogik (Handler)

### DTOs in Shared Library

Alle DTOs, die zwischen API und Client geteilt werden, gehören in `BonGoo.Shared/DTOs/`.

### Schichten

```
API Layer (FastEndpoints)
    ↓
Business Logic (in Endpoint-Klasse)
    ↓
Data Access (EF Core Repositories)
    ↓
Database (PostgreSQL)
```

## Auth-Architektur

### Rollen
- `VeranstalterAdmin`: Vollzugriff inkl. Veranstaltungen und User-Verwaltung
- `SetupUser`: Konfiguration ohne Event-Verwaltung
- `Kassaterminal`: 6-stelliger Zeitcode
- `Bedienung`: QR-Code Token

### Auth-Flows
- **E-Mail/Passwort**: Registration + Login mit JWT
- **Kassa-Code**: Zeitlich begrenzter 6-stelliger Code → Token
- **QR-Login**: Token-basiert mit Geräte-Kopplung

## API Design

### Endpoint-Struktur

```csharp
public class CreateVeranstaltungEndpoint : Endpoint<CreateRequest, Response>
{
    public override void Configure()
    {
        Post("/api/veranstaltungen");
        Roles("VeranstalterAdmin");
    }

    public override Task HandleAsync(CreateRequest req, CancellationToken ct)
    {
        // Business Logic direkt hier oder in separate Service-Klasse
    }
}
```

### Response Patterns

- **Erfolg**: `SendAsync(response, 201)`
- **Nicht gefunden**: `SendNotFoundAsync()`
- **Validation Error**: `AddErrors(ModelState); ThrowIfAnyErrors();`
- **Authorization Error**: `SendUnauthorizedAsync()`

## Datenbank-Design

### EF Core Konventionen

- Guid als Primary Key für alle Entitäten
- `CreatedAt` und `UpdatedAt` auf allen Tabellen
- Soft Delete bevorzugen (nicht physisch löschen)
- Cascading Deletes für Child-Entities

### Migrationen

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Fehlerbehandlung

- Exceptions mit spezifischen Typen fangen
- Logging mit Correlation ID
- User-freundliche Fehlermeldungen in Deutsch
- Stacktrace nur in Development

## Konfiguration

- `appsettings.json` für Development
- `appsettings.Production.json` für Produktion
- Environment-Variablen für Secrets