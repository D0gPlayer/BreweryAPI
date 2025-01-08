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
        public BreweryController(BaseCRUDService<Brewery, BreweryDTO> baseService) : base(baseService){ }

    }
}
