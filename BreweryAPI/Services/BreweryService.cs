using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BreweryStock> _breweryStockRepository;
        public BreweryService(IMapper mapper, IRepository<BreweryStock> breweryStockRepository)
        {
            _mapper = mapper;
            _breweryStockRepository = breweryStockRepository;
        }

        public async Task<bool> AddBeerToStock(AddBeerToStockDTO dto)
        {
            var entity = _mapper.Map<AddBeerToStockDTO, BreweryStock>(dto);
            return await _breweryStockRepository.Add(entity);
        }

        public async Task<IList<BreweryStock>> GetStock(Guid id)
        {
            return await _breweryStockRepository.DbSet.AsNoTracking().Where(x => x.BreweryId == id).ToListAsync();
        }
    }
}
