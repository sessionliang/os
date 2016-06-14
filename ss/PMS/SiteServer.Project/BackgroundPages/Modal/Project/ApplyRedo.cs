using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.Project.Model;
using System.Text;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ApplyRedo : BackgroundBasePage
	{
        protected TextBox tbRedoRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("要求返工", "modal_applyRedo.aspx", arguments, "IDCollection", "请选择需要返工的办件！", 500, 400);
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
                if (string.IsNullOrEmpty(this.tbRedoRemark.Text))
                {
                    base.FailMessage("要求返工失败，必须填写意见");
                    return;
                }

                foreach (int applyID in this.idArrayList)
                {
                    EApplyState state = DataProvider.ApplyDAO.GetState(applyID);
                    if (state == EApplyState.Replied || state == EApplyState.Redo)
                    {
                        RemarkInfo remarkInfo = new RemarkInfo(0, applyID, ERemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.RemarkDAO.Insert(remarkInfo);

                        ApplyManager.Log(applyID, EProjectLogType.Redo);
                        DataProvider.ApplyDAO.UpdateState(applyID, EApplyState.Redo);
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
