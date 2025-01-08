using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Beer : BaseEntity
    {
        public string Name{ get;set;}
        public float Price { get; set;}
        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; }
        public ICollection<BreweryStock> BreweryStocks { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }

        public Beer(string name, float price, Guid breweryId)
        {
            Name = name;
            Price = price;
            BreweryId = breweryId;
        }
    }

    public record BeerDTO
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public Guid BreweryId { get; set; }
    }
}
