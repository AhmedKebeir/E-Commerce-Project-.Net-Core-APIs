using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_DeliveryMethods_DeliveryMethodId",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_LName",
                table: "Orders",
                newName: "ShippingAddress_LastName");

            migrationBuilder.RenameIndex(
                name: "IX_orders_DeliveryMethodId",
                table: "Orders",
                newName: "IX_Orders_DeliveryMethodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_LastName",
                table: "orders",
                newName: "ShippingAddress_LName");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "orders",
                newName: "IX_orders_DeliveryMethodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_DeliveryMethods_DeliveryMethodId",
                table: "orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
