using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Models.Main;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.Services.Security
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManager<UserSet> _userManager;
        private readonly OutFitMaker.DataAccess.DbContext.OutFitMakerDbContext _context;
        public UserCreationService(UserManager<UserSet> userManager , OutFitMakerDbContext context )
        {
            _userManager = userManager;
            _context = context;
        }

      

        public async Task CreateUsersAsync()
        {
            var adminUser = new UserSet
            {
                UserName = "admin",
                Email = "Admin@gmail.com",
                Name = "Admin",
                PhoneNumber = "01200673084",
                EmailConfirmed = true,

            };

            var result = await _userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, RolesEnum.Admin.ToString());
            }
        }
    }
}
