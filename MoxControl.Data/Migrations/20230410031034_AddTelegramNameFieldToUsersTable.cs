using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramNameFieldToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelegramName",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramName",
                table: "Users");
        }
    }
}
