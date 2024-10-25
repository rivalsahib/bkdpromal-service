using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models
{
    public class ResponOtentikasi
    {
        public string pesan { get; set; }
        public string nama_pengguna { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime expire_in { get; set; }
    }
}
