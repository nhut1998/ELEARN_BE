using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.Models.Globally
{
    public class ErrorModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message
        {
            get
            {
                return message.Replace("ORA-20002:", "").Split("ORA-06512:")[0].Trim();//RemoveDatabaseCode(message);
            }
            set
            {
                message = value;
            }
        }
        private string message;

        [JsonProperty("message_detail")]
        public string MessageDetail
        {
            get
            {
                return this.messagedDetail ?? message;
            }
            set
            {
                messagedDetail = value;
            }
        }
        private string messagedDetail;

        [JsonProperty("data")]
        public object Data { get; set; } = new List<object>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        private string RemoveDatabaseCode(string strInput)
        {
            try
            {
                string strReturn = null;
                int indexStart = strInput.IndexOf("ORA-", 0, strInput.Length);
                if (indexStart >= 0 && strInput.Length >= indexStart + 9)
                {
                    string strReplace = strInput.Substring(indexStart, 9);
                    strReturn = strInput.Replace(strReplace, "");
                    int indexStartSecond = strInput.IndexOf("ORA-", 0, strInput.Length);
                    if (indexStartSecond >= 0 && strReturn.Length >= indexStartSecond + 9)
                    {
                        strReturn = RemoveDatabaseCode(strReturn);
                    }
                }
                return strReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
