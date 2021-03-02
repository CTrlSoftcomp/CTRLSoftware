using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrlSoft.Models;
using CTrlSoft.Models.Dto;

namespace CTrlSoft.BLL.System.User
{
    public interface IUser
    {
        JsonResult GetLogin(string UserID, string pwd);
        JsonResult List(List<DataFilters> filters);
        JsonResult Get(long noid);
        JsonResult Save(MUser obj, MUser user);
        JsonResult Delete(MUser obj, MUser user);
        JsonResult Update(MUser obj, MUser user);
    }
}
