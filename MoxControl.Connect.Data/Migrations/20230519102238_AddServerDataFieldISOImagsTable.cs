using Microsoft.EntityFrameworkCore.Migrations;
using MoxControl.Connect.Models.Entities;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServerDataFieldISOImagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ServerData>(
                name: "ServerData",
                schema: "connect",
                table: "ISOImages",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerData",
                schema: "connect",
                table: "ISOImages");
        }
    }
}
