# PROJ-2: Auth-System

## Status: ✅ Abgeschlossen

### Geplante Features

| Feature | Beschreibung |
|---------|--------------|
| Registration | E-Mail + Passwort mit Verification |
| Login | JWT Access + Refresh Token |
| Password Reset | Via E-Mail Link |
| Kassa-Code | 6-stelliger zeitlich begrenzter Code |
| QR-Login | Token-basierte Auth für Geräte |

### Auth-Methoden

1. **E-Mail/Passwort** (VeranstalterAdmin, SetupUser)
   - Registration mit Verification-Code
   - Login → JWT + Refresh Token
   - Password Reset via Link

2. **6-stelliger Code** (Kassaterminal)
   - Admin generiert Code (5 Min gültig)
   - Kassa tauscht Code gegen Token

3. **QR-Code** (Abgabestellen, Bedienung)
   - Token wird generiert + als QR angezeigt
   - Gerät scannt und erhält JWT

### Rollen & Berechtigungen

| Rolle | Beschreibung | Berechtigungen |
|-------|--------------|----------------|
| VeranstalterAdmin | Admin eines Vereins | Alles inkl. Events |
| SetupUser | Setup-Benutzer | Alles außer Events |
| Kassaterminal | POS-Terminal | 6-stelliger Code |
| Bedienung | Service-Mitarbeiter | QR-Login |
| Abgabestelle | Ausgabestelle | QR-Login |

### Skills-Relevanz

- **Architecture**: JWT mit Refresh Token Pattern
- **QA**: Security-Validierung, Token-Rotation

### Technische Umsetzung

```csharp
// FastEndpoints Auth Example
public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Validate credentials, generate JWT
    }
}
```

### Referenzen

- `.skills/architecture.md` - Auth-Architektur
- `.skills/qa.md` - Security-Checklist