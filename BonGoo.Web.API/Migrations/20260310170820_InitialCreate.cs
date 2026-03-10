using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonGoo.Web.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KassaCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    KassaId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    VeranstaltungId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KassaCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Abgabestellen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Einzeldruck = table.Column<bool>(type: "boolean", nullable: false),
                    Kassastelle = table.Column<bool>(type: "boolean", nullable: false),
                    TakeAway = table.Column<bool>(type: "boolean", nullable: false),
                    Drucker = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abgabestellen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CounterProdukte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Reihenfolge = table.Column<int>(type: "integer", nullable: false),
                    AbgabestelleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterProdukte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterProdukte_Abgabestellen_AbgabestelleId",
                        column: x => x.AbgabestelleId,
                        principalTable: "Abgabestellen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produkte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reihenfolge = table.Column<int>(type: "integer", nullable: false),
                    Ausverkauft = table.Column<bool>(type: "boolean", nullable: false),
                    HatCounter = table.Column<bool>(type: "boolean", nullable: false),
                    Preis = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AbgabestelleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CounterProduktId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produkte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produkte_Abgabestellen_AbgabestelleId",
                        column: x => x.AbgabestelleId,
                        principalTable: "Abgabestellen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produkte_CounterProdukte_CounterProduktId",
                        column: x => x.CounterProduktId,
                        principalTable: "CounterProdukte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Bedienungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedienungen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BedienungenBarmittel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Betrag = table.Column<decimal>(type: "numeric", nullable: false),
                    Wechselgeld = table.Column<bool>(type: "boolean", nullable: false),
                    Abfuhr = table.Column<bool>(type: "boolean", nullable: false),
                    BedienungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedienungenBarmittel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedienungenBarmittel_Bedienungen_BedienungId",
                        column: x => x.BedienungId,
                        principalTable: "Bedienungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QrLoginTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    AbgabestelleId = table.Column<Guid>(type: "uuid", nullable: true),
                    BedienungId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UsedByClientInfo = table.Column<string>(type: "text", nullable: true),
                    UsedByIpAddress = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrLoginTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrLoginTokens_Abgabestellen_AbgabestelleId",
                        column: x => x.AbgabestelleId,
                        principalTable: "Abgabestellen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QrLoginTokens_Bedienungen_BedienungId",
                        column: x => x.BedienungId,
                        principalTable: "Bedienungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Bestellungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BestellNr = table.Column<int>(type: "integer", nullable: false),
                    TischNr = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BedienungId = table.Column<Guid>(type: "uuid", nullable: true),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    SammelrechnungId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestellungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bestellungen_Bedienungen_BedienungId",
                        column: x => x.BedienungId,
                        principalTable: "Bedienungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Bons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Anmerkung = table.Column<string>(type: "text", nullable: true),
                    Abgerechnet = table.Column<bool>(type: "boolean", nullable: false),
                    Fremdverpflegung = table.Column<bool>(type: "boolean", nullable: false),
                    Eigenverbrauch = table.Column<bool>(type: "boolean", nullable: false),
                    Einpacken = table.Column<bool>(type: "boolean", nullable: false),
                    Kassiert = table.Column<bool>(type: "boolean", nullable: false),
                    ErzeugtStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AbgerechnetStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Druck = table.Column<bool>(type: "boolean", nullable: false),
                    DruckStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Zurückgestellt = table.Column<bool>(type: "boolean", nullable: false),
                    Selbstabholung = table.Column<bool>(type: "boolean", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    BestellungId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProduktId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bons_Bestellungen_BestellungId",
                        column: x => x.BestellungId,
                        principalTable: "Bestellungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bons_Produkte_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EinAuszahlungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Betroffener = table.Column<string>(type: "text", nullable: false),
                    Einzahlung = table.Column<bool>(type: "boolean", nullable: false),
                    Auszahlung = table.Column<bool>(type: "boolean", nullable: false),
                    Betrag = table.Column<decimal>(type: "numeric", nullable: false),
                    Beschreibung = table.Column<string>(type: "text", nullable: true),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinAuszahlungen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Festführer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Prozente = table.Column<int>(type: "integer", nullable: false),
                    RechnungName = table.Column<string>(type: "text", nullable: true),
                    RechnungStrasseHnr = table.Column<string>(type: "text", nullable: true),
                    RechnungPLZOrt = table.Column<string>(type: "text", nullable: true),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festführer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fremdverpflegungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Empfänger = table.Column<string>(type: "text", nullable: false),
                    BonText = table.Column<string>(type: "text", nullable: false),
                    Anzahl = table.Column<int>(type: "integer", nullable: false),
                    Ausgedruckt = table.Column<bool>(type: "boolean", nullable: false),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fremdverpflegungen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sammelrechnungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "text", nullable: false),
                    Aktiv = table.Column<bool>(type: "boolean", nullable: false),
                    HatFestführer = table.Column<bool>(type: "boolean", nullable: false),
                    FestführerId = table.Column<Guid>(type: "uuid", nullable: true),
                    VeranstaltungId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sammelrechnungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sammelrechnungen_Festführer_FestführerId",
                        column: x => x.FestführerId,
                        principalTable: "Festführer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    VerificationToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VerificationTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PasswordResetTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    VeranstalterId = table.Column<Guid>(type: "uuid", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veranstalter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Beschreibung = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    Plz = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Ort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Strasse = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veranstalter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veranstalter_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veranstaltungen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Beschreibung = table.Column<string>(type: "text", nullable: true),
                    Von = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Bis = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aktiv = table.Column<bool>(type: "boolean", nullable: false),
                    VeranstalterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veranstaltungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veranstaltungen_Veranstalter_VeranstalterId",
                        column: x => x.VeranstalterId,
                        principalTable: "Veranstalter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abgabestellen_VeranstaltungId",
                table: "Abgabestellen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bedienungen_VeranstaltungId",
                table: "Bedienungen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_BedienungenBarmittel_BedienungId",
                table: "BedienungenBarmittel",
                column: "BedienungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellungen_BedienungId",
                table: "Bestellungen",
                column: "BedienungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellungen_SammelrechnungId",
                table: "Bestellungen",
                column: "SammelrechnungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestellungen_VeranstaltungId",
                table: "Bestellungen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bons_BestellungId",
                table: "Bons",
                column: "BestellungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bons_ProduktId",
                table: "Bons",
                column: "ProduktId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterProdukte_AbgabestelleId",
                table: "CounterProdukte",
                column: "AbgabestelleId");

            migrationBuilder.CreateIndex(
                name: "IX_EinAuszahlungen_VeranstaltungId",
                table: "EinAuszahlungen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Festführer_VeranstaltungId",
                table: "Festführer",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Fremdverpflegungen_VeranstaltungId",
                table: "Fremdverpflegungen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Produkte_AbgabestelleId",
                table: "Produkte",
                column: "AbgabestelleId");

            migrationBuilder.CreateIndex(
                name: "IX_Produkte_CounterProduktId",
                table: "Produkte",
                column: "CounterProduktId");

            migrationBuilder.CreateIndex(
                name: "IX_QrLoginTokens_AbgabestelleId",
                table: "QrLoginTokens",
                column: "AbgabestelleId");

            migrationBuilder.CreateIndex(
                name: "IX_QrLoginTokens_BedienungId",
                table: "QrLoginTokens",
                column: "BedienungId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sammelrechnungen_FestführerId",
                table: "Sammelrechnungen",
                column: "FestführerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sammelrechnungen_VeranstaltungId",
                table: "Sammelrechnungen",
                column: "VeranstaltungId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_VeranstalterId",
                table: "Users",
                column: "VeranstalterId");

            migrationBuilder.CreateIndex(
                name: "IX_Veranstalter_UserId",
                table: "Veranstalter",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Veranstaltungen_VeranstalterId",
                table: "Veranstaltungen",
                column: "VeranstalterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abgabestellen_Veranstaltungen_VeranstaltungId",
                table: "Abgabestellen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bedienungen_Veranstaltungen_VeranstaltungId",
                table: "Bedienungen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellungen_Sammelrechnungen_SammelrechnungId",
                table: "Bestellungen",
                column: "SammelrechnungId",
                principalTable: "Sammelrechnungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellungen_Veranstaltungen_VeranstaltungId",
                table: "Bestellungen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EinAuszahlungen_Veranstaltungen_VeranstaltungId",
                table: "EinAuszahlungen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Festführer_Veranstaltungen_VeranstaltungId",
                table: "Festführer",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fremdverpflegungen_Veranstaltungen_VeranstaltungId",
                table: "Fremdverpflegungen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sammelrechnungen_Veranstaltungen_VeranstaltungId",
                table: "Sammelrechnungen",
                column: "VeranstaltungId",
                principalTable: "Veranstaltungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Veranstalter_VeranstalterId",
                table: "Users",
                column: "VeranstalterId",
                principalTable: "Veranstalter",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veranstalter_Users_UserId",
                table: "Veranstalter");

            migrationBuilder.DropTable(
                name: "BedienungenBarmittel");

            migrationBuilder.DropTable(
                name: "Bons");

            migrationBuilder.DropTable(
                name: "EinAuszahlungen");

            migrationBuilder.DropTable(
                name: "Fremdverpflegungen");

            migrationBuilder.DropTable(
                name: "KassaCodes");

            migrationBuilder.DropTable(
                name: "QrLoginTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Bestellungen");

            migrationBuilder.DropTable(
                name: "Produkte");

            migrationBuilder.DropTable(
                name: "Bedienungen");

            migrationBuilder.DropTable(
                name: "Sammelrechnungen");

            migrationBuilder.DropTable(
                name: "CounterProdukte");

            migrationBuilder.DropTable(
                name: "Festführer");

            migrationBuilder.DropTable(
                name: "Abgabestellen");

            migrationBuilder.DropTable(
                name: "Veranstaltungen");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Veranstalter");
        }
    }
}
