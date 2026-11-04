
using Microsoft.AspNetCore.Authorization;
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
        // Anyone can view all movies without login
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            return Ok(movies);
        }

        // GET: api/Movies/1
        // Anyone can view a particular movie without login
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound(new
                {
                    message = "Movie not found."
                });
            }

            return Ok(movie);
        }

        // POST: api/Movies
        // Only Admin can add movies
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            var result = await _movieService.AddMovieAsync(movie);

            return Ok(result);
        }

        // PUT: api/Movies/1
        // Only Admin can update movies
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest(new
                {
                    message = "Movie ID mismatch."
                });
            }

            var result = await _movieService.UpdateMovieAsync(id, movie);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Movie not found."
                });
            }

            return Ok(result);
        }

        // DELETE: api/Movies/1
        // Only Admin can delete movies
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Movie not found."
                });
            }

            return NoContent();
        }
    }
}
