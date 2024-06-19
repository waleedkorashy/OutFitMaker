using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutFitMaker.DataAccess.DTOs.Security;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Constants.Statics;
using OutFitMaker.Domain.DTOs.Security;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Security;
using System.ComponentModel.DataAnnotations;

namespace OutFitMaker.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Properties and constructors
        private IUserRepo _userRepo { get; }
        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        } 
        #endregion


        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInDto dto)
        {
            var response = await _userRepo.SignInAsync(dto);
            return Ok(response);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        [Produces(typeof(object))]
        public async Task<IActionResult> SignUp([FromForm] BasicRegisterDto dto)
        {
            var response = await _userRepo.SignUpAsync(dto);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        [Produces(typeof(object))]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationDto dto)
        {
            var response = await _userRepo.ConfirmEmailAsync(dto);
            return Ok(response);
        }
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        [Produces(typeof(Object))]

        public async Task<IActionResult> ResendPasswordCode(string email)
        {
            var response = await _userRepo.ResendPasswordCode(email);
            return Ok(response);
        }
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        [Produces(typeof(Object))]
        public async Task<IActionResult> ConfirmPasswordCode(EmailConfirmationDto dto)
        {
            var response = await _userRepo.ConfirmPasswordCodeAsync(dto);
            return Ok(response);
        }
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        [Produces(typeof(object))]

        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var response = await _userRepo.ChangePasswordAsync(dto);
            return Ok(response);
        }
        //[Route("[action]")]
        //[HttpPost]
        //[AllowAnonymous]
        //[Authorize(Roles = GlobalStatices.Customer)]
        //public async Task<IActionResult> ConfirmSize([Required]string Size)
        //{
        //    var response = await _userRepo.ConfirmUserSize(Size);
        //    return Ok(response);
        //}
    }
}
