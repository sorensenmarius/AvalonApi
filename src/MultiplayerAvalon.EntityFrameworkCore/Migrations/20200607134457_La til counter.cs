using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Latilcounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpeditionResultVote",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ExpeditionVote",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "FailedTeams",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamExpVote",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotesForTeam",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPlayerId",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "counter",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentPlayerId",
                table: "Games",
                column: "CurrentPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_CurrentPlayerId",
                table: "Games",
                column: "CurrentPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_CurrentPlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CurrentPlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FailedTeams",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "TeamExpVote",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "VotesForTeam",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CurrentPlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "counter",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "ExpeditionResultVote",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpeditionVote",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
