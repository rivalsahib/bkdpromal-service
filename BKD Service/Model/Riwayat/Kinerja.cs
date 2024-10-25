using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class Kinerja
    {
        public int id { get; set; }
        public string id_bkn { get; set; }
        public string nipbaru { get; set; }
        public string niplama { get; set; }
        public string hasilKinerja { get; set; }
        public string hasilKinerjaNilai { get; set; }
        public string kuadranKinerja { get; set; }
        public string KuadranKinerjaNilai { get; set; }
        public string namaPenilai { get; set; }
        public string nipNrpPenilai { get; set; }
        public string penilaiGolonganId { get; set; }
        public string penilaiJabatanNm { get; set; }
        public string penilaiUnorNm { get; set; }
        public string perilakuKerja { get; set; }
        public string PerilakuKerjaNilai { get; set; }
        public string pnsDinilaiId { get; set; }
        public string statusPenilai { get; set; }
        public string tahun { get; set; }
        public string path { get; set; }
        public string statusVerifikasi { get; set; }
        public string pesanBKD { get; set; }
        public string pesanOPD { get; set; }
    }
}
