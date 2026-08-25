using System.Security.Claims;
using LankaParts_Backend.DTOs.Orders;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize(Roles = UserRoles.Customer)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDto dto)
        {
            try { return Ok(await _service.CheckoutAsync(GetUserId(), dto)); }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetMine() => Ok(await _service.GetMineAsync(GetUserId()));

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = await _service.GetMineByIdAsync(GetUserId(), orderId);
            return order is null ? NotFound(new { message = "Order not found." }) : Ok(order);
        }

        [HttpPatch("{orderId:int}/cancel")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            try { return Ok(await _service.CancelAsync(GetUserId(), orderId)); }
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
