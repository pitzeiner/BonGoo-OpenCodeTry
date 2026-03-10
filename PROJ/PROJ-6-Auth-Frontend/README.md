# PROJ-6: Frontend - Auth

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Web.Auth` |
| Framework | Blazor WebAssembly |
| Zielgruppe | Alle Benutzer |
| Typ | Auth-Komponente (wird in anderen Apps verwendet) |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Login | E-Mail + Passwort |
| Register | Registration mit Verification |
| Password Reset | Via E-Mail Link |
| Kassa-Code | 6-stelliger Code Eingabe |
| QR-Scanner | QR-Code für Abgabestellen/Bedienung |
| Session Management | JWT Token Handling |
| Auto-Refresh | Token automatisch erneuern |

### Auth-Methoden

| Methode | Zielgruppe | Flow |
|---------|------------|------|
| E-Mail/Passwort | VeranstalterAdmin, SetupUser | Login → JWT |
| 6-stelliger Code | Kassaterminal | Code → JWT |
| QR-Code | Abgabestelle, Bedienung | Token → JWT |

### Komponenten

```
Components/
└── Auth/
    ├── Login.razor
    ├── Register.razor
    ├── PasswordReset.razor
    ├── KassaCodeLogin.razor
    ├── QrScanner.razor
    └── TokenManager.razor
```

### Services

```csharp
public interface IAuthService
{
    Task<LoginResult> LoginAsync(string email, string password);
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
    Task<PasswordResetResult> ResetPasswordAsync(string email);
    Task<KassaCodeResult> LoginWithCodeAsync(string code);
    Task<QrLoginResult> LoginWithQrAsync(string token);
    Task RefreshTokenAsync();
    Task LogoutAsync();
}
```

### Integration

Diese Auth-Komponenten werden in alle anderen Frontend-Apps eingebunden:

- **Setup/Admin** (`BonGoo.Web.Setup`)
- **Kassaterminal** (`BonGoo.Kassaterminal`)
- **Abgabestellen** (`BonGoo.Abgabestellen`)
- **Bedienung** (`BonGoo.Bedienung`)

### Referenzen

- `.skills/frontend.md` - Frontend Guidelines
- `.skills/architecture.md` - Auth-Architektur
- `PROJ/PROJ-2-Auth/` - Backend Auth API
- `BonGoo.Shared/DTOs/Auth/` - Auth DTOs
