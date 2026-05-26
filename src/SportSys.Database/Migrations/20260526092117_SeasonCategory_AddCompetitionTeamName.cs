using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeasonCategory_AddCompetitionTeamName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompetitionTeamName",
                schema: "sport",
                table: "SeasonCategory",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionTeamName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionTeamName",
                schema: "sport",
                table: "SeasonCategory")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionTeamName");
        }
    }
}
