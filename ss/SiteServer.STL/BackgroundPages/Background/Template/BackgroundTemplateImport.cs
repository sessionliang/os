using System;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundTemplateImport : BackgroundBasePage
	{
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public Literal ltlPageTitle;
		public Repeater rptContents;

        private ETemplateType templateType;
		SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl(int publishmentSystemID, ETemplateType templateType)
        {
            return PageUtils.GetSTLUrl(string.Format("background_templateImport.aspx?publishmentSystemID={0}&templateType={1}", publishmentSystemID, ETemplateTypeUtils.GetValue(templateType)));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.templateType = ETemplateTypeUtils.GetEnumType(base.GetQueryString("templateType"));

			if (!IsPostBack)
            {
                string pageTitle = string.Empty;
                if (this.templateType == ETemplateType.IndexPageTemplate)
                {
                    pageTitle = "更换首页模板";
                }
                else if (this.templateType == ETemplateType.ChannelTemplate)
                {
                    pageTitle = "更换栏目模板";
                }
                else if (this.templateType == ETemplateType.ContentTemplate)
                {
                    pageTitle = "更换内容模板";
                }

                if (base.GetQueryString("isReplace") != null)
                {
                    string directoryName = base.GetQueryString("directoryName");

                    string userKeyPrefix = StringUtils.GUID();

                    PageUtils.Redirect(BackgroundProgressBar.GetIndependentTemplateReplaceUrl(base.PublishmentSystemID, directoryName, userKeyPrefix));
                }
                else
                {
                    this.sortedlist = IndependentTemplateManager.Instance.GetIndependentTemplateSortedList(base.PublishmentSystemInfo.PublishmentSystemType, this.templateType);

                    this.ltlPageTitle.Text = pageTitle;
                    base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, pageTitle, AppManager.Platform.Permission.Platform_Site);

                    BindGrid();
                }
			}
		}

		public void BindGrid()
		{
			try
			{
				ArrayList directoryArrayList = new ArrayList();
				foreach (string directoryName in this.sortedlist.Keys)
				{
                    string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
					DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
					directoryArrayList.Add(dirInfo);
				}

                this.rptContents.DataSource = directoryArrayList;
                this.rptContents.ItemDataBound += rptContents_ItemDataBound;
                this.rptContents.DataBind();
			}
			catch (Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DirectoryInfo dirInfo = (DirectoryInfo)e.Item.DataItem;

                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;

                IndependentTemplateInfo independentTemplateInfo = this.sortedlist[dirInfo.Name] as IndependentTemplateInfo;
                if (independentTemplateInfo != null && !string.IsNullOrEmpty(independentTemplateInfo.IndependentTemplateName))
                {
                    string templateSN = dirInfo.Name.ToUpper().Substring(3);
                    if (!string.IsNullOrEmpty(independentTemplateInfo.WebSiteUrl))
                    {
                        templateSN = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.ParseConfigRootUrl(independentTemplateInfo.WebSiteUrl), templateSN);
                    }

                    ltlTitle.Text = templateSN;

                    if (!string.IsNullOrEmpty(independentTemplateInfo.PicFileNames))
                    {
                        StringBuilder itemBuilder = new StringBuilder();

                        int i = 0;
                        foreach (string picFileName in TranslateUtils.StringCollectionToStringList(independentTemplateInfo.PicFileNames))
                        {
                            string siteTemplateUrl = PageUtility.GetIndependentTemplatesUrl(dirInfo.Name);
                            if (i == 0)
                            {
                                itemBuilder.AppendFormat(@"<div class=""active item""><img src=""{0}"" data-pinit=""registered""></div>", PageUtility.GetIndependentTemplateMetadataUrl(siteTemplateUrl, picFileName));
                            }
                            else
                            {
                                itemBuilder.AppendFormat(@"<div class=""item""><img src=""{0}"" data-pinit=""registered""></div>", PageUtility.GetIndependentTemplateMetadataUrl(siteTemplateUrl, picFileName));
                            }
                            i++;
                        }

                        ltlImageUrl.Text = string.Format(@"
<div id=""myCarousel_{0}"" class=""carousel slide"">
  <div class=""carousel-inner"">{1}</div>
  <a class=""carousel-control left"" href=""#myCarousel_{0}"" data-slide=""prev"">&lsaquo;</a>
  <a class=""carousel-control right"" href=""#myCarousel_{0}"" data-slide=""next"">&rsaquo;</a>
</div>", e.Item.ItemIndex, itemBuilder.ToString());
                    }

                    ltlSelect.Text = string.Format(@"<a href=""{0}&isReplace=True&directoryName={1}"" class=""btn btn-success"" onclick=""javascript:return confirm('此操作将替换当前模板为所选模板，确认吗？');"">替换为本模板</a>", BackgroundTemplateImport.GetRedirectUrl(base.PublishmentSystemID, this.templateType), dirInfo.Name);
                }
            }
        }
	}
}
