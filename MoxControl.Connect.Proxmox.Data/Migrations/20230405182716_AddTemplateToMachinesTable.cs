using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateToMachinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TemplateId",
                schema: "connect_proxmox",
                table: "ProxmoxMachines",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "connect_proxmox",
                table: "ProxmoxMachines");
        }
    }
}
