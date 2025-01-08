using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Brewery : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Beer> Beers { get; set; }
        public ICollection<BreweryStock> BreweryStocks { get; set; }
        public Brewery(string name)
        {
            Name = name;
        }
    }

    public record BreweryDTO
    {
        public string Name{ get;set;}

        public BreweryDTO(string name)
        {
            Name = name;
        }
    }
}
