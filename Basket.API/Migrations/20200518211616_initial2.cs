using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.API.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_CustomerBasket_CustomerBasketBuyerId",
                table: "BasketItem");

            migrationBuilder.DropIndex(
                name: "IX_BasketItem_CustomerBasketBuyerId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "CustomerBasketBuyerId",
                table: "BasketItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerBasketBuyerId",
                table: "BasketItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_CustomerBasketBuyerId",
                table: "BasketItem",
                column: "CustomerBasketBuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_CustomerBasket_CustomerBasketBuyerId",
                table: "BasketItem",
                column: "CustomerBasketBuyerId",
                principalTable: "CustomerBasket",
                principalColumn: "BuyerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
