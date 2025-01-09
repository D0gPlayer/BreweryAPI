using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : BaseCRUDController<Beer, BeerDTO>
    {
        public BeerController(BaseCRUDService<Beer, BeerDTO> baseService) : base(baseService)
        {
        }
    }
}
