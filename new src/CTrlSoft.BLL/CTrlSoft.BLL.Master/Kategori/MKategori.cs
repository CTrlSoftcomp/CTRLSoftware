using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.BLL.Master.Kategori
{
    public class MKategori
    {
        public long id { get; set; }
        public long idparent { get; set; }
        public string kode { get; set; }
        public string nama { get; set; }
        public bool aktif { get; set; }
    }
}
