using System;
using System.Collections.Generic;
using System.Text;

namespace REALvisionApiLib
{
    public class ApiJwtResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string access_token { get; set; }

        public ApiJwtResponse()
        {
        }

        public ApiJwtResponse(string token_type, string expires_in, string ext_expires_in, string expires_on, string access_token)
        {
            this.token_type = token_type;
            this.expires_in = expires_in;
            this.ext_expires_in = ext_expires_in;
            this.expires_on = expires_on;
            this.access_token = access_token;
        }
    }
}


