using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SarajevoGuide.Migrations
{
    /// <inheritdoc />
    public partial class najnovijamigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ocjena",
                table: "Recenzija",
                newName: "Ocjena");

            migrationBuilder.RenameColumn(
                name: "korisnikId",
                table: "Recenzija",
                newName: "KorisnikId");

            migrationBuilder.RenameColumn(
                name: "komentar",
                table: "Recenzija",
                newName: "Komentar");

            migrationBuilder.RenameColumn(
                name: "eventId",
                table: "Recenzija",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Recenzija",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ocjena",
                table: "Recenzija",
                newName: "ocjena");

            migrationBuilder.RenameColumn(
                name: "KorisnikId",
                table: "Recenzija",
                newName: "korisnikId");

            migrationBuilder.RenameColumn(
                name: "Komentar",
                table: "Recenzija",
                newName: "komentar");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Recenzija",
                newName: "eventId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recenzija",
                newName: "id");
        }
    }
}
