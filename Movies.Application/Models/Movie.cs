﻿using System.Text.RegularExpressions;

namespace Movies.Application.Models
{
    public partial class Movie
    {
        public required Guid Id { get; init; }
        public string Slug => GenerateSlug();
        public required string Title { get; set; }
        public required int YearOfRelease { get; set; }
        public required List<string> Genres { get; init; } = new();

        public string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, String.Empty)
                                    .ToLower()
                                    .Replace(" ", "-");
            return $"{sluggedTitle}-{YearOfRelease}";
        }

        [GeneratedRegex(@"[^0-9A-Za-z\s_-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}