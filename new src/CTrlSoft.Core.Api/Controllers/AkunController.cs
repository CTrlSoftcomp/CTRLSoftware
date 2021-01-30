using log4net;
using System;

using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace CTrlSoft.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AkunController : ControllerBase
    {
        private IAkun _interface;
        private ILog _ilog;
        private IWebHostEnvironment _hostEnvironment;

        public AkunController(AkunContext context, ILog log, IWebHostEnvironment environment)
        {
            this._interface = context;
            this._ilog = log;
            this._hostEnvironment = environment;
        }

        [HttpGet]
        public ActionResult<Models.JsonResult> GetAll()
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.GetAll();
        }

        [HttpGet("{id}", Name = "GetID")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.Get(_hostEnvironment, id);
        }

        [HttpGet, Route("get_by_kode")]
        public ActionResult<Models.JsonResult> GetByKode(string kode)
        {
            if (kode.Trim() != "") { 
                _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                return _interface.GetByKode(_hostEnvironment, kode);
            }else
                return BadRequest("Error while creating");

        }

        [HttpGet, Route("get_by_name")]
        public ActionResult<Models.JsonResult> GetByNama(string nama)
        {
            if (nama.Trim() != "")
            {
                _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
                return _interface.GetByNama(_hostEnvironment, nama);
            }
            else
                return BadRequest("Error while creating");

        }

        [HttpPost, Route("save")]
        public ActionResult<Models.JsonResult> Save([FromBody] Akun akun)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    return _interface.Save(akun);
                }
            } catch (Exception ex)
            {
                if (_ilog != null)
                    _ilog.Error("Error :", ex);
            }

            return BadRequest("Error while creating");
        }

        [HttpPost, Route("update")]
        public ActionResult<Models.JsonResult> Update([FromBody] Akun akun)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    return _interface.Update(akun);
                }
            }
            catch (Exception ex)
            {
                if (_ilog != null)
                    _ilog.Error("Error :", ex);
            }

            return BadRequest("Error while creating");
        }

        [HttpPost, Route("delete")]
        public ActionResult<Models.JsonResult> Delete([FromBody] Akun akun)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            try
            {
                if (akun == null || akun.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    return _interface.Delete(akun);
                }
            }
            catch (Exception ex)
            {
                if (_ilog != null)
                    _ilog.Error("Error :", ex);
            }

            return BadRequest("Error while creating");
        }
    }
}
