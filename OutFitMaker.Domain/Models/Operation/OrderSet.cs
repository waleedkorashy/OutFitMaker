using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Operation
{
    [Table("Orders", Schema = "Operation")]
    public class OrderSet : BaseEntity
    {
        public OrderStatusEnum OrderStatus { get; set; }
        public double Total {  get; set; }
        public required string OrderNumber { get; set; }

        public UserSet? User { get; set; } = default!;
        public Guid UserId { get; set; }

        public ICollection<OrderItemsSet> orderItems { get; set; } = default!;
    }
}
