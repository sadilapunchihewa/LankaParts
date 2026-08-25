using System.Security.Claims;
using LankaParts_Backend.DTOs.Reviews;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;
        public ReviewsController(IReviewService service) => _service = service;

        [HttpGet("part/{sparePartId:int}")]
        public async Task<IActionResult> GetForPart(int sparePartId) =>
            Ok(await _service.GetForPartAsync(sparePartId));

        [HttpGet("mine")]
        [Authorize(Roles = UserRoles.Customer)]
        public async Task<IActionResult> GetMine() => Ok(await _service.GetMineAsync(GetUserId()));

        [HttpPost]
        [Authorize(Roles = UserRoles.Customer)]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            try { return Ok(await _service.CreateAsync(GetUserId(), dto)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpPut("{reviewId:int}")]
        [Authorize(Roles = UserRoles.Customer)]
        public async Task<IActionResult> Update(int reviewId, UpdateReviewDto dto)
        {
            try { return Ok(await _service.UpdateAsync(GetUserId(), reviewId, dto)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpDelete("{reviewId:int}")]
        [Authorize(Roles = UserRoles.Customer)]
        public async Task<IActionResult> Delete(int reviewId)
        {
            try { await _service.DeleteAsync(GetUserId(), reviewId); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
