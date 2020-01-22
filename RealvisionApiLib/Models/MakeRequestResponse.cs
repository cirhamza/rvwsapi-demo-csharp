using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace REALvisionApiLib.Models
{
    public class MakeRequestResponse
    {
        public HttpStatusCode RequestStatusCode { get; set; }
        public string ResponseString { get; set; }

        public MakeRequestResponse(HttpStatusCode requestStatusCode, string responseString)
        {
            RequestStatusCode = requestStatusCode;
            ResponseString = responseString;
        }
    }
}
