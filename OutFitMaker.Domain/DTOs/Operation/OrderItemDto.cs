using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.DTOs.Operation
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public string? Note { get; set; }


    }
}
