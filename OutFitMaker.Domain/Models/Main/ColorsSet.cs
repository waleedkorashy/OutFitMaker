using OutFitMaker.DataAccess.Repositories.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Main
{
    [Table("Colors", Schema = "Main")]
    public class ColorsSet : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<ProductSet> Products { get; set; } = default!;
    }
}
