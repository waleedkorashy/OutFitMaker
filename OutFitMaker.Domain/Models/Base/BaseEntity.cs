using OutFitMaker.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.Repositories.Base
{
    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            IsDeleted = false;
            CreationDate = DateTimeOffset.UtcNow;
            LastUpdatedDate = DateTimeOffset.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
    }
}
