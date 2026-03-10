using FastEndpoints;
using BonGoo.Web.API.Data;
using BonGoo.Web.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace BonGoo.Web.API.Features.Auth;

public static class RegisterEndpoint
{
    public class Request
    {
        [Required(ErrorMessage = "E-Mail ist erforderlich")]
        [EmailAddress(ErrorMessage = "Ungültiges E-Mail-Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Passwort muss mindestens 8 Zeichen lang sein")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vorname ist erforderlich")]
        [StringLength(50, ErrorMessage = "Vorname darf maximal 50 Zeichen lang sein")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nachname ist erforderlich")]
        [StringLength(50, ErrorMessage = "Nachname darf maximal 50 Zeichen lang sein")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Ungültiges Telefonnummernformat")]
        public string? Phone { get; set; }
    }

    public class Response
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
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
            Post("/api/auth/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email, ct))
            {
                _logger.LogWarning("Registration attempt with existing email {Email}", req.Email);
                AddError(r => r.Email, "E-Mail bereits registriert");
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                FirstName = req.FirstName,
                LastName = req.LastName,
                Phone = req.Phone,
                Role = UserRole.SetupUser,
                RegistrationDate = DateTime.UtcNow,
                IsActive = true,
                VerificationToken = Guid.NewGuid().ToString("N")
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("New user registered: {UserId} ({Email})", user.Id, user.Email);

            Response = new() { UserId = user.Id, Email = user.Email, Message = "Registrierung erfolgreich." };
        }
    }
}