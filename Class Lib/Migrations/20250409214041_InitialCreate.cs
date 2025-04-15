using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => new { x.Longitude, x.Latitude });
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ISO2Code = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ISO2Code);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryVehicles",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxStorageCapacity = table.Column<uint>(type: "INTEGER", nullable: false),
                    MaxVolume = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryVehicles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeoDataLongitude = table.Column<double>(type: "REAL", nullable: false),
                    GeoDataLatitude = table.Column<double>(type: "REAL", nullable: false),
                    WarehouseType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    MaxStorageCapacity = table.Column<uint>(type: "INTEGER", nullable: true),
                    IsAutomated = table.Column<bool>(type: "INTEGER", nullable: true),
                    HandlesPublicDropOffs = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsRegionalHQ = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Coordinates_GeoDataLongitude_GeoDataLatitude",
                        columns: x => new { x.GeoDataLongitude, x.GeoDataLatitude },
                        principalTable: "Coordinates",
                        principalColumns: new[] { "Longitude", "Latitude" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    WorkplaceID = table.Column<uint>(type: "INTEGER", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Clients_Locations_WorkplaceID",
                        column: x => x.WorkplaceID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageID = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderID = table.Column<uint>(type: "INTEGER", nullable: false),
                    ReceiverID = table.Column<uint>(type: "INTEGER", nullable: false),
                    SentFromID = table.Column<uint>(type: "INTEGER", nullable: false),
                    SentToID = table.Column<uint>(type: "INTEGER", nullable: false),
                    CurrentLocationLongitude = table.Column<double>(type: "REAL", nullable: false),
                    CurrentLocationLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryVehicleID = table.Column<uint>(type: "INTEGER", nullable: true),
                    WarehouseID = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageID);
                    table.ForeignKey(
                        name: "FK_Packages_Clients_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Packages_Clients_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Packages_Coordinates_CurrentLocationLongitude_CurrentLocationLatitude",
                        columns: x => new { x.CurrentLocationLongitude, x.CurrentLocationLatitude },
                        principalTable: "Coordinates",
                        principalColumns: new[] { "Longitude", "Latitude" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Packages_DeliveryVehicles_DeliveryVehicleID",
                        column: x => x.DeliveryVehicleID,
                        principalTable: "DeliveryVehicles",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Packages_DeliveryVehicles_PackageID",
                        column: x => x.PackageID,
                        principalTable: "DeliveryVehicles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Packages_Locations_SentFromID",
                        column: x => x.SentFromID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Packages_Locations_SentToID",
                        column: x => x.SentToID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Packages_Locations_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Locations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PackageID = table.Column<uint>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => new { x.Name, x.PackageID });
                    table.ForeignKey(
                        name: "FK_Content_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageEvents",
                columns: table => new
                {
                    PackageID = table.Column<uint>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LocationLongitude = table.Column<double>(type: "REAL", nullable: false),
                    LocationLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageEvents", x => new { x.Timestamp, x.PackageID });
                    table.ForeignKey(
                        name: "FK_PackageEvents_Coordinates_LocationLongitude_LocationLatitude",
                        columns: x => new { x.LocationLongitude, x.LocationLatitude },
                        principalTable: "Coordinates",
                        principalColumns: new[] { "Longitude", "Latitude" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageEvents_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_WorkplaceID",
                table: "Clients",
                column: "WorkplaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Content_PackageID",
                table: "Content",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_GeoDataLongitude_GeoDataLatitude",
                table: "Locations",
                columns: new[] { "GeoDataLongitude", "GeoDataLatitude" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageEvents_LocationLongitude_LocationLatitude",
                table: "PackageEvents",
                columns: new[] { "LocationLongitude", "LocationLatitude" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageEvents_PackageID",
                table: "PackageEvents",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_CurrentLocationLongitude_CurrentLocationLatitude",
                table: "Packages",
                columns: new[] { "CurrentLocationLongitude", "CurrentLocationLatitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliveryVehicleID",
                table: "Packages",
                column: "DeliveryVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ReceiverID",
                table: "Packages",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_SenderID",
                table: "Packages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_SentFromID",
                table: "Packages",
                column: "SentFromID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_SentToID",
                table: "Packages",
                column: "SentToID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_WarehouseID",
                table: "Packages",
                column: "WarehouseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "PackageEvents");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "DeliveryVehicles");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Coordinates");
        }
    }
}
