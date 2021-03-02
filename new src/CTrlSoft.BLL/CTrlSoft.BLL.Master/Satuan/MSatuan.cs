using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.BLL.Master.Satuan
{
    public class MSatuan
    {
        public long id { get; set; }
        public string kode { get; set; }
        public string nama { get; set; }
        public int konversi { get; set; }
        public bool aktif { get; set; }
    }
}
