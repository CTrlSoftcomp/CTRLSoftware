﻿using System;

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

        [HttpGet, Route("get_all")]
        public ActionResult<Models.JsonResult> GetAll(long iduser)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.GetAll", "All Data");

                _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                return _interface.GetAll(iduser);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.GetAll", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("get_by_id")]
        public ActionResult<Models.JsonResult> GetByID(long iduser, long id)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Get", id.ToString());

                _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                return _interface.Get(iduser, id);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.Get", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("get_by_kode")]
        public ActionResult<Models.JsonResult> GetByKode(long iduser, string kode)
        {
            if (kode.Trim() != "")
            {
                try
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Get_By_Kode", kode);

                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByKode(iduser, kode);
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
        public ActionResult<Models.JsonResult> GetByNama(long iduser, string nama)
        {
            if (nama.Trim() != "")
            {
                try
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Get_By_Nama", nama);

                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByNama(iduser, nama);
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
        public ActionResult<Models.JsonResult> Save(long iduser, [FromBody] MKontak Kontak)
        {
            try
            {
                if (Kontak == null || Kontak.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Save", Newtonsoft.Json.JsonConvert.SerializeObject(Kontak));

                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.Save(iduser, Kontak);
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
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Update", Newtonsoft.Json.JsonConvert.SerializeObject(Kontak));

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
        public ActionResult<Models.JsonResult> Delete(long iduser, [FromBody] MKontak Kontak)
        {
            try
            {
                if (Kontak == null || Kontak.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.Delete", Newtonsoft.Json.JsonConvert.SerializeObject(Kontak));

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
        public ActionResult<Models.JsonResult> List(long iduser, [FromBody] List<Models.DataFilters> filters)
        {
            try
            {
                if (filters == null)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Kontak.List", Newtonsoft.Json.JsonConvert.SerializeObject(filters));

                    _interface = HttpContext.RequestServices.GetService(typeof(KontakContext)) as KontakContext;
                    return _interface.GetByFilter(iduser, filters);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Kontak.List", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
