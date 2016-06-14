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
	public class GovPublicApplySwitchTo : BackgroundBasePage
	{
        protected TextBox tbSwitchToRemark;
        public HtmlControl divAddDepartment;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("转办申请", "modal_govPublicApplySwitchTo.aspx", arguments, "IDCollection", "请选择需要转办的申请！", 500, 500);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                this.divAddDepartment.Attributes.Add("onclick", Modal.GovPublicCategoryDepartmentSelect.GetOpenWindowString(base.PublishmentSystemID));
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

                foreach (int applyID in this.idArrayList)
                {
                    EGovPublicApplyState state = DataProvider.GovPublicApplyDAO.GetState(applyID);
                    if (state != EGovPublicApplyState.Denied && state != EGovPublicApplyState.Checked)
                    {
                        DataProvider.GovPublicApplyDAO.UpdateDepartmentID(applyID, switchToDepartmentID);

                        GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, applyID, EGovPublicApplyRemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                        GovPublicApplyManager.LogSwitchTo(base.PublishmentSystemID, applyID, switchToDepartmentName);
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
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('申请转办成功!');"));
			}
		}

	}
}
