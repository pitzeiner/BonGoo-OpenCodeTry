using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.VeranstalterEndpoints;

public static class UpdateVeranstalterEndpoint
{
    public class Request
    {
        [Required(ErrorMessage = "Bezeichnung ist erforderlich")]
        [StringLength(100, ErrorMessage = "Bezeichnung darf maximal 100 Zeichen lang sein")]
        public string Bezeichnung { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Beschreibung darf maximal 500 Zeichen lang sein")]
        public string? Beschreibung { get; set; }

        [StringLength(10, ErrorMessage = "PLZ darf maximal 10 Zeichen lang sein")]
        public string? Plz { get; set; }

        [StringLength(100, ErrorMessage = "Ort darf maximal 100 Zeichen lang sein")]
        public string? Ort { get; set; }

        [StringLength(200, ErrorMessage = "Strasse darf maximal 200 Zeichen lang sein")]
        public string? Strasse { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;
        public string? Beschreibung { get; set; }
        public string? Plz { get; set; }
        public string? Ort { get; set; }
        public string? Strasse { get; set; }
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
            Put("/api/veranstalter/{id}");
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
            var veranstalter = await _db.Veranstalter.FindAsync([id], ct);

            if (veranstalter is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            if (veranstalter.UserId != userId)
            {
                AddError("Sie sind nicht berechtigt, diesen Veranstalter zu bearbeiten");
                await SendErrorsAsync(403, ct);
                return;
            }

            veranstalter.Bezeichnung = req.Bezeichnung;
            veranstalter.Beschreibung = req.Beschreibung;
            veranstalter.Plz = req.Plz;
            veranstalter.Ort = req.Ort;
            veranstalter.Strasse = req.Strasse;

            await _db.SaveChangesAsync(ct);

            Response = new Response
            {
                Id = veranstalter.Id,
                Bezeichnung = veranstalter.Bezeichnung,
                Beschreibung = veranstalter.Beschreibung,
                Plz = veranstalter.Plz,
                Ort = veranstalter.Ort,
                Strasse = veranstalter.Strasse
            };
        }
    }
}