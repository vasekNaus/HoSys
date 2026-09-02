using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class SportCodebookAddIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "sport",
                table: "Team",
                type: "bit",
                nullable: false,
                defaultValue: true)
                .Annotation("Relational:DefaultConstraintName", "DF_Team_IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "sport",
                table: "SeasonCategory",
                type: "bit",
                nullable: false,
                defaultValue: true)
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "sport",
                table: "Season",
                type: "bit",
                nullable: false,
                defaultValue: true)
                .Annotation("Relational:DefaultConstraintName", "DF_Season_IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "sport",
                table: "IceRink",
                type: "bit",
                nullable: false,
                defaultValue: true)
                .Annotation("Relational:DefaultConstraintName", "DF_IceRink_IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "sport",
                table: "Team")
                .Annotation("Relational:DefaultConstraintName", "DF_Team_IsActive");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "sport",
                table: "SeasonCategory")
                .Annotation("Relational:DefaultConstraintName", "DF_SeasonCategory_IsActive");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "sport",
                table: "Season")
                .Annotation("Relational:DefaultConstraintName", "DF_Season_IsActive");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "sport",
                table: "IceRink")
                .Annotation("Relational:DefaultConstraintName", "DF_IceRink_IsActive");
        }
    }
}
