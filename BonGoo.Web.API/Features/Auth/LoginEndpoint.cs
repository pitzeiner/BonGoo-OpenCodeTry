using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace BonGoo.Web.API.Features.Auth;

public static class LoginEndpoint
{
    public class Request
    {
        [Required(ErrorMessage = "E-Mail ist erforderlich")]
        [EmailAddress(ErrorMessage = "Ungültiges E-Mail-Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Passwort muss mindestens 6 Zeichen lang sein")]
        public string Password { get; set; } = string.Empty;
    }

    public class Response
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class Endpoint : Endpoint<Request, Response>
    {
        private readonly BonGooDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(BonGooDbContext db, IConfiguration config, ILogger<Endpoint> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email, ct);

            if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for {Email}", req.Email);
                AddError("Ungültige Anmeldedaten");
                await SendErrorsAsync(401, ct);
                return;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login attempt for deactivated user {Email}", req.Email);
                AddError("Benutzerkonto ist deaktiviert");
                await SendErrorsAsync(403, ct);
                return;
            }

            var accessToken = GenerateJwtToken(user);
            var refreshToken = Guid.NewGuid().ToString("N");

            var rt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("User {UserId} ({Email}) logged in successfully", user.Id, user.Email);

            Response = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}