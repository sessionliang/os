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
	public class ApplyComment : BackgroundBasePage
	{
        protected TextBox tbCommentRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("批注", "modal_applyComment.aspx", arguments, "IDCollection", "请选择需要批注的办件！", 500, 400);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
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
                    base.FailMessage("批注失败，必须填写意见");
                    return;
                }

                foreach (int applyID in this.idArrayList)
                {
                    RemarkInfo remarkInfo = new RemarkInfo(0, applyID, ERemarkType.Comment, this.tbCommentRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                    DataProvider.RemarkDAO.Insert(remarkInfo);

                    ApplyManager.Log(applyID, EProjectLogType.Comment);
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
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('批注成功!');"));
			}
		}

	}
}
