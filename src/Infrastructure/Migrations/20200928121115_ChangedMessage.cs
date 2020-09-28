using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangedMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "DateSended",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSended",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
