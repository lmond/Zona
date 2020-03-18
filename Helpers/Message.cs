using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Zona.Models;

namespace Zona.Helpers
{
    class Message
    {
        public static async Task<MessageModel> GetMessageAsync()
        {
            MessageModel result = null;
            try
            {
                string url = "https://www.lmond.com/muviz/messagenew.json";
                string response = await Browser.GetResponse(url).ConfigureAwait(false) ?? null;
                result = JsonConvert.DeserializeObject<MessageModel>(response);
            }
            catch (Exception)
            {

            }
            return result;
        }
    }
}