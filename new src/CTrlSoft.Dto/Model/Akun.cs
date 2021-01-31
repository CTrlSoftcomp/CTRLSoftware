using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.Models.Dto
{
    class Akun : Basic
    {
        public long idparent { get; set; }
        public int iddepartemen { get; set; }
        public string keterangan { get; set; }
        public int idtype { get; set; }
        public bool isdebet { get; set; }
        public bool iskasbank { get; set; }
        public string norekening { get; set; }
        public string atasnamarekening { get; set; }
        public int idtypebank { get; set; }
        public bool isneraca { get; set; }
    }
}
