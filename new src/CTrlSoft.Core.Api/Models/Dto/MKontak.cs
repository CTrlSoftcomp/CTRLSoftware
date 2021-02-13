using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models.Dto
{
    public class MKontak : Basic
    {
        public string alamat1 { get; set; }
        public string alamat2 { get; set; }
        public string alamat3 { get; set; }
        public string hp { get; set; }
        public string telpon { get; set; }
        public bool iswhatsapp { get; set; }
        public string norekening { get; set; }
        public string bank { get; set; }
        public string atasnamarekening { get; set; }
    }

    public class MKontakD
    {
        public long idkontak { get; set; }
        public long iduser { get; set; }
    }
}
