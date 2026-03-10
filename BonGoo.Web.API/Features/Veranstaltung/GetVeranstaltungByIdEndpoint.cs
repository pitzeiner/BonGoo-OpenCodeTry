using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;

namespace BonGoo.Web.API.Features.VeranstaltungEndpoints;

public static class GetVeranstaltungByIdEndpoint
{
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

    public class Endpoint : Endpoint<Guid, Response>
    {
        private readonly BonGooDbContext _db;

        public Endpoint(BonGooDbContext db)
        {
            _db = db;
        }

        public override void Configure()
        {
            Get("/api/veranstaltungen/{id}");
            Roles("VeranstalterAdmin", "SetupUser");
        }

        public override async Task HandleAsync(Guid id, CancellationToken ct)
        {
            var veranstaltung = await _db.Veranstaltungen.FindAsync([id], ct);

            if (veranstaltung is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

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