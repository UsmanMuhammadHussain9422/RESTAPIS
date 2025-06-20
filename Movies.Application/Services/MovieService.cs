﻿using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Validators;

namespace Movies.Application.Services
{
    public class MovieService : IMoviesService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _movieValidator;

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> validator)
        {
            _movieRepository = movieRepository;
            _movieValidator = validator;
        }

        public async Task<bool> CreateAsync(Movie movie)
        {
            await _movieValidator.ValidateAndThrowAsync(movie);
            return await _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _movieRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return _movieRepository.GetAllAsync();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return _movieRepository.GetByIdAsync(id);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            return _movieRepository.GetBySlugAsync(slug);
        }

        public async Task<Movie?> UpdateAsync(Movie movie)
        {
            await _movieValidator.ValidateAndThrowAsync(movie);

            var movieExists = await _movieRepository.ExistByID(movie.Id);
            if(!movieExists)
            {
                return null;
            }

            await _movieRepository.UpdateAsync(movie);
            return movie;
        }
    }
}
