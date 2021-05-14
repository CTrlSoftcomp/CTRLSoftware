using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CTrlSoft.Core.Api.Repository
{
    public class RepLogger
    {
        private string BOT_API_KEY, ChannelID;
        public RepLogger(string BOT_API_KEY, string ChannelID)
        {
            this.BOT_API_KEY = BOT_API_KEY;
            this.ChannelID = ChannelID;
        }
        private ITelegramBotClient botClient;

        public async void BOT_SendMessageAsync(string Pesan)
        {
            string Message = "";
            try
            {
                if (botClient is null)
                {
                    botClient = new TelegramBotClient("");
                }

                if (checkInternet())
                {
                    Message = "[CTRLSoft] :" + Constants.vbCrLf + Constants.vbCrLf + "Reason : Pesan " + Constants.vbCrLf + Constants.vbCrLf + "Message : " + Pesan;
                    await botClient.SendTextMessageAsync(ChannelID, Message);
                }
            }
            catch (Exception)
            {
                //LogData("BOT_SendMessageAsync : " + ex.Message, "LogError_" + DateTime.Now.ToString("yyMMdd") + ".txt");
            }
        }

        public async void BOT_SendMessageAsync(Exception exception)
        {
            string Message = "";
            try
            {
                if (botClient is null)
                {
                    botClient = new TelegramBotClient(BOT_API_KEY);
                }

                if (checkInternet())
                {
                    Message = "[CTRLSoft] :" + Constants.vbCrLf + Constants.vbCrLf + "Reason : ERROR " + Constants.vbCrLf + Constants.vbCrLf + "Message : " + exception.Message + Constants.vbCrLf + Constants.vbCrLf + "StackTrace : " + exception.StackTrace; // & vbCrLf &
                    // "InnerException : " & IIf(exception.InnerException Is Nothing OrElse exception.InnerException Is System.DBNull.valu, "", exception.InnerException.Message & " at " & exception.InnerException.StackTrace)

                    await botClient.SendTextMessageAsync(ChannelID, Message);
                }
            }
            catch (Exception)
            {
                //LogData("BOT_SendMes*/sageAsync : " + ex.Message, "LogError_" + DateTime.Now.ToString("yyMMdd") + ".txt");
            }
        }

        private bool checkInternet()
        {
            try
            {
                if (botClient is null)
                {
                    botClient = new TelegramBotClient(BOT_API_KEY);
                }

                var hasil = botClient.GetMeAsync().Result;
                // Console.WriteLine("Hello, World! I am user {hasil.Id} and my name is {hasil.FirstName}.")
                return true;
            }
            catch (Exception)
            {
                //LogData("CheckInternet : " + ex.Message, "LogError_" + DateTime.Now.ToString("yyMMdd") + ".txt");
                return false;
            }
        }
    }
}
