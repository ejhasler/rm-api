using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeletedFKProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Products_ProductId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ProductId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "MenuItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "MenuItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ProductId",
                table: "MenuItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Products_ProductId",
                table: "MenuItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
