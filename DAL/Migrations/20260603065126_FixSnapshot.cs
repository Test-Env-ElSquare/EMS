using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Lines_LineId",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                schema: "Definitions",
                table: "Machines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Building",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faunctionality",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDominant",
                schema: "Definitions",
                table: "Machines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "RatedSpeed",
                schema: "Definitions",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UID",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfSpeed",
                schema: "Definitions",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineLoads",
                schema: "RealTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SKU = table.Column<int>(type: "int", nullable: true),
                    JobOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKUName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LineId = table.Column<int>(type: "int", nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPending = table.Column<bool>(type: "bit", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineLoads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineLoads_Machines_MachineId",
                        column: x => x.MachineId,
                        principalSchema: "Definitions",
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Signals",
                schema: "RealTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Qreject = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountDiff = table.Column<int>(type: "int", nullable: false),
                    QrejectDiff = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    FactoryId = table.Column<int>(type: "int", nullable: true),
                    LineId = table.Column<int>(type: "int", nullable: true),
                    Fault = table.Column<int>(type: "int", nullable: false),
                    Functionality = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signals_Machines_MachineId",
                        column: x => x.MachineId,
                        principalSchema: "Definitions",
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SKUs",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicalSKU = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineId = table.Column<int>(type: "int", nullable: true),
                    PackSize = table.Column<int>(type: "int", nullable: true),
                    CartonSize = table.Column<int>(type: "int", nullable: true),
                    PallSize = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkuCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SKUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SKUs_Lines_LineId",
                        column: x => x.LineId,
                        principalSchema: "Definitions",
                        principalTable: "Lines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MachineShifts",
                schema: "Definitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    SKUId = table.Column<int>(type: "int", nullable: true),
                    ShiftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedQty = table.Column<int>(type: "int", nullable: true),
                    ActualQty = table.Column<int>(type: "int", nullable: true),
                    RejectedQty = table.Column<int>(type: "int", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineShifts_Machines_MachineId",
                        column: x => x.MachineId,
                        principalSchema: "Definitions",
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineShifts_SKUs_SKUId",
                        column: x => x.SKUId,
                        principalSchema: "Definitions",
                        principalTable: "SKUs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineLoads_MachineId",
                schema: "RealTime",
                table: "MachineLoads",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineShifts_MachineId",
                schema: "Definitions",
                table: "MachineShifts",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineShifts_SKUId",
                schema: "Definitions",
                table: "MachineShifts",
                column: "SKUId");

            migrationBuilder.CreateIndex(
                name: "IX_Signals_MachineId",
                schema: "RealTime",
                table: "Signals",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_SKUs_LineId",
                schema: "Definitions",
                table: "SKUs",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Lines_LineId",
                schema: "Definitions",
                table: "Machines",
                column: "LineId",
                principalSchema: "Definitions",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Lines_LineId",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropTable(
                name: "MachineLoads",
                schema: "RealTime");

            migrationBuilder.DropTable(
                name: "MachineShifts",
                schema: "Definitions");

            migrationBuilder.DropTable(
                name: "Signals",
                schema: "RealTime");

            migrationBuilder.DropTable(
                name: "SKUs",
                schema: "Definitions");

            migrationBuilder.DropColumn(
                name: "Building",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Faunctionality",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Floor",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "IsDominant",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "RatedSpeed",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Room",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "UID",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "UnitOfSpeed",
                schema: "Definitions",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "LineId",
                schema: "Definitions",
                table: "Machines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Lines_LineId",
                schema: "Definitions",
                table: "Machines",
                column: "LineId",
                principalSchema: "Definitions",
                principalTable: "Lines",
                principalColumn: "Id");
        }
    }
}
