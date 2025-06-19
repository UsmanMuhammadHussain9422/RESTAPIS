namespace FirstAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Author { get; set; }
        public string? Description { get; set; } = null;
        public int YearPublished { get; set; }

        // Method to copy properties from another Book object
        public void CopyFrom(Book other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Id = other.Id;
            Title = other.Title;
            Author = other.Author;
            Description = other.Description;
            YearPublished = other.YearPublished;
        }
    }
}
