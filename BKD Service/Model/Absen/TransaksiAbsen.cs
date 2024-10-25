using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models.Absen
{
    public class TransaksiAbsen
    {
        public int id { get; set; }
        public string id_orang { get; set; }
        public string nip { get; set; }
        public string jenis_absen { get; set; }
        public string jenis_waktu_absen { get; set; }
        public string tanggal { get; set; }
        public string waktu { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
}
