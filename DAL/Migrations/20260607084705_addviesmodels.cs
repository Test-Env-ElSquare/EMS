using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addviesmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineAnalysis",
                schema: "Calculated",
                columns: table => new
                {
                    LineId = table.Column<int>(type: "int", nullable: false),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Availability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustedAvailability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnergyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LineHourlyAnalysis",
                schema: "Calculated",
                columns: table => new
                {
                    LineId = table.Column<int>(type: "int", nullable: false),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Availability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustedAvailability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnergyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ZoneAnalysis",
                schema: "Calculated",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransformerId = table.Column<int>(type: "int", nullable: true),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnergyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ZoneHourlyAnalysis",
                schema: "Calculated",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransformerId = table.Column<int>(type: "int", nullable: true),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnergyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineAnalysis",
                schema: "Calculated");

            migrationBuilder.DropTable(
                name: "LineHourlyAnalysis",
                schema: "Calculated");

            migrationBuilder.DropTable(
                name: "ZoneAnalysis",
                schema: "Calculated");

            migrationBuilder.DropTable(
                name: "ZoneHourlyAnalysis",
                schema: "Calculated");
        }
    }
}
