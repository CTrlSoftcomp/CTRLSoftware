using CTrlSoft.Core.Api.Models;
using CTrlSoft.Core.Api.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTrlSoft.Core.Api.Bll
{
    interface IBase<T>
        where T : class
    {
        JsonResult Save(T obj);
        JsonResult Update(T obj);
        JsonResult Delete(T obj);
        JsonResult GetAll();
    }
}
