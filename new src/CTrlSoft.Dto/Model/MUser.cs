using System.Collections.Generic;

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
        public MRole roleuser { get; set; }
        public MUser()
        {
            this.roleuser = new MRole();
        }
    }

    public class MRole
    {
        public int id { get; set; }
        public string role { get; set; }
        public bool issupervisor { get; set; }
        public List<MRoleD> mrolesd { get; set; }
        public MRole()
        {
            this.mrolesd = new List<MRoleD>();
        }
    }

    public class MRoleD
    {
        public int idrole { get; set; }
        public int idmenu { get; set; }
        public bool aktif { get; set; }
    }
}
