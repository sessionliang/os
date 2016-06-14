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
	public class GovInteractApplyRedo : BackgroundBasePage
	{
        protected TextBox tbRedoRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("要求返工", "modal_govInteractApplyRedo.aspx", arguments, "IDCollection", "请选择需要返工的申请！", 450, 320);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                if (string.IsNullOrEmpty(this.tbRedoRemark.Text))
                {
                    base.FailMessage("要求返工失败，必须填写意见");
                    return;
                }

                foreach (int contentID in this.idArrayList)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    if (contentInfo.State == EGovInteractState.Replied || contentInfo.State == EGovInteractState.Redo)
                    {
                        GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, EGovInteractRemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                        GovInteractApplyManager.Log(base.PublishmentSystemID, contentInfo.NodeID, contentID, EGovInteractLogType.Redo);
                        DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, contentID, EGovInteractState.Redo);
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
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('要求返工成功!');"));
			}
		}

	}
}
