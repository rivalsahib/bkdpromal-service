using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BKD_API.Models
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string GenerateToken()
        {
            var randomNumber = new Byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
