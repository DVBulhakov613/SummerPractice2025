using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class PriceChangedToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Deliveries",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Deliveries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
