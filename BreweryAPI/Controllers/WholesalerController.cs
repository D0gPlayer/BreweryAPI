using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholesalerController : BaseCRUDController<Wholesaler,WholesalerDTO>
    {
        public WholesalerController(BaseCRUDService<Wholesaler, WholesalerDTO> baseService) : base(baseService)
        {
        }
    }
}
