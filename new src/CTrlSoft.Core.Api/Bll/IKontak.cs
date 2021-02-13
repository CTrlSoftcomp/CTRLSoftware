using System.Collections.Generic;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Models;

namespace CTrlSoft.Core.Api.Bll
{
    interface IKontak : IBase<MKontak>
    {
        JsonResult Get(long iduser, long id);
        JsonResult GetAll(long iduser);
        JsonResult GetByKode(long iduser, string kode);
        JsonResult GetByNama(long iduser, string name);
        JsonResult GetByFilter(long iduser, List<DataFilters> filters);

        //JsonResult Save(Kontak obj);
        JsonResult Save(long iduser, MKontak obj, ref ValidationError validationError);
        JsonResult Update(long iduser, MKontak obj, ref ValidationError validationError);
    }
}
