using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGround.Migrations
{
    public partial class NameLengthUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PointOfInterests",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PointOfInterests",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
