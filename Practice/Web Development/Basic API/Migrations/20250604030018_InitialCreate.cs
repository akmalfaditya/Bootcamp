using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Electronic devices and gadgets", true, "Electronics" },
                    { 2, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Books, magazines, and reading materials", true, "Books" },
                    { 3, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Apparel and fashion items", true, "Clothing" },
                    { 4, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Home improvement and gardening supplies", true, "Home & Garden" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsActive", "LastModifiedDate", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Latest smartphone with advanced features", true, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Smartphone X1", 699.99m, 50 },
                    { 2, 1, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "High-performance laptop for professionals", true, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop Pro 15", 1299.99m, 25 },
                    { 3, 2, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Learn the basics of programming", true, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Programming Fundamentals", 49.99m, 100 },
                    { 4, 2, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Advanced web development techniques", true, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Web Development Mastery", 79.99m, 75 },
                    { 5, 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Comfortable cotton t-shirt", true, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Cotton T-Shirt", 19.99m, 200 },
                    { 6, 4, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Complete set of gardening tools", true, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Garden Tools Set", 89.99m, 30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive",
                table: "Categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_Unique",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                table: "Products",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
