using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Models.Dto
{
    public class MUser
    {
        public long id { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
        public string nama { get; set; }
        public int idkontak { get; set; }
        public int idrole { get; set; }
        public bool aktif { get; set; }
    }

    public class MRole
    {
        public int id { get; set; }
        public string role { get; set; }
        public bool issupervisor { get; set; }
    }
}
