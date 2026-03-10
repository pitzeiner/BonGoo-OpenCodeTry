using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonGoo.Web.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenToQrLoginToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "QrLoginTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "QrLoginTokens",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "QrLoginTokens");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "QrLoginTokens");
        }
    }
}
