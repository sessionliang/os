using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;


namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ProjectDocumentAdd : BackgroundBasePage
	{
        public HtmlInputFile myFile;
        public TextBox tbDescription;

        private int projectID;

        public static string GetShowPopWinStringToAdd(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加项目文档", "modal_projectDocumentAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);

			if (!IsPostBack)
			{
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
            {
                string fileName = myFile.PostedFile.FileName;
                try
                {
                    string filePath = PathUtils.Combine(ProjectManager.GetDirectoryPath(this.projectID), fileName);
                    DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                    myFile.PostedFile.SaveAs(filePath);

                    ProjectDocumentInfo documentInfo = new ProjectDocumentInfo(0, this.projectID, fileName, this.tbDescription.Text, AdminManager.Current.UserName, DateTime.Now);
                    DataProvider.ProjectDocumentDAO.Insert(documentInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "文件上传失败！");
                    return;
                }
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_projectDocument.aspx?ProjectID={0}", this.projectID));
			}
		}
	}
}
