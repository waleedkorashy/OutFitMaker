using Microsoft.AspNetCore.Identity;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.Services.Security
{
    public class RoleCreationService : IRoleCreationService
    {
        private readonly RoleManager<RoleSet> _roleManager;

        public RoleCreationService(RoleManager<RoleSet> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task CreateRolesAsync()
        {
            var Roles = Enum.GetNames(typeof(RolesEnum));
            foreach (var role in Roles)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity is null)
                {
                    await _roleManager.CreateAsync(new RoleSet()
                    {
                        Name = role
                    });
                }
            }
        }
    }
}
