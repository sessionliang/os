using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.B2C
{
    public class RequestResponse
    {
        public RequestInfo Request { get; set; }
        public List<RequestAnswerInfo> AnswerList { get; set; }
    }
}