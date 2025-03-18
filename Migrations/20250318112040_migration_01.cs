using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssemblyService.Migrations
{
    /// <inheritdoc />
    public partial class migration_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assembles",
                columns: table => new
                {
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    NIC = table.Column<int>(type: "int", nullable: false),
                    assignee_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    isCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    attachment_path = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assembles", x => new { x.vehicle_id, x.NIC });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assembles");
        }
    }
}
