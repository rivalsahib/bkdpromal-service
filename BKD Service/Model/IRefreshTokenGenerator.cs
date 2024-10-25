using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken();
    }
}
