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
    public class BackgroundGovInteractPageAccept : BackgroundGovInteractPageBasePage
	{
        public TextBox tbAcceptRemark;
        public TextBox tbDenyReply;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, "待受理办件", AppManager.CMS.Permission.WebSite.GovInteract);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govInteractPageAccept.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(listPageUrl)));
        }

        public void Accept_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, base.contentInfo.NodeID, base.contentInfo.ID, EGovInteractRemarkType.Accept, this.tbAcceptRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Accept);
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, this.contentInfo.ID, EGovInteractState.Accepted);
                base.SuccessMessage("申请受理成功");

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

        public void Deny_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbDenyReply.Text))
            {
                base.FailMessage("拒绝失败，必须填写拒绝理由");
                return;
            }
            try
            {
                DataProvider.GovInteractReplyDAO.DeleteByContentID(base.PublishmentSystemID, this.contentInfo.ID);
                GovInteractReplyInfo replyInfo = new GovInteractReplyInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, this.tbDenyReply.Text, string.Empty, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractReplyDAO.Insert(replyInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Deny);
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, this.contentInfo.ID, EGovInteractState.Denied);

                base.SuccessMessage("拒绝申请成功");

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
