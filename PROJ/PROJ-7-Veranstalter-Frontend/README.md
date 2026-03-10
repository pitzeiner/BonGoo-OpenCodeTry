# PROJ-7: Frontend - Veranstalter

## Status: ⏳ Geplant

### Projektübersicht

| Eigenschaft | Wert |
|-------------|------|
| Projekt | `BonGoo.Web.Veranstalter` |
| Framework | Blazor WebAssembly |
| Zielgruppe | VeranstalterAdmin |
| Typ | Verwaltungskomponente |

### Features

| Feature | Beschreibung |
|---------|--------------|
| Veranstalter-Profil | Vereinsdaten bearbeiten |
| Benutzerverwaltung | Benutzer anlegen, Rollen zuweisen |
| Logo-Upload | Vereinslogo hochladen |
| Einstellungen |PLZ, Ort, Straße |

### Seitenstruktur

```
/veranstalter                 → Übersicht
/veranstalter/edit            → Bearbeiten
/veranstalter/users           → Benutzerverwaltung
/veranstalter/users/{id}      → Benutzer bearbeiten
```

### Komponenten

```
Pages/
├── Veranstalter.razor
├── Edit.razor
└── Users/
    ├── Index.razor
    └── Edit.razor

Components/
├── VeranstalterForm.razor
├── UserList.razor
├── UserEditor.razor
└── LogoUpload.razor
```

### Services

```csharp
public interface IVeranstalterService
{
    Task<VeranstalterDto> GetAsync();
    Task<VeranstalterDto> UpdateAsync(UpdateVeranstalterRequest request);
    Task<IReadOnlyList<UserDto>> GetUsersAsync();
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task UpdateUserAsync(Guid id, UpdateUserRequest request);
    Task DeleteUserAsync(Guid id);
}
```

### Berechtigungen

| Aktion | VeranstalterAdmin | SetupUser |
|--------|-------------------|------------|
| Veranstalter lesen | ✅ | ✅ |
| Veranstalter ändern | ✅ | ❌ |
| Benutzer verwalten | ✅ | ❌ |

### Referenzen

- `.skills/frontend.md` - Frontend Guidelines
- `PROJ/PROJ-3-Events/` - Backend Veranstalter API
- `BonGoo.Shared/DTOs/Veranstalter/` - DTOs
