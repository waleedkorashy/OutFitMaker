using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Quentity",
                schema: "Operation",
                table: "OrdersItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                schema: "Operation",
                table: "OrdersItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "Main",
                table: "Logos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Operation",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "Main",
                table: "Logos");

            migrationBuilder.AlterColumn<int>(
                name: "Quentity",
                schema: "Operation",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
