using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Model.Riwayat
{
    public class PendidikanBKN
    {
        public string id { get; set; }
        public string idPns { get; set; }
        public string nipBaru { get; set; }
        public string nipLama { get; set; }
        public string pendidikanId { get; set; }
        public string pendidikanNama { get; set; }
        public string tkPendidikanId { get; set; }
        public string tkPendidikanNama { get; set; }
        public string tahunLulus { get; set; }
        public string tglLulus { get; set; }
        public string isPendidikanPertama { get; set; }
        public string nomorIjasah { get; set; }
        public string namaSekolah { get; set; }
        public object gelarDepan { get; set; }
        public string gelarBelakang { get; set; }
        public Dictionary<string, PathItem> path { get; set; }

    }
}
