using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.BLL.System.Menu
{
    public class MMenu
    {
        public int id { get; set; }
        public int idparent { get; set; }
        public int nourut { get; set; }
        public string name { get; set; }
        public string caption { get; set; }
        public bool isbig { get; set; }
        public bool isbegingroup { get; set; }
        public bool aktif { get; set; }
    }
}
