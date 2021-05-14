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
    [Route("[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private static RepLogger telegramBot = new RepLogger(ConfigurationManager.AppSetting["BotConfiguration:BotToken"], ConfigurationManager.AppSetting["BotConfiguration:ChannelID"]);
        private IWebHostEnvironment _hostEnvironment;

        public TelegramController(IWebHostEnvironment environment)
        {
            this._hostEnvironment = environment;
        }

        [HttpGet, Route("sendMessage")]
        public ActionResult<Models.JsonResult> sendMessage(string pesan)
        {
            try
            {
                telegramBot.BOT_SendMessageAsync(pesan);
                Models.JsonResult jsonResult = new Models.JsonResult { JSONMessage = "Pesan kekirim", JSONResult = true, JSONRows = 0, JSONValue = null };
                return jsonResult;
            }
            catch (Exception ex)
            {
                Repository.RepSqlDatabase.LogErrorQuery(_hostEnvironment, "Telegram.sendMessage", ex);
                return BadRequest("Error while creating");
            }
        }
    }
}
