using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models
{
    public class RefreshToken
    {
        public int token_id { get; set; }
        public string nama_pengguna { get; set; }
        public string token { get; set; }
        public DateTime expired_date { get; set; }
    }
}
