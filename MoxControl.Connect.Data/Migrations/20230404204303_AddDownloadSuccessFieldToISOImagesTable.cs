using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadSuccessFieldToISOImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DownloadSuccess",
                schema: "connect",
                table: "ISOImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadSuccess",
                schema: "connect",
                table: "ISOImages");
        }
    }
}
