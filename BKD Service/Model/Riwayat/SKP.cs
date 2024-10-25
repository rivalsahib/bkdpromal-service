using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class SKP
    {
        public int Id { get; set; }
        public string IdBkn { get; set; }
        public string IdOrang { get; set; }
        public string NipBaru { get; set; }
        public string NipLama { get; set; }
        public string Tahun { get; set; }
        public string Jenjab { get; set; }
        public string JenisPeraturanKinerjaKd { get; set; }
        public string Pelayanan { get; set; }
        public string Komitmen { get; set; }
        public string Kerjasama { get; set; }
        public string InisiatifKerja { get; set; }
        public string Integritas { get; set; }
        public string Disiplin { get; set; }
        public string Kepemimpinan { get; set; }
        public string NilaiPerilakuKerja { get; set; }
        public string NilaiPrestasiKerja { get; set; }
        public string NilaiSkp { get; set; }
        public string NilaiPerilakuKerjaRataRata { get; set; }
        public string KonversiNilai { get; set; }
        public string NilaiIntegrasi { get; set; }
        public string Jumlah { get; set; }
        public string PenilaiIdOrang { get; set; }
        public string PenilaiNonPns { get; set; }
        public string PenilaiNipNrp { get; set; }
        public string PenilaiNama { get; set; }
        public string PenilaiGolongan { get; set; }
        public string PenilaiTmtGolongan { get; set; }
        public string PenilaiJabatan { get; set; }
        public string PenilaiUnorNama { get; set; }
        public string PenilaiStatus { get; set; }
        public string AtasanPenilaiIdOrang { get; set; }
        public string AtasanPenilaiNonPns { get; set; }
        public string AtasanPenilaiNipNrp { get; set; }
        public string AtasanPenilaiNama { get; set; }
        public string AtasanPenilaiGolongan { get; set; }
        public string AtasanPenilaiTmtGolongan { get; set; }
        public string AtasanPenilaiJabatan { get; set; }
        public string AtasanPenilaiUnorNama { get; set; }
        public string AtasanPenilaiStatus { get; set; }
        public string path { get; set; }
        public string StatusVerifikasi { get; set; }
        public string PesanBKD { get; set; }
        public string PesanOPD { get; set; }
    }
}
