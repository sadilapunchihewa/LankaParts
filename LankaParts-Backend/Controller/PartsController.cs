using LankaParts_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LankaParts_Backend.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartsController : ControllerBase
    {
        private readonly ISparePartService _service;
        public PartsController(ISparePartService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Browse([FromQuery] string? search, [FromQuery] int? categoryId) =>
            Ok(await _service.BrowseAsync(search, categoryId));

        [HttpGet("{partId:int}")]
        public async Task<IActionResult> GetById(int partId)
        {
            var part = await _service.GetPublicByIdAsync(partId);
            return part is null ? NotFound(new { message = "Spare part not found." }) : Ok(part);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories() => Ok(await _service.GetCategoriesAsync());
    }
}
