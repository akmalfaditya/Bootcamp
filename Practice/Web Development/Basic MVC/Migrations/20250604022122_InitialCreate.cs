using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Branch = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Section = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentID = table.Column<int>(type: "INTEGER", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    GradeValue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LetterGrade = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    GradeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentID", "Branch", "Email", "EnrollmentDate", "Gender", "Name", "Section" },
                values: new object[,]
                {
                    { 1, "Computer Science", "john.smith@university.edu", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "John Smith", "A" },
                    { 2, "Engineering", "sarah.johnson@university.edu", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "Sarah Johnson", "B" },
                    { 3, "Business Administration", "mike.davis@university.edu", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "Mike Davis", "A" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeID", "Comments", "GradeDate", "GradeValue", "LetterGrade", "StudentID", "Subject" },
                values: new object[,]
                {
                    { 1, "Excellent understanding of algorithms", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 95.5m, "A", 1, "Data Structures" },
                    { 2, "Good work on MVC project", new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 88.0m, "B", 1, "Web Development" },
                    { 3, "Strong mathematical foundation", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 92.0m, "A", 2, "Calculus" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentID",
                table: "Grades",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
