using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models
{
    public class RefreshRequest
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
