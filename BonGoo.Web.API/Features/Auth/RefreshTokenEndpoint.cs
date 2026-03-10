using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace BonGoo.Web.API.Features.Auth;

public static class RefreshTokenEndpoint
{
    public class Request
    {
        [Required(ErrorMessage = "Refresh Token ist erforderlich")]
        [StringLength(64, MinimumLength = 32, ErrorMessage = "Ungültiges Token-Format")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class Response
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class Endpoint : Endpoint<Request, Response>
    {
        private readonly BonGooDbContext _db;
        private readonly IConfiguration _config;

        public Endpoint(BonGooDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public override void Configure()
        {
            Post("/api/auth/refresh");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var rt = await _db.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == req.RefreshToken, ct);

            if (rt is null || rt.ExpiresAt < DateTime.UtcNow || rt.User is null)
            {
                AddError("Ungültiger oder abgelaufener Refresh Token");
                await SendErrorsAsync(401, ct);
                return;
            }

            if (!rt.User.IsActive)
            {
                AddError("Benutzerkonto ist deaktiviert");
                await SendErrorsAsync(403, ct);
                return;
            }

            _db.RefreshTokens.Remove(rt);

            var accessToken = GenerateJwtToken(rt.User);
            var newRefreshToken = Guid.NewGuid().ToString("N");

            var newRt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
                UserId = rt.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _db.RefreshTokens.Add(newRt);
            await _db.SaveChangesAsync(ct);

            Response = new() { AccessToken = accessToken, RefreshToken = newRefreshToken };
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