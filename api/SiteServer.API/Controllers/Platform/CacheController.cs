using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
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
    public class CacheController : ApiController
    {
        /// <summary>
        /// Çå³þUser»º´æ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveUserCache")]
        public IHttpActionResult RemoveUserCache()
        {
            string userName = RequestUtils.GetPostString("userName");
            if (!string.IsNullOrEmpty(userName))
                UserManager.RemoveCache(string.Empty, userName);
            else
                UserManager.Clear();
            return Ok(new { UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// Çå³þTableManager»º´æ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveTableManagerCache")]
        public IHttpActionResult RemoveTableManagerCache()
        {
            TableStyleManager.IsChanged = true;
            return Ok(new { UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// Çå³þUserConfig»º´æ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveUserConfigCache")]
        public IHttpActionResult RemoveUserConfigCache()
        {
            UserConfigManager.Clear();
            return Ok(new { UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// Çå³þÕ¾µã»º´æ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("clearPublishmentSystemCache")]
        public IHttpActionResult clearPublishmentSystemCache()
        {
            PublishmentSystemManager.ClearCache(false);
            return Ok(new { UpdateTime = DateTime.Now });
        }
    }
}
