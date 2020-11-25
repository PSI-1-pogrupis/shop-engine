using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComparisonShoppingEngineAPI.Migrations
{
    public partial class ReceiptProduct_Amount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "ReceiptProducts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ReceiptProducts");
        }
    }
}
