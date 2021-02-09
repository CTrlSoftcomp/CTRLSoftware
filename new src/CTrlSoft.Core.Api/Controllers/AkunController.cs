using System;

using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using CTrlSoft.Core.Api.DataAccess;
using System.Collections.Generic;

namespace CTrlSoft.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AkunController : ControllerBase
    {
        private IAkun _interface;
        private IWebHostEnvironment _hostEnvironment;

        public AkunController(AkunContext context, IWebHostEnvironment environment)
        {
            this._interface = context;
            this._hostEnvironment = environment;
        }

        [HttpGet]
        public ActionResult<Models.JsonResult> GetAll()
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.GetAll", "All Data");

                _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                return _interface.GetAll();
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.GetAll", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Get", id.ToString());

                _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                return _interface.Get(id);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Get", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("get_by_kode")]
        public ActionResult<Models.JsonResult> GetByKode(string kode)
        {
            if (kode.Trim() != "") {
                try
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Get_By_Kode", kode);

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.GetByKode(kode);
                }
                catch (Exception ex)
                {
                    Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Get_By_Kode", ex);
                    return BadRequest("Error while creating");
                }
            }else
                return BadRequest("Error while creating");

        }

        [HttpGet, Route("get_by_name")]
        public ActionResult<Models.JsonResult> GetByNama(string nama)
        {
            if (nama.Trim() != "")
            {
                try
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Get_By_Nama", nama);

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.GetByNama(nama);
                }
                catch (Exception ex)
                {
                    Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Get_By_Nama", ex);
                    return BadRequest("Error while creating");
                }
            }
            else
                return BadRequest("Error while creating");

        }

        [HttpPost, Route("save")]
        public ActionResult<Models.JsonResult> Save([FromBody] MAkun akun)
        {
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Save", Newtonsoft.Json.JsonConvert.SerializeObject(akun));

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.Save(akun);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Save", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("update")]
        public ActionResult<Models.JsonResult> Update([FromBody] MAkun akun)
        {
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Update", Newtonsoft.Json.JsonConvert.SerializeObject(akun));

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.Update(akun);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Update", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("delete")]
        public ActionResult<Models.JsonResult> Delete([FromBody] MAkun akun)
        {
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.Delete", Newtonsoft.Json.JsonConvert.SerializeObject(akun));

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.Delete(akun);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.Delete", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("list")]
        public ActionResult<Models.JsonResult> List([FromBody] List<Models.DataFilters> filters)
        {
            try
            {
                if (filters == null)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Akun.List", Newtonsoft.Json.JsonConvert.SerializeObject(filters));

                    _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                    return _interface.GetByFilter(filters);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Akun.List", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
