using AutoMapper;
using BreweryAPI.Models;

namespace BreweryAPI.DataConfig
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile() 
        {
            CreateMap<BreweryDTO, Brewery>();
            CreateMap<BeerDTO, Beer>();
            CreateMap<AddBeerToStockDTO, BreweryStock>();
            CreateMap<WholesalerDTO, Wholesaler>();
            CreateMap<SellBeerDTO, BreweryStock>();
            CreateMap<SellBeerDTO, WholesalerStock>();
        }
    }
}
