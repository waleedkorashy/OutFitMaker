using Microsoft.EntityFrameworkCore;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Interfaces.Main;
using OutFitMaker.Domain.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.Repositories.Main
{
    public class SizeCreationServices : ISizeCreationServices
    {
        private readonly OutFitMaker.DataAccess.DbContext.OutFitMakerDbContext _context;
        public SizeCreationServices(OutFitMakerDbContext context )
        {
            _context = context;
        }
        public async Task CreateSizesAsync()
        {
            if (!await _context.Sizes.AnyAsync())
            {
                var size = new List<SizesSet>
                {
                    new() { Name = "S" },
                    new() { Name = "M" },
                    new() { Name = "L" },
                    new() { Name = "XL" },
                    new() { Name = "XXL" },
                    new() { Name = "XXXL" },
                    new() { Name = "XXXXL" }
                };
                await _context.Sizes.AddRangeAsync(size);
                await _context.SaveChangesAsync();
            }
            else if (!await _context.Colors.AnyAsync())
            {
                var color = new List<ColorsSet>
                {
                    new() { Name = "C1"},
                    new() { Name = "C2"},
                    new() { Name = "C3"},
                    new() { Name = "C4"},
                    new() { Name = "C5"}
                };
                await _context.Colors.AddRangeAsync(color);
                await _context.SaveChangesAsync();
            }
            else if (!await _context.Categories.AnyAsync())
            {
                var category = new List<CategorySet>
                {
                    new() { Name = "men_crew_short" , Gender = GenderEnum.Male},
                    new() { Name = "women_crew_short" , Gender = GenderEnum.Female}
                };
                await _context.Categories.AddRangeAsync(category);
                await _context.SaveChangesAsync();
            }



        }

    }
}
