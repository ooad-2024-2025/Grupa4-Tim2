using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SarajevoGuide.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys depending on RegistrovaniKorisnik.Id first
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_RegistrovaniKorisnik_UserId",
                table: "Bookmark");

            // Drop indexes that use RegistrovaniKorisnik.Id
            migrationBuilder.DropIndex(
                name: "IX_Bookmark_UserId",
                table: "Bookmark");

            // Drop old 'id' column (int identity)
            migrationBuilder.DropColumn(
                name: "id",
                table: "RegistrovaniKorisnik");

            // Add new 'Id' column of type string, non-nullable
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // Add new Identity-related columns
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "RegistrovaniKorisnik",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "RegistrovaniKorisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "RegistrovaniKorisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "RegistrovaniKorisnik",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "RegistrovaniKorisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "RegistrovaniKorisnik",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "RegistrovaniKorisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Recreate indexes on RegistrovaniKorisnik
            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "RegistrovaniKorisnik",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "RegistrovaniKorisnik",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            // Recreate foreign key and index on Bookmark.UserId -> RegistrovaniKorisnik.Id
            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_UserId",
                table: "Bookmark",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_RegistrovaniKorisnik_UserId",
                table: "Bookmark",
                column: "UserId",
                principalTable: "RegistrovaniKorisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys and indexes
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_RegistrovaniKorisnik_UserId",
                table: "Bookmark");

            migrationBuilder.DropIndex(
                name: "IX_Bookmark_UserId",
                table: "Bookmark");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "RegistrovaniKorisnik");

            // Drop new Identity columns
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "RegistrovaniKorisnik");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "RegistrovaniKorisnik");

            // Drop new 'Id' column (string)
            migrationBuilder.DropColumn(
                name: "Id",
                table: "RegistrovaniKorisnik");

            // Add back old 'id' column (int identity)
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "RegistrovaniKorisnik",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Recreate index and foreign key on Bookmark.UserId -> RegistrovaniKorisnik.id
            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_UserId",
                table: "Bookmark",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_RegistrovaniKorisnik_UserId",
                table: "Bookmark",
                column: "UserId",
                principalTable: "RegistrovaniKorisnik",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
