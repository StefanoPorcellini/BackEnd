using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esercizio_Pizzeria_In_Forno.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductToOrders_Orders_OrderId",
                table: "ProductToOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductToOrders_OrderId",
                table: "ProductToOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductToOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductToOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductToOrders_OrderId",
                table: "ProductToOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductToOrders_Orders_OrderId",
                table: "ProductToOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
