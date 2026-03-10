using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BonGoo.Web.API.Features.VeranstalterEndpoints;

public static class CreateVeranstalterEndpoint
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
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(BonGooDbContext db, ILogger<Endpoint> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/api/veranstalter");
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

            var veranstalter = new Entities.Veranstalter
            {
                Id = Guid.NewGuid(),
                Bezeichnung = req.Bezeichnung,
                Beschreibung = req.Beschreibung,
                Plz = req.Plz,
                Ort = req.Ort,
                Strasse = req.Strasse,
                UserId = userId
            };

            _db.Veranstalter.Add(veranstalter);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Veranstalter {VeranstalterId} created by user {UserId}", veranstalter.Id, userId);

            Response = new Response
            {
                Id = veranstalter.Id,
                Bezeichnung = veranstalter.Bezeichnung,
                Beschreibung = veranstalter.Beschreibung,
                Plz = veranstalter.Plz,
                Ort = veranstalter.Ort,
                Strasse = veranstalter.Strasse
            };

            await SendCreatedAtAsync<Endpoint>($"/api/veranstalter/{veranstalter.Id}", Response, cancellation: ct);
        }
    }
}