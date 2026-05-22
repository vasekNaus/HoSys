using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class IceRink_Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                schema: "sport",
                table: "IceRink",
                newName: "Street");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                schema: "sport",
                table: "IceRink",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:DefaultConstraintName", "DF_IceRink_ZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                schema: "sport",
                table: "IceRink")
                .Annotation("Relational:DefaultConstraintName", "DF_IceRink_ZipCode");

            migrationBuilder.RenameColumn(
                name: "Street",
                schema: "sport",
                table: "IceRink",
                newName: "Address");
        }
    }
}
