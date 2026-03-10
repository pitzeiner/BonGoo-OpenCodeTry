# Frontend Skill

Richtlinien für die Entwicklung der BonGoo Frontend-Anwendungen.

## Design-Prinzipien

- **Minimalistisch**: Weniger ist mehr - Fokus auf Daten
- **Dezent**: Gedeckte Farben, keine grellen Töne
- **Subtil**: Sanfte Animationen, keine aufdringlichen Effekte
- **Funktional**: Jedes Element hat einen Zweck
- **Professionell**: SaaS-ähnliche Datenanwendung

## Tech Stack

| Anwendung | Framework | Zielplattform |
|-----------|-----------|---------------|
| Setup/Admin | Blazor WASM | Web, PWA |
| Kassaterminal | Blazor WASM / MAUI | Desktop, Touchscreen |
| Abgabestellen | Blazor WASM / MAUI | Touchscreen |
| Bedienung | MAUI (empfohlen) | Mobile (iOS, Android) |

## Layout-Struktur

### Auth-Layout (vor Login)

Zentriertes Layout ohne Navigation. Sauberes, schlichtes Design.

```
┌─────────────────────────────────────┐
│                                     │
│         [Logo]                      │
│                                     │
│         Email    [________]         │
│         Passwort [________]         │
│                                     │
│         [Anmelden]                  │
│                                     │
│         Passwort vergessen?          │
│                                     │
└─────────────────────────────────────┘
```

### Main-Layout (nach Login)

**Linke Sidebar** mit Navigation. Content-Bereich rechts.

```
┌────────────┬────────────────────────┐
│            │  Header: Breadcrumb    │
│  Sidebar   ├────────────────────────┤
│            │                        │
│  [Home]    │   Content Area         │
│  [Events]  │                        │
│  [Stands]  │   Page Content        │
│  [Orders]  │                        │
│  [Stats]   │                        │
│            │                        │
│  ────────  │                        │
│  [Logout]  │                        │
└────────────┴────────────────────────┘
```

### Sidebar Komponente

```razor
<div class="sidebar bg-light border-end" style="width: 220px; min-height: 100vh;">
    <div class="p-3 border-bottom">
        <strong>BonGoo</strong>
    </div>
    <nav class="nav flex-column p-2">
        <a class="nav-link" href="/">
            <i class="bi bi-house"></i> Startseite
        </a>
        <a class="nav-link" href="/veranstaltungen">
            <i class="bi bi-calendar-event"></i> Veranstaltungen
        </a>
        <a class="nav-link" href="/abgabestellen">
            <i class="bi bi-shop"></i> Abgabestellen
        </a>
        <a class="nav-link" href="/bestellungen">
            <i class="bi bi-receipt"></i> Bestellungen
        </a>
        <a class="nav-link" href="/statistiken">
            <i class="bi bi-graph-up"></i> Statistiken
        </a>
    </nav>
    <div class="mt-auto p-2 border-top">
        <a class="nav-link text-muted" href="/logout">
            <i class="bi bi-box-arrow-right"></i> Abmelden
        </a>
    </div>
</div>
```

### Layout-Logik

```csharp
// App.razor
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
                <NotAuthorized>
                    <AuthLayout>
                        <RedirectToLogin />
                    </AuthLayout>
                </NotAuthorized>
            </AuthorizeRouteView>
        </Found>
    </Router>
</CascadingAuthenticationState>
```

### Responsive Sidebar

- **Desktop**: Sidebar immer sichtbar (220px)
- **Tablet**: Sidebar einklappbar
- **Mobile**: Hamburger-Menü oder Bottom-Navigation

```css
@media (max-width: 768px) {
    .sidebar {
        position: fixed;
        z-index: 1000;
        transform: translateX(-100%);
        transition: transform 0.3s ease;
    }
    .sidebar.show {
        transform: translateX(0);
    }
}
```

## UI-Framework

