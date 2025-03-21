﻿using BreweryAPI.Models;

namespace BreweryAPI.Services.Interfaces
{
    public interface IBreweryService
    {
        public Task<bool> AddBeerToStock(AddBeerToStockDTO dto);
        public Task<IList<BreweryStock>> GetStock(Guid id);
        public Task<bool> SellBeerToWholesaler(SellBeerDTO dto);
    }
}
