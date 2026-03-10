using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.VeranstalterEndpoints;

public static class DeleteVeranstalterEndpoint
{
    public class Endpoint : Endpoint<Guid>
    {
        private readonly BonGooDbContext _db;

        public Endpoint(BonGooDbContext db)
        {
            _db = db;
        }

        public override void Configure()
        {
            Delete("/api/veranstalter/{id}");
            Roles("VeranstalterAdmin");
        }

        public override async Task HandleAsync(Guid id, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var veranstalter = await _db.Veranstalter.FindAsync([id], ct);

            if (veranstalter is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            if (veranstalter.UserId != userId)
            {
                AddError("Sie sind nicht berechtigt, diesen Veranstalter zu löschen");
                await SendErrorsAsync(403, ct);
                return;
            }

            _db.Veranstalter.Remove(veranstalter);
            await _db.SaveChangesAsync(ct);

            await SendOkAsync(ct);
        }
    }
}