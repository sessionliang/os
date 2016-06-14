using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using BaiRong.Model;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleSiteTemplate : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button Import;

        private SortedList sortedlist = new SortedList();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                string siteTemplateDir = base.GetQueryString("SiteTemplateDir");

                try
                {
                    SiteTemplateManager.Instance.DeleteSiteTemplate(siteTemplateDir);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除应用模板", string.Format("应用模板:{0}", siteTemplateDir));

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (!Page.IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "应用模板管理", AppManager.Platform.Permission.Platform_Site);

                this.sortedlist = SiteTemplateManager.Instance.GetAllSiteTemplateSortedList();
                BindGrid();

                this.Import.Attributes.Add("onclick", UploadSiteTemplate.GetOpenWindowString());
            }
        }

        public void BindGrid()
        {
            try
            {
                ArrayList directoryArrayList = new ArrayList();
                foreach (string directoryName in sortedlist.Keys)
                {
                    string directoryPath = PathUtility.GetSiteTemplatesPath(directoryName);
                    DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                    directoryArrayList.Add(dirInfo);
                }

                this.dgContents.DataSource = directoryArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DirectoryInfo dirInfo = (DirectoryInfo)e.Item.DataItem;

                Literal ltlTemplateName = e.Item.FindControl("ltlTemplateName") as Literal;
                Literal ltlTemplateType = e.Item.FindControl("ltlTemplateType") as Literal;
                Literal ltlDirectoryName = e.Item.FindControl("ltlDirectoryName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlCreationDate = e.Item.FindControl("ltlCreationDate") as Literal;
                Literal ltlDownloadUrl = e.Item.FindControl("ltlDownloadUrl") as Literal;
                Literal ltlCreateUrl = e.Item.FindControl("ltlCreateUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                SiteTemplateInfo siteTemplateInfo = this.sortedlist[dirInfo.Name] as SiteTemplateInfo;
                if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.SiteTemplateName))
                {
                    ltlTemplateName.Text = siteTemplateInfo.SiteTemplateName;
                    EPublishmentSystemType publishmentSystemType = EPublishmentSystemTypeUtils.GetEnumType(siteTemplateInfo.PublishmentSystemType);
                    ltlTemplateType.Text = EPublishmentSystemTypeUtils.GetHtml(publishmentSystemType, false);
                    ltlDirectoryName.Text = dirInfo.Name;
                    ltlDescription.Text = siteTemplateInfo.Description;
                    if (!string.IsNullOrEmpty(siteTemplateInfo.PicFileName))
                    {
                        string siteTemplateUrl = PageUtility.GetSiteTemplatesUrl(dirInfo.Name);
                        ltlDownloadUrl.Text += string.Format("<a href=\"{0}\" target=_blank>样图</a>&nbsp;&nbsp;", PageUtility.GetSiteTemplateMetadataUrl(siteTemplateUrl, siteTemplateInfo.PicFileName));
                    }
                    ltlCreationDate.Text = DateUtils.GetDateString(dirInfo.CreationTime);
                    if (!string.IsNullOrEmpty(siteTemplateInfo.WebSiteUrl))
                    {
                        ltlDownloadUrl.Text += string.Format("<a href=\"{0}\" target=_blank>演示</a>&nbsp;&nbsp;", PageUtils.ParseConfigRootUrl(siteTemplateInfo.WebSiteUrl));
                    }

                    string fileName = dirInfo.Name + ".zip";
                    string filePath = PathUtility.GetSiteTemplatesPath(fileName);
                    if (FileUtils.IsFileExists(filePath))
                    {
                        ltlDownloadUrl.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}"">重新压缩</a>&nbsp;&nbsp;", ProgressBar.GetOpenWindowStringWithSiteTemplateZip(dirInfo.Name));

                        ltlDownloadUrl.Text += string.Format(@"<a href=""{0}"" target=""_blank"">下载压缩包</a>", PageUtility.GetSiteTemplatesUrl(fileName));
                    }
                    else
                    {
                        ltlDownloadUrl.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}"">压缩</a>", ProgressBar.GetOpenWindowStringWithSiteTemplateZip(dirInfo.Name));
                    }

                    string urlAdd = PageUtils.GetSTLUrl(string.Format("console_publishmentSystemAdd.aspx?siteTemplate={0}", dirInfo.Name));
                    ltlCreateUrl.Text = string.Format(@"<a href=""{0}"">创建应用</a>", urlAdd);

                    ltlDeleteUrl.Text = string.Format(@"<a href=""console_siteTemplate.aspx?Delete=True&SiteTemplateDir={0}"" onClick=""javascript:return confirm('此操作将会删除此应用模板“{1}”，确认吗？');"">删除</a>", dirInfo.Name, siteTemplateInfo.SiteTemplateName);
                }
            }
        }

    }
}