**Bootstrap 5 (dezent)** - Empfohlen für alle Apps
- Schlankes, minimalistisches Design
- Dezente Farben und Animationen
- Keine aufdringlichen Komponenten
- Fokus auf Daten und Funktionalität

### Features

| Feature | Lösung |
|---------|--------|
| Toast-Benachrichtigungen | Bootstrap Toasts oder `toastr.js` |
| Modal-Dialoge | Bootstrap Modals |
| Tabellen | Bootstrap Tables mit dezentem Styling |
| Formulare | Bootstrap Forms |
| Icons | Bootstrap Icons (leichtgewichtige SVG) |

### Alternativen (wenn Bootstrap nicht reicht)

- **BootStrapBlazor** - Mehr Komponenten, aber umfangreicher
- **Radzen** - Nur wenn schnellere Entwicklung nötig

## Architektur

### Client-Server Communication

```
Blazor Client
    ↓ HttpClient
API (FastEndpoints)
    ↓
EF Core + PostgreSQL
```

### Shared DTOs

Alle DTOs gehören in `BonGoo.Shared/DTOs/`:

```
BonGoo.Shared/
└── DTOs/
    ├── Auth/
    ├── Veranstalter/
    ├── Veranstaltung/
    ├── Abgabestelle/
    ├── Bestellung/
    └── Produkt/
```

### State Management

- **Lokaler State**: Component-Parameter, Cascading Values
- **Global State**: Blazor Fluxor oder Lazy Service
- **Server State**: HttpClient mit JWT

## Auth-Integration

### JWT Token Handling

```csharp
// HttpClient mit Authorization Header
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);
```

### Token Refresh

```csharp
// Automatisch bei 401
handler.InnerHandler = new HttpClientHandler();
var pipeline = new Pipeline(() => token, refreshToken, handler);
```

## Component Guidelines

### Naming

| Type | Pattern | Example |
|------|---------|---------|
| Pages | `*.razor` | `Index.razor`, `Orders.razor` |
| Components | `*.razor` | `ProductCard.razor`, `OrderList.razor` |
| Code-Behind | `*.razor.cs` | `ProductCard.razor.cs` |

### Component Structure

```razor
@namespace BonGoo.Web.Setup.Pages

@page "/products"

<h5 class="mb-3">Produkte</h5>

@if (products is null)
{
    <LoadingSpinner />
}
else
{
    <div class="table-responsive">
        <table class="table table-hover table-sm">
            <thead class="table-light">
                <tr>
                    <th>Bezeichnung</th>
                    <th class="text-end">Preis</th>
                    <th class="text-center">Status</th>
                    <th class="text-end">Aktionen</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var product in products)
                {
                    <tr>
                        <td>@product.Bezeichnung</td>
                        <td class="text-end">@product.Preis.ToString("C")</td>
                        <td class="text-center">
                            @if (product.Ausverkauft)
                            {
                                <span class="badge bg-secondary">Ausverkauft</span>
                            }
                            else
                            {
                                <span class="badge bg-success">Verfügbar</span>
                            }
                        </td>
                        <td class="text-end">
                            <button class="btn btn-sm btn-outline-secondary" @onclick="() => Edit(product)">
                                Bearbeiten
                            </button>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}

@code {
    private IReadOnlyList<ProductDto>? products;
    
    protected override async Task OnInitializedAsync()
    {
        products = await ProductService.GetAllAsync();
    }
}
```

### Styling

- **CSS**: Scoped CSS in `.razor` Dateien
- **CSS Variables**: Theme-Farben zentral definieren (dezent)
- **Responsive**: Mobile-First
- **Minimal**: Keine überflüssigen visuellen Elemente

