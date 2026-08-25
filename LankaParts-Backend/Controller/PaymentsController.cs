using System.Security.Claims;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize(Roles = UserRoles.Customer)]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentsController(IPaymentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMine() => Ok(await _service.GetMineAsync(GetUserId()));

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var payment = await _service.GetByOrderAsync(GetUserId(), orderId);
            return payment is null ? NotFound(new { message = "Payment not found." }) : Ok(payment);
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
