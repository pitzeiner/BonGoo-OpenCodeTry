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
    string? GetAccessToken();
    string? GetRefreshToken();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
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
            await (_authStateProvider as AuthStateProvider)!.NotifyAuthStateChanged();
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
            await (_authStateProvider as AuthStateProvider)!.NotifyAuthStateChanged();
        }
        return result;
    }

    public async Task LogoutAsync()
    {
        await _http.PostAsync("api/auth/logout", null);
        await ClearTokensAsync();
        await (_authStateProvider as AuthStateProvider)!.NotifyAuthStateChanged();
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        var token = GetAccessToken();
        return Task.FromResult(!string.IsNullOrEmpty(token));
    }

    public string? GetAccessToken() => 
        BrowserStorage.GetItem(AccessTokenKey);

    public string? GetRefreshToken() => 
        BrowserStorage.GetItem(RefreshTokenKey);

    private async Task StoreTokensAsync(string accessToken, string refreshToken)
    {
        BrowserStorage.SetItem(AccessTokenKey, accessToken);
        BrowserStorage.SetItem(RefreshTokenKey, refreshToken);
        _http.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    private async Task ClearTokensAsync()
    {
        BrowserStorage.RemoveItem(AccessTokenKey);
        BrowserStorage.RemoveItem(RefreshTokenKey);
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
        var token = _authService.GetAccessToken();
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

    public static string? GetItem(string key)
    {
        try
        {
            var task = _js!.InvokeAsync<string>("localStorage.getItem", key);
            return task.IsCompleted ? task.Result : null;
        }
        catch
        {
            return null;
        }
    }

    public static void SetItem(string key, string value)
    {
        _js?.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public static void RemoveItem(string key)
    {
        _js?.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
