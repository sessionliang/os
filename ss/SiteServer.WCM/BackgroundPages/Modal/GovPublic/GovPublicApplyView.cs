﻿using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using System.Text;

using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
    public class GovPublicApplyView : BackgroundGovPublicApplyToDetailBasePage
	{
        public static string GetOpenWindowString(int publishmentSystemID, int applyID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ApplyID", applyID.ToString());
            arguments.Add("ReturnUrl", string.Empty);
            return PageUtilityWCM.GetOpenWindowString("快速查看", "modal_govPublicApplyView.aspx", arguments, 750, 600, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            
        }
	}
}
