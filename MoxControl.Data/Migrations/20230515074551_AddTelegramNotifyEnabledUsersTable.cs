using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramNotifyEnabledUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TelegramNotifyEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramNotifyEnabled",
                table: "Users");
        }
    }
}
