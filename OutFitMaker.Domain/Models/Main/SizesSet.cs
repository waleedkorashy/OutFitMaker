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
    [Table("Sizes", Schema = "Main")]
    public class SizesSet : BaseEntity
    {

        #region Properties
        public required string Name { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<UserSet> Users { get; set; } = default!;

        public ICollection<ProductSet> Products { get; set; } = default!;
        #endregion

    }
}
