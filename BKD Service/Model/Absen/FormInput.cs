using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models.Absen
{
    public class FormInput
    {
        public string bulan { get; set; }
        public string tahun { get; set; }
        public string unker { get; set; }
        public string tipe { get; set; }
        public int retval { get; set; }
    }
}
