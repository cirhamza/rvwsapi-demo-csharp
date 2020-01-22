using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib.Models
{
    public class ApiSettings
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }

        public ApiSettings()
        {
        }

        public ApiSettings(string apiKey, string apiUrl)
        {
            ApiKey = apiKey;
            ApiUrl = apiUrl;

        }
    }   
}
