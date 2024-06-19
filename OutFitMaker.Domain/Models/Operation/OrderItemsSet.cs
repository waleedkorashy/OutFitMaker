using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain.Models.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Operation
{
    [Table("OrdersItems", Schema = "Operation")]
    public class OrderItemsSet : BaseEntity
    {
        public double Quentity { get; set; }
        public double Price { get; set; }
        public string? Note { get; set; }


        public OrderSet Order { get; set; }
        public Guid OrderId { get; set; }

        public ProductSet Product { get; set; }
        public Guid ProductId { get; set; }

    }
}
