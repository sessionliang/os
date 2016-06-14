using BaiRong.Core;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    public class PublishmentSystemController : ApiController
    {
        [HttpPost]
        [ActionName("ClearCache")]
        public IHttpActionResult ClearCache()
        {
            PublishmentSystemManager.ClearCache(false);

            return Ok();
        }
    }
}
