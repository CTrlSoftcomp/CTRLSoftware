using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models.Dto
{
    class MJenisTransaksi : Basic
    {
        
        public int nourut { get; set; }
        public string keterangan { get; set; }
        public List<MJenisTransaksiD> jenistransaksid { get; set; }

        public MJenisTransaksi()
        {
            jenistransaksid = new List<MJenisTransaksiD>();
        }
    }

    class MJenisTransaksiD
    {
        public int nourut { get; set; }
        public long idjenistransaksi { get; set; }
        public string jenis { get; set; }
        public string prefix { get; set; }
        public string sufix { get; set; }
        public int digit { get; set; }
        public string format { get; set; }

    }
}
