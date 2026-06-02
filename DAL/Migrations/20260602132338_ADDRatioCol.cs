using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ADDRatioCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RatioFromParent",
                schema: "Definitions",
                table: "Zones",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RatioFromParent",
                schema: "Definitions",
                table: "Lines",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                schema: "Definitions",
                table: "Lines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ZoneId",
                schema: "Definitions",
                table: "Lines",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Zones_ZoneId",
                schema: "Definitions",
                table: "Lines",
                column: "ZoneId",
                principalSchema: "Definitions",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Zones_ZoneId",
                schema: "Definitions",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ZoneId",
                schema: "Definitions",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "RatioFromParent",
                schema: "Definitions",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "RatioFromParent",
                schema: "Definitions",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                schema: "Definitions",
                table: "Lines");
        }
    }
}
