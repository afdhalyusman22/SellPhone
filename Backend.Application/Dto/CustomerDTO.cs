using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Application.Dto
{
    public partial class CustomerDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
