using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;

namespace BreweryAPI.Services
{
    public class BeerService : BaseCRUDService<Beer, BeerDTO>
    {
        public BeerService(IRepository<Beer> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
