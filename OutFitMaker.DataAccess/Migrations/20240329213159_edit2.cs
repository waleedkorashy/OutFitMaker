using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class edit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "weight",
                schema: "Security",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSizeId",
                schema: "Security",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProductSizeId",
                schema: "Security",
                table: "Users",
                column: "ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProductSizes_ProductSizeId",
                schema: "Security",
                table: "Users",
                column: "ProductSizeId",
                principalSchema: "Main",
                principalTable: "ProductSizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProductSizes_ProductSizeId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProductSizeId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                schema: "Security",
                table: "Users");

            migrationBuilder.AddColumn<double>(
                name: "Height",
                schema: "Security",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "weight",
                schema: "Security",
                table: "Users",
                type: "float",
                nullable: true);
        }
    }
}
