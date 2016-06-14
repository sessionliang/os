using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class GatherDatabaseSet : BackgroundBasePage
	{
		protected DropDownList NodeIDDropDownList;
		protected TextBox GatherNum;

		private string gatherRuleName;

        public static string GetOpenWindowString(int publishmentSystemID, string gatherRuleName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherRuleName", gatherRuleName);
            return PageUtility.GetOpenWindowString("信息采集", "modal_gatherDatabaseSet.aspx", arguments, 460, 280);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "GatherRuleName");
            this.gatherRuleName = base.GetQueryString("GatherRuleName");

			if (!IsPostBack)
			{
				GatherDatabaseRuleInfo gatherDatabaseRuleInfo = DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(this.gatherRuleName, base.PublishmentSystemID);
                base.InfoMessage("采集名称：" + this.gatherRuleName);

				this.GatherNum.Text = gatherDatabaseRuleInfo.GatherNum.ToString();

                NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);
                ControlUtils.SelectListItems(this.NodeIDDropDownList, gatherDatabaseRuleInfo.NodeID.ToString());
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            GatherDatabaseRuleInfo gatherDatabaseRuleInfo = DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

			gatherDatabaseRuleInfo.NodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
			gatherDatabaseRuleInfo.GatherNum = TranslateUtils.ToInt(this.GatherNum.Text);
            DataProvider.GatherDatabaseRuleDAO.Update(gatherDatabaseRuleInfo);

            PageUtils.Redirect(ProgressBar.GetRedirectUrlStringWithGatherDatabase(base.PublishmentSystemID, this.gatherRuleName));
		}
	}
}
