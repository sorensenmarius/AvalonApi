using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class Tester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrueRoleId",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TrueRoleId",
                table: "Players",
                column: "TrueRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Roles_TrueRoleId",
                table: "Players",
                column: "TrueRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Roles_TrueRoleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TrueRoleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TrueRoleId",
                table: "Players");
        }
    }
}
