using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using System.Text;


namespace SiteServer.Project.BackgroundPages.Modal
{
    public class ApplyView : BackgroundApplyToDetailBasePage
	{
        public static string GetShowPopWinString(int projectID, int applyID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("ApplyID", applyID.ToString());
            arguments.Add("ReturnUrl", string.Empty);
            return JsUtils.OpenWindow.GetOpenWindowString("快速查看", "modal_applyView.aspx", arguments, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
            }
        }
	}
}
