using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ADDTRANZONEREALTION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransformerId",
                schema: "Definitions",
                table: "Zones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_TransformerId",
                schema: "Definitions",
                table: "Zones",
                column: "TransformerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Transformer_TransformerId",
                schema: "Definitions",
                table: "Zones",
                column: "TransformerId",
                principalSchema: "Definitions",
                principalTable: "Transformer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Transformer_TransformerId",
                schema: "Definitions",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_TransformerId",
                schema: "Definitions",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "TransformerId",
                schema: "Definitions",
                table: "Zones");
        }
    }
}
