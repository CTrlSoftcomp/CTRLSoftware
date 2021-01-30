using System;
using System.Collections.Generic;
using System.Text;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Models;
using Microsoft.AspNetCore.Hosting;

namespace CTrlSoft.Core.Api.Bll
{
    interface IAkun : IBase<Akun>
    {
        JsonResult Get(IWebHostEnvironment environment, long id);
        JsonResult GetByKode(IWebHostEnvironment environment, string kode);
        JsonResult GetByNama(IWebHostEnvironment environment, string name);

        //JsonResult Save(Akun obj);
        JsonResult Save(Akun obj, ref ValidationError validationError);
        JsonResult Update(Akun obj, ref ValidationError validationError);
    }
}
