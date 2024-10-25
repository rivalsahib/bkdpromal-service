using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class Hukuman
    {
        public int id { get; set; }  // int
        public string id_bkn { get; set; }
        public string kategori { get; set; }
        public string pns_orang_id { get; set; }  // varchar(50)
        public string pns_orang_nip { get; set; }  // varchar(50)
        public string jenis_tingkat_hukuman_id { get; set; }  // varchar(50)
        public string kedudukan_hukum { get; set; }  // varchar(50)
        public string golongan_id { get; set; }  // varchar(50)
        public string golongan_lama_id { get; set; }  // varchar(50)
        public string jenis_hukuman_id { get; set; }  // varchar(50)
        public string jenis_hukuman_nama { get; set; }  // varchar(150)
        public string sk_nomor { get; set; }  // varchar(70)
        public string sk_tanggal { get; set; }  // date
        public string hukuman_tanggal { get; set; }  // date
        public int masa_tahun { get; set; }  // int
        public int masa_bulan { get; set; }  // int
        public string akhir_hukum_tanggal { get; set; }  // date
        public string nomor_pp { get; set; }  // varchar(50)
        public string sk_pembatalan_nomor { get; set; }  // varchar(70)
        public string sk_pembatalan_tanggal { get; set; }  // date
        public string ncsistime { get; set; }  // datetime
        public string alasan_hukuman { get; set; }  // varchar(50)
        public string alasan_hukuman_nama { get; set; }  // varchar(150)
        public string rw_hukuman_disiplin { get; set; }  // varchar(150)
        public string keterangan { get; set; }  // varchar(150)
        public string path_sk_penetapan_hukuman { get; set; }  // varchar(150)
        public string path_sk_pengaktifan_hukuman { get; set; }  // varchar(150)
    }
}
