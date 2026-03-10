using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.VeranstaltungEndpoints;

public static class UpdateVeranstaltungEndpoint
{
    public class Request
    {
        [Required(ErrorMessage = "Bezeichnung ist erforderlich")]
        [StringLength(100, ErrorMessage = "Bezeichnung darf maximal 100 Zeichen lang sein")]
        public string Bezeichnung { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Beschreibung darf maximal 500 Zeichen lang sein")]
        public string? Beschreibung { get; set; }

        [Required(ErrorMessage = "Start-Datum ist erforderlich")]
        public DateTime Von { get; set; }

        public DateTime? Bis { get; set; }

        public bool Aktiv { get; set; }

        [Required(ErrorMessage = "Veranstalter-ID ist erforderlich")]
        public Guid VeranstalterId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;
        public string? Beschreibung { get; set; }
        public DateTime Von { get; set; }
        public DateTime? Bis { get; set; }
        public bool Aktiv { get; set; }
        public Guid VeranstalterId { get; set; }
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
            Put("/api/veranstaltungen/{id}");
            Roles("VeranstalterAdmin");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var id = Route<Guid>("id");
            var veranstaltung = await _db.Veranstaltungen.FindAsync([id], ct);

            if (veranstaltung is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var veranstalter = await _db.Veranstalter.FindAsync([veranstaltung.VeranstalterId], ct);
            if (veranstalter is null || veranstalter.UserId != userId)
            {
                AddError("Sie sind nicht berechtigt, diese Veranstaltung zu bearbeiten");
                await SendErrorsAsync(403, ct);
                return;
            }

            veranstaltung.Bezeichnung = req.Bezeichnung;
            veranstaltung.Beschreibung = req.Beschreibung;
            veranstaltung.Von = req.Von;
            veranstaltung.Bis = req.Bis;
            veranstaltung.Aktiv = req.Aktiv;
            veranstaltung.VeranstalterId = req.VeranstalterId;

            await _db.SaveChangesAsync(ct);

            Response = new Response
            {
                Id = veranstaltung.Id,
                Bezeichnung = veranstaltung.Bezeichnung,
                Beschreibung = veranstaltung.Beschreibung,
                Von = veranstaltung.Von,
                Bis = veranstaltung.Bis,
                Aktiv = veranstaltung.Aktiv,
                VeranstalterId = veranstaltung.VeranstalterId
            };
        }
    }
}