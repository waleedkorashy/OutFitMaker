using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutFitMaker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class finaledit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sleeve",
                schema: "Main",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                schema: "Main",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                schema: "Main",
                table: "Categories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Sleeve",
                schema: "Main",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
