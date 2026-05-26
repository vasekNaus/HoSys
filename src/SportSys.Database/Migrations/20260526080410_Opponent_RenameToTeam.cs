using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class Opponent_RenameToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Opponent_AwayOpponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Opponent_HomeOpponent_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Opponent_IceRink_HomeIceRink_Id",
                schema: "sport",
                table: "Opponent");

            migrationBuilder.RenameTable(
                name: "Opponent",
                schema: "sport",
                newName: "Team",
                newSchema: "sport");

            migrationBuilder.RenameColumn(
                name: "HomeOpponent_Id",
                schema: "sport",
                table: "Match",
                newName: "HomeTeam_Id");

            migrationBuilder.RenameColumn(
                name: "AwayOpponent_Id",
                schema: "sport",
                table: "Match",
                newName: "AwayTeam_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_AwayTeam_Id",
                schema: "sport",
                table: "Match",
                column: "AwayTeam_Id",
                principalSchema: "sport",
                principalTable: "Team",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_HomeTeam_Id",
                schema: "sport",
                table: "Match",
                column: "HomeTeam_Id",
                principalSchema: "sport",
                principalTable: "Team",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_IceRink_HomeIceRink_Id",
                schema: "sport",
                table: "Team",
                column: "HomeIceRink_Id",
                principalSchema: "sport",
                principalTable: "IceRink",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_AwayTeam_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_HomeTeam_Id",
                schema: "sport",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_IceRink_HomeIceRink_Id",
                schema: "sport",
                table: "Team");

            migrationBuilder.RenameTable(
                name: "Team",
                schema: "sport",
                newName: "Opponent",
                newSchema: "sport");

            migrationBuilder.RenameColumn(
                name: "HomeTeam_Id",
                schema: "sport",
                table: "Match",
                newName: "HomeOpponent_Id");

            migrationBuilder.RenameColumn(
                name: "AwayTeam_Id",
                schema: "sport",
                table: "Match",
                newName: "AwayOpponent_Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Opponent_IceRink_HomeIceRink_Id",
                schema: "sport",
                table: "Opponent",
                column: "HomeIceRink_Id",
                principalSchema: "sport",
                principalTable: "IceRink",
                principalColumn: "Id");
        }
    }
}
