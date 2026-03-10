using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonGoo.Web.API.Features.VeranstaltungEndpoints;

public static class GetActiveVeranstaltungenEndpoint
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

    public class Endpoint : EndpointWithoutRequest<List<Response>>
    {
        private readonly BonGooDbContext _db;

        public Endpoint(BonGooDbContext db)
        {
            _db = db;
        }

        public override void Configure()
        {
            Get("/api/veranstaltungen/active");
            Roles("VeranstalterAdmin", "SetupUser");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var veranstaltungen = await _db.Veranstaltungen
                .Where(v => v.Aktiv)
                .Select(v => new Response
                {
                    Id = v.Id,
                    Bezeichnung = v.Bezeichnung,
                    Beschreibung = v.Beschreibung,
                    Von = v.Von,
                    Bis = v.Bis,
                    Aktiv = v.Aktiv,
                    VeranstalterId = v.VeranstalterId
                })
                .ToListAsync(ct);

            Response = veranstaltungen;
        }
    }
}