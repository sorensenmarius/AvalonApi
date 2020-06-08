using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class LatilintcoloumforPointsevilgoodogbad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsEvil",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsInnocent",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsEvil",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PointsInnocent",
                table: "Games");
        }
    }
}
