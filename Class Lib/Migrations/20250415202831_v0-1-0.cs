using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class v010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Locations_WorkplaceID",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Packages_PackageID",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_ReceiverID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_SenderID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_DeliveryVehicles_DeliveryVehicleID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_DeliveryVehicles_PackageID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Locations_WarehouseID",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DeliveryVehicles");

            migrationBuilder.DropIndex(
                name: "IX_Packages_DeliveryVehicleID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_WarehouseID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Clients_WorkplaceID",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Content",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "DeliveryVehicleID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "WorkplaceID",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "Contents");

            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "Packages",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PackageID",
                table: "Contents",
                newName: "IX_Contents_PackageID");

            migrationBuilder.AddColumn<int>(
                name: "PackageStatus",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Packages",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Locations",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Coordinates",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Clients",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Contents",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                columns: new[] { "Name", "PackageID" });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkplaceID = table.Column<uint>(type: "INTEGER", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Employees_Locations_WorkplaceID",
                        column: x => x.WorkplaceID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkplaceID",
                table: "Employees",
                column: "WorkplaceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Packages_PackageID",
                table: "Contents",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Clients_ReceiverID",
                table: "Packages",
                column: "ReceiverID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Clients_SenderID",
                table: "Packages",
                column: "SenderID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Packages_PackageID",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_ReceiverID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_SenderID",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PackageStatus",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Contents");

            migrationBuilder.RenameTable(
                name: "Contents",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Packages",
                newName: "PackageID");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_PackageID",
                table: "Content",
                newName: "IX_Content_PackageID");

            migrationBuilder.AddColumn<uint>(
                name: "DeliveryVehicleID",
                table: "Packages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<uint>(
                name: "WarehouseID",
                table: "Packages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Coordinates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Clients",
                type: "TEXT",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "WorkplaceID",
                table: "Clients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                columns: new[] { "Name", "PackageID" });

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

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ISO2Code", "FullName" },
                values: new object[,]
                {
                    { "PL", "Poland" },
                    { "RO", "Romania" },
                    { "UA", "Ukraine" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliveryVehicleID",
                table: "Packages",
                column: "DeliveryVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_WarehouseID",
                table: "Packages",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_WorkplaceID",
                table: "Clients",
                column: "WorkplaceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Locations_WorkplaceID",
                table: "Clients",
                column: "WorkplaceID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Packages_PackageID",
                table: "Content",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "PackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Clients_ReceiverID",
                table: "Packages",
                column: "ReceiverID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Clients_SenderID",
                table: "Packages",
                column: "SenderID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_DeliveryVehicles_DeliveryVehicleID",
                table: "Packages",
                column: "DeliveryVehicleID",
                principalTable: "DeliveryVehicles",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_DeliveryVehicles_PackageID",
                table: "Packages",
                column: "PackageID",
                principalTable: "DeliveryVehicles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Locations_WarehouseID",
                table: "Packages",
                column: "WarehouseID",
                principalTable: "Locations",
                principalColumn: "ID");
        }
    }
}
