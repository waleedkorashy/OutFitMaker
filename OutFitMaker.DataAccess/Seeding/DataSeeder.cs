using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Services.IServices.Security;
using OutFitMaker.Services.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.Seeding
{
    public static class DataSeeder
    {
        public static OutFitMakerDbContext context;

        public static IServiceCollection Services { get; set; }
        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    Services = services;
        //    //Services.AddScoped<IUserCreationService, UserCreationService>();
        //    //Services.AddScoped<IRoleCreationService, RoleCreationService>();
        //}
        //public async static Task EnsureDatabaseExistAsync(OutFitMakerDbContext context)
        //{
        //    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        //    if (pendingMigrations.Any())
        //    {
        //        await context.Database.MigrateAsync();
        //    }
        //}
        //public async static Task<bool> SeedDataAsync(IServiceCollection services)
        //{


        //    var provider = Services.BuildServiceProvider();
        //    var context = provider.GetService<OutFitMakerDbContext>();
        //    //  await EnsureDatabaseExistAsync(context);

        //    using (var transaction = await context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            if (!context.Users.Any())
        //            {
                        
                      
        //                await SeedIdentityAsync(provider);
        //            }
                   

        //            await transaction.CommitAsync();

        //            return true;
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();

        //            return false;
        //        }
        //    }
        //}
        //private async static Task SeedIdentityAsync(ServiceProvider serviceProvider)
        //{
        //    var userService = serviceProvider.GetService<IUserCreationService>();
        //    var roleService = serviceProvider.GetService<IRoleCreationService>();

        //    await roleService.CreateRolesAsync();
        //    await userService.CreateUsersAsync();
        //}

    }
}
