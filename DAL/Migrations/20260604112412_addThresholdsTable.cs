using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addThresholdsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergyHeatmapThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LowFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LowTo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediumFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediumTo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HighFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HighTo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyHeatmapThresholds", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyHeatmapThresholds");
        }
    }
}
