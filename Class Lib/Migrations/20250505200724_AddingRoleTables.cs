using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class AddingRoleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.AddColumn<long>(
                name: "RoleID",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleID = table.Column<long>(type: "bigint", nullable: false),
                    PermissionID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1L, "LocalPermissions" },
                    { 2L, "GlobalPermissions" },
                    { 10L, "ReadPackage" },
                    { 11L, "CreatePackage" },
                    { 12L, "UpdatePackage" },
                    { 13L, "DeletePackage" },
                    { 20L, "ReadEvent" },
                    { 21L, "CreateEvent" },
                    { 22L, "UpdateEvent" },
                    { 23L, "DeleteEvent" },
                    { 30L, "ReadContent" },
                    { 31L, "CreateContent" },
                    { 32L, "UpdateContent" },
                    { 33L, "DeleteContent" },
                    { 40L, "ReadPerson" },
                    { 41L, "CreatePerson" },
                    { 42L, "UpdatePerson" },
                    { 43L, "DeletePerson" },
                    { 50L, "ReadLocation" },
                    { 51L, "CreateLocation" },
                    { 52L, "UpdateLocation" },
                    { 53L, "DeleteLocation" },
                    { 60L, "ReadReport" },
                    { 61L, "CreateReport" },
                    { 62L, "UpdateReport" },
                    { 63L, "DeleteReport" },
                    { 70L, "ReadDeliveryVehicle" },
                    { 71L, "CreateDeliveryVehicle" },
                    { 72L, "UpdateDeliveryVehicle" },
                    { 73L, "DeleteDeliveryVehicle" },
                    { 80L, "ReadContentType" },
                    { 81L, "CreateContentType" },
                    { 82L, "UpdateContentType" },
                    { 83L, "DeleteContentType" },
                    { 90L, "ReadPackageStatus" },
                    { 91L, "CreatePackageStatus" },
                    { 92L, "UpdatePackageStatus" },
                    { 93L, "DeletePackageStatus" },
                    { 100L, "ReadPackageType" },
                    { 101L, "CreatePackageType" },
                    { 102L, "UpdatePackageType" },
                    { 103L, "DeletePackageType" },
                    { 110L, "ReadCountry" },
                    { 111L, "CreateCountry" },
                    { 112L, "UpdateCountry" },
                    { 113L, "DeleteCountry" },
                    { 120L, "ReadUser" },
                    { 121L, "CreateUser" },
                    { 122L, "UpdateUser" },
                    { 123L, "DeleteUser" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleID",
                table: "Employees",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionID",
                table: "RolePermissions",
                column: "PermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleID",
                table: "Employees",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
