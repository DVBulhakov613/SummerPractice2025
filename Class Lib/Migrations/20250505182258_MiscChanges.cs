using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class MiscChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Coordinates_GeoDataLongitude_GeoDataLatitude",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Locations_GeoDataLongitude_GeoDataLatitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GeoDataLatitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GeoDataLongitude",
                table: "Locations");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Packages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "Packages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Locations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Locations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Locations");

            migrationBuilder.AddColumn<double>(
                name: "GeoDataLatitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GeoDataLongitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => new { x.Longitude, x.Latitude });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_GeoDataLongitude_GeoDataLatitude",
                table: "Locations",
                columns: new[] { "GeoDataLongitude", "GeoDataLatitude" });

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Coordinates_GeoDataLongitude_GeoDataLatitude",
                table: "Locations",
                columns: new[] { "GeoDataLongitude", "GeoDataLatitude" },
                principalTable: "Coordinates",
                principalColumns: new[] { "Longitude", "Latitude" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
