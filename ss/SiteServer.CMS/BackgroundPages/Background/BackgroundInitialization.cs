using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;

using BaiRong.Model;

using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundInitialization : BackgroundBasePage
	{
        public Literal ltlContent;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            string redirectUrl = PageUtils.GetAdminDirectoryUrl("main.aspx");

            List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
            if (publishmentSystemIDList == null || publishmentSystemIDList.Count == 0)
            {
                if (PermissionsManager.Current.IsSystemAdministrator)
                {
                    redirectUrl = PageUtils.GetSTLUrl("console_appAdd.aspx");
                }
            }

            this.ltlContent.Text = string.Format(@"
<img src=""../pic/animated_loading.gif"" align=""absmiddle"">
&nbsp;正在加载数据，请稍候...
<script language=""javascript"">
function redirectUrl()
{{
   location.href = ""{0}"";
}}
setTimeout(""redirectUrl()"", 2000);
</script>
", redirectUrl);
            
        }
	}
}
