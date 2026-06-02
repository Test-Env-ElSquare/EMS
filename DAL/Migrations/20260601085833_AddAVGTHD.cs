using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAVGTHD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvgTHDi",
                schema: "Calculated",
                table: "TransformerHourlyAnalysis",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AvgTHDv",
                schema: "Calculated",
                table: "TransformerHourlyAnalysis",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AvgTHDi",
                schema: "Calculated",
                table: "TransformerAnalysis",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AvgTHDv",
                schema: "Calculated",
                table: "TransformerAnalysis",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgTHDi",
                schema: "Calculated",
                table: "TransformerHourlyAnalysis");

            migrationBuilder.DropColumn(
                name: "AvgTHDv",
                schema: "Calculated",
                table: "TransformerHourlyAnalysis");

            migrationBuilder.DropColumn(
                name: "AvgTHDi",
                schema: "Calculated",
                table: "TransformerAnalysis");

            migrationBuilder.DropColumn(
                name: "AvgTHDv",
                schema: "Calculated",
                table: "TransformerAnalysis");
        }
    }
}
