using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Controls;
using System.Web;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser;

namespace SiteServer.CMS.Services
{
    public class DynamicOutput : BasePage
    {
        public Literal ltlOutput;

        public void Page_Load(object sender, EventArgs E)
        {
            try
            {
                int pageNodeID = TranslateUtils.ToInt(base.Request["pageNodeID"], base.PublishmentSystemID);
                int pageContentID = TranslateUtils.ToInt(base.Request["pageContentID"]);
                int pageTemplateID = TranslateUtils.ToInt(base.Request["pageTemplateID"]);
                bool isPageRefresh = TranslateUtils.ToBool(base.Request["isPageRefresh"]);
                string templateContent = RuntimeUtils.DecryptStringByTranslate(Request["templateContent"]);
                string ajaxDivID = PageUtils.FilterSqlAndXss(base.Request["ajaxDivID"]);

                int channelID = TranslateUtils.ToInt(base.Request["channelID"], pageNodeID);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"], pageContentID);

                string pageUrl = RuntimeUtils.DecryptStringByTranslate(base.Request["pageUrl"]);
                int pageIndex = TranslateUtils.ToInt(base.Request["pageNum"]);
                if (pageIndex > 0)
                {
                    pageIndex--;
                }

                NameValueCollection queryString = PageUtils.GetQueryStringFilterXSS(PageUtils.UrlDecode(base.Request.RawUrl));
                queryString.Remove("publishmentSystemID");

                this.ltlOutput.Text = StlUtility.ParseDynamicContent(base.PublishmentSystemID, channelID, contentID, pageTemplateID, isPageRefresh, templateContent, pageUrl, pageIndex, ajaxDivID, queryString);
            }
            catch { }
        }
    }
}