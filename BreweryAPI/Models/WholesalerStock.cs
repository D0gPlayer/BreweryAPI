using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class WholesalerStock
    {
        public Guid Id { get; set; }
        public Guid WholesalerId { get; set; }
        public Guid BeerId { get; set; } 
        public int Amount { get; set; }
    }
}
