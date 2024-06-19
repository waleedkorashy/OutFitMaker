using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.IServices.Security
{
    public interface IRoleCreationService
    {
        Task CreateRolesAsync();
    }
}
