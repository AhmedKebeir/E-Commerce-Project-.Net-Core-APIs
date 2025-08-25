using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }

        public int BrandId { get; set; } //Foregin Key

        public ProductBrand Brand { get; set; } //Navigational Property 1

        public int CategoryId { get; set; } //Foregin Key
        public ProductCategory Category { get; set; } // Navigational Property 1
    }
}
