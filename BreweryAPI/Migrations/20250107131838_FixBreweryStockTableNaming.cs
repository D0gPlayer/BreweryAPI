using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreweryAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixBreweryStockTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreweryStock_Beers_BeerId",
                table: "BreweryStock");

            migrationBuilder.DropForeignKey(
                name: "FK_BreweryStock_Breweries_BreweryId",
                table: "BreweryStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BreweryStock",
                table: "BreweryStock");

            migrationBuilder.RenameTable(
                name: "BreweryStock",
                newName: "BreweryStocks");

            migrationBuilder.RenameIndex(
                name: "IX_BreweryStock_BreweryId",
                table: "BreweryStocks",
                newName: "IX_BreweryStocks_BreweryId");

            migrationBuilder.RenameIndex(
                name: "IX_BreweryStock_BeerId",
                table: "BreweryStocks",
                newName: "IX_BreweryStocks_BeerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BreweryStocks",
                table: "BreweryStocks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BreweryStocks_Beers_BeerId",
                table: "BreweryStocks",
                column: "BeerId",
                principalTable: "Beers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BreweryStocks_Breweries_BreweryId",
                table: "BreweryStocks",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreweryStocks_Beers_BeerId",
                table: "BreweryStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_BreweryStocks_Breweries_BreweryId",
                table: "BreweryStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BreweryStocks",
                table: "BreweryStocks");

            migrationBuilder.RenameTable(
                name: "BreweryStocks",
                newName: "BreweryStock");

            migrationBuilder.RenameIndex(
                name: "IX_BreweryStocks_BreweryId",
                table: "BreweryStock",
                newName: "IX_BreweryStock_BreweryId");

            migrationBuilder.RenameIndex(
                name: "IX_BreweryStocks_BeerId",
                table: "BreweryStock",
                newName: "IX_BreweryStock_BeerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BreweryStock",
                table: "BreweryStock",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BreweryStock_Beers_BeerId",
                table: "BreweryStock",
                column: "BeerId",
                principalTable: "Beers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BreweryStock_Breweries_BreweryId",
                table: "BreweryStock",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "Id");
        }
    }
}
