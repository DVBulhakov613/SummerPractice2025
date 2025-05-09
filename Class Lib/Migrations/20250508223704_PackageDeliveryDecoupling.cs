using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_Lib.Migrations
{
    /// <inheritdoc />
    public partial class PackageDeliveryDecoupling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_ReceiverID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Clients_SenderID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_SentFromID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Warehouses_SentToID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ReceiverID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_SenderID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_SentFromID",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_SentToID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ReceiverID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "SentFromID",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "SentToID",
                table: "Packages",
                newName: "DeliveryID");

            migrationBuilder.AddColumn<long>(
                name: "DeliveryID",
                table: "PackageEvents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackageID = table.Column<long>(type: "bigint", nullable: false),
                    SenderID = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverID = table.Column<long>(type: "bigint", nullable: false),
                    SentFromID = table.Column<long>(type: "bigint", nullable: false),
                    SentToID = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Deliveries_Clients_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Clients_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deliveries_Warehouses_SentFromID",
                        column: x => x.SentFromID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Warehouses_SentToID",
                        column: x => x.SentToID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageEvents_DeliveryID",
                table: "PackageEvents",
                column: "DeliveryID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_PackageID",
                table: "Deliveries",
                column: "PackageID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ReceiverID",
                table: "Deliveries",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SenderID",
                table: "Deliveries",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SentFromID",
                table: "Deliveries",
                column: "SentFromID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SentToID",
                table: "Deliveries",
                column: "SentToID");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageEvents_Deliveries_DeliveryID",
                table: "PackageEvents",
                column: "DeliveryID",
                principalTable: "Deliveries",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageEvents_Deliveries_DeliveryID",
                table: "PackageEvents");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_PackageEvents_DeliveryID",
                table: "PackageEvents");

            migrationBuilder.DropColumn(
                name: "DeliveryID",
                table: "PackageEvents");

            migrationBuilder.RenameColumn(
                name: "DeliveryID",
                table: "Packages",
                newName: "SentToID");

            migrationBuilder.AddColumn<long>(
                name: "ReceiverID",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SenderID",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SentFromID",
                table: "Packages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "Packages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
        }
    }
}
