using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.ModelS.Absen
{
    public class Kehadiran
    {
        public int id { get; set; }
        public string no { get; set; }
        public string nipbaru { get; set; }
        public string nama { get; set; }
        public DateTime tanggal { get; set; }
        public string shift { get; set; }
        public string status_absensi { get; set; }
        public string jam_masuk { get; set; }
        public string jam_pulang { get; set; }
        public string terlambat { get; set; }
        public string pulang_cepat { get; set; }
        public string jam_kerja { get; set; }

    }
}
