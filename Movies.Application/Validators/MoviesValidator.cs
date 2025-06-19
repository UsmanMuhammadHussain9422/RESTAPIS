using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application.Validators
{
    public class MoviesValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepository;
        public MoviesValidator(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
            RuleFor(x => x.Genres).NotEmpty();
            RuleFor(x => x.Slug)
                .MustAsync(ValidateSlug)
                .WithMessage("This movies already exists in the system!");

        }

        private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token)
        {
            var movieSlugExists = await _movieRepository.GetBySlugAsync(slug);
            if (movieSlugExists is not null)
            {
                return movieSlugExists.Id == movie.Id;
            }

            return movieSlugExists is null;
        }
    }
}
