using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrlSoft.Models;
using CTrlSoft.Models.Dto;

namespace CTrlSoft.BLL.Master.Satuan
{
    public interface ISatuan
    {
        JsonResult List(List<DataFilters> filters);
        JsonResult Get(long noid);
        JsonResult Save(MSatuan obj, MUser user);
        JsonResult Delete(MSatuan obj, MUser user);
        JsonResult Update(MSatuan obj, MUser user);
    }
}
