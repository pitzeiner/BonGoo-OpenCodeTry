using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.VeranstaltungEndpoints;

public static class DeleteVeranstaltungEndpoint
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
            Delete("/api/veranstaltungen/{id}");
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

            var veranstaltung = await _db.Veranstaltungen.FindAsync([id], ct);

            if (veranstaltung is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var veranstalter = await _db.Veranstalter.FindAsync([veranstaltung.VeranstalterId], ct);
            if (veranstalter is null || veranstalter.UserId != userId)
            {
                AddError("Sie sind nicht berechtigt, diese Veranstaltung zu löschen");
                await SendErrorsAsync(403, ct);
                return;
            }

            _db.Veranstaltungen.Remove(veranstaltung);
            await _db.SaveChangesAsync(ct);

            await SendOkAsync(ct);
        }
    }
}