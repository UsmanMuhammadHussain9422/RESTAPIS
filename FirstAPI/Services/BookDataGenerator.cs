using FirstAPI.Models;

namespace FirstAPI.Services
{
    public class BookDataGenerator
    {
        public static List<Book> GenerateBooks()
        {
            var books = new List<Book>();

            // Real-world inspired book titles
            var titles = new[]
            {
                "To Kill a Mockingbird",
                "1984",
                "The Great Gatsby",
                "Moby Dick",
                "Pride and Prejudice",
                "War and Peace",
                "The Catcher in the Rye",
                "The Hobbit",
                "The Lord of the Rings",
                "Harry Potter and the Sorcerer's Stone"
            };

            // Diverse authors
            var authors = new[]
            {
                "Harper Lee",
                "George Orwell",
                "F. Scott Fitzgerald",
                "Herman Melville",
                "Jane Austen",
                "Leo Tolstoy",
                "J.D. Salinger",
                "J.R.R. Tolkien",
                "J.K. Rowling",
                "Mark Twain"
            };

            for (int i = 0; i < titles.Length-1; i++)
            {
                books.Add(new Book
                {
                    Id = i + 1,
                    Title = titles[i % titles.Length], // Ensures titles are reused if count exceeds the list
                    Author = authors[i], // Randomly selects an author
                    Description = $"A classic book titled '{titles[i % titles.Length]}'.",
                    YearPublished = DateTime.Now.AddMonths(12*i).Year
                });
            }

            return books;
        }
    }
}
