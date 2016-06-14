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
	public class GovInteractApplyComment : BackgroundBasePage
	{
        protected TextBox tbCommentRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("批示", "modal_govInteractApplyComment.aspx", arguments, "IDCollection", "请选择需要批示的申请！", 450, 320);
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
                if (string.IsNullOrEmpty(this.tbCommentRemark.Text))
                {
                    base.FailMessage("批示失败，必须填写意见");
                    return;
                }

                foreach (int contentID in this.idArrayList)
                {
                    int nodeID = DataProvider.GovInteractContentDAO.GetNodeID(base.PublishmentSystemInfo, contentID);
                    GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, nodeID, contentID, EGovInteractRemarkType.Comment, this.tbCommentRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                    DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                    GovInteractApplyManager.Log(base.PublishmentSystemID, nodeID, contentID, EGovInteractLogType.Comment);
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
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('批示成功!');"));
			}
		}

	}
}
