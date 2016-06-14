using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class Touch : BackgroundBasePage
	{
        public Literal ltlPageInfoScript;

        private int leadID;
        private int orderID;
        public static string GetShowPopWinString(int leadID, int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("leadID", leadID.ToString());
            arguments.Add("orderID", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("联系情况", "modal_touch.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.leadID = TranslateUtils.ToInt(base.Request.QueryString["leadID"]);
            this.orderID = TranslateUtils.ToInt(base.Request.QueryString["orderID"]);

			if (!IsPostBack)
			{
                this.ltlPageInfoScript.Text = string.Format(@"
<script>
var $pageInfo = {{leadID : {0}, orderID : {1}}};
</script>
", this.leadID, this.orderID);
			}
		}
	}
}
