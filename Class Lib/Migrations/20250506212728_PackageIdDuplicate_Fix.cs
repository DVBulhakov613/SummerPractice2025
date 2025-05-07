using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class PackageIdDuplicate_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "Packages",
                newName: "Width");

            migrationBuilder.AddColumn<long>(
                name: "Height",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Packages",
                newName: "PackageID");
        }
    }
}
