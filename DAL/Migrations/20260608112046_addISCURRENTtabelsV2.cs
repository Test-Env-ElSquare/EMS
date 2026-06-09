using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addISCURRENTtabelsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Calculated",
                table: "ZoneHourlyAnalysisCurrent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Calculated",
                table: "LineHourlyAnalysisCurrent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Calculated",
                table: "ZoneHourlyAnalysisCurrent");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Calculated",
                table: "LineHourlyAnalysisCurrent");
        }
    }
}
