using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeasonCategory_CompetitionCodeLength10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompetitionCode",
                schema: "sport",
                table: "SeasonCategory",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldUnicode: false,
                oldMaxLength: 5,
                oldDefaultValue: "")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompetitionCode",
                schema: "sport",
                table: "SeasonCategory",
                type: "varchar(5)",
                unicode: false,
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldUnicode: false,
                oldMaxLength: 10,
                oldDefaultValue: "")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_SeasonCategory_CompetitionCode");
        }
    }
}
