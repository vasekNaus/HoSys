using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportSys.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateSequence<int>(
                name: "InventoryItemSeq",
                schema: "inventory");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategory_Id = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryKindJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategory_Id",
                        column: x => x.ParentCategory_Id,
                        principalSchema: "inventory",
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventorySession",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySession", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Loan",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    Member_Id = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedReturnDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReturnedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loan_User_Member_Id",
                        column: x => x.Member_Id,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentLocation_Id = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Location_ParentLocation_Id",
                        column: x => x.ParentLocation_Id,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDocument",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryCheck",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventorySession_Id = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    CheckedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckedByUser_Id = table.Column<int>(type: "int", nullable: true),
                    Found = table.Column<bool>(type: "bit", nullable: false),
                    ActualLocation_Id = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryCheck_InventorySession_InventorySession_Id",
                        column: x => x.InventorySession_Id,
                        principalSchema: "inventory",
                        principalTable: "InventorySession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryCheck_Location_ActualLocation_Id",
                        column: x => x.ActualLocation_Id,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryCheck_User_CheckedByUser_Id",
                        column: x => x.CheckedByUser_Id,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemLocationHistory",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    PreviousLocation_Id = table.Column<int>(type: "int", nullable: true),
                    NewLocation_Id = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByUser_Id = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLocationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLocationHistory_Location_NewLocation_Id",
                        column: x => x.NewLocation_Id,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLocationHistory_Location_PreviousLocation_Id",
                        column: x => x.PreviousLocation_Id,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLocationHistory_User_ChangedByUser_Id",
                        column: x => x.ChangedByUser_Id,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "(NEXT VALUE FOR [inventory].[InventoryItemSeq])"),
                    InventoryNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    AssignedLocationId = table.Column<int>(type: "int", nullable: true),
                    CurrentLocationId = table.Column<int>(type: "int", nullable: true),
                    ItemStatus = table.Column<int>(type: "int", nullable: false),
                    AcquisitionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AcquisitionPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    QRCodeValue = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WarrantyUntil = table.Column<DateOnly>(type: "date", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inventory",
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Asset_Location_AssignedLocationId",
                        column: x => x.AssignedLocationId,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Asset_Location_CurrentLocationId",
                        column: x => x.CurrentLocationId,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Asset_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "(NEXT VALUE FOR [inventory].[InventoryItemSeq])"),
                    InventoryNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    AssignedLocationId = table.Column<int>(type: "int", nullable: true),
                    CurrentLocationId = table.Column<int>(type: "int", nullable: true),
                    ItemStatus = table.Column<int>(type: "int", nullable: false),
                    AcquisitionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AcquisitionPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    QRCodeValue = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemKind_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inventory",
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_ItemKind_ItemKind_Id",
                        column: x => x.ItemKind_Id,
                        principalSchema: "inventory",
                        principalTable: "ItemKind",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_Location_AssignedLocationId",
                        column: x => x.AssignedLocationId,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_Location_CurrentLocationId",
                        column: x => x.CurrentLocationId,
                        principalSchema: "dbo",
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPurchase",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDocument_Id = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemPurchase_PurchaseDocument_PurchaseDocument_Id",
                        column: x => x.PurchaseDocument_Id,
                        principalSchema: "inventory",
                        principalTable: "PurchaseDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransaction",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    TransactionType_Id = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransaction_TransactionType_TransactionType_Id",
                        column: x => x.TransactionType_Id,
                        principalSchema: "inventory",
                        principalTable: "TransactionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryTransaction_User_User_Id",
                        column: x => x.User_Id,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
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

            migrationBuilder.InsertData(
                schema: "inventory",
                table: "TransactionType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nákup" },
                    { 2, "Zapůjčení" },
                    { 3, "Vrácení" },
                    { 4, "Přesun" },
                    { 5, "Zahájení opravy" },
                    { 6, "Ukončení opravy" },
                    { 7, "Inventura" },
                    { 8, "Ztráta" },
                    { 9, "Vyřazení" }
                });

            migrationBuilder.CreateIndex(
                name: "UX_InventoryCheck_SessionItem",
                schema: "inventory",
                table: "InventoryCheck",
                columns: new[] { "InventorySession_Id", "InventoryItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPurchase_InventoryItemId",
                schema: "inventory",
                table: "InventoryItemPurchase",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPurchase_PurchaseDocument_Id",
                schema: "inventory",
                table: "InventoryItemPurchase",
                column: "PurchaseDocument_Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_ItemDate",
                schema: "inventory",
                table: "InventoryTransaction",
                columns: new[] { "InventoryItemId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemLocationHistory_ItemDate",
                schema: "inventory",
                table: "ItemLocationHistory",
                columns: new[] { "InventoryItemId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_InventoryItemId",
                schema: "inventory",
                table: "Loan",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_Member_Id",
                schema: "inventory",
                table: "Loan",
                column: "Member_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asset",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Equipment",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "InventoryCheck",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "InventoryItemPurchase",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransaction",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemLocationHistory",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Loan",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemKind",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventorySession",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PurchaseDocument",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "TransactionType",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "InventoryItemSeq",
                schema: "inventory");
        }
    }
}
