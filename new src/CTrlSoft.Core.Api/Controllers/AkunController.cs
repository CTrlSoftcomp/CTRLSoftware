using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AkunController : ControllerBase
    {
        private IAkun _interface;

        public AkunController(AkunContext context)
        {
            this._interface = context;
        }

        // GET: api/Akun
        [HttpGet]
        public ActionResult<Models.JsonResult> GetAll()
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.GetAll();
        }

        // GET: api/Akun/{id}
        [HttpGet("{id}", Name = "GetID")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.Get(id);
        }

        // GET: api/Akun/kode/{kode}
        [HttpGet("kode/{kode}", Name = "GetKode")]
        public ActionResult<Models.JsonResult> GetKode(string kode)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.GetByKode(kode);
        }

        // GET: api/Akun/nama/{nama}
        [HttpGet("nama/{nama}", Name = "GetName")]
        public ActionResult<Models.JsonResult> GetNama(string nama)
        {
            _interface = HttpContext.RequestServices.GetService(typeof(AkunContext)) as AkunContext;
            return _interface.GetByName(nama);
        }

    }
}
