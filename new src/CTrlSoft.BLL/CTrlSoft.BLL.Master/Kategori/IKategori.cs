using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrlSoft.Models;
using CTrlSoft.Models.Dto;

namespace CTrlSoft.BLL.Master.Kategori
{
    public interface IKategori
    {
        JsonResult List(List<DataFilters> filters);
        JsonResult Get(long noid);
        JsonResult Save(MKategori obj, MUser user);
        JsonResult Delete(MKategori obj, MUser user);
    }
}
