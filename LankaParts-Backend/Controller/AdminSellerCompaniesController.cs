using System.Security.Claims;
using LankaParts_Backend.DTOs.Admin;
using LankaParts_Backend.Helpers;
using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/admin/seller-companies")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminSellerCompaniesController : ControllerBase
    {
        private readonly IAdminSellerCompanyService _service;

        public AdminSellerCompaniesController(IAdminSellerCompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status = null)
        {
            try
            {
                return Ok(await _service.GetAllAsync(status));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{companyId:int}")]
        public async Task<IActionResult> GetById(int companyId)
        {
            var company = await _service.GetByIdAsync(companyId);
            return company is null
                ? NotFound(new { message = "Seller company application not found." })
                : Ok(company);
        }

        [HttpPatch("{companyId:int}/approve")]
        public Task<IActionResult> Approve(int companyId, ReviewSellerCompanyDto dto) =>
            Review(companyId, dto, true);

        [HttpPatch("{companyId:int}/reject")]
        public Task<IActionResult> Reject(int companyId, ReviewSellerCompanyDto dto) =>
            Review(companyId, dto, false);

        private async Task<IActionResult> Review(
            int companyId, ReviewSellerCompanyDto dto, bool approve)
        {
            try
            {
                var company = approve
                    ? await _service.ApproveAsync(companyId, GetUserId(), dto.Note)
                    : await _service.RejectAsync(companyId, GetUserId(), dto.Note);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
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
