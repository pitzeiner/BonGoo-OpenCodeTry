using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonGoo.Web.API.Features.VeranstalterEndpoints;

public static class GetVeranstalterByIdEndpoint
{
    public class Response
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;
        public string? Beschreibung { get; set; }
        public string? Plz { get; set; }
        public string? Ort { get; set; }
        public string? Strasse { get; set; }
        public Guid UserId { get; set; }
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
            Get("/api/veranstalter/{id}");
            Roles("VeranstalterAdmin", "SetupUser");
        }

        public override async Task HandleAsync(Guid id, CancellationToken ct)
        {
            var veranstalter = await _db.Veranstalter.FindAsync([id], ct);

            if (veranstalter is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            Response = new Response
            {
                Id = veranstalter.Id,
                Bezeichnung = veranstalter.Bezeichnung,
                Beschreibung = veranstalter.Beschreibung,
                Plz = veranstalter.Plz,
                Ort = veranstalter.Ort,
                Strasse = veranstalter.Strasse,
                UserId = veranstalter.UserId
            };
        }
    }
}