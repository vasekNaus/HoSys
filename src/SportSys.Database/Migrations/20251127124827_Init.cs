using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "plan");

            migrationBuilder.CreateTable(
                name: "Block",
                schema: "plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Room_id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableReservation = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LockTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UnlockTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaxReservations = table.Column<short>(type: "smallint", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AllowExternalReservations = table.Column<bool>(type: "bit", nullable: true),
                    AllowAnonymousReservations = table.Column<bool>(type: "bit", nullable: true),
                    AllowExternalUserReservations = table.Column<bool>(type: "bit", nullable: true),
                    IsHiddenForSearch = table.Column<bool>(type: "bit", nullable: false),
                    IsShuttleOnly = table.Column<bool>(type: "bit", nullable: false),
                    NoteColor = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                schema: "plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Block_id = table.Column<int>(type: "int", nullable: true),
                    Patient_id = table.Column<int>(type: "int", nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeArrival = table.Column<TimeOnly>(type: "time", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    PriceNote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PriceFull = table.Column<decimal>(type: "money", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRearranged = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    Part = table.Column<short>(type: "smallint", nullable: false),
                    FinalDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsExternalBonus = table.Column<bool>(type: "bit", nullable: false),
                    JobDone_id = table.Column<int>(type: "int", nullable: true),
                    InsurancePrice = table.Column<decimal>(type: "money", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceCompany_code = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true),
                    AmbulatoryBookNumber = table.Column<decimal>(type: "decimal(12,0)", nullable: true),
                    IsIcBilled = table.Column<bool>(type: "bit", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: true),
                    UrlOnlineMeeting = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Urgency = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Block_Block_id",
                        column: x => x.Block_id,
                        principalSchema: "plan",
                        principalTable: "Block",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_Block_id",
                schema: "plan",
                table: "Task",
                column: "Block_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task",
                schema: "plan");

            migrationBuilder.DropTable(
                name: "Block",
                schema: "plan");
        }
    }
}
