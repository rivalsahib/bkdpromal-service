using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_API.Models.Akun
{
    public class Pengguna
    {
        public int id { get; set; }
        public string id_orang { get; set; }
        public string nama { get; set; }
        public string id_profil_pengguna { get; set; }
        public string nama_pengguna { get; set; }
        public DateTime tmt_pengguna { get; set; }
        public DateTime tgl_selesai_pengguna { get; set; }
        public string dokumen { get; set; }
        public string status { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_edited { get; set; }
        public DateTime date_deleted { get; set; }

    }
}
