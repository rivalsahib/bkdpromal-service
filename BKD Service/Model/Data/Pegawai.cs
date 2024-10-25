using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Data
{
    public class Pegawai
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string nip { get; set; }
        public string gelar_depan { get; set; }
        public string nama { get; set; }
        public string gelar_belakang { get; set; }
        public string tgl_lahir { get; set; }
        public string email { get; set; }
        public string nomor_telp { get; set; }

        public string pangkat { get; set; }
        public string jabatan { get; set; }
        public string id_satker { get; set; }
        public string satker { get; set; }
        public string id_unit_kerja { get; set; }
        public string unit_kerja { get; set; }
        public string foto { get; set; }
    }
}
