using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping
{
    public static class ContractMapping
    {
        public static Movie MapToMovie(this CreateMovieRequest movieRequest)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = movieRequest.Title,
                YearOfRelease = movieRequest.YearOfRelease,
                Genres = movieRequest.Genres.ToList()
            };
            return movie;
        }

        public static MovieResponse MapToMovieResponse(this Movie movie)
        {
            var movieResponse = new MovieResponse
            {
                Id = movie.Id,
                Slug = movie.Slug,
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
                Genres = movie.Genres
            };
            return movieResponse;
        }

        public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies)
        {
            return new MoviesResponse
            {
                Items = movies.Select(MapToMovieResponse)
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest UpdateMovieRequest, Guid id)
        {
            var movie = new Movie
            {
                Id = id,
                Title = UpdateMovieRequest.Title,
                YearOfRelease = UpdateMovieRequest.YearOfRelease,
                Genres = UpdateMovieRequest.Genres.ToList()
            };
            return movie;
        }
    }
}
