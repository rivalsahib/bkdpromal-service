using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class PK
    {
        public int id { get; set; }
        public string id_orang { get; set; }
        public string nipbaru { get; set; }
        public string nama { get; set; }
        public string jenis_jabatan { get; set; }
        public string jenis_formasi { get; set; }
        public string kd_tingkat_pendidikan { get; set; }
        public string tingkat_pendidikan { get; set; }
        public string kd_pendidikan { get; set; }
        public string pendidikan { get; set; }
        public string kd_jabatan { get; set; }
        public string jabatan { get; set; }
        public string id_satker { get; set; }
        public string satker { get; set; }
        public string unker { get; set; }
        public string id_lokasi_kerja { get; set; }
        public string lokasi_kerja { get; set; }
        public string rencana_perjanjian_kontrak_kerja { get; set; }
        public string tanggal_awal { get; set; }
        public string tanggal_akhir { get; set; }
        public string gol_ruang_awal { get; set; }
        public string tahun_gaji { get; set; }
        public string gaji { get; set; }
        public string path_pk { get; set; }
    }
}
