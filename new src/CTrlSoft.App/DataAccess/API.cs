using CTrlSoft.App.CTrlSoft.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrlSoft.Repository;
using Newtonsoft.Json;

namespace CTrlSoft.App.DataAccess
{
    class API
    {
        private static CTrlSoft.Core.Api.Client client = new Client(Properties.Settings.Default.Base_Url, new System.Net.Http.HttpClient());
        
        public static async Task<bool> getLoginAsync(string UserID, string Pwd)
        {
            bool hasil = false;

            client.BaseUrl = Properties.Settings.Default.Base_Url;
            JsonResult json = await client.LoginAsync(UserID, Pwd);
            if (json != null)
            {
                hasil = (bool) json.JsonResult1;
                if (hasil)
                {
                    string jsonStr = JsonConvert.SerializeObject(json.JsonValue);
                    Repository.Utils.UserLogin = JsonConvert.DeserializeObject<Models.Dto.MUser>(jsonStr);
                } else
                {
                    RepLogger.ShowMessage(json.JsonMessage, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            return hasil;
        }

    }
}
