using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OutFitMaker.Domain.Models.Main;
using OutFitMaker.Domain.Models.Operation;
using OutFitMaker.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.DbContext
{
    public class OutFitMakerDbContext : IdentityDbContext<UserSet , RoleSet , Guid>
    {
        #region Constructors
        public OutFitMakerDbContext(DbContextOptions<OutFitMakerDbContext> options) : base(options)
        {
        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            #region Security
            modelBuilder.Entity<UserSet>().ToTable("Users", "Security");
            modelBuilder.Entity<RoleSet>().ToTable("Roles", "Security");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UsersRoles", "Security");
            modelBuilder.Entity<UserSet>().Property(e => e.UserName).HasColumnType("varchar").HasMaxLength(25);
            modelBuilder.Entity<UserSet>().Property(e => e.NormalizedUserName).HasColumnType("varchar").HasMaxLength(25);
            modelBuilder.Entity<UserSet>().Property(e => e.PhoneNumber).HasMaxLength(15);
            modelBuilder.Entity<UserSet>().Property(e => e.Email).HasMaxLength(50);
            modelBuilder.Entity<UserSet>().Property(e => e.NormalizedEmail).HasMaxLength(50);
            modelBuilder.Entity<RoleSet>().Property(e => e.Name).HasMaxLength(25);
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
            #endregion
        }
        #endregion

        #region DbSets

        #region Main
      
        public DbSet<FavouriteProductSet> FavouriteProduct { get; set; }
        public DbSet<ProductSet> Products { get; set; }
        public DbSet<ColorsSet> Colors { get; set; }
       // public DbSet<LogosSet> Logos { get; set; }
        public DbSet<SizesSet> Sizes { get; set; }
        public DbSet<OrderSet> Orders { get; set; }
        public DbSet<OrderItemsSet> OrderItems { get; set; }
        public DbSet<CategorySet> Categories { get; set; }
        #endregion

        #endregion

        //public class YourDbContextFactory : IDesignTimeDbContextFactory<OutFitMakerDbContext>
        //{
        //    public OutFitMakerDbContext CreateDbContext(string[] args)
        //    {
        //        var optionsBuilder = new DbContextOptionsBuilder<OutFitMakerDbContext>();
        //        optionsBuilder.UseSqlServer(SecretConstant.DefaultConnectionString
        //                                    , opt => opt.UseNetTopologySuite());
        //        return new OutFitMakerDbContext(optionsBuilder.Options);
        //    }
        //}
    }
}
