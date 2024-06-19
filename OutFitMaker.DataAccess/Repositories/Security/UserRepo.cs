using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.DataAccess.DTOs.Security;
using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.Domain;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Constants.Statics;
using OutFitMaker.Domain.DTOs.Security;
using OutFitMaker.Domain.Helper;
using OutFitMaker.Domain.Interfaces.Base;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Generic;
using OutFitMaker.Services.IServices.Security;
using OutFitMaker.Services.Services.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.Services.Security
{
    public class UserRepo : GenricRepo<UserSet>, IUserRepo
    {
        private IHttpContextAccessor _httpContextAccessor { get; }
        private UserManager<UserSet> _userManager { get; }
        private SignInManager<UserSet> _signInManager { get; }
        private IBaseServices _baseServices { get; }

        public UserRepo(OutFitMakerDbContext context 
            , UserManager<UserSet> userManager,
            SignInManager<UserSet> signInManager,
            IBaseServices baseServices,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _baseServices = baseServices;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<APIResponse<object>> SignInAsync(SignInDto dto)
        {
            var response = new APIResponse<object>();
            try
            {
                var userEntity = await _userManager.Users.Include(e=>e.Size).FirstOrDefaultAsync(e => !e.IsDeleted
                       && e.Email == dto.Email);
                if (userEntity is null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                var checkrole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userEntity.Id);
                var role = "";
                if (checkrole is not null)
                {
                    role = _context.Roles.FirstOrDefault(e => e.Id == checkrole.RoleId)!.Name;
                }
                var isInRole = await _userManager.IsInRoleAsync(userEntity, role);
                if (!isInRole)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(userEntity, dto.Password, false);
                if (!signInResult.Succeeded)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.InvalidEmailOrPassword;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                response.Data = new 
                {
                    Id = userEntity.Id,
                    Token = await _baseServices.GenerateJwt(userEntity, role.ToString()),
                    Email = userEntity.Email,
                    Name = userEntity.Name,
                    PhoneNumber = userEntity.PhoneNumber,
                    Gender = userEntity.Gender,
                    EmailConfirmed = userEntity.EmailConfirmed,
                    Size = userEntity.Size!= null ? userEntity.Size.Name : "",
                    Role = role.ToString(),

                };
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> SignUpAsync(BasicRegisterDto dto)
        {
            var response = new APIResponse<object>();
            using var trans = await BeginTransactionAsync();

            try
            {
                var checkExist = await _context.Users.FirstOrDefaultAsync(e => e.Email == dto.Email);
                if (checkExist != null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.IsExist;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }

                foreach (var validator in _userManager.PasswordValidators)
                {
                    var validPasswordresponse = await validator.ValidateAsync(_userManager, null, dto.Password);
                    if (!validPasswordresponse.Succeeded)
                    {
                        await trans.RollbackAsync();
                        response.Message = GlobalStatices.Fail;
                        foreach (var error in validPasswordresponse.Errors)
                        {
                            response.Message += error.Description;
                        }
                        response.Status = StatusCodes.Status400BadRequest;
                        return response;
                    }
                }
                var userNames = await _context.Users.Select(e => e.UserName).ToListAsync();
                var user = new UserSet()
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Gender = dto.Gender,
                    UserName = _baseServices.GenerateValidCode(userNames),
                };
               
                var userresponse = await _userManager.CreateAsync(user, dto.Password);
                if (!userresponse.Succeeded)
                {
                    await trans.RollbackAsync();
                    response.Message = GlobalStatices.Fail;
                    foreach (var error in userresponse.Errors)
                    {
                        response.Message += error.Description;
                    }
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                await _userManager.AddToRoleAsync(user, RolesEnum.Customer.ToString());

                user.EmailVerificationCode = _baseServices.GenerateRandomNumbers(4);
                string emailMessageBody = string.Format(GlobalStatices.EmailMessageBodyTemplate, $"{user.EmailVerificationCode}");

                MailService.SendMessage(user.Email, GlobalStatices.EmailMessageSubject, emailMessageBody);
                 _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                return response;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> ConfirmEmailAsync(EmailConfirmationDto dto)
        {
            var response = new APIResponse<object>();
            using var trans = await BeginTransactionAsync();
            try
            {
                var userEntity = await _context.Users.FirstOrDefaultAsync(e=> !e.IsDeleted && e.Email == dto.Email);
                if (userEntity is null) 
                {
                    await trans.RollbackAsync();
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                if (string.IsNullOrEmpty(userEntity.EmailVerificationCode))
                {
                    await trans.RollbackAsync();
                    response.Message = GlobalStatices.Fail + GlobalStatices.VerificationCodeTimeOutPleaseResendCode;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                if (userEntity.EmailVerificationCode != dto.VerifyCode)
                {
                    await trans.RollbackAsync();
                    response.Message = GlobalStatices.Fail + GlobalStatices.InvalidVerificationCode;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                userEntity.EmailConfirmed = true;
                userEntity.EmailVerificationCode = null;
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                return response;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> ResendPasswordCode(string email)
        {
            var response = new APIResponse<Object>();
            try
            {
                var userEntity = await _userManager.Users.FirstOrDefaultAsync(e => !e.IsDeleted && e.Email == email);
                if (userEntity is null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                var code = _baseServices.GenerateRandomNumbers(4);

                userEntity.PasswordVerificationCode = code;
                string emailMessageBody = string.Format(GlobalStatices.EmailMessageBodyTemplate, $"{code}");

                MailService.SendMessage(email, GlobalStatices.PasswordMessageSubject,
                    emailMessageBody);
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> ConfirmPasswordCodeAsync(EmailConfirmationDto dto)
        {
            var response = new APIResponse<Object>();
            try
            {
                var userEntity = await _userManager.Users.FirstOrDefaultAsync(e => !e.IsDeleted && e.Email == dto.Email);
                if (userEntity is null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                if (userEntity.PasswordVerificationCode != dto.VerifyCode)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.InvalidVerificationCode;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                userEntity.PasswordConfirmed = true;
                userEntity.PasswordVerificationCode = null;
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> ChangePasswordAsync(ResetPasswordDto dto)
        {
            var response = new APIResponse<Object>();
            try
            {
                var userEntity = await _userManager.Users.FirstOrDefaultAsync(e => !e.IsDeleted && e.Email == dto.Email);
                if (userEntity is null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (!userEntity.PasswordConfirmed)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.PleaseConfirmPasswordCodeFirst;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                foreach (var validator in _userManager.PasswordValidators)
                {
                    var validPasswordresponse = await validator.ValidateAsync(_userManager, null, dto.Password);
                    if (!validPasswordresponse.Succeeded)
                    {
                        response.Message = GlobalStatices.Fail;
                        foreach (var error in validPasswordresponse.Errors)
                        {
                            response.Message += error.Description;
                        }
                        response.Status = StatusCodes.Status400BadRequest;
                        return response;
                    }
                   
                }
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(userEntity, dto.Password);
                userEntity.PasswordHash = newPasswordHash;
                userEntity.PasswordVerificationCode = null;
                userEntity.PasswordConfirmed = false;
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;

            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }
        public Guid GetCurrentUserId()
        {
            var _httpcontext = _httpContextAccessor.HttpContext;
            if (_httpcontext != null
                    && _httpcontext.User != null
                    && _httpcontext.User.Identity != null
                    && _httpcontext.User.Identity.IsAuthenticated)
            {
                return Guid.Parse(_httpcontext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return Guid.Empty;
        }
        public async Task<APIResponse<object>> ConfirmUserSize(string size)
        {
            var response = new APIResponse<object>();
            try
            {
                var userId = GetCurrentUserId();
                var checkSize = await _context.Sizes.AsNoTracking().FirstOrDefaultAsync(e=>!e.IsDeleted && e.Name.ToLower().Contains( size.ToLower()));
                if (checkSize == null)
                {
                    checkSize = await _context.Sizes.AsNoTracking().FirstOrDefaultAsync();
                    if (checkSize == null)
                    {
                        response.Message = GlobalStatices.Fail + GlobalStatices.SizeNotFount;
                        response.Status = StatusCodes.Status400BadRequest;
                        return response;
                    }
                   
                }
                var userentity = await _context.Users.FirstOrDefaultAsync(e => !e.IsDeleted && e.Id == userId);
                if (userentity == null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.UserNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                   
                }
                userentity.SizeId = checkSize.Id;
                _context.Users.Update(userentity);
                await _context.SaveChangesAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;


            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }
    }
}
