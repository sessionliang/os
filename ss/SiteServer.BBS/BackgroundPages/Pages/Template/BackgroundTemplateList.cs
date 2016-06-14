using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundTemplateList : BackgroundBasePage
    {
        public DataGrid dgContents;

        private string templateDir = string.Empty;
        private string directoryName = string.Empty;

        public static string GetRedirectUrl(int publishmentSystemID, string templateDir, string directoryName)
        {
            return PageUtils.GetBBSUrl(string.Format("background_templateList.aspx?publishmentSystemID={0}&templateDir={1}&directoryName={2}", publishmentSystemID, templateDir, directoryName));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.templateDir = base.GetQueryString("templateDir");
            this.directoryName = base.GetQueryString("directoryName");

            if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "模板管理", AppManager.BBS.Permission.BBS_Template);

                if (base.GetQueryString("Delete") != null)
                {
                    string name = base.GetQueryString("Name");

                    try
                    {
                        
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }
            }
            BindGrid();
        }

        public void BindGrid()
        {
            try
            {
                string directoryPath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(base.PublishmentSystemID, this.templateDir), this.directoryName);
                ArrayList nameList = new ArrayList();
                nameList.AddRange(DirectoryUtils.GetDirectoryPaths(directoryPath));
                nameList.AddRange(DirectoryUtils.GetFilePaths(directoryPath));
                this.dgContents.DataSource = nameList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string path = e.Item.DataItem as string;
                bool isDirectory = PathUtils.IsDirectoryPath(path);

                Literal ltlName = (Literal)e.Item.FindControl("ltlName");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");

                if (isDirectory)
                {
                    string dir = PathUtils.GetDirectoryName(path);
                    ltlName.Text = string.Format(@"<a href=""{0}"">{1}&nbsp;<span style=""font-size:8pt;font-family:arial;color:#f26c4f"">(文件夹)</span></a>", BackgroundTemplateList.GetRedirectUrl(base.PublishmentSystemID, this.templateDir, PageUtils.Combine(this.directoryName, dir)), dir);
                }
                else
                {
                    string fileName = PathUtils.GetFileName(path);
                    ltlName.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundTemplateEdit.GetRedirectUrl(base.PublishmentSystemID, this.templateDir, this.directoryName, fileName), fileName);

                    ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundTemplateEdit.GetRedirectUrl(base.PublishmentSystemID, this.templateDir, this.directoryName, fileName));
                }
            }
        }

    }
}