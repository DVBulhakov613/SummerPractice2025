using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class TPTMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Locations_SentFromID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Locations_SentToID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Locations_WarehouseID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "HandlesPublicDropOffs",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsAutomated",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsRegionalHQ",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WarehouseType",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "MaxStorageCapacity",
                table: "Locations",
                newName: "ManagerID");

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Managers_Employees_ID",
                        column: x => x.ID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    MaxStorageCapacity = table.Column<long>(type: "bigint", nullable: false),
                    IsAutomated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Warehouses_Locations_ID",
                        column: x => x.ID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Administrators_Managers_ID",
                        column: x => x.ID,
                        principalTable: "Managers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostalOffices",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    HandlesPublicDropOffs = table.Column<bool>(type: "bit", nullable: false),
                    IsRegionalHQ = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalOffices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostalOffices_Warehouses_ID",
                        column: x => x.ID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ManagerID",
                table: "Locations",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations",
                column: "ManagerID",
                principalTable: "Managers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Warehouses_SentFromID",
                table: "Packages",
                column: "SentFromID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Warehouses_SentToID",
                table: "Packages",
                column: "SentToID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Warehouses_WarehouseID",
                table: "Packages",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_SentFromID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_SentToID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_WarehouseID",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "PostalOffices");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ManagerID",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ManagerID",
                table: "Locations",
                newName: "MaxStorageCapacity");

            migrationBuilder.AddColumn<bool>(
                name: "HandlesPublicDropOffs",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutomated",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegionalHQ",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseType",
                table: "Locations",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Locations_SentFromID",
                table: "Packages",
                column: "SentFromID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Locations_SentToID",
                table: "Packages",
                column: "SentToID",
                principalTable: "Locations",
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
