using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var shows = await _showService.GetAllShowsAsync();
            return Ok(shows);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);

            if (show == null)
                return NotFound("Show not found.");

            return Ok(show);
        }

        [HttpPost]
        public async Task<IActionResult> AddShow(Show show)
        {
            var createdShow = await _showService.AddShowAsync(show);

            return CreatedAtAction(
                nameof(GetShowById),
                new { id = createdShow.ShowId },
                createdShow);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(int id, Show show)
        {
            var updatedShow = await _showService.UpdateShowAsync(id, show);

            if (updatedShow == null)
                return NotFound("Show not found.");

            return Ok(updatedShow);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var deleted = await _showService.DeleteShowAsync(id);

            if (!deleted)
                return NotFound("Show not found.");

            return Ok("Show deleted successfully.");
        }
    }
}