using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweryController : BaseCRUDController<Brewery, BreweryDTO>
    {
        private IBreweryService _breweryService;
        public BreweryController(BaseCRUDService<Brewery, BreweryDTO> baseService, IBreweryService breweryService) : base(baseService)
        {
            _breweryService = breweryService;
        }

        [HttpPost("AddBeerToStock")]
        public async Task<IActionResult> AddBeerToStock(AddBeerToStockDTO dto)
        {
            var added = await _breweryService.AddBeerToStock(dto);
            return added ? Ok() : BadRequest();
        }

        [HttpGet("GetStock")]
        public virtual async Task<IActionResult> GetStock(Guid id)
        {
            var stock = await _breweryService.GetStock(id);
            return Ok(stock);
        }
    }
}
