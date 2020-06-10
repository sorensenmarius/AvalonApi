using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Roles_RoleId1",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Players_RoleId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "IsEvil",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEvil",
                table: "Players");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                table: "Players",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEvil = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_RoleId1",
                table: "Players",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Roles_RoleId1",
                table: "Players",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
