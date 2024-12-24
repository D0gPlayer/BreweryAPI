using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Brewery
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Brewery(string name)
        {
            Name = name;
        }
    }
}
