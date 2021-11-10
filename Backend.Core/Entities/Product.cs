using Backend.Core.Entities.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Core.Entities
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
