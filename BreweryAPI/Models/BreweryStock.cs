﻿using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models
{
    [PrimaryKey(nameof(Id))]
    public class BreweryStock
    {
        public Guid Id { get; set; }
        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; }
        public Beer Beer { get; set; }
        public Guid BeerId { get; set; } 
        public int Amount { get; set; }
    }
}
