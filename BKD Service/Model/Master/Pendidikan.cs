using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Master
{
    public class Pendidikan
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string id_tingkat_pendidikan  { get; set; }
        public string id_tingkat_pendidikan_bkn   { get; set; }
        public string mgr_cepat_kode { get; set; }
        public string nama_asli { get; set; }
        public string status { get; set; }
        public string subrumpun_prog_id { get; set; }
        public string cepat_kode_induk { get; set; }
        public string subrumpun_prog_kode { get; set; }
        public string kode_cepat { get; set; }
        public string nama_pendidikan { get; set; }
    }
}
