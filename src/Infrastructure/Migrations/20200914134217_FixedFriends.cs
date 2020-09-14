using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger.Infrastructure.Migrations
{
    public partial class FixedFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_AspNetUsers_UserId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_Friend_UserId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Friend");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Friend",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_ApplicationUserId",
                table: "Friend",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_AspNetUsers_ApplicationUserId",
                table: "Friend",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_AspNetUsers_ApplicationUserId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_Friend_ApplicationUserId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Friend");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Friend",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_AspNetUsers_UserId",
                table: "Friend",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
