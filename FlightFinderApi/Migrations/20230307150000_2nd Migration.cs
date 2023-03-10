using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightFinderApi.Migrations
{
    public partial class _2ndMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Flight_id",
                table: "Itineraries",
                newName: "Flight_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Flight_Id",
                table: "Itineraries",
                newName: "Flight_id");
        }
    }
}
