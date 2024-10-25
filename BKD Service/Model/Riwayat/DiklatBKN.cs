using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class DiklatBKN
    {
        public string id {get;set;}
        public string idPns { get; set; }
        public string nipBaru { get; set; }
        public string nipLama { get; set; }
        public string latihanStrukturalId { get; set; }
        public string latihanStrukturalNama { get; set; }
        public string nomor { get; set; }
        public string tanggal { get; set; }
        public string tahun {get;set;}
        public Dictionary<string, PathItem> path { get; set; }
        public string tanggalSelesai { get; set; }
        public string institusiPenyelenggara { get; set; }
    }
}
