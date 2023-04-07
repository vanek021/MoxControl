using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProxmoxNameFieldToProxmoxMachinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CPUCores",
                schema: "connect_proxmox",
                table: "ProxmoxMachines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CPUSockets",
                schema: "connect_proxmox",
                table: "ProxmoxMachines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProxmoxName",
                schema: "connect_proxmox",
                table: "ProxmoxMachines",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPUCores",
                schema: "connect_proxmox",
                table: "ProxmoxMachines");

            migrationBuilder.DropColumn(
                name: "CPUSockets",
                schema: "connect_proxmox",
                table: "ProxmoxMachines");

            migrationBuilder.DropColumn(
                name: "ProxmoxName",
                schema: "connect_proxmox",
                table: "ProxmoxMachines");
        }
    }
}
