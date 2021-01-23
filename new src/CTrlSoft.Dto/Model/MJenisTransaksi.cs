using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.Dto.Model
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
        //nourut int2	16	0	0					0	-1	0	0
        //idjenistransaksi int2	16	0	0					0	-1	0	0
        //jenis varchar	255	0	-1			pg_catalog	default	0	0	0	0
        //prefix varchar	255	0	-1			pg_catalog	default	0	0	0	0
        //sufix varchar	255	0	-1			pg_catalog	default	0	0	0	0
        //digit int2	16	0	-1					0	0	0	0
        //format varchar	255	0	-1			pg_catalog	default	0	0	0	0


        public int nourut { get; set; }
        public long idjenistransaksi { get; set; }
        public string jenis { get; set; }
        public string prefix { get; set; }
        public string sufix { get; set; }
        public int digit { get; set; }
        public string format { get; set; }

    }
}
