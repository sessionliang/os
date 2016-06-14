using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.CMS.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractApplySwitchTo : BackgroundBasePage
	{
        protected TextBox tbSwitchToRemark;
        public HtmlControl divAddDepartment;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int nodeID;
        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("转办办件", "modal_govInteractApplySwitchTo.aspx", arguments, "IDCollection", "请选择需要转办的办件！", 500, 500);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                this.divAddDepartment.Attributes.Add("onclick", Modal.GovInteractDepartmentSelect.GetOpenWindowString(base.PublishmentSystemID, this.nodeID));
                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                int switchToDepartmentID = TranslateUtils.ToInt(base.Request.Form["switchToDepartmentID"]);
                if (switchToDepartmentID == 0)
                {
                    base.FailMessage("转办失败，必须选择转办部门");
                    return;
                }
                string switchToDepartmentName = DepartmentManager.GetDepartmentName(switchToDepartmentID);

                foreach (int contentID in this.idArrayList)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    if (contentInfo.State != EGovInteractState.Denied && contentInfo.State != EGovInteractState.Checked)
                    {
                        DataProvider.GovInteractContentDAO.UpdateDepartmentID(base.PublishmentSystemInfo, contentID, switchToDepartmentID);

                        if (!string.IsNullOrEmpty(this.tbSwitchToRemark.Text))
                        {
                            GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, contentInfo.NodeID, contentID, EGovInteractRemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                            DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);
                        }

                        GovInteractApplyManager.LogSwitchTo(base.PublishmentSystemID, contentInfo.NodeID, contentID, switchToDepartmentName);
                    }
                }

                isChanged = true;
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('办件转办成功!');"));
			}
		}

	}
}
