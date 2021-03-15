using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrlSoft.Models;
using CTrlSoft.Models.Dto;

namespace CTrlSoft.BLL.System.Menu
{
    public interface IMenu
    {
        JsonResult List(List<DataFilters> filters);
        JsonResult Get(long noid);
        JsonResult Save(MMenu obj, MUser user);
        JsonResult Delete(MMenu obj, MUser user);
        JsonResult Update(MMenu obj, MUser user);
    }
}
