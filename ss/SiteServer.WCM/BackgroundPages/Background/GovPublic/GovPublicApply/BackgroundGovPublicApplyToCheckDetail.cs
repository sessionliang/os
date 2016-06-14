using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;

using System.Web.UI.HtmlControls;

using System.Text;

namespace SiteServer.WCM.BackgroundPages
{
	public class BackgroundGovPublicApplyToCheckDetail : BackgroundGovPublicApplyToDetailBasePage
	{
        public TextBox tbRedoRemark;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApply, "待审核申请", AppManager.CMS.Permission.WebSite.GovPublicApply);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int applyID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govPublicApplyToCheckDetail.aspx?PublishmentSystemID={0}&ApplyID={1}&ReturnUrl={2}", publishmentSystemID, applyID, StringUtils.ValueToUrl(listPageUrl)));
        }

        public void Redo_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbRedoRemark.Text))
            {
                base.FailMessage("要求返工失败，必须填写意见");
                return;
            }
            try
            {
                GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyRemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Redo);
                DataProvider.GovPublicApplyDAO.UpdateState(this.applyInfo.ID, EGovPublicApplyState.Redo);

                base.SuccessMessage("要求返工成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Check_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Check);
                DataProvider.GovPublicApplyDAO.UpdateState(this.applyInfo.ID, EGovPublicApplyState.Checked);
                base.SuccessMessage("审核申请成功");
                base.AddWaitAndRedirectScript(this.ListPageUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
	}
}
