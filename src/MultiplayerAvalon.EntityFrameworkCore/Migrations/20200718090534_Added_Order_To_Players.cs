using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Added_Order_To_Players : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "counter",
                table: "Games",
                newName: "Counter");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Players",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Counter",
                table: "Games",
                newName: "counter");
        }
    }
}
