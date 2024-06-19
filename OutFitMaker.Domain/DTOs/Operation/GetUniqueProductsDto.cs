using OutFitMaker.Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.DTOs.Operation
{
    public class GetUniqueProductsDto
    {
        public Guid? ColorId { get; set; }
        public ProductFeatureEnum? Feature { get; set; }
        public Guid? CategoryId { get; set; }
        public GenderEnum? Gender { get; set; }
    }
}
