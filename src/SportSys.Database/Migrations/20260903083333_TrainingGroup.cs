using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class TrainingGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingGroup",
                schema: "sport",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Training_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroup", x => new { x.GroupId, x.Training_Id });
                    table.ForeignKey(
                        name: "FK_TrainingGroup_Training_Training_Id",
                        column: x => x.Training_Id,
                        principalSchema: "sport",
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanGroup",
                schema: "sport",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingPlan_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanGroup", x => new { x.GroupId, x.TrainingPlan_Id });
                    table.ForeignKey(
                        name: "FK_TrainingPlanGroup_TrainingPlan_TrainingPlan_Id",
                        column: x => x.TrainingPlan_Id,
                        principalSchema: "sport",
                        principalTable: "TrainingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_TrainingGroup_TrainingId",
                schema: "sport",
                table: "TrainingGroup",
                column: "Training_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_TrainingPlanGroup_TrainingPlanId",
                schema: "sport",
                table: "TrainingPlanGroup",
                column: "TrainingPlan_Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingGroup",
                schema: "sport");

            migrationBuilder.DropTable(
                name: "TrainingPlanGroup",
                schema: "sport");
        }
    }
}
