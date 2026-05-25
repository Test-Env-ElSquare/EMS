using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameEnergyToTransformers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Energy_Factories_FactoryId",
                schema: "Definitions",
                table: "Energy");

            migrationBuilder.DropForeignKey(
                name: "FK_LineTransformers_Energy_TransformerId",
                schema: "Definitions",
                table: "LineTransformers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Energy",
                schema: "Definitions",
                table: "Energy");

            migrationBuilder.RenameTable(
                name: "Energy",
                schema: "Definitions",
                newName: "Transformer",
                newSchema: "Definitions");

            migrationBuilder.RenameIndex(
                name: "IX_Energy_FactoryId",
                schema: "Definitions",
                table: "Transformer",
                newName: "IX_Transformer_FactoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transformer",
                schema: "Definitions",
                table: "Transformer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineTransformers_Transformer_TransformerId",
                schema: "Definitions",
                table: "LineTransformers",
                column: "TransformerId",
                principalSchema: "Definitions",
                principalTable: "Transformer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transformer_Factories_FactoryId",
                schema: "Definitions",
                table: "Transformer",
                column: "FactoryId",
                principalSchema: "Definitions",
                principalTable: "Factories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineTransformers_Transformer_TransformerId",
                schema: "Definitions",
                table: "LineTransformers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transformer_Factories_FactoryId",
                schema: "Definitions",
                table: "Transformer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transformer",
                schema: "Definitions",
                table: "Transformer");

            migrationBuilder.RenameTable(
                name: "Transformer",
                schema: "Definitions",
                newName: "Energy",
                newSchema: "Definitions");

            migrationBuilder.RenameIndex(
                name: "IX_Transformer_FactoryId",
                schema: "Definitions",
                table: "Energy",
                newName: "IX_Energy_FactoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Energy",
                schema: "Definitions",
                table: "Energy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Energy_Factories_FactoryId",
                schema: "Definitions",
                table: "Energy",
                column: "FactoryId",
                principalSchema: "Definitions",
                principalTable: "Factories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineTransformers_Energy_TransformerId",
                schema: "Definitions",
                table: "LineTransformers",
                column: "TransformerId",
                principalSchema: "Definitions",
                principalTable: "Energy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
