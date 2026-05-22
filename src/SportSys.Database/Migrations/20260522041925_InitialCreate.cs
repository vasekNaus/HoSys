using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "sport");

            migrationBuilder.CreateSequence<int>(
                name: "SportEventSeq",
                schema: "sport");

            migrationBuilder.CreateTable(
                name: "Coach",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(101)", maxLength: 101, nullable: false, computedColumnSql: "(([FirstName]+N' ')+[LastName])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoachRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IceRink",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<Geometry>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceRink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchType",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipationType",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    From = table.Column<DateOnly>(type: "date", nullable: false),
                    To = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPhase",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPhase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingState",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingType",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Opponent",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomeIceRink_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opponent_IceRink_HomeIceRink_Id",
                        column: x => x.HomeIceRink_Id,
                        principalSchema: "sport",
                        principalTable: "IceRink",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeasonCategory",
                schema: "sport",
                columns: table => new
                {
                    Season_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    BirthYears = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false, defaultValue: "[]")
                        .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_BirthYears")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonCategory", x => new { x.Season_Id, x.Name });
                    table.ForeignKey(
                        name: "FK_SeasonCategory_Season_Season_Id",
                        column: x => x.Season_Id,
                        principalSchema: "sport",
                        principalTable: "Season",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Match",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                        .Annotation("Relational:DefaultConstraintName", "DF_Match_Id"),
                    SeasonCategory_Season_Id = table.Column<int>(type: "int", nullable: false),
                    SeasonCategory_Name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    IceRink_Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(minute,[TimeFrom],[TimeTo]))", stored: true),
                    Note = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MatchCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Opponent_Id = table.Column<int>(type: "int", nullable: false),
                    IsHome = table.Column<bool>(type: "bit", nullable: false),
                    Home = table.Column<int>(type: "int", nullable: false),
                    Away = table.Column<int>(type: "int", nullable: false),
                    MatchType_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_IceRink_IceRink_Id",
                        column: x => x.IceRink_Id,
                        principalSchema: "sport",
                        principalTable: "IceRink",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_MatchType_MatchType_Id",
                        column: x => x.MatchType_Id,
                        principalSchema: "sport",
                        principalTable: "MatchType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Opponent_Opponent_Id",
                        column: x => x.Opponent_Id,
                        principalSchema: "sport",
                        principalTable: "Opponent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name",
                        columns: x => new { x.SeasonCategory_Season_Id, x.SeasonCategory_Name },
                        principalSchema: "sport",
                        principalTable: "SeasonCategory",
                        principalColumns: new[] { "Season_Id", "Name" });
                    table.ForeignKey(
                        name: "FK_Match_Season_SeasonCategory_Season_Id",
                        column: x => x.SeasonCategory_Season_Id,
                        principalSchema: "sport",
                        principalTable: "Season",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingEntitlement",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonCategory_Season_Id = table.Column<int>(type: "int", nullable: false),
                    SeasonCategory_Name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TrainingType_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingPhase_Id = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateOnly>(type: "date", nullable: false),
                    To = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingEntitlement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingEntitlement_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name",
                        columns: x => new { x.SeasonCategory_Season_Id, x.SeasonCategory_Name },
                        principalSchema: "sport",
                        principalTable: "SeasonCategory",
                        principalColumns: new[] { "Season_Id", "Name" });
                    table.ForeignKey(
                        name: "FK_TrainingEntitlement_TrainingPhase_TrainingPhase_Id",
                        column: x => x.TrainingPhase_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPhase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingEntitlement_TrainingType_TrainingType_Id",
                        column: x => x.TrainingType_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlan",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonCategory_Season_Id = table.Column<int>(type: "int", nullable: false),
                    SeasonCategory_Name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TrainingType_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingPhase_Id = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateOnly>(type: "date", nullable: false),
                    To = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(minute,[TimeFrom],[TimeTo]))", stored: true),
                    DayName = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlan_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name",
                        columns: x => new { x.SeasonCategory_Season_Id, x.SeasonCategory_Name },
                        principalSchema: "sport",
                        principalTable: "SeasonCategory",
                        principalColumns: new[] { "Season_Id", "Name" });
                    table.ForeignKey(
                        name: "FK_TrainingPlan_TrainingPhase_TrainingPhase_Id",
                        column: x => x.TrainingPhase_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPhase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingPlan_TrainingType_TrainingType_Id",
                        column: x => x.TrainingType_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoachTrainingEntitlement",
                schema: "sport",
                columns: table => new
                {
                    Coach_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingEntitlement_Id = table.Column<int>(type: "int", nullable: false),
                    CoachRole_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachTrainingEntitlement", x => new { x.Coach_Id, x.TrainingEntitlement_Id, x.CoachRole_Id });
                    table.ForeignKey(
                        name: "FK_CoachTrainingEntitlement_CoachRole_CoachRole_Id",
                        column: x => x.CoachRole_Id,
                        principalSchema: "dbo",
                        principalTable: "CoachRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoachTrainingEntitlement_Coach_Coach_Id",
                        column: x => x.Coach_Id,
                        principalSchema: "dbo",
                        principalTable: "Coach",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoachTrainingEntitlement_TrainingEntitlement_TrainingEntitlement_Id",
                        column: x => x.TrainingEntitlement_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingEntitlement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoachTrainingPlan",
                schema: "sport",
                columns: table => new
                {
                    Coach_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingPlan_Id = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachTrainingPlan", x => new { x.Coach_Id, x.TrainingPlan_Id, x.ValidFrom, x.ValidTo });
                    table.ForeignKey(
                        name: "FK_CoachTrainingPlan_Coach_Coach_Id",
                        column: x => x.Coach_Id,
                        principalSchema: "dbo",
                        principalTable: "Coach",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoachTrainingPlan_TrainingPlan_TrainingPlan_Id",
                        column: x => x.TrainingPlan_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Training",
                schema: "sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                        .Annotation("Relational:DefaultConstraintName", "DF_Training_Id"),
                    SeasonCategory_Season_Id = table.Column<int>(type: "int", nullable: false),
                    SeasonCategory_Name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TrainingType_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingPhase_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingState_Id = table.Column<int>(type: "int", nullable: false),
                    TrainingPlan_Id = table.Column<int>(type: "int", nullable: true),
                    IceRink_Id = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(minute,[TimeFrom],[TimeTo]))", stored: true),
                    Note = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_IceRink_IceRink_Id",
                        column: x => x.IceRink_Id,
                        principalSchema: "sport",
                        principalTable: "IceRink",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name",
                        columns: x => new { x.SeasonCategory_Season_Id, x.SeasonCategory_Name },
                        principalSchema: "sport",
                        principalTable: "SeasonCategory",
                        principalColumns: new[] { "Season_Id", "Name" });
                    table.ForeignKey(
                        name: "FK_Training_Season_SeasonCategory_Season_Id",
                        column: x => x.SeasonCategory_Season_Id,
                        principalSchema: "sport",
                        principalTable: "Season",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingPhase_TrainingPhase_Id",
                        column: x => x.TrainingPhase_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPhase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingPlan_TrainingPlan_Id",
                        column: x => x.TrainingPlan_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingState_TrainingState_Id",
                        column: x => x.TrainingState_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingState",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingType_TrainingType_Id",
                        column: x => x.TrainingType_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoachTraining",
                schema: "sport",
                columns: table => new
                {
                    Coach_Id = table.Column<int>(type: "int", nullable: false),
                    Training_Id = table.Column<int>(type: "int", nullable: false),
                    ParticipationType_Id = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "")
                        .Annotation("Relational:DefaultConstraintName", "DF_CoachTraining_Note")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachTraining", x => new { x.Coach_Id, x.Training_Id, x.ParticipationType_Id });
                    table.ForeignKey(
                        name: "FK_CoachTraining_Coach_Coach_Id",
                        column: x => x.Coach_Id,
                        principalSchema: "dbo",
                        principalTable: "Coach",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoachTraining_ParticipationType_ParticipationType_Id",
                        column: x => x.ParticipationType_Id,
                        principalSchema: "sport",
                        principalTable: "ParticipationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoachTraining_Training_Training_Id",
                        column: x => x.Training_Id,
                        principalSchema: "sport",
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "CoachRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Hlavní" },
                    { 2, "Asistent" },
                    { 3, "Obránci" },
                    { 4, "Gólmani" },
                    { 5, "Fyzio" },
                    { 6, "Kondiční" }
                });

            migrationBuilder.InsertData(
                schema: "sport",
                table: "MatchType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ligový zápas" },
                    { 2, "Příprava" },
                    { 3, "Turnaj" }
                });

            migrationBuilder.InsertData(
                schema: "sport",
                table: "ParticipationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Účast" },
                    { 2, "Bez reakce" },
                    { 3, "Neúčast" },
                    { 4, "Spojený trénink" },
                    { 5, "Neúčast - jiný trénink" }
                });

            migrationBuilder.InsertData(
                schema: "sport",
                table: "TrainingPhase",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Přípravné období" },
                    { 2, "Předzávodní období" },
                    { 3, "Závodní období" },
                    { 4, "Přechodné (regenerační) období" }
                });

            migrationBuilder.InsertData(
                schema: "sport",
                table: "TrainingState",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Plán" },
                    { 2, "Potvrzený KIS" },
                    { 3, "Jen KIS" },
                    { 4, "Změna času" },
                    { 5, "Zrušený" },
                    { 6, "Změna termínu" },
                    { 7, "Porucha ZS" }
                });

            migrationBuilder.InsertData(
                schema: "sport",
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Suchá příprava" },
                    { 2, "Led" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachTraining_Coach_Date",
                schema: "sport",
                table: "CoachTrainingPlan",
                columns: new[] { "Coach_Id", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_CoachTraining_Training_Date",
                schema: "sport",
                table: "CoachTrainingPlan",
                columns: new[] { "TrainingPlan_Id", "ValidFrom", "ValidTo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachTraining",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "CoachTrainingEntitlement",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "CoachTrainingPlan",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "Match",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "ParticipationType",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "Training",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "CoachRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TrainingEntitlement",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "Coach",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MatchType",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "Opponent",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "TrainingPlan",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "TrainingState",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "IceRink",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "SeasonCategory",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "TrainingPhase",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "TrainingType",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "Season",
                schema: "sport");

            migrationBuilder.DropSequence(
                name: "SportEventSeq",
                schema: "sport");
        }
    }
}
