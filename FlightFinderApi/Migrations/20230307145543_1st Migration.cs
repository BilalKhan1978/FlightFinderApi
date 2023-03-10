using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightFinderApi.Migrations
{
    public partial class _1stMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adult = table.Column<double>(type: "float", nullable: false),
                    Child = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roots",
                columns: table => new
                {
                    Route_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalDestination = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roots", x => x.Route_Id);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Flight_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    PricesId = table.Column<int>(type: "int", nullable: false),
                    RootRoute_Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Flight_id);
                    table.ForeignKey(
                        name: "FK_Itineraries_Prices_PricesId",
                        column: x => x.PricesId,
                        principalTable: "Prices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itineraries_Roots_RootRoute_Id",
                        column: x => x.RootRoute_Id,
                        principalTable: "Roots",
                        principalColumn: "Route_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_PricesId",
                table: "Itineraries",
                column: "PricesId");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_RootRoute_Id",
                table: "Itineraries",
                column: "RootRoute_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Roots");
        }
    }
}
