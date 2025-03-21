﻿using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class Wholesaler : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }
    }

    public record WholesalerDTO
    {
        public string Name { get; set; }
    }
}
