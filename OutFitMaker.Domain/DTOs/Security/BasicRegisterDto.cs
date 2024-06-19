using Microsoft.AspNetCore.Http;
using OutFitMaker.Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.DTOs.Security
{
    public class BasicRegisterDto
    {
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public GenderEnum Gender { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
