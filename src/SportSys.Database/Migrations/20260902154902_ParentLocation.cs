using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class ParentLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Location_ParentLocation_Id",
                schema: "dbo",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ParentLocation_Id",
                schema: "dbo",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                schema: "dbo",
                newName: "Location",
                newSchema: "inventory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Location",
                schema: "inventory",
                newName: "Location",
                newSchema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "ParentLocation_Id",
                schema: "dbo",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Location_ParentLocation_Id",
                schema: "dbo",
                table: "Location",
                column: "ParentLocation_Id",
                principalSchema: "dbo",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
