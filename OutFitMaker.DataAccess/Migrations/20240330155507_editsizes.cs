using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editsizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProductSizes_ProductSizeId",
                schema: "Security",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                schema: "Security",
                table: "Users",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ProductSizeId",
                schema: "Security",
                table: "Users",
                newName: "IX_Users_SizeId");

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                schema: "Main",
                table: "ProductSizes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sizes",
                schema: "Main",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeId",
                schema: "Main",
                table: "ProductSizes",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                schema: "Main",
                table: "ProductSizes",
                column: "SizeId",
                principalSchema: "Main",
                principalTable: "Sizes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Sizes_SizeId",
                schema: "Security",
                table: "Users",
                column: "SizeId",
                principalSchema: "Main",
                principalTable: "Sizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                schema: "Main",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Sizes_SizeId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Sizes",
                schema: "Main");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeId",
                schema: "Main",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "SizeId",
                schema: "Main",
                table: "ProductSizes");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                schema: "Security",
                table: "Users",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SizeId",
                schema: "Security",
                table: "Users",
                newName: "IX_Users_ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProductSizes_ProductSizeId",
                schema: "Security",
                table: "Users",
                column: "ProductSizeId",
                principalSchema: "Main",
                principalTable: "ProductSizes",
                principalColumn: "Id");
        }
    }
}
