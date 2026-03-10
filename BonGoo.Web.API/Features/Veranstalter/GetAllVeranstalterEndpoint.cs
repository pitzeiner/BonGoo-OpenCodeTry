using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonGoo.Web.API.Features.VeranstalterEndpoints;

public static class GetAllVeranstalterEndpoint
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

    public class Endpoint : EndpointWithoutRequest<List<Response>>
    {
        private readonly BonGooDbContext _db;

        public Endpoint(BonGooDbContext db)
        {
            _db = db;
        }

        public override void Configure()
        {
            Get("/api/veranstalter");
            Roles("VeranstalterAdmin", "SetupUser");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var veranstalter = await _db.Veranstalter
                .Select(v => new Response
                {
                    Id = v.Id,
                    Bezeichnung = v.Bezeichnung,
                    Beschreibung = v.Beschreibung,
                    Plz = v.Plz,
                    Ort = v.Ort,
                    Strasse = v.Strasse,
                    UserId = v.UserId
                })
                .ToListAsync(ct);

            Response = veranstalter;
        }
    }
}