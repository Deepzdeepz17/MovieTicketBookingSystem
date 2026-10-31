
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        // GET: api/Movies/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
                return NotFound("Movie not found");

            return Ok(movie);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            var createdMovie = await _movieService.AddMovieAsync(movie);

            return CreatedAtAction(
                nameof(GetMovieById),
                new { id = createdMovie.MovieId },
                createdMovie);
        }

        // PUT: api/Movies/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            var updatedMovie = await _movieService.UpdateMovieAsync(id, movie);

            if (updatedMovie == null)
                return NotFound("Movie not found");

            return Ok(updatedMovie);
        }

        // DELETE: api/Movies/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (!result)
                return NotFound("Movie not found");

            return NoContent();
        }
    }
}
