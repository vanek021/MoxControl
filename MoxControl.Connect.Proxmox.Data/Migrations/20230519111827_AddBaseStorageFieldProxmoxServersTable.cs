using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseStorageFieldProxmoxServersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseStorage",
                schema: "connect_proxmox",
                table: "ProxmoxServers",
                type: "text",
                nullable: false,
                defaultValue: "local");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseStorage",
                schema: "connect_proxmox",
                table: "ProxmoxServers");
        }
    }
}
