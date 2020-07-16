using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Added_PreviousRounds_To_Game : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Rounds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_GameId",
                table: "Rounds",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Games_GameId",
                table: "Rounds",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Games_GameId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_GameId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Rounds");
        }
    }
}
