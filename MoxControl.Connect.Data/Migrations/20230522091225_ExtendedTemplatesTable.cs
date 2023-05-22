using Microsoft.EntityFrameworkCore.Migrations;
using MoxControl.Connect.Models.Entities;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedTemplatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TemplateAvailableServerData>(
                name: "AvailableServerData",
                schema: "connect",
                table: "Templates",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CPUCores",
                schema: "connect",
                table: "Templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CPUSockets",
                schema: "connect",
                table: "Templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HDDSize",
                schema: "connect",
                table: "Templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RAMSize",
                schema: "connect",
                table: "Templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableServerData",
                schema: "connect",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "CPUCores",
                schema: "connect",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "CPUSockets",
                schema: "connect",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "HDDSize",
                schema: "connect",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "RAMSize",
                schema: "connect",
                table: "Templates");
        }
    }
}
