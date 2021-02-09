using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.DataAccess;
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
    public class RoleController : ControllerBase
    {
        private IRole _interface;
        private IWebHostEnvironment _hostEnvironment;

        public RoleController(RoleContext context, IWebHostEnvironment environment)
        {
            this._interface = context;
            this._hostEnvironment = environment;
        }

        [HttpGet]
        public ActionResult<Models.JsonResult> GetAll()
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Role.GetAll", "GetAll");

                _interface = HttpContext.RequestServices.GetService(typeof(RoleContext)) as RoleContext;
                return _interface.GetAll();
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Role.GetAll", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Role.Get", id.ToString());

                _interface = HttpContext.RequestServices.GetService(typeof(RoleContext)) as RoleContext;
                return _interface.Get(id);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Role.Get", ex);
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
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "Role.List", Newtonsoft.Json.JsonConvert.SerializeObject(filters));

                    _interface = HttpContext.RequestServices.GetService(typeof(RoleContext)) as RoleContext;
                    return _interface.GetByFilter(filters);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Role.List", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
