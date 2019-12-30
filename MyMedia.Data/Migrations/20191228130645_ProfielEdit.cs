using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMedia.Migrations
{
    public partial class ProfielEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavorieteKleur",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavorieteKleur",
                table: "AspNetUsers");
        }
    }
}
