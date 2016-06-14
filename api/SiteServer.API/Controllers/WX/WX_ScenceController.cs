using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SiteServer.API.Controllers.WX
{
    public class WX_ScenceController : ApiController
    {
        [HttpGet]
        [ActionName("GetScenceParameter")]
        public IHttpActionResult GetScenceParameter(int id)
        {
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            string poweredBy = string.Empty;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            DataProviderWX.ScenceDAO.UpdateClickNum(id, publishmentSystemInfo.PublishmentSystemID);

            ScenceInfo scenceInfo = DataProviderWX.ScenceDAO.GetScenceInfo(id);

            return Ok(scenceInfo.ClickNum);
        }
    }
}