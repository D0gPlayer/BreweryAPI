using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;

namespace BreweryAPI.Services
{
    public class BreweryService : BaseCRUDService<Brewery, BreweryDTO>
    {
        public BreweryService(IRepository<Brewery> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
