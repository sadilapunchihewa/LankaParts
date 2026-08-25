using System.Security.Claims;
using LankaParts_Backend.DTOs.SellerCompanies;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/seller-companies")]
    [Authorize(Roles = UserRoles.Seller)]
    public class SellerCompaniesController : ControllerBase
    {
        private readonly ISellerCompanyService _sellerCompanyService;

        public SellerCompaniesController(ISellerCompanyService sellerCompanyService)
        {
            _sellerCompanyService = sellerCompanyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSellerCompanyDto dto)
        {
            try
            {
                var company = await _sellerCompanyService.CreateAsync(GetUserId(), dto);
                return CreatedAtAction(nameof(GetMine), null, company);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMine()
        {
            var company = await _sellerCompanyService.GetMineAsync(GetUserId());
            return company is null
                ? NotFound(new { message = "Company profile not found." })
                : Ok(company);
        }

        private int GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var userId)
                ? userId
                : throw new UnauthorizedAccessException("Invalid user token.");
        }
    }
}
