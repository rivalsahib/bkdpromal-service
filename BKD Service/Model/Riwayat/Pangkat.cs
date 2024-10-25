using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.riwayat
{
    public class Pangkat
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string id_orang { get; set; }
        public string nipbaru { get; set; }
        public string niplama { get; set; }
        public int kd_pangkat { get; set; }
        public string nama_pangkat_gol { get; set; }
        public string nomor_sk { get; set; }
        public string tgl_sk { get; set; }
        public string tmt_pangkat { get; set; }
        public string pejabat_yang_menetapkan { get; set; }
        public string nomor_pertek { get; set; }
        public string tgl_pertek { get; set; }
        public string jumlahKreditUtama { get; set; }
        public string jumlahKreditTambahan { get; set; }
        public string id_jenis_kp_bkn { get; set; }
        public string kd_jenis_kp { get; set; }
        public string jenis_kp { get; set; } 
        public string gaji { get; set; }
        public string ms_th { get; set; }
        public string ms_bl { get; set; }
        public string path { get; set; }
    }
}
