﻿using System.Collections.Generic;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Models;

namespace CTrlSoft.Core.Api.Bll
{
    interface IAkun : IBase<MAkun>
    {
        JsonResult Get(long id);
        JsonResult GetByKode(string kode);
        JsonResult GetByNama(string name);
        JsonResult GetByFilter(List<DataFilters> filters);

        //JsonResult Save(Akun obj);
        JsonResult Save(MAkun obj, ref ValidationError validationError);
        JsonResult Update(MAkun obj, ref ValidationError validationError);
    }
}
