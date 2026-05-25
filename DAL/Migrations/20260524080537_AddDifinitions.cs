using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDifinitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Definitions");

            migrationBuilder.CreateTable(
                name: "Factories",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Energy",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransformerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Energy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Energy_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalSchema: "Definitions",
                        principalTable: "Factories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactoryId = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lines_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalSchema: "Definitions",
                        principalTable: "Factories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LineTransformers",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    TransformerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineTransformers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineTransformers_Energy_TransformerId",
                        column: x => x.TransformerId,
                        principalSchema: "Definitions",
                        principalTable: "Energy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineTransformers_Lines_LineId",
                        column: x => x.LineId,
                        principalSchema: "Definitions",
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_Lines_LineId",
                        column: x => x.LineId,
                        principalSchema: "Definitions",
                        principalTable: "Lines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Energy_FactoryId",
                schema: "Definitions",
                table: "Energy",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_FactoryId",
                schema: "Definitions",
                table: "Lines",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransformers_LineId",
                schema: "Definitions",
                table: "LineTransformers",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransformers_TransformerId",
                schema: "Definitions",
                table: "LineTransformers",
                column: "TransformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_LineId",
                schema: "Definitions",
                table: "Machines",
                column: "LineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineTransformers",
                schema: "Definitions");

            migrationBuilder.DropTable(
                name: "Machines",
                schema: "Definitions");

            migrationBuilder.DropTable(
                name: "Energy",
                schema: "Definitions");

            migrationBuilder.DropTable(
                name: "Lines",
                schema: "Definitions");

            migrationBuilder.DropTable(
                name: "Factories",
                schema: "Definitions");
        }
    }
}
