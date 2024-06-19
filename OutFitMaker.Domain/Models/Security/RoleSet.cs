using Microsoft.AspNetCore.Identity;
using OutFitMaker.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Models.Security
{
    public class RoleSet : IdentityRole<Guid> , IBaseEntity
    {
        #region Base 

        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastUpdatedDate { get; set; } = DateTimeOffset.UtcNow;
        #endregion
    }
}
