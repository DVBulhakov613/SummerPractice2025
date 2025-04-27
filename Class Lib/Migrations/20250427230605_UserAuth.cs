using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class UserAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "FirstName");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    PersonID = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Users_Employees_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Clients",
                newName: "Name");
        }
    }
}
