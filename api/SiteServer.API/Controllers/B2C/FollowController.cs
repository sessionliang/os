using BaiRong.Core;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.B2C
{
    public class FollowController : ApiController
    {
        [HttpGet]
        [ActionName("GetFollowList")]
        public IHttpActionResult GetFollowList()
        {
            List<FollowInfo> list = DataProviderB2C.FollowDAO.GetFollowInfoList(RequestUtils.CurrentUserName);
            List<FollowInfo> followList = new List<FollowInfo>();

            foreach (FollowInfo followInfo in list)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(followInfo.PublishmentSystemID);
                followInfo.ContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, followInfo.ChannelID, followInfo.ContentID);
                if (followInfo.ContentInfo != null)
                {
                    followInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, followInfo.ContentInfo, publishmentSystemInfo.Additional.VisualType);
                    followList.Add(followInfo);
                }
            }
            return Ok(list);
        }
    }
}
