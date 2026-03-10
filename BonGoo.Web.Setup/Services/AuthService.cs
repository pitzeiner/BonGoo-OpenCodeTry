using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using BonGoo.Shared.DTOs.Auth;

namespace BonGoo.Web.Setup.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
    Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly IServiceProvider _serviceProvider;
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";

    public AuthService(HttpClient http, IServiceProvider serviceProvider)
    {
        _http = http;
        _serviceProvider = serviceProvider;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result is not null)
        {
            await StoreTokensAsync(result.AccessToken, result.RefreshToken);
            NotifyAuthenticationStateChanged();
        }
        return result;
    }

    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        var response = await _http.PostAsJsonAsync("api/auth/refresh", new RefreshTokenRequest { RefreshToken = refreshToken });
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        if (result is not null)
        {
            await StoreTokensAsync(result.AccessToken, result.RefreshToken);
            NotifyAuthenticationStateChanged();
        }
        return result;
    }

    public async Task LogoutAsync()
    {
        await _http.PostAsync("api/auth/logout", null);
        await ClearTokensAsync();
        NotifyAuthenticationStateChanged();
    }

    private void NotifyAuthenticationStateChanged()
    {
        var authState = _serviceProvider.GetService<AuthenticationStateProvider>() as AuthStateProvider;
        authState?.NotifyAuthStateChanged();
    }

    public Task<bool> IsAuthenticatedAsync()
        => IsAuthenticatedInternalAsync();

    public Task<string?> GetAccessTokenAsync()
        => BrowserStorage.GetItemAsync(AccessTokenKey);

    public Task<string?> GetRefreshTokenAsync()
        => BrowserStorage.GetItemAsync(RefreshTokenKey);

    private async Task<bool> IsAuthenticatedInternalAsync()
    {
        var token = await GetAccessTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    private async Task StoreTokensAsync(string accessToken, string refreshToken)
    {
        await BrowserStorage.SetItemAsync(AccessTokenKey, accessToken);
        await BrowserStorage.SetItemAsync(RefreshTokenKey, refreshToken);
        _http.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    private async Task ClearTokensAsync()
    {
        await BrowserStorage.RemoveItemAsync(AccessTokenKey);
        await BrowserStorage.RemoveItemAsync(RefreshTokenKey);
        _http.DefaultRequestHeaders.Authorization = null;
    }
}

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;

    public AuthStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _authService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "User"),
            new Claim(ClaimTypes.Role, "User")
        };
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }

    public async Task NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

public static class BrowserStorage
{
    private static IJSRuntime? _js;

    public static void Initialize(IJSRuntime js) => _js = js;

    public static async Task<string?> GetItemAsync(string key)
    {
        try
        {
            return await _js!.InvokeAsync<string?>("localStorage.getItem", key);
        }
        catch
        {
            return null;
        }
    }

    public static Task SetItemAsync(string key, string value)
    {
        return _js?.InvokeVoidAsync("localStorage.setItem", key, value).AsTask() ?? Task.CompletedTask;
    }

    public static Task RemoveItemAsync(string key)
    {
        return _js?.InvokeVoidAsync("localStorage.removeItem", key).AsTask() ?? Task.CompletedTask;
    }
}
