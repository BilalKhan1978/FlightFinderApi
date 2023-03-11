using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightFinderApi.Migrations
{
    public partial class Userupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_Roots_RootRoute_Id",
                table: "Itineraries");

            migrationBuilder.AddColumn<byte[]>(
                name: "PassHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PassSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "RootRoute_Id",
                table: "Itineraries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_Roots_RootRoute_Id",
                table: "Itineraries",
                column: "RootRoute_Id",
                principalTable: "Roots",
                principalColumn: "Route_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_Roots_RootRoute_Id",
                table: "Itineraries");

            migrationBuilder.DropColumn(
                name: "PassHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PassSalt",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "RootRoute_Id",
                table: "Itineraries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_Roots_RootRoute_Id",
                table: "Itineraries",
                column: "RootRoute_Id",
                principalTable: "Roots",
                principalColumn: "Route_Id");
        }
    }
}
