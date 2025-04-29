using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class Manager_ManagedLocsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations",
                column: "ManagerID",
                principalTable: "Managers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Managers_ManagerID",
                table: "Locations",
                column: "ManagerID",
                principalTable: "Managers",
                principalColumn: "ID");
        }
    }
}
