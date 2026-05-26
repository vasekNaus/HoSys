using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class Match_ResultJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Opponent_Opponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Away",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Home",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "IsHome",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "TimeTo",
                schema: "sport",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "Opponent_Id",
                schema: "sport",
                table: "Match",
                newName: "HomeOpponent_Id");

            migrationBuilder.AddColumn<int>(
                name: "AwayOpponent_Id",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                schema: "sport",
                table: "Match",
                type: "json",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Opponent_AwayOpponent_Id",
                schema: "sport",
                table: "Match",
                column: "AwayOpponent_Id",
                principalSchema: "sport",
                principalTable: "Opponent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Opponent_HomeOpponent_Id",
                schema: "sport",
                table: "Match",
                column: "HomeOpponent_Id",
                principalSchema: "sport",
                principalTable: "Opponent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Opponent_AwayOpponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Opponent_HomeOpponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "AwayOpponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Result",
                schema: "sport",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "HomeOpponent_Id",
                schema: "sport",
                table: "Match",
                newName: "Opponent_Id");

            migrationBuilder.AddColumn<int>(
                name: "Away",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Home",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                schema: "sport",
                table: "Match",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeTo",
                schema: "sport",
                table: "Match",
                type: "time(0)",
                precision: 0,
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                schema: "sport",
                table: "Match",
                type: "int",
                nullable: true,
                computedColumnSql: "(datediff(minute,[TimeFrom],[TimeTo]))",
                stored: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Opponent_Opponent_Id",
                schema: "sport",
                table: "Match",
                column: "Opponent_Id",
                principalSchema: "sport",
                principalTable: "Opponent",
                principalColumn: "Id");
        }
    }
}
