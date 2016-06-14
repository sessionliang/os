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
	public class ApplyTranslate : BackgroundBasePage
	{
        protected DropDownList ddlProjectID;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("转移办件", "modal_applyTranslate.aspx", arguments, "IDCollection", "请选择需要转移的办件！", 500, 300);
        }

        public static string GetShowPopWinString(int projectID, int applyID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("IDCollection", applyID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("转移办件", "modal_applyTranslate.aspx", arguments, 500, 300);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                ArrayList projectInfoArrayList = DataProvider.ProjectDAO.GetProjectInfoArrayList(false);
                foreach (ProjectInfo projectInfo in projectInfoArrayList)
                {
                    if (projectInfo.ProjectID != this.ProjectID)
                    {
                        this.ddlProjectID.Items.Add(new ListItem(projectInfo.ProjectName, projectInfo.ProjectID.ToString()));
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                int projectID = TranslateUtils.ToInt(this.ddlProjectID.SelectedValue);
                if (projectID > 0)
                {
                    foreach (int applyID in this.idArrayList)
                    {
                        ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(applyID);
                        applyInfo.ProjectID = projectID;
                        DataProvider.ApplyDAO.Update(applyInfo);
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
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('办件转移成功!');"));
			}
		}

	}
}
