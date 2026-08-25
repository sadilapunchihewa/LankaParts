using System.Security.Claims;
using LankaParts_Backend.DTOs.Cart;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Roles = UserRoles.Customer)]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAsync(GetUserId()));

        [HttpPost("items")]
        public async Task<IActionResult> Add(AddCartItemDto dto)
        {
            try { return Ok(await _service.AddAsync(GetUserId(), dto)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("items/{cartItemId:int}")]
        public async Task<IActionResult> Update(int cartItemId, UpdateCartItemDto dto)
        {
            try { return Ok(await _service.UpdateAsync(GetUserId(), cartItemId, dto.Quantity)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("items/{cartItemId:int}")]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            try { await _service.RemoveAsync(GetUserId(), cartItemId); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _service.ClearAsync(GetUserId());
            return NoContent();
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
