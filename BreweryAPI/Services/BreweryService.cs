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
        private readonly IRepository<WholesalerStock> _wholesalerStockRepository;
        public BreweryService(IMapper mapper, IRepository<BreweryStock> breweryStockRepository, IRepository<WholesalerStock> wholesalerStockRepository)
        {
            _mapper = mapper;
            _breweryStockRepository = breweryStockRepository;
            _wholesalerStockRepository = wholesalerStockRepository;
        }

        public async Task<bool> AddBeerToStock(AddBeerToStockDTO dto)
        {
            var breweryStock = await _breweryStockRepository.DbSet.FirstOrDefaultAsync(x =>
                    x.BreweryId == dto.BreweryId && x.BeerId == dto.BeerId);
            if (breweryStock == null)
            {
                var entity = _mapper.Map<AddBeerToStockDTO, BreweryStock>(dto);
                return await _breweryStockRepository.Add(entity);
            }
            else
            {
                breweryStock.Amount += dto.Amount;
                return await _breweryStockRepository.Update(breweryStock);
            }
        }

        public async Task<bool> SellBeerToWholesaler(SellBeerDTO dto)
        {
            var breweryStock = await _breweryStockRepository.DbSet.FirstOrDefaultAsync(x =>
                x.BreweryId == dto.BreweryId && x.BeerId == dto.BeerId);

            if(!IsBeerStockSufficient(breweryStock, dto.Amount)) 
                throw new InvalidDataException($"Brewery is out of stock(Requested: {dto.Amount}, Stock: {breweryStock?.Amount})");
            
            breweryStock.Amount -= dto.Amount;
            await _breweryStockRepository.Update(breweryStock);

            var wholesalerStock = await _wholesalerStockRepository.DbSet.FirstOrDefaultAsync(x =>
                x.WholesalerId == dto.WholesalerId && x.BeerId == dto.BeerId);
            if (wholesalerStock == null)
            {
                var entity = _mapper.Map<SellBeerDTO, WholesalerStock>(dto);
                return await _wholesalerStockRepository.Add(entity);
            }
            else
            {
                wholesalerStock.Amount += dto.Amount;
                return await _wholesalerStockRepository.Update(wholesalerStock);
            }
        }

        public static bool IsBeerStockSufficient(BreweryStock? breweryStock, int amount)
        {
            return (breweryStock != null && breweryStock.Amount >= amount);
        }

        public async Task<IList<BreweryStock>> GetStock(Guid id)
        {
            return await _breweryStockRepository.DbSet.AsNoTracking().Where(x => x.BreweryId == id).ToListAsync();
        }
    }
}