```css
/* wwwroot/css/app.css */
:root {
    --bon-primary: #4a6fa5;        /* Gedecktes Blau */
    --bon-secondary: #6c757d;       /* Neutrales Grau */
    --bon-success: #5a8f5a;        /* Gedecktes Grün */
    --bon-danger: #b85450;         /* Gedecktes Rot */
    --bon-warning: #c9a227;        /* Gedecktes Gelb */
    --bon-bg: #f8f9fa;             /* Helles Grau */
    --bon-surface: #ffffff;        /* Weiß */
    --bon-border: #e9ecef;         /* Subtile Border */
    --bon-text: #343a40;           /* Dunkelgrau */
}

body {
    background-color: var(--bon-bg);
    color: var(--bon-text);
}

/* Dezente Animationen */
.fade-in {
    animation: fadeIn 0.2s ease-in-out;
}

@keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
}

## Best Practices

### Performance

- **Lazy Loading**: Components bei Bedarf laden
- **Virtualization**: Lange Listen mit `Virtualize`
- **Caching**: Produktdaten cached halten
- **Debouncing**: Suchfelder debouncen

### Error Handling

```csharp
try
{
    products = await ProductService.GetAllAsync();
}
catch (HttpRequestException ex)
{
    ToastService.ShowError("Fehler beim Laden der Daten");
}
catch (UnauthorizedAccessException)
{
    Navigation.NavigateTo("/login");
}
```

### Toast Service

```csharp
public interface IToastService
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowWarning(string message);
    void ShowInfo(string message);
}
```

### Modal Dialog

```razor
<button class="btn btn-sm btn-outline-secondary" @onclick="() => ShowModal = true">
    Löschen
</button>

@if (ShowModal)
{
    <div class="modal fade show d-block" tabindex="-1" style="background: rgba(0,0,0,0.3)">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Bestätigen</h5>
                    <button type="button" class="btn-close" @onclick="() => ShowModal = false"></button>
                </div>
                <div class="modal-body">
                    <p>Möchten Sie diesen Eintrag wirklich löschen?</p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary btn-sm" @onclick="() => ShowModal = false">Abbrechen</button>
                    <button class="btn btn-danger btn-sm" @onclick="ConfirmDelete">Löschen</button>
                </div>
            </div>
        </div>
    </div>
}

### Loading States

```razor
@if (isLoading)
{
    <div class="d-flex justify-content-center p-3">
        <div class="spinner-border text-secondary" role="status">
            <span class="visually-hidden">Laden...</span>
        </div>
    </div>
}
```

## API-Aufrufe

### Service Layer

```csharp
public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(Guid id);
    Task CreateAsync(CreateProductRequest request);
}

public class ProductService : IProductService
{
    private readonly HttpClient _http;
    
    public ProductService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<IReadOnlyList<ProductDto>>("/api/products")
            ?? [];
    }
}
```

### DI Registration

```csharp
// Program.cs
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
```

## PWA Support

### Service Worker

```json
// wwwroot/service-worker.js
self.resourcesToCache = [
    "./",
    "./index.html",
    "./css/app.css",
    "./_framework/blazor.webassembly.js"
];
```

## Mobile Besonderheiten

### Touch-Optimierung

- Große Buttons (min. 44px)
- Ausreichend Abstand zwischen klickbaren Elementen
- Swipe-Gesten für häufige Aktionen

### Offline-Fähigkeit

- LocalStorage für User-Session
- Queue für Bestellungen wenn offline
- Sync-Mechanismus bei Wiederherstellung

## Referenzen

- [Bootstrap 5 Docs](https://getbootstrap.com/docs/5.3/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [Blazor WebAssembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/)

## Git Workflow

### Ein Projekt = Ein Branch

Für jedes Frontend-Projekt einen eigenen Branch erstellen:

```bash
# Neuen Branch für Projekt erstellen und auschecken
git checkout -b PROJ-6-Auth-Frontend

# Oder für Feature-Branches
git checkout -b feature/veranstalter-crud-frontend
```

**Wichtige Regeln:**
- NIEMALS direkt auf `main` entwickeln
- Erst testen, dann in `main` mergen
- Bei mehreren Sessions: jeder auf eigenem Branch arbeiten
