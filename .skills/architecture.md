# Architecture Skill

Führende Richtlinien für Architekturentscheidungen im BonGoo-Projekt.

## Technologie-Stack

- **Backend**: ASP.NET Core 10, FastEndpoints, Entity Framework Core
- **Frontend**: Blazor WebAssembly
- **Datenbank**: PostgreSQL (Npgsql)
- **Auth**: Custom JWT mit Refresh Tokens

## Architektur-Prinzipien

### Vertical Slice Architecture

Jede Feature-Slice ist eine **static class** mit **nested subclasses** für alle Komponenten:

```
Features/
└── Auth/
    ├── RegisterEndpoint.cs    # Static class mit Request, Response, Endpoint
    ├── LoginEndpoint.cs
    ├── RefreshTokenEndpoint.cs
    └── ...
```

**Pattern: Eine static class pro Use-Case**

```csharp
public static class RegisterEndpoint  // Use-Case als static class
{
    // Request DTO
    public class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Response DTO
    public class Response
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    // Endpoint-Logik
    public class Endpoint : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/api/auth/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            // Business Logic
        }
    }

    // Optional: Validator (wenn nicht auto-validation)
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(8);
        }
    }

    // Optional: Service-Klasse für komplexe Business Logic
    public class Service
    {
        public async Task<User> CreateUserAsync(Request req)
        {
            // ...
        }
    }
}
```

**Wichtige Regeln:**
- Alles in einer Datei (static class mit nested classes)
- Keine separaten Dateien für Request/Response/Validator
- Business Logic direkt in Endpoint oder in nested Service-Klasse
- Request/Response müssen不同的 Namen haben als `Endpoint<Response>` (Konflikt!)

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

## Git Workflow

### Ein Projekt = Ein Branch

Für jedes Projekt/Feature einen eigenen Branch erstellen:

```bash
# Neuen Branch für Projekt erstellen
git checkout -b PROJ-2-Auth

# Oder für Feature-Branches
git checkout -b feature/veranstalter-crud
```

**Wichtige Regeln:**
- NIEMALS direkt auf `main` entwickeln
- Erst testen, dann in `main` mergen
- Bei mehreren Sessions: jeder auf eigenem Branch arbeiten