using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class Penghargaan
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string id_orang { get; set; }
        public string nipbaru { get; set; }
        public string niplama { get; set; }
        public string kd_penghargaan { get; set; }
        public string kd_penghargaan_bkn { get; set; }
        public string no_SK { get; set; }
        public string tgl_SK { get; set; }
        public int tahun { get; set; }
        public string asal_perolehan { get; set; }
        public string path { get; set; }
    }
}
