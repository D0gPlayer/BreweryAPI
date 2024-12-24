using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Beer
    {
        public Guid Id { get; set; }
        public string Name{ get;set;}
        public float Price { get; set;}

        [ForeignKey(nameof(Brewery))]
        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; }

        public Beer(string name, float price, Brewery brewery)
        {
            Name = name;
            Price = price;
            Brewery = brewery;
        }
    }
}
