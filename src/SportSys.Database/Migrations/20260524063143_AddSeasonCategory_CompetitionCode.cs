using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonCategory_CompetitionCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompetitionCode",
                schema: "sport",
                table: "SeasonCategory",
                type: "varchar(5)",
                unicode: false,
                maxLength: 5,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionCode",
                schema: "sport",
                table: "SeasonCategory")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode");
        }
    }
}
