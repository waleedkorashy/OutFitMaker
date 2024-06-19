using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Main
{
    [Table("FavoriteProducts", Schema = "Operation")]
    public class FavouriteProductSet : BaseEntity
    {
        public UserSet? User { get; set; } = default!;
        public Guid UserId { get; set; }

        public ProductSet? Product { get; set; } = default!;
        public Guid ProductId { get; set; }
    }
}
