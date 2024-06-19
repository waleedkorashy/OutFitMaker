using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addordernumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                schema: "Operation",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "Operation",
                table: "Orders");
        }
    }
}
