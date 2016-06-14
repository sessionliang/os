using System.Web.UI.WebControls;
using BaiRong.Core;
using System;


using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundNavFunction : BackgroundBasePage
	{
        public PlaceHolder phQCloud;

		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "微功能大全", AppManager.Platform.Permission.Platform_Site);

                if (FileConfigManager.Instance.IsSaasQCloud)
                {
                    this.phQCloud.Visible = true;
                }
            }
		}
	}
}
