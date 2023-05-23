using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageIdFieldProxmoxMachinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                schema: "connect_proxmox",
                table: "TemplateMachines",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                schema: "connect_proxmox",
                table: "ProxmoxMachines",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "connect_proxmox",
                table: "TemplateMachines");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "connect_proxmox",
                table: "ProxmoxMachines");
        }
    }
}
