using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.riwayat
{
    public class Pendidikan
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string id_orang { get; set; }
        public string nipbaru { get; set; }
        public string niplama { get; set; }
        public string id_pendidikan_bkn { get; set; }
        public string kd_pendidikan { get; set; }
        public string nama_pendidikan { get; set; }
        public string id_tingkat_pendidikan_bkn { get; set; }
        public string kd_tingkat_pendidikan { get; set; }
        public string nama_tingkat_pendidikan { get; set; }
        public string jurusan { get; set; }
        public string tahun_lulus { get; set; }
        public string tgl_lulus { get; set; }
        public string isPendidikanPertama { get; set; }

        public string nama_kepsek_rektor { get; set; }
        public string nama_sekolah { get; set; }
        public string nomor_ijazah { get; set; }
        public string tempat_sekolah { get; set; }
        public string gd { get; set; }
        public string gb { get; set; }
        public string path_ijazah { get; set; }
        public string path_transkrip { get; set; }
    }
}
