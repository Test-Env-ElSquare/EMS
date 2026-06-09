using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addISCURRENTtabelsForTrans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransformerHourlyAnalysisCurrent",
                schema: "Calculated",
                columns: table => new
                {
                    TransformerId = table.Column<int>(type: "int", nullable: false),
                    TransformerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactoryId = table.Column<int>(type: "int", nullable: false),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalEnergyConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowerFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Voltage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Current = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hermonics = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvgTHDv = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvgTHDi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransformerHourlyAnalysisCurrent",
                schema: "Calculated");
        }
    }
}
