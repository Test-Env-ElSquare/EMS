using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Editsonvies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                schema: "Calculated",
                table: "ZoneHourlyAnalysis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                schema: "Calculated",
                table: "ZoneAnalysis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Calculated",
                table: "ZoneAnalysis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                schema: "Calculated",
                table: "LineHourlyAnalysis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                schema: "Calculated",
                table: "LineAnalysis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Calculated",
                table: "LineAnalysis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactoryId",
                schema: "Calculated",
                table: "ZoneHourlyAnalysis");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                schema: "Calculated",
                table: "ZoneAnalysis");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Calculated",
                table: "ZoneAnalysis");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                schema: "Calculated",
                table: "LineHourlyAnalysis");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                schema: "Calculated",
                table: "LineAnalysis");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Calculated",
                table: "LineAnalysis");
        }
    }
}
