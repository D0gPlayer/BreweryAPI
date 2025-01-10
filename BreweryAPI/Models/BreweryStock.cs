using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class BreweryStock : BaseEntity
    {
        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; }
        public Beer Beer { get; set; }
        public Guid BeerId { get; set; } 
        public int Amount { get; set; }
    }

    public record AddBeerToStockDTO
    {
        public Guid BreweryId { get; set; }
        public Guid BeerId { get; set; } 
        public int Amount { get; set; }
    }

    public record SellBeerDTO
    {
        public Guid BreweryId { get; set; }
        public Guid BeerId { get; set; }
        public Guid WholesalerId { get; set; }
        public int Amount { get; set; }
    }
}
