using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseDisksStorageFieldProxmoxMachinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseDisksStorage",
                schema: "connect_proxmox",
                table: "ProxmoxServers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseDisksStorage",
                schema: "connect_proxmox",
                table: "ProxmoxServers");
        }
    }
}
