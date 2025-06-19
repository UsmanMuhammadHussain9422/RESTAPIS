using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstAPI.Migrations
{
    /// <inheritdoc />
    public partial class DataAddedToBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Title", "YearPublished" },
                values: new object[,]
                {
                    { 1, "Harper Lee", "A classic book titled 'To Kill a Mockingbird'.", "To Kill a Mockingbird", 2025 },
                    { 2, "George Orwell", "A classic book titled '1984'.", "1984", 2026 },
                    { 3, "F. Scott Fitzgerald", "A classic book titled 'The Great Gatsby'.", "The Great Gatsby", 2027 },
                    { 4, "Herman Melville", "A classic book titled 'Moby Dick'.", "Moby Dick", 2028 },
                    { 5, "Jane Austen", "A classic book titled 'Pride and Prejudice'.", "Pride and Prejudice", 2029 },
                    { 6, "Leo Tolstoy", "A classic book titled 'War and Peace'.", "War and Peace", 2030 },
                    { 7, "J.D. Salinger", "A classic book titled 'The Catcher in the Rye'.", "The Catcher in the Rye", 2031 },
                    { 8, "J.R.R. Tolkien", "A classic book titled 'The Hobbit'.", "The Hobbit", 2032 },
                    { 9, "J.K. Rowling", "A classic book titled 'The Lord of the Rings'.", "The Lord of the Rings", 2033 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
