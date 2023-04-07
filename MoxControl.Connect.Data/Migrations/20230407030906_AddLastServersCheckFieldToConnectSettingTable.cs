using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastServersCheckFieldToConnectSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastServersCheck",
                schema: "connect",
                table: "ConnectSettings",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastServersCheck",
                schema: "connect",
                table: "ConnectSettings");
        }
    }
}
