using Backend.Core.Entities.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Core.Entities
{
    public partial class OrderDetail : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
