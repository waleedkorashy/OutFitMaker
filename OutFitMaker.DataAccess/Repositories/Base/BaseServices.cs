using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Constants.Statics;
using OutFitMaker.Domain.Helper;
using OutFitMaker.Domain.Interfaces.Base;
using OutFitMaker.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.Repositories.Base
{
    public class BaseServices : IBaseServices
    {
        private readonly OutFitMakerDbContext _context;
        private readonly IOptions<EncryptionKey> _encryptionKey;
        private readonly UserManager<UserSet> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseServices(OutFitMakerDbContext context, IOptions<EncryptionKey> encryptionKey,
           UserManager<UserSet> userManager,
           IHttpContextAccessor httpContextAccessor) 
        {

            _context = context;
            _encryptionKey = encryptionKey;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
           
        }

        public string DecryptQRCode(string qrCode)
        {
            char[] result = new char[50];
            string ENC = "@QnhU64!z&9#Ke84hfogueb748%H&*@DghJ!kwfJLp&@A3z%s7";
            int i = 0;
            for (; i < qrCode.Length; i++)
            {
                result[i] = (char)(qrCode[i] ^ ENC[i]);
            }
            string s = new string(result)[0..^(result.Length - i)];
            return s;
        }

        public string EncryptQRCode(string randomCode)
        {
            char[] result = new char[50];
            for (int j = 0; j < 50; j++)
            {
                result[j] = 'X';
            }
            string ENC = "@QnhU64!z&9#Ke84hfogueb748%H&*@DghJ!kwfJLp&@A3z%s7";
            int i = 0;
            for (; i < randomCode.Length; i++)
            {
                result[i] = (char)(randomCode[i] ^ ENC[i]);
            }
            string s = new string(result)[0..^(result.Length - i)];
            return s;
        }

        public async  Task<string> GenerateJwt(UserSet user, string? role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalStatices.JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
             };

            var token = new JwtSecurityToken(

                issuer: null,
                audience: null,
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRandomCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var stringChars = new char[length];

            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public string GenerateRandomNumbers(int length)
        {
            Random random = new Random();
            var code = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                code.Append(random.Next(0, 10));
            }
            return code.ToString();
        }

        public string GenerateValidCode(List<string> codes, int size = 10)
        {
            var code = GenerateUniqueCode(size);

            while (codes.Contains(code))
            {
                code = GenerateUniqueCode(size);
            }

            return code;
        }
        private string GenerateUniqueCode(int size)
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
