using System;
using System.Collections.Generic;

namespace AgriEnergy.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime ProductionDate { get; set; }
        public int FarmerId { get; set; }

        public virtual Farmer Farmer { get; set; } = null!;
    }
}
