using System.Security.Claims;
using LankaParts_Backend.DTOs.SellerOrders;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/seller/orders")]
    [Authorize(Roles = UserRoles.Seller)]
    public class SellerOrdersController : ControllerBase
    {
        private readonly ISellerOrderService _service;
        public SellerOrdersController(ISellerOrderService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status = null)
        {
            try { return Ok(await _service.GetAllAsync(GetUserId(), status)); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            try
            {
                var order = await _service.GetByIdAsync(GetUserId(), orderId);
                return order is null ? NotFound(new { message = "Seller order not found." }) : Ok(order);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, UpdateOrderStatusDto dto)
        {
            try { return Ok(await _service.UpdateStatusAsync(GetUserId(), orderId, dto.Status)); }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return Conflict(new { message = ex.Message }); }
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
