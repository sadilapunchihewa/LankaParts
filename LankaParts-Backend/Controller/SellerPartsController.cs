using System.Security.Claims;
using LankaParts_Backend.DTOs.Parts;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/seller/parts")]
    [Authorize(Roles = UserRoles.Seller)]
    public class SellerPartsController : ControllerBase
    {
        private readonly ISparePartService _service;
        public SellerPartsController(ISparePartService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMine() => Ok(await _service.GetMineAsync(GetUserId()));

        [HttpPost]
        public async Task<IActionResult> Create(UpsertSparePartDto dto)
        {
            try
            {
                var part = await _service.CreateAsync(GetUserId(), dto);
                return CreatedAtAction(nameof(GetMine), new { }, part);
            }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{partId:int}")]
        public async Task<IActionResult> Update(int partId, UpsertSparePartDto dto)
        {
            try { return Ok(await _service.UpdateAsync(GetUserId(), partId, dto)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{partId:int}")]
        public async Task<IActionResult> Deactivate(int partId)
        {
            try { await _service.DeactivateAsync(GetUserId(), partId); return NoContent(); }
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
