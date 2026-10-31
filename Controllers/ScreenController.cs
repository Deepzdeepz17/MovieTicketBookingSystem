using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScreens()
        {
            var screens = await _screenService.GetAllScreensAsync();
            return Ok(screens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var screen = await _screenService.GetScreenByIdAsync(id);

            if (screen == null)
                return NotFound("Screen not found");

            return Ok(screen);
        }

        [HttpPost]
        public async Task<IActionResult> AddScreen(Screen screen)
        {
            var createdScreen = await _screenService.AddScreenAsync(screen);

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = createdScreen.ScreenId },
                createdScreen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(int id, Screen screen)
        {
            var updatedScreen = await _screenService.UpdateScreenAsync(id, screen);

            if (updatedScreen == null)
                return NotFound("Screen not found");

            return Ok(updatedScreen);
        }

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