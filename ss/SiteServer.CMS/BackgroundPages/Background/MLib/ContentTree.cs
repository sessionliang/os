using SiteServer.CMS.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ContentTree : MLibBackgroundBasePage
    {
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.PublishmentSystemID == 0)
            {
                var publishmentSystemInfo = PublishmentSystemManager.GetUniqueMLib();
                if (publishmentSystemInfo != null)
                {

                    Response.Redirect("background_contentTree.aspx?publishmentsystemid=" + publishmentSystemInfo.PublishmentSystemID);
                }
            }
        }
    }
}
