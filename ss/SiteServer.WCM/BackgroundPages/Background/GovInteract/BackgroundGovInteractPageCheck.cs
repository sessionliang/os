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
	public class BackgroundGovInteractPageCheck : BackgroundGovInteractPageBasePage
	{
        public TextBox tbRedoRemark;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, "待审核办件", AppManager.CMS.Permission.WebSite.GovInteract);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govInteractPageCheck.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(listPageUrl)));
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
                GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, base.contentInfo.NodeID, base.contentInfo.ID, EGovInteractRemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, base.contentInfo.NodeID, base.contentInfo.ID, EGovInteractLogType.Redo);
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, base.contentInfo.ID, EGovInteractState.Redo);

                base.SuccessMessage("要求返工成功");

                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
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
                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Check);
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, this.contentInfo.ID, EGovInteractState.Checked);
                base.SuccessMessage("审核申请成功");

                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
	}
}
