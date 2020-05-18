using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "basket_item_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CustomerBasket",
                columns: table => new
                {
                    BuyerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBasket", x => x.BuyerId);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 50, nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    OldUnitPrice = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CustomerBasketId = table.Column<string>(nullable: true),
                    CustomerBasketBuyerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_CustomerBasket_CustomerBasketBuyerId",
                        column: x => x.CustomerBasketBuyerId,
                        principalTable: "CustomerBasket",
                        principalColumn: "BuyerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketItem_CustomerBasket_CustomerBasketId",
                        column: x => x.CustomerBasketId,
                        principalTable: "CustomerBasket",
                        principalColumn: "BuyerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_CustomerBasketBuyerId",
                table: "BasketItem",
                column: "CustomerBasketBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_CustomerBasketId",
                table: "BasketItem",
                column: "CustomerBasketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.DropTable(
                name: "CustomerBasket");

            migrationBuilder.DropSequence(
                name: "basket_item_hilo");
        }
    }
}
