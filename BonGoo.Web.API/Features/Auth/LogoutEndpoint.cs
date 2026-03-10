using FastEndpoints;
using BonGoo.Web.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.Auth;

public static class LogoutEndpoint
{
    public class Request { }

    public class Response
    {
        public string Message { get; set; } = string.Empty;
    }

    public class Endpoint : Endpoint<Request, Response>
    {
        private readonly BonGooDbContext _db;

        public Endpoint(BonGooDbContext db)
        {
            _db = db;
        }

        public override void Configure()
        {
            Post("/api/auth/logout");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var refreshTokens = await _db.RefreshTokens.Where(r => r.UserId == userId).ToListAsync(ct);
            _db.RefreshTokens.RemoveRange(refreshTokens);
            await _db.SaveChangesAsync(ct);

            Response = new() { Message = "Erfolgreich abgemeldet" };
        }
    }
}