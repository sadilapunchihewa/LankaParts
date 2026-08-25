using System.Security.Claims;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;
        public AdminDashboardController(IAdminDashboardService service) => _service = service;

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard() => Ok(await _service.GetDashboardAsync());

        [HttpGet("users")]
        public async Task<IActionResult> Users(
            [FromQuery] string? role, [FromQuery] bool? isActive, [FromQuery] string? search)
        {
            try { return Ok(await _service.GetUsersAsync(role, isActive, search)); }
            catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("users/{userId:int}/activate")]
        public Task<IActionResult> Activate(int userId) => SetActive(userId, true);

        [HttpPatch("users/{userId:int}/deactivate")]
        public Task<IActionResult> Deactivate(int userId) => SetActive(userId, false);

        [HttpGet("parts/low-stock")]
        public async Task<IActionResult> LowStock([FromQuery] int threshold = 5)
        {
            try { return Ok(await _service.GetLowStockAsync(threshold)); }
            catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        }

        private async Task<IActionResult> SetActive(int userId, bool active)
        {
            try { return Ok(await _service.SetUserActiveAsync(GetUserId(), userId, active)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
