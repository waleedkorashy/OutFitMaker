using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.DTOs.Security
{
    public class EmailConfirmationDto
    {
        public required string Email { get; set; }
        public required string VerifyCode { get; set; }
        public string? FCMToken { get; set; }
    }
}
