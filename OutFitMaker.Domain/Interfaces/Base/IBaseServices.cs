using OutFitMaker.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Interfaces.Base
{
    public interface IBaseServices
    {
        public Task<string> GenerateJwt(UserSet user, string role);
        public string GenerateRandomCode(int length);
        public string DecryptQRCode(string QR);
        public string EncryptQRCode(string randomCode);
        string GenerateRandomNumbers(int length);
        string GenerateValidCode(List<string> codes, int size = 10);
    }
}
