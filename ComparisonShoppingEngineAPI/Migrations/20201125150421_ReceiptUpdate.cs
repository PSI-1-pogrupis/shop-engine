using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComparisonShoppingEngineAPI.Migrations
{
    public partial class ReceiptUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ReceiptProducts");

            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "ReceiptProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "ReceiptProducts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ReceiptProducts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerQuantity",
                table: "ReceiptProducts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ReceiptProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ReceiptProducts");

            migrationBuilder.DropColumn(
                name: "PricePerQuantity",
                table: "ReceiptProducts");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "ReceiptProducts",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPrice",
                table: "ReceiptProducts",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
