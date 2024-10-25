using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models.Akun
{
    public class ResetPengguna
    {
        public string nama_pengguna { get; set; }
        public string kata_sandi { get; set; }
        public string otp { get; set; }
    }
}
