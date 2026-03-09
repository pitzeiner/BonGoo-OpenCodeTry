# QA Skill

Richtlinien für Qualitätssicherung und Testing im BonGoo-Projekt.

## Test-Strategie

### Test-Pyramide

```
        /\
       /  \      E2E Tests (wenige)
      /----\
     /      \   Integration Tests (mittel)
    /--------\
   /          \  Unit Tests (viele)
  /____________\
```

### Test-Abdeckung

- **Unit Tests**: Geschäftslogik, DTOs, Validierung
- **Integration Tests**: API-Endpoints, Datenbank-Zugriffe
- **E2E Tests**: Kritische User-Flows (Login, Bestellung)

## Unit Tests

### Namenskonvention

`[MethodName]_[Scenario]_[ExpectedResult]`

```csharp
public class VeranstaltungTests
{
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedVeranstaltung() { }
}
```

### AAA Pattern

```csharp
[Fact]
public void CalculateTotal_ThreeItems_ReturnsSum()
{
    // Arrange
    var items = new[] { new Item { Price = 10 }, new Item { Price = 20 }, new Item { Price = 30 } };
    
    // Act
    var total = CalculateTotal(items);
    
    // Assert
    Assert.Equal(60, total);
}
```

### Was testen

- Happy Path
- Edge Cases (null, empty, boundary values)
- Exception Handling
- Validierung

### Was NICHT testen

- Framework-Funktionalität (EF Core, FastEndpoints)
- Drittanbieter-Libraries
- Triviale Getter/Setter

## Integration Tests

### API Testing mit FastEndpoints

```csharp
public class VeranstaltungApiTests : IClassFixture<TestWebAppFactory>
{
    private readonly HttpClient _client;

    [Fact]
    public async Task GetAll_Authenticated_ReturnsVeranstaltungen()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/veranstaltungen");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Test-Datenbank

- Separate Test-Datenbank oder In-Memory
- Saubere Daten vor jedem Test
- Factory-Pattern für Test-Data

## Code-Review Checklist

### Funktionalität
- [ ] Anforderungen erfüllt?
- [ ] Edge Cases behandelt?
- [ ] Fehlerbehandlung korrekt?
- [ ] Logging vorhanden?

### Sicherheit
- [ ] Auth/Authorization korrekt?
- [ ] Input-Validierung?
- [ ] SQL Injection verhindert?
- [ ] Secrets nicht im Code?

### Qualität
- [ ] Naming klar und konsistent?
- [ ] Keine Code-Duplikation?
- [ ] Methoden klein und fokussiert?
- [ ] DRY Prinzip befolgt?

### Performance
- [ ] N+1 Queries vermieden?
- [ ] Async/Await korrekt verwendet?
- [ ] Caching wo sinnvoll?

## Logging

### Struktur

```csharp
_logger.LogInformation("Veranstaltung {Id} created by {UserId}", 
    veranstaltung.Id, 
    userId);
```

### Log-Level

- **Debug**: Entwicklungs-Details
- **Information**: Geschäfts-Events
- **Warning**: Behandelbare Fehler
- **Error**: Unerwartete Fehler
- **Critical**: System-Fehler

### Was loggen

- User-Aktionen (Login, Logout, CRUD)
- Geschäfts-Events (Bestellung erstellt, Bezahlt)
- Fehler mit Kontext

## Validierung

### Input-Validierung

```csharp
public class CreateRequest
{
    [Required]
    [StringLength(100)]
    public string Bezeichnung { get; set; }

    [Required]
    public DateTime Von { get; set; }

    public DateTime? Bis { get; set; }
}
```

### Business Rules

- Validierung in der Endpoint-Klasse oder Service
- Klare Fehlermeldungen
- Deutschsprachige Messages

## Performance

### Caching

- Output-Caching für statische Daten
- In-Memory Cache für Referenzdaten

### Database

- Indizes auf Foreign Keys und häufige Queries
- Pagination bei Listen
- Projection statt vollständige Entitäten

## CI/CD

### Build

```bash
dotnet build -c Release
dotnet test --no-build
```

### Qualitäts-Checks

- dotnet-format für Code-Style
- SonarQube für statische Analyse
- Coverage Reports mit Coverlet