using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class addingchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Roles_TrueRoleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TrueRoleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TrueRoleId",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                table: "Players",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Roles_RoleId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RoleId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TrueRoleId",
                table: "Players",
                type: "uniqueidentifier",
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
    }
}
