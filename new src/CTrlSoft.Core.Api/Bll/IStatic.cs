using CTrlSoft.Core.Api.Models;
using CTrlSoft.Core.Api.Models.Dto;
using System.Collections.Generic;

namespace CTrlSoft.Core.Api.Bll
{
    interface IJenisTransaksi : IBase<MJenisTransaksi>
    {
        JsonResult Get(long id);
        JsonResult GetByFilter(List<DataFilters> filters);
    }

    interface IRole : IBase<MRole>
    {
        JsonResult Get(long id);
        JsonResult GetByFilter(List<DataFilters> filters);
    }
}
