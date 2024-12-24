using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Wholesaler
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Wholesaler(string name)
        {
            Name = name;
        }
    }
}
