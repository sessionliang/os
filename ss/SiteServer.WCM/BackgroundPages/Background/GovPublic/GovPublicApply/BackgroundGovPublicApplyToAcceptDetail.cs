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
    public class BackgroundGovPublicApplyToAcceptDetail : BackgroundGovPublicApplyToDetailBasePage
	{
        public TextBox tbAcceptRemark;
        public TextBox tbDenyReply;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApply, "待受理申请", AppManager.CMS.Permission.WebSite.GovPublicApply);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int applyID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govPublicApplyToAcceptDetail.aspx?PublishmentSystemID={0}&ApplyID={1}&ReturnUrl={2}", publishmentSystemID, applyID, StringUtils.ValueToUrl(listPageUrl)));
        }

        public void Accept_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, base.applyInfo.ID, EGovPublicApplyRemarkType.Accept, this.tbAcceptRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Accept);
                DataProvider.GovPublicApplyDAO.UpdateState(this.applyInfo.ID, EGovPublicApplyState.Accepted);
                base.SuccessMessage("申请受理成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Deny_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbDenyReply.Text))
            {
                base.FailMessage("拒绝失败，必须填写拒绝理由");
                return;
            }
            try
            {
                DataProvider.GovPublicApplyReplyDAO.DeleteByApplyID(this.applyInfo.ID);
                GovPublicApplyReplyInfo replyInfo = new GovPublicApplyReplyInfo(0, base.PublishmentSystemID, this.applyInfo.ID, this.tbDenyReply.Text, string.Empty, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyReplyDAO.Insert(replyInfo);

                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Deny);
                DataProvider.GovPublicApplyDAO.UpdateState(this.applyInfo.ID, EGovPublicApplyState.Denied);

                base.SuccessMessage("拒绝申请成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
	}
}
