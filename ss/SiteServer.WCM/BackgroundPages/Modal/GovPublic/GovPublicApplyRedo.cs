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
	public class GovPublicApplyRedo : BackgroundBasePage
	{
        protected TextBox tbRedoRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("要求返工", "modal_govPublicApplyRedo.aspx", arguments, "IDCollection", "请选择需要返工的申请！", 450, 320);
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

                foreach (int applyID in this.idArrayList)
                {
                    EGovPublicApplyState state = DataProvider.GovPublicApplyDAO.GetState(applyID);
                    if (state == EGovPublicApplyState.Replied || state == EGovPublicApplyState.Redo)
                    {
                        GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, applyID, EGovPublicApplyRemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                        GovPublicApplyManager.Log(base.PublishmentSystemID, applyID, EGovPublicApplyLogType.Redo);
                        DataProvider.GovPublicApplyDAO.UpdateState(applyID, EGovPublicApplyState.Redo);
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
