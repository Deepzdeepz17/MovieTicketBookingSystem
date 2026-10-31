using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        // GET: api/Theatres
        [HttpGet]
        public async Task<IActionResult> GetAllTheatres()
        {
            var theatres = await _theatreService.GetAllTheatresAsync();
            return Ok(theatres);
        }

        // GET: api/Theatres/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(int id)
        {
            var theatre = await _theatreService.GetTheatreByIdAsync(id);

            if (theatre == null)
                return NotFound("Theatre not found");

            return Ok(theatre);
        }

        // POST: api/Theatres
        [HttpPost]
        public async Task<IActionResult> AddTheatre(Theatre theatre)
        {
            var createdTheatre = await _theatreService.AddTheatreAsync(theatre);

            return CreatedAtAction(
                nameof(GetTheatreById),
                new { id = createdTheatre.TheatreId },
                createdTheatre);
        }

        // PUT: api/Theatres/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheatre(int id, Theatre theatre)
        {
            var updatedTheatre = await _theatreService.UpdateTheatreAsync(id, theatre);

            if (updatedTheatre == null)
                return NotFound("Theatre not found");

            return Ok(updatedTheatre);
        }

        // DELETE: api/Theatres/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(int id)
        {
            var result = await _theatreService.DeleteTheatreAsync(id);

            if (!result)
                return NotFound("Theatre not found");

            return NoContent();
        }
    }
}