using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Application.Dto
{
    public partial class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
