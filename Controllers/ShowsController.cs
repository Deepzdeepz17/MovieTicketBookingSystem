using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // GET: api/Shows
        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var shows = await _showService.GetAllShowsAsync();

            return Ok(shows);
        }

        // GET: api/Shows/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);

            if (show == null)
                return NotFound("Show not found.");

            return Ok(show);
        }

        // GET: api/Shows/1/seats
        [HttpGet("{showId}/seats")]
        public async Task<IActionResult> GetShowSeats(int showId)
        {
            // First check whether the show exists
            var show = await _showService.GetShowByIdAsync(showId);

            if (show == null)
                return NotFound("Show not found.");

            // Get all seats and their availability
            var seats = await _showService.GetShowSeatsAsync(showId);

            return Ok(seats);
        }

        // POST: api/Shows
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddShow(Show show)
        {
            var createdShow = await _showService.AddShowAsync(show);

            return CreatedAtAction(
                nameof(GetShowById),
                new { id = createdShow.ShowId },
                createdShow);
        }

        // PUT: api/Shows/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateShow(int id, Show show)
        {
            var updatedShow = await _showService.UpdateShowAsync(id, show);

            if (updatedShow == null)
                return NotFound("Show not found.");

            return Ok(updatedShow);
        }

        // DELETE: api/Shows/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var deleted = await _showService.DeleteShowAsync(id);

            if (!deleted)
                return NotFound("Show not found.");

            return Ok("Show deleted successfully.");
        }
    }
}