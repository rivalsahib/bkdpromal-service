using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class JabatanBKN
    {
        public string eselon { get; set; }
        public string eselonId { get; set; }
        public string id { get; set; }
        public string idPns { get; set; }
        public string instansiKerjaId { get; set; }
        public string instansiKerjaNama { get; set; }
        public string jabatanFungsionalId { get; set; }
        public string jabatanFungsionalNama { get; set; }
        public string jabatanFungsionalUmumId { get; set; }
        public string jabatanFungsionalUmumNama { get; set; }
        public string jenisJabatan { get; set; }
        public string namaJabatan { get; set; }
        public string namaUnor { get; set; }
        public string nipBaru { get; set; }
        public string nipLama { get; set; }
        public string nomorSk { get; set; }
        public Dictionary<string, PathItem> path { get; set; } 
        public string satuanKerjaId { get; set; }
        public string satuanKerjaNama { get; set; }
        public string tanggalSk { get; set; }
        public string tmtJabatan { get; set; }
        public string tmtPelantikan { get; set; }
        public string unorId { get; set; }
        public string unorIndukId { get; set; }
        public string unorIndukNama { get; set; }
        public string unorNama { get; set; }
    }
}
