using Backend.Core.Entities.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Core.Entities
{
    public partial class Order : BaseEntity
    {
        public Guid OrderNo { get; set; }
        public long CustomerId { get; set; }
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
