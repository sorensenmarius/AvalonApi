using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Added_TeamString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamString",
                table: "Rounds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamString",
                table: "Rounds");
        }
    }
}
