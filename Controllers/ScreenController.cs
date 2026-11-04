
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        // GET: api/Screens
        [HttpGet]
        public async Task<IActionResult> GetAllScreens()
        {
            var screens = await _screenService.GetAllScreensAsync();
            return Ok(screens);
        }

        // GET: api/Screens/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var screen = await _screenService.GetScreenByIdAsync(id);

            if (screen == null)
                return NotFound("Screen not found");

            return Ok(screen);
        }

        // POST: api/Screens
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddScreen(Screen screen)
        {
            var createdScreen = await _screenService.AddScreenAsync(screen);

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = createdScreen.ScreenId },
                createdScreen);
        }

        // PUT: api/Screens/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(int id, Screen screen)
        {
            var updatedScreen = await _screenService.UpdateScreenAsync(id, screen);

            if (updatedScreen == null)
                return NotFound("Screen not found");

            return Ok(updatedScreen);
        }

        // DELETE: api/Screens/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var result = await _screenService.DeleteScreenAsync(id);

            if (!result)
                return NotFound("Screen not found");

            return NoContent();
        }
    }
}
