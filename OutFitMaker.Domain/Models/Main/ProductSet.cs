using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Main
{
    [Table("Products", Schema = "Main")]
    public class ProductSet: BaseEntity
    {
        #region Properties
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required double Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrlWithLogo { get; set; }
        public double Rate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int OrdersCount { get; set; }
        public bool IsUnique { get; set; } = false;
        public ProductFeatureEnum Type { get; set; }
        #endregion

        #region  Navigation properties
        public CategorySet Category { get; set; }
        public Guid CategoryId { get; set; }
        public ColorsSet Color { get; set; }
        public Guid ColorId { get; set; }

        public SizesSet Size { get; set; }
        public Guid SizeId { get; set; }
       

      
        #endregion


    }
}
