
using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Main
{
    [Table("Categories", Schema = "Main")]
    public class CategorySet : BaseEntity
    {
        #region Properties
       
        public required string Name { get; set; }
        public string? Description { get; set; } 
        public GenderEnum Gender { get; set; }
        #endregion


    }
}
