using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Repository;
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

    }
}
