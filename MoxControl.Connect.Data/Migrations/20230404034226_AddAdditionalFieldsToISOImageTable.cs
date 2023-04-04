using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalFieldsToISOImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "connect",
                table: "ISOImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "connect",
                table: "ISOImages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StorageMethod",
                schema: "connect",
                table: "ISOImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "connect",
                table: "ISOImages");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "connect",
                table: "ISOImages");

            migrationBuilder.DropColumn(
                name: "StorageMethod",
                schema: "connect",
                table: "ISOImages");
        }
    }
}
