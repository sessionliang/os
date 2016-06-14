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
    public class ConsoleIndependentTemplate : BackgroundBasePage
	{
		public DataGrid dgContents;
        public Button Import;

		private SortedList sortedlist = new SortedList();

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                string independentTemplateDir = base.GetQueryString("IndependentTemplateDir");
			
				try
				{
					IndependentTemplateManager.Instance.DeleteIndependentTemplate(independentTemplateDir);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除独立模板", string.Format("独立模板:{0}", independentTemplateDir));

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}
			
			if (!Page.IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "独立模板管理", AppManager.Platform.Permission.Platform_Site);

                this.sortedlist = IndependentTemplateManager.Instance.GetAllIndependentTemplateSortedList();
				BindGrid();

                this.Import.Attributes.Add("onclick", UploadIndependentTemplate.GetOpenWindowString());
			}
		}

		public void BindGrid()
		{
			try
			{
				ArrayList directoryArrayList = new ArrayList();
				foreach (string directoryName in sortedlist.Keys)
				{
                    string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
					DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
					directoryArrayList.Add(dirInfo);
				}

                this.dgContents.DataSource = directoryArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
			catch(Exception ex)
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
                Literal ltlPublishmentSystemType = e.Item.FindControl("ltlPublishmentSystemType") as Literal;
                Literal ltlTemplateTypes = e.Item.FindControl("ltlTemplateTypes") as Literal;
                Literal ltlDirectoryName = e.Item.FindControl("ltlDirectoryName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlCreationDate = e.Item.FindControl("ltlCreationDate") as Literal;
                Literal ltlDownloadUrl = e.Item.FindControl("ltlDownloadUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                IndependentTemplateInfo independentTemplateInfo = this.sortedlist[dirInfo.Name] as IndependentTemplateInfo;
                if (independentTemplateInfo != null && !string.IsNullOrEmpty(independentTemplateInfo.IndependentTemplateName))
                {
                    ltlTemplateName.Text = independentTemplateInfo.IndependentTemplateName;
                    ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(EPublishmentSystemTypeUtils.GetEnumType(independentTemplateInfo.PublishmentSystemType), false);
                    foreach (string templateType in TranslateUtils.StringCollectionToStringList(independentTemplateInfo.TemplateTypes))
                    {
                        ltlTemplateTypes.Text += string.Format("{0}&nbsp;", ETemplateTypeUtils.GetText(ETemplateTypeUtils.GetEnumType(templateType)));
                    }
                    ltlDirectoryName.Text = dirInfo.Name;
                    ltlDescription.Text = independentTemplateInfo.Description;
                    if (!string.IsNullOrEmpty(independentTemplateInfo.PicFileNames))
                    {
                        string independentTemplateUrl = PageUtility.GetIndependentTemplatesUrl(dirInfo.Name);
                        foreach (string picFileName in TranslateUtils.StringCollectionToStringList(independentTemplateInfo.PicFileNames))
                        {
                            ltlDownloadUrl.Text += string.Format("<a href=\"{0}\" target=_blank>样图</a>&nbsp;&nbsp;", PageUtility.GetIndependentTemplateMetadataUrl(independentTemplateUrl, picFileName));
                        }
                    }
                    ltlCreationDate.Text = DateUtils.GetDateString(dirInfo.CreationTime);
                    if (!string.IsNullOrEmpty(independentTemplateInfo.WebSiteUrl))
                    {
                        ltlDownloadUrl.Text += string.Format("<a href=\"{0}\" target=_blank>演示</a>&nbsp;&nbsp;", PageUtils.ParseConfigRootUrl(independentTemplateInfo.WebSiteUrl));
                    }

                    string fileName = dirInfo.Name + ".zip";
                    string filePath = PathUtility.GetIndependentTemplatesPath(fileName);
                    if (FileUtils.IsFileExists(filePath))
                    {
                        ltlDownloadUrl.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}"">重新压缩</a>&nbsp;&nbsp;", ProgressBar.GetOpenWindowStringWithIndependentTemplateZip(dirInfo.Name));

                        ltlDownloadUrl.Text += string.Format(@"<a href=""{0}"" target=""_blank"">下载压缩包</a>", PageUtility.GetIndependentTemplatesUrl(fileName));
                    }
                    else
                    {
                        ltlDownloadUrl.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}"">压缩</a>", ProgressBar.GetOpenWindowStringWithIndependentTemplateZip(dirInfo.Name));
                    }

                    ltlDeleteUrl.Text = string.Format(@"<a href=""console_independentTemplate.aspx?Delete=True&IndependentTemplateDir={0}"" onClick=""javascript:return confirm('此操作将会删除此独立模板“{1}”，确认吗？');"">删除</a>", dirInfo.Name, dirInfo.Name);
                }
            }
        }

	}
}
