using OutFitMaker.DataAccess.DTOs.Security;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.DTOs.Security;
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
        Task<APIResponse<object>> SignInAsync(SignInDto dto);
        Task<APIResponse<object>> SignUpAsync(BasicRegisterDto dto);
        Task<APIResponse<object>> ConfirmEmailAsync(EmailConfirmationDto dto);
        Task<APIResponse<object>> ConfirmPasswordCodeAsync(EmailConfirmationDto dto);
        Task<APIResponse<Object>> ResendPasswordCode(string email);
        Task<APIResponse<Object>> ConfirmUserSize(string size);
        Guid GetCurrentUserId();
        Task<APIResponse<Object>> ChangePasswordAsync(ResetPasswordDto dto);


    }
}
