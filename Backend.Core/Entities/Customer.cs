using Backend.Core.Entities.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Core.Entities
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
