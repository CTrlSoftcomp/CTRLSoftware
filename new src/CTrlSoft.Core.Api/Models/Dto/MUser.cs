using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models.Dto
{
    public class MUser
    {
        public int id { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
        public string nama { get; set; }
        public long idkontak { get; set; }
        public int idrole { get; set; }
    }

    public class MRole
    {
        public int id { get; set; }
        public string role { get; set; }
        public bool issupervisor { get; set; }
    }
}
