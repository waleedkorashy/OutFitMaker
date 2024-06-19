using OutFitMaker.DataAccess.Repositories.Base;
using OutFitMaker.DataAccess.Repositories.Main;
using OutFitMaker.DataAccess.Repositories.Operation;
using OutFitMaker.Domain.Interfaces.Base;
using OutFitMaker.Domain.Interfaces.Main;
using OutFitMaker.Domain.Interfaces.Operation;
using OutFitMaker.Services.IServices.Security;
using OutFitMaker.Services.Services.Security;

namespace OutFitMaker.API.Utilities;

public static class InjectedDependenciesExtensions
{
    public static IServiceCollection InjectedDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IBaseEntity,BaseEntity>();
        services.AddScoped<IUserCreationService, UserCreationService>();
        services.AddScoped<IRoleCreationService, RoleCreationService>();
        services.AddScoped<IBaseServices,BaseServices>();
        services.AddScoped<ISizeCreationServices,SizeCreationServices> ();
        services.AddScoped<IOrderServices, OrderServices>();
        return services;
    }
}
