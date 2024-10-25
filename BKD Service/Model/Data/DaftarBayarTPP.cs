using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Data
{
    public class DaftarBayarTPP
    {
        public int Id { get; set; }
        public string IdOrang { get; set; }
        public string NIPBaru { get; set; }
        public string Nama { get; set; }
        public string NPWP { get; set; }
        public string KodeStatusKepegawaian { get; set; }
        public string StatusKepegawaian { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string IdSatker { get; set; }
        public string Path { get; set; }
        public string Satker { get; set; }
        public string Unker { get; set; }
        public string Jenjab { get; set; }
        public string IdJabatan { get; set; }
        public string Jabatan { get; set; }
        public string IdKelasJabatan { get; set; }
        public string KelasJabatan { get; set; }
        public double NB { get; set; }
        public double NP { get; set; }
        public double NKK { get; set; }
        public double NKP { get; set; }
        public long BasicTPP { get; set; }
        public long BasicBebanKerja { get; set; }
        public long BasicPrestasiKerja { get; set; }
        public long BasicKondisiKerja { get; set; }
        public long BasicKelangkaanProfesi { get; set; }
        public int JumlahKegiatan { get; set; }
        public int JumlahHari { get; set; }
        public double PersenTidakMasuk { get; set; }
        public double PersenTL1 { get; set; }
        public double PersenTL2 { get; set; }
        public double PersenTL3 { get; set; }
        public double PersenTL4 { get; set; }
        public double PersenPSW1 { get; set; }
        public double PersenPSW2 { get; set; }
        public double PersenPSW3 { get; set; }
        public double PersenPSW4 { get; set; }
        public double PersenTidakApel { get; set; }
        public double PersenDisiplinKerja { get; set; }
        public double PersenProduktivitasKerja { get; set; }
        public long CapaianBebanKerja { get; set; }
        public long CapaianPrestasiKerja { get; set; }
        public long TPPSebelumPajak { get; set; }
        public double PersenPotonganPPH { get; set; }
        public long PotonganPPH { get; set; }
        public long TPPSetelahPPH { get; set; }
        public double PersenPotonganBPJS4Persen { get; set; }
        public double PersenPotonganBPJS { get; set; }
        public long PotonganBPJS4Persen { get; set; }
        public long PotonganBPJS { get; set; }
        public long TPPBersih { get; set; }
        public string DapatTPP { get; set; }
        public string Keterangan { get; set; }
    }
}
