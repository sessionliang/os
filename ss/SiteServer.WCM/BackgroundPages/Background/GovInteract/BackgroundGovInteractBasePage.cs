using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WCM.BackgroundPages
{
	public class BackgroundGovInteractBasePage : BackgroundBasePage
	{
        protected override void OnInit(EventArgs e)
        {
            GovInteractManager.Initialize(base.PublishmentSystemInfo);
            base.OnInit(e);
        }
	}
}
