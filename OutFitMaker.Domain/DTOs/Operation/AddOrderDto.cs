using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.DTOs.Operation
{
    public class AddOrderDto
    {
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
