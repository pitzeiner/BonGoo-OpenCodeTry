# AGENTS.md - BonGoo Development Guide

## Project Overview

This is a .NET 9 solution with three projects:
- **BonGoo.Web.API** - ASP.NET Core Web API (Backend)
- **BonGoo.Shared** - Shared class library (Entities, DTOs, Enums)
- **BonGoo.Web.Setup** - Blazor WebAssembly (Setup/Admin App)

All projects target `.NET 9.0` with `Nullable` and `ImplicitUsings` enabled.

## Tech Stack

- **Database**: PostgreSQL (license-free, Best Practice)
- **ORM**: Entity Framework Core with Npgsql provider
- **Authentication**: Custom JWT-based solution (NOT ASP.NET Identity)

---

## Build, Run, and Test Commands

### Build Commands

```bash
# Build the entire solution
dotnet build

# Build a specific project
dotnet build BonGoo.Web.API/BonGoo.Web.API.csproj
dotnet build BonGoo.Shared/BonGoo.Shared.csproj
dotnet build Web.Setup/Web.Setup.csproj

# Build in Release mode
dotnet build -c Release
```

### Running the Application

```bash
# Run the Web API
cd BonGoo.Web.API && dotnet run

# Run with specific URL
cd BonGoo.Web.API && dotnet run --urls "http://localhost:5000"

# Run Blazor WebAssembly (requires API running)
cd Web.Setup && dotnet run
```

### Test Commands

```bash
# Run all tests
dotnet test

# Run a single test by fully qualified name
dotnet test --filter "FullyQualifiedName~Namespace.ClassName.MethodName"

# Run tests matching a pattern
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run tests in parallel
dotnet test --parallel

# Run tests with detailed output
dotnet test --verbosity detailed
```

---

## Code Style Guidelines

### General Principles

- **DO NOT ADD COMMENTS** unless explicitly required
- Keep code concise and readable
- Follow SOLID principles
- Use meaningful, descriptive names

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes/Interfaces | PascalCase | `UserService`, `IUserRepository` |
| Methods | PascalCase | `GetUserById`, `CalculateTotal` |
| Properties | PascalCase | `UserName`, `IsActive` |
| Private Fields | camelCase | `_userRepository`, `_config` |
| Constants | PascalCase | `MaxRetryCount` |
| Enums | PascalCase | `UserRole.Admin` |
| Parameters/Local Variables | camelCase | `userId`, `isValid` |

### File Organization

```
Project/
├── Controllers/       # API controllers
├── Models/           # Domain entities
├── DTOs/             # Data transfer objects
├── Services/         # Business logic
├── Repositories/     # Data access
└── Program.cs        # Entry point
```

### Imports

- Use implicit usings
- Order: System → Third-party → Project-specific

### Types and Nullable

- Use explicit types for public APIs, return types, properties
- Use `var` for local variables when type is obvious
- Use nullable `?` for reference types
- Use null-coalescing `??` and null-conditional `?.`

### Error Handling

- Use exceptions for exceptional cases only
- Catch specific exceptions, not base `Exception`
- Use `IResult` or `ProblemDetails` for API errors
- Log exceptions with context before rethrowing

```csharp
try
{
    var entity = await _repository.GetByIdAsync(id);
    if (entity is null)
        return Results.NotFound(new { Message = "Not found" });
    return Results.Ok(entity);
}
catch (DbException ex)
{
    _logger.LogError(ex, "Database error for {Id}", id);
    return Results.Problem("Database error", statusCode: 500);
}
```

### Async/Await

- Always use `async`/`await` for I/O-bound operations
- Use `Task.WhenAll` for parallel independent operations
- Avoid blocking (no `.Result` or `.Wait()`)

### Dependency Injection

- Register services in `Program.cs`
- Use constructor injection
- Use `AddScoped` for per-request services
- Use `AddSingleton` for stateless services
- Use `AddTransient` for lightweight services

### API Design

- Use minimal APIs (`app.MapGet`, `app.MapPost`) or controllers
- Return appropriate HTTP status codes
- Use `[FromQuery]`, `[FromBody]`, `[FromRoute]` explicitly
- Validate input using data annotations

### Collections

- Use `IReadOnlyList<T>` or `IEnumerable<T>` for read-only
- Prefer collection expressions `[]` over `new List<T>()`

---

## Domain Entities

### Core Entities

- **Veranstalter** - Event organizers/clubs
- **Veranstaltungen** - Events (has Veranstalter, date range, Aktiv flag)
- **Abgabestellen** - Distribution points (Bar, Kitchen, etc.)
- **Bedienungen** - Service stands/staff at an event
- **Produkte** - Products sold at an event
- **Bestellungen** - Orders (with BestellNr, TischNr)
- **Bons** - Receipts/line items (core voucher document)

### Supporting Entities

- **Sammelrechnungen** - Collective/group invoices
- **Festführer** - VIPs with percentage billing
- **Fremdverpflegungen** - External catering orders
- **EinAuszahlungen** - Cash in/out transactions
- **QrLoginTokens** - QR code login tokens
- **BedienungenBarmittel** - Cash handling for staff
- **CounterProdukte** - Counter-specific products

### Authentication

- Custom User entity (not ASP.NET Identity)
- Roles: Admin, Veranstalter, Bedienung
- JWT token-based authentication

---

## Project-Specific Notes

- Solution uses top-level statements in `Program.cs`
- OpenAPI/Swagger enabled in development
- Blazor WebAssembly in `Web.Setup/`
- Later: Separate apps for Kassaterminal, Abgabestellen, Bedienung