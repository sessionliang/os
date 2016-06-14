using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using SiteServer.STL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundRedirect : BackgroundBasePage
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];

            if (type == "DynamicPage")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);

                string redirectUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, 0, 0, 0, 0));
                
                PageUtils.Redirect(redirectUrl);
            }
        }
    }
}
