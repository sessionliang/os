using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.CRM.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class ApplySwitchTo : BackgroundBasePage
	{
        protected TextBox tbSwitchToRemark;
        public HtmlControl divSwitchToUserName;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("转办办件", "modal_applySwitchTo.aspx", arguments, "IDCollection", "请选择需要转办的办件！", 600, 500);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                this.divSwitchToUserName.Attributes.Add("onclick", Modal.UserNameSelect.GetShowPopWinString(0, "switchToUserName"));
                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                string switchToUserName = base.Request.Form["switchToUserName"];
                if (string.IsNullOrEmpty(switchToUserName))
                {
                    base.FailMessage("转办失败，必须选择转办人员");
                    return;
                }

                foreach (int applyID in this.idArrayList)
                {
                    EApplyState state = DataProvider.ApplyDAO.GetState(applyID);
                    if (state != EApplyState.Denied && state != EApplyState.Checked)
                    {
                        ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(applyID);
                        applyInfo.DepartmentID = AdminManager.GetDepartmentID(switchToUserName);
                        applyInfo.UserName = switchToUserName;
                        DataProvider.ApplyDAO.Update(applyInfo);

                        RemarkInfo remarkInfo = new RemarkInfo(0, applyInfo.ID, ERemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.RemarkDAO.Insert(remarkInfo);

                        ApplyManager.LogSwitchTo(applyInfo.ID, switchToUserName);
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
