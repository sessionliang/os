using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.STL.Parser;
using SiteServer.STL.StlTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    [RoutePrefix("api/services/dynamic")]
    public class DynamicController : ApiController
    {
        private int PublishmentSystemID;
        private PublishmentSystemInfo PublishmentSystemInfo;
        private string ResutlString;

        [HttpPost]
        [Route("output")]
        public HttpResponseMessage Output()
        {

            PublishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetQueryString("PublishmentSystemID"));
            PublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(PublishmentSystemID);
            bool isCrossDomain = PageUtility.IsCrossDomain(PublishmentSystemInfo);

            if (PublishmentSystemInfo != null)
            {

                int pageNodeID = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageNodeID"), PublishmentSystemID);
                int pageContentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageContentID"));
                int pageTemplateID = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageTemplateID"));
                bool isPageRefresh = TranslateUtils.ToBool(RequestUtils.GetRequestString("isPageRefresh"));
                string templateContent = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("templateContent"));
                string ajaxDivID = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("ajaxDivID"));

                int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"), pageNodeID);
                int contentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("contentID"), pageContentID);

                string pageUrl = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("pageUrl"));
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageNum"));
                if (pageIndex > 0)
                {
                    pageIndex--;
                }

                NameValueCollection queryString = PageUtils.GetQueryStringFilterXSS(PageUtils.UrlDecode(HttpContext.Current.Request.RawUrl));
                queryString.Remove("publishmentSystemID");

                ResutlString = StlUtility.ParseDynamicContent(PublishmentSystemID, channelID, contentID, pageTemplateID, isPageRefresh, templateContent, pageUrl, pageIndex, ajaxDivID, queryString);

                ResutlString = ResutlString.Replace("\r\n", "").Trim("\"".ToCharArray());

            }

            //模拟原有aspx页面response.write，保证前台js统一处理返回数据
            //api默认返回的数据是string或者json
            //update by sessionliang at 20151221
            var response = new HttpResponseMessage();
            response.Content = new StringContent(ResutlString);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
