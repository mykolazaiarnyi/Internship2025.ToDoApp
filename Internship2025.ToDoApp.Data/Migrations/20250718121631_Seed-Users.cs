using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship2025.ToDoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1f0df12a-929d-4a7e-ab5d-56a3c4540f90", "John Doe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1f0df12a-929d-4a7e-ab5d-56a3c4540f90");
        }
    }
}
