using System;
using System.Collections.Generic;
using System.Text;

namespace CTrlSoft.Core.Bll.Api
{
    interface IBase<T>
        where T : class
    {
        int Save(T obj);
        int Update(T obj);
        int Delete(T obj);
        IList<T> GetAll();
    }
}
