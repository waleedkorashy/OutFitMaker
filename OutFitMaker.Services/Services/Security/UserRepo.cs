using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Generic;
using OutFitMaker.Services.IServices.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Services.Services.Security
{
    public class UserRepo : GenricRepo<UserSet>, IUserRepo
    {

    }
}
