using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class Diklat
    {
        public string id { get; set; }
        public string id_bkn { get; set; }
        public string id_orang { get; set; }
        public string nipbaru { get; set; }
        public string niplama { get; set; }
        public string kd_jenis_diklat { get; set; }
        public string jenis_diklat { get; set; }
        public string kd_diklat { get; set; }
        public string nama_diklat { get; set; }
        public string jenis_kursus_sertifikat { get; set; }
        public string tempat_diklat { get; set; }
        public string penyelenggara { get; set; }
        public string angkatan { get; set; }
        public int jam { get; set; }
        public string tgl_mulai { get; set; }
        public string tgl_selesai { get; set; }
        public string tahun { get; set; }
        public string no_sttpp { get; set; }
        public string tgl_sttpp { get; set; }
        public string path_sertifikat { get; set; }
    }
}
