using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ISO2Code", "FullName" },
                values: new object[,]
                {
                    { "PL", "Poland" },
                    { "RO", "Romania" },
                    { "UA", "Ukraine" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ISO2Code",
                keyValue: "PL");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ISO2Code",
                keyValue: "RO");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ISO2Code",
                keyValue: "UA");
        }
    }
}
