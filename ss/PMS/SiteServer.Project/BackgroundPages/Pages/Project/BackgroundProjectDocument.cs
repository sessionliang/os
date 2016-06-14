using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundProjectDocument : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        private int projectID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["DocumentID"] != null)
            {
                int documentID = TranslateUtils.ToInt(base.Request.QueryString["DocumentID"]);
                try
                {
                    DataProvider.ProjectDocumentDAO.Delete(documentID);
                    base.SuccessMessage("成功删除项目文档");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除项目文档失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                this.dgContents.DataSource = DataProvider.ProjectDocumentDAO.GetDataSource(this.projectID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.ProjectDocumentAdd.GetShowPopWinStringToAdd(this.projectID));
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int documentID = TranslateUtils.EvalInt(e.Item.DataItem, "DocumentID");
                int projectID = TranslateUtils.EvalInt(e.Item.DataItem, "ProjectID");
                string fileName = TranslateUtils.EvalString(e.Item.DataItem, "FileName");
                string description = TranslateUtils.EvalString(e.Item.DataItem, "Description");
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");

                Literal ltlFileName = e.Item.FindControl("ltlFileName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlFileName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.Combine(ProjectManager.GetDirectoryUrl(this.projectID), fileName), fileName);
                ltlDescription.Text = description;
                ltlUserName.Text = AdminManager.GetDisplayName(userName, false);
                ltlAddDate.Text = DateUtils.GetDateString(addDate);
                ltlDeleteUrl.Text = string.Format(@"<a href=""background_projectDocument.aspx?Delete=True&DocumentID={0}"" onClick=""javascript:return confirm('此操作将删除项目文档“{1}”，确认吗？');"">删除</a>", documentID, fileName);
            }
        }
    }
}
