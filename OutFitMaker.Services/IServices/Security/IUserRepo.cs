using OutFitMaker.DataAccess.DTOs.Security;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Helper;
//using OutFitMaker.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.IServices.Security
{
    public interface IUserRepo
    {
        Task<APIResponse<object>> SignInAsync(SignInDto dto, RolesEnum role, bool? fromMobile = null);
    }
}
