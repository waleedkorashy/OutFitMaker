using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutFitMaker.Domain.Interfaces.Base;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Models.Main;

namespace OutFitMaker.Domain.Models.Security
{
    public class UserSet : IdentityUser<Guid>, IBaseEntity
    {
        #region Base 
        [Column(TypeName = "Varchar(50)")]
        [Required]
        public override string? UserName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastUpdatedDate { get; set; } = DateTimeOffset.UtcNow;

        
        #endregion

        #region Properties

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(6)")]
        public string? PasswordVerificationCode { get; set; }
        public string? EmailVerificationCode { get; set; }

        public bool PasswordConfirmed { get; set; }
       public bool EmailConfirmed { get; set; }
        public GenderEnum Gender { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? TempEmail { get; set; }
        public bool IsBanned { get; set; }
        public string? BanReason { get; set; }

        #endregion


        #region Navigation properties
        public Guid? SizeId { get; set; }
        public SizesSet Size { get; set; }
        #endregion
    }
}
