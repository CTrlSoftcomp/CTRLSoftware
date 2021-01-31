using System.Collections.Generic;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Models;

namespace CTrlSoft.Core.Api.Bll
{
    interface IKontak : IBase<MKontak>
    {
        JsonResult Get(long id);
        JsonResult GetByKode(string kode);
        JsonResult GetByNama(string name);
        JsonResult GetByFilter(List<DataFilters> filters);

        //JsonResult Save(Kontak obj);
        JsonResult Save(MKontak obj, ref ValidationError validationError);
        JsonResult Update(MKontak obj, ref ValidationError validationError);
    }
}
