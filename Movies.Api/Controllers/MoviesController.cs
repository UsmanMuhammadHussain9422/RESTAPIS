using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Diagnostics.Tracing;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        public MoviesController(IMoviesService movieRepository)
        {
            _moviesService = movieRepository;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            if (request == null) { return BadRequest(); }

            var movie = request.MapToMovie();
            if (await _moviesService.CreateAsync(movie))
            {
                var response = movie.MapToMovieResponse();
                return CreatedAtAction(nameof(GetMovie), new { idOrSlug = movie.Id }, response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetMovie([FromRoute] string idOrslug)
        {
            var movie = Guid.TryParse(idOrslug, out var id)
                ? await _moviesService.GetByIdAsync(id)
                : await _moviesService.GetBySlugAsync(idOrslug);

            if (movie is null)
            {
                return NotFound();
            }
            else
            {
                var response = movie.MapToMovieResponse();
                return Ok(response);
            }
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _moviesService.GetAllAsync();
            return Ok(movies);
        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]UpdateMovieRequest updateMovieRequest)
        {
            var movie = updateMovieRequest.MapToMovie(id);
            var updatedMovie = await _moviesService.UpdateAsync(movie);
            if (updatedMovie == null)
                return NotFound();
            var response = updatedMovie.MapToMovieResponse();
            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var movieRemoved = await _moviesService.DeleteByIdAsync(id);
            if (!movieRemoved)
                return NotFound();
            return Ok();
        }
    }
}