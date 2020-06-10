using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class AddedCurrentTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoundId1",
                table: "Players",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rounds_RoundId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RoundId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoundId1",
                table: "Players");
        }
    }
}
