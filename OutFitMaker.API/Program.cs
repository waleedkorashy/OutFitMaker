using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Constants.Statics;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Domain.Helper;
using OutFitMaker.API.Utilities;
using OutFitMaker.DataAccess.Seeding;
using Microsoft.Extensions.DependencyInjection;
using OutFitMaker.Services.IServices.Security;
using OutFitMaker.Domain.Interfaces.Main;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy(GlobalStatices.CorsPolicy,
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddIdentity<UserSet, RoleSet>()
          .AddEntityFrameworkStores<OutFitMakerDbContext>()
          .AddDefaultTokenProviders();
builder.Services.AddDbContext<OutFitMakerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(GlobalStatices.OutFitMakerConnectionString),
    b => b.MigrationsAssembly(typeof(OutFitMakerDbContext).Assembly.FullName)));

builder.Services.AddJWT()
                .AddSwaggerDocumantation()
                .ConfigIdentityOptions()
                .ConfigureApiBehaviorOptions();
builder.Services.AddEndpointsApiExplorer();

#region DI

builder.Services.InjectedDependencies();

#endregion



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<EncryptionKey>(builder.Configuration.GetSection("EncryptionKey"));
builder.Services.AddHttpContextAccessor();


// Register HttpClient
builder.Services.AddHttpClient();


var app = builder.Build();
SeedData(app);
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
static async void SeedData(IHost app) //can be placed at the very bottom under app.Run()
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
   
    var dbInitializer1 = scope.ServiceProvider.GetService<IRoleCreationService>();
    await dbInitializer1!.CreateRolesAsync();

    var dbInitializer = scope.ServiceProvider.GetService<IUserCreationService>();
    await dbInitializer!.CreateUsersAsync(); 
    var dbInitializer2 = scope.ServiceProvider.GetService<ISizeCreationServices>();
    await dbInitializer2!.CreateSizesAsync();
}

var directory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
if (!Directory.Exists(directory))
    Directory.CreateDirectory(directory);
StaticFileOptions staticFileOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine
          (app.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
};
app.UseStaticFiles(staticFileOptions);




if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerDocumantation();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
