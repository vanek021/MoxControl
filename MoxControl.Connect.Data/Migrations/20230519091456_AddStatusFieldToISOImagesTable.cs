using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusFieldToISOImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
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
                name: "Status",
                schema: "connect",
                table: "ISOImages");
        }
    }
}
