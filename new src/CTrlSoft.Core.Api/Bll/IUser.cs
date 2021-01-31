using System.Collections.Generic;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Models;

namespace CTrlSoft.Core.Api.Bll
{
    interface IUser : IBase<MUser>
    {
        JsonResult Get(long id);
        JsonResult GetLogin(string userid, string pwd);
        JsonResult GetByFilter(List<DataFilters> filters);
        JsonResult GetChangePwd(MUser user, string oldpwd, string newpwd);

        //JsonResult Save(Kontak obj);
        JsonResult Save(MUser obj, ref ValidationError validationError);
        JsonResult Update(MUser obj, ref ValidationError validationError);
    }
}
