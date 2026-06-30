using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddItemKind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSizesJson",
                schema: "inventory",
                table: "Category",
                newName: "CategoryKindJson");

            migrationBuilder.AddColumn<int>(
                name: "ItemKindId",
                schema: "inventory",
                table: "Equipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemKind",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemKind", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "inventory",
                table: "ItemKind",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Dětské" },
                    { 2, "Youth" },
                    { 3, "Juniorské" },
                    { 4, "Seniorské" },
                    { 5, "Dámské" },
                    { 6, "Pánské" },
                    { 7, "Unisex" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_ItemKind_ItemKindId",
                schema: "inventory",
                table: "Equipment",
                column: "ItemKindId",
                principalSchema: "inventory",
                principalTable: "ItemKind",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_ItemKind_ItemKindId",
                schema: "inventory",
                table: "Equipment");

            migrationBuilder.DropTable(
                name: "ItemKind",
                schema: "inventory");

            migrationBuilder.DropColumn(
                name: "ItemKindId",
                schema: "inventory",
                table: "Equipment");

            migrationBuilder.RenameColumn(
                name: "CategoryKindJson",
                schema: "inventory",
                table: "Category",
                newName: "AvailableSizesJson");
        }
    }
}
