using Microsoft.EntityFrameworkCore.Migrations;
using MoxControl.Connect.Models.Entities;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageDataFieldProxmoxServersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ImageData>(
                name: "ImageData",
                schema: "connect_proxmox",
                table: "ProxmoxServers",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "connect_proxmox",
                table: "ProxmoxServers");
        }
    }
}
