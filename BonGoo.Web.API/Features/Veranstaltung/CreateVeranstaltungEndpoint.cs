using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace BonGoo.Web.API.Features.VeranstaltungEndpoints;

public static class CreateVeranstaltungEndpoint
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
            Post("/api/veranstaltungen");
            Roles("VeranstalterAdmin");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var veranstaltung = new Entities.Veranstaltung
            {
                Id = Guid.NewGuid(),
                Bezeichnung = req.Bezeichnung,
                Beschreibung = req.Beschreibung,
                Von = req.Von,
                Bis = req.Bis,
                Aktiv = req.Aktiv,
                VeranstalterId = req.VeranstalterId
            };

            _db.Veranstaltungen.Add(veranstaltung);
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

            await SendCreatedAtAsync<Endpoint>($"/api/veranstaltungen/{veranstaltung.Id}", Response, cancellation: ct);
        }
    }
}