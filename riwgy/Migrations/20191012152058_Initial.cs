using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace riwgy.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Riwgy = table.Column<string>(nullable: true),
                    OriginalUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMapping", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlMapping_Riwgy",
                table: "UrlMapping",
                column: "Riwgy",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlMapping");
        }
    }
}
