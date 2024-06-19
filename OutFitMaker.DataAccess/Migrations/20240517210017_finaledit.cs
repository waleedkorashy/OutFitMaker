using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class finaledit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Logos_LogoId",
                schema: "Main",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Logos",
                schema: "Main");

            migrationBuilder.DropIndex(
                name: "IX_Products_LogoId",
                schema: "Main",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LogoId",
                schema: "Main",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsUnique",
                schema: "Main",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnique",
                schema: "Main",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "LogoId",
                schema: "Main",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Logos",
                schema: "Main",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_LogoId",
                schema: "Main",
                table: "Products",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Logos_LogoId",
                schema: "Main",
                table: "Products",
                column: "LogoId",
                principalSchema: "Main",
                principalTable: "Logos",
                principalColumn: "Id");
        }
    }
}
