using CTrlSoft.Models;
using CTrlSoft.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.BLL.System.Role
{
    public interface IRole
    {
        JsonResult List(List<DataFilters> filters);
        JsonResult Get(int id);
        JsonResult Save(MRole obj, MUser user);
        JsonResult Delete(MRole obj, MUser user);
        JsonResult Update(MRole obj, MUser user);
    }
}
