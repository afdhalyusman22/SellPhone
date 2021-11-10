using Backend.Core.Entities;
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Application.Dto
{
    public partial class OrderDTO
    {
        public long Id { get; set; }
        public Guid OrderNo { get; set; }
        public long CustomerId { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }

    public partial class OrderAddDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public long CustomerId { get; set; }
    }

    public partial class OrderPaymentDTO
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
    }
}
