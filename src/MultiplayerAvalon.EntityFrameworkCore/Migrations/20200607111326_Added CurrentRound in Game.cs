using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class AddedCurrentRoundinGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRoundId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentRoundId",
                table: "Games",
                column: "CurrentRoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Rounds_CurrentRoundId",
                table: "Games",
                column: "CurrentRoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Rounds_CurrentRoundId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CurrentRoundId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CurrentRoundId",
                table: "Games");
        }
    }
}
