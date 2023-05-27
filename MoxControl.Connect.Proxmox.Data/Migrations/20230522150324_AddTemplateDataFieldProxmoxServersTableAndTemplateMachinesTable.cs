using Microsoft.EntityFrameworkCore.Migrations;
using MoxControl.Connect.Models.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoxControl.Connect.Proxmox.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateDataFieldProxmoxServersTableAndTemplateMachinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TemplateData>(
                name: "TemplateData",
                schema: "connect_proxmox",
                table: "ProxmoxServers",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TemplateMachines",
                schema: "connect_proxmox",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServerId = table.Column<long>(type: "bigint", nullable: false),
                    ProxmoxName = table.Column<string>(type: "text", nullable: true),
                    ProxmoxId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<long>(type: "bigint", nullable: true),
                    RAMSize = table.Column<int>(type: "integer", nullable: false),
                    HDDSize = table.Column<int>(type: "integer", nullable: false),
                    CPUSockets = table.Column<int>(type: "integer", nullable: false),
                    CPUCores = table.Column<int>(type: "integer", nullable: false),
                    Stage = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateMachines_ProxmoxServers_ServerId",
                        column: x => x.ServerId,
                        principalSchema: "connect_proxmox",
                        principalTable: "ProxmoxServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateMachines_ServerId",
                schema: "connect_proxmox",
                table: "TemplateMachines",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateMachines",
                schema: "connect_proxmox");

            migrationBuilder.DropColumn(
                name: "TemplateData",
                schema: "connect_proxmox",
                table: "ProxmoxServers");
        }
    }
}
