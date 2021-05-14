using System;

using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using CTrlSoft.Core.Api.DataAccess;

namespace CTrlSoft.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUser _interface;
        private IWebHostEnvironment _hostEnvironment;

        public UserController(UserContext context, IWebHostEnvironment environment)
        {
            this._interface = context;
            this._hostEnvironment = environment;
        }

        //[HttpGet]
        //public ActionResult<Models.JsonResult> GetAll()
        //{
        //    try
        //    {
        //        Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.GetAll", "GetAll");

        //        _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
        //        return _interface.GetAll();
        //    }
        //    catch (Exception ex)
        //    {
        //        Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.GetAll", ex);
        //        return BadRequest("Error while creating");
        //    }
        //}

        [HttpGet("{id}")]
        public ActionResult<Models.JsonResult> GetByID(long id)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Get", id.ToString());

                _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                return _interface.Get(id);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Get", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("available")]
        public ActionResult<Models.JsonResult> GetAvailableUser(string userid)
        {
            try
            {
                Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Available", userid);

                _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                return _interface.GetAvailableUser(userid);
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Get", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("save")]
        public ActionResult<Models.JsonResult> Save([FromBody] MUser User)
        {
            try
            {
                if (User == null || User.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Save", Newtonsoft.Json.JsonConvert.SerializeObject(User));

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.Save(User);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Save", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("update")]
        public ActionResult<Models.JsonResult> Update([FromBody] MUser User)
        {
            try
            {
                if (User == null || User.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Update", Newtonsoft.Json.JsonConvert.SerializeObject(User));

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.Update(User);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Update", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("delete")]
        public ActionResult<Models.JsonResult> Delete([FromBody] MUser User)
        {
            try
            {
                if (User == null || User.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Delete", Newtonsoft.Json.JsonConvert.SerializeObject(User));

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.Delete(User);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Delete", ex);
                return BadRequest("Error while creating");
            }
        }

        //[HttpPost, Route("list")]
        //public ActionResult<Models.JsonResult> List([FromBody] List<Models.DataFilters> filters)
        //{
        //    try
        //    {
        //        if (filters == null)
        //        {
        //            return BadRequest("Error while creating");
        //        }
        //        else
        //        {
        //            Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.List", Newtonsoft.Json.JsonConvert.SerializeObject(filters));

        //            _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
        //            return _interface.GetByFilter(filters);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.List", ex);
        //        return BadRequest("Error while creating");
        //    }
        //}

        [HttpGet, Route("login")]
        public ActionResult<Models.JsonResult> Login(string userid, string pwd)
        {
            try
            {
                if (userid == null || userid.Length <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else if (pwd == null || pwd.Length < 8)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Login", "UserID : " + userid + ", Pwd : " + pwd);

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.GetLogin(userid, pwd);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Login", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpGet, Route("available")]
        public ActionResult<Models.JsonResult> Available(string userid)
        {
            try
            {
                if (userid == null || userid.Length <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.Available", "UserID : " + userid);

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.GetAvailableUser(userid);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Login", ex);
                return BadRequest("Error while creating");
            }
        }

        [HttpPost, Route("changepassword")]
        public ActionResult<Models.JsonResult> ChangePwd([FromBody] MUser User, string oldpwd, string newpwd)
        {
            try
            {
                if (User == null || User.id <= 0)
                {
                    return BadRequest("Error while creating");
                }
                else if (oldpwd == null || oldpwd.Length < 8)
                {
                    return BadRequest("Error while creating");
                }
                else if (newpwd == null || newpwd.Length < 8)
                {
                    return BadRequest("Error while creating");
                }
                else
                {
                    Repository.RepSqlDatabase.LogConnection(_hostEnvironment, "User.ChangePassword", Newtonsoft.Json.JsonConvert.SerializeObject(User) + ", PwdOld : " + oldpwd + ", PwdNew : " + newpwd);

                    _interface = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
                    return _interface.GetChangePwd(User, oldpwd, newpwd);
                }
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "User.Login", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
