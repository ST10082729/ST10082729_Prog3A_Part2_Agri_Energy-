using System;
using System.Collections.Generic;

namespace AgriEnergy.Models
{
    public partial class Farmer
    {
        public Farmer()
        {
            Products = new HashSet<Product>();
        }

        public int FarmerId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactInfo { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
