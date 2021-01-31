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
    public class KontakController : ControllerBase
    {
        private IKontak _interface;
        private IWebHostEnvironment _hostEnvironment;

        public KontakController(KontakContext context, IWebHostEnvironment environment)
        {
            this._interface = context;
            this._hostEnvironment = environment;
        }

        [HttpGet]
        public ActionResult<Models.JsonResult> GetAll()
        {
            try
            {
                _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                return _interface.GetAll();
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.GetAll", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            try
            {
                _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                return _interface.Get(id);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Get", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("get_by_kode")]
        public ActionResult<Models.JsonResult> GetByKode(string kode)
        {
            if (kode.Trim() != "")
            {
                try
                {
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByKode(kode);
                }
                catch (Exception ex)
                {
                    Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Get_By_Kode", ex);
                    return BadRequest("Error while creating");
                }
            }
            else
                return BadRequest("Error while creating");

        }

        [HttpGet, Route("get_by_name")]
        public ActionResult<Models.JsonResult> GetByNama(string nama)
        {
            if (nama.Trim() != "")
            {
                try
                {
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByNama(nama);
                }
                catch (Exception ex)
                {
                    Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Get_By_Nama", ex);
                    return BadRequest("Error while creating");
                }
            }
            else
                return BadRequest("Error while creating");

        }

        [HttpPost, Route("save")]
        public ActionResult<Models.JsonResult> Save([FromBody] MKontak Kontak)
        {
            try
            {
                if (Kontak == null || Kontak.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.Save(Kontak);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Save", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("update")]
        public ActionResult<Models.JsonResult> Update([FromBody] MKontak Kontak)
        {
            try
            {
                if (Kontak == null || Kontak.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.Update(Kontak);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Update", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("delete")]
        public ActionResult<Models.JsonResult> Delete([FromBody] MKontak Kontak)
        {
            try
            {
                if (Kontak == null || Kontak.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.Delete(Kontak);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Delete", ex);
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
                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByFilter(filters);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Delete", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
