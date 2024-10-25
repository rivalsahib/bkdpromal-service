using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Model.Dokumen
{
    public class Dokumen
    {
        public string id_riwayat { get; set; }
        public string id_ref_dokumen { get; set; }
        public string file { get; set; }

        public string token_api_bkn { get; set; }

        public string token_akses_bkn { get; set; }
    }
}
