using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class PangkatBKN
    {
        public string golongan { get; set; }
        public string golonganId { get; set; }
        public string id { get; set; }
        public string idPns { get; set; }
        public string jenisKPId { get; set; }
        public string jenisKPNama { get; set; }
        public string jumlahKreditTambahan { get; set; }
        public string jumlahKreditUtama { get; set; }
        public string masaKerjaGolonganBulan { get; set; }
        public string masaKerjaGolonganTahun { get; set; }
        public string nipBaru { get; set; }
        public string nipLama { get; set; }
        public string noPertekBkn { get; set; }
        public Dictionary<string, PathItem> path { get; set; }
        public string skNomor { get; set; }
        public string skTanggal { get; set; }
        public string tglPertekBkn { get; set; }
        public string tmtGolongan { get; set; }

    }
}
