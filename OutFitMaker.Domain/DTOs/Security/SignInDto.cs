using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.DTOs.Security
{
    public class SignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FCMToken { get; set; }
    }
}
