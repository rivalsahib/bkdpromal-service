using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class KursusBKN
    {
        public string id { get; set; }
        public string idPns { get; set; }
        public string nipBaru { get; set; }
        public string nipLama { get; set; }
        public string jenisKursusSertifikat { get; set; }
        public string institusiPenyelenggara { get; set; }
        public string jenisKursusId { get; set; }
        public string jumlahJam { get; set; }
        public string namaKursus { get; set; }
        public string noSertipikat { get; set; }
        public string tahunKursus { get; set; }
        public string tanggalKursus { get; set; }
        public Dictionary<string, PathItem> path { get; set; }
        public string jenisDiklatId { get; set; }
        public string tanggalSelesaiKursus { get; set; }
    }
}
