using CTrlSoft.Core.Api.Models;

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
