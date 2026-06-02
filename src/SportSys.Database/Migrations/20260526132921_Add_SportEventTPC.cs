using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_SportEventTPC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "sport",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_Training_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                schema: "sport",
                table: "Match",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_Match_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "sport",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                .Annotation("Relational:DefaultConstraintName", "DF_Training_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                schema: "sport",
                table: "Match",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "(NEXT VALUE FOR [sport].[SportEventSeq])")
                .Annotation("Relational:DefaultConstraintName", "DF_Match_Id");
        }
    }
}
