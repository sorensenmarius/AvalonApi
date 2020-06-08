using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Endretgruppa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rounds_RoundId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RoundId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ExpFailureVote",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "TeamExpVote",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "RoundId1",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "MissionVoteBad",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MissionVoteGood",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotesAgainstTeam",
                table: "Rounds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MissionVoteBad",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "MissionVoteGood",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "VotesAgainstTeam",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "ExpFailureVote",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamExpVote",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RoundId1",
                table: "Players",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_RoundId1",
                table: "Players",
                column: "RoundId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Rounds_RoundId1",
                table: "Players",
                column: "RoundId1",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
