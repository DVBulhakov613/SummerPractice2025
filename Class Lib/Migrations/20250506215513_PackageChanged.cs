using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class PackageChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Locations_CurrentLocationID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_WarehouseID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_CurrentLocationID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CurrentLocationID",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "WarehouseID",
                table: "Packages",
                newName: "StoredInWarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_WarehouseID",
                table: "Packages",
                newName: "IX_Packages_StoredInWarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Warehouses_StoredInWarehouseID",
                table: "Packages",
                column: "StoredInWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_StoredInWarehouseID",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "StoredInWarehouseID",
                table: "Packages",
                newName: "WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_StoredInWarehouseID",
                table: "Packages",
                newName: "IX_Packages_WarehouseID");

            migrationBuilder.AddColumn<long>(
                name: "CurrentLocationID",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_CurrentLocationID",
                table: "Packages",
                column: "CurrentLocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Locations_CurrentLocationID",
                table: "Packages",
                column: "CurrentLocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Warehouses_WarehouseID",
                table: "Packages",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID");
        }
    }
}
