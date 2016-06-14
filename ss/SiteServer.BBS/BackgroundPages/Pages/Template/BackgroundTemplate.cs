using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

using System.IO;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundTemplate : BackgroundBasePage
    {
        public DataGrid dgContents;

        private SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_template.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                string templateDir = base.GetQueryString("TemplateDir");

                try
                {
                    TemplateManager.GetInstance(base.PublishmentSystemID).DeleteTemplate(templateDir);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除论坛模板", string.Format("论坛模板:{0}", templateDir));

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, ex.Message);
                }
            }
            else if (base.GetQueryString("Default") != null)
            {
                string templateDir = base.GetQueryString("TemplateDir");

                try
                {
                    base.Additional.TemplateDir = templateDir;

                    ConfigurationManager.Update(base.PublishmentSystemID);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置论坛当前使用模板", string.Format("论坛模板:{0}", templateDir));

                    base.SuccessMessage("成功设置论坛当前使用模板");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, ex.Message);
                }
            }

            if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "模板管理", AppManager.BBS.Permission.BBS_Template);

                this.sortedlist = TemplateManager.GetInstance(base.PublishmentSystemID).GetAllTemplateSortedList();
                BindGrid();
            }
        }

        public void BindGrid()
        {
            try
            {
                ArrayList directoryArrayList = new ArrayList();
                foreach (string directoryName in sortedlist.Keys)
                {
                    string directoryPath = PathUtilityBBS.GetTemplateDirectoryPath(base.PublishmentSystemID, directoryName);
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

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DirectoryInfo dirInfo = e.Item.DataItem as DirectoryInfo;
                string templateDir = dirInfo.Name;

                Literal ltlTemplateName = (Literal)e.Item.FindControl("ltlTemplateName");
                Literal ltlDescription = (Literal)e.Item.FindControl("ltlDescription");
                Literal ltlSamplePic = (Literal)e.Item.FindControl("ltlSamplePic");
                Literal ltlIsDefault = (Literal)e.Item.FindControl("ltlIsDefault");
                Literal ltlDefaultUrl = (Literal)e.Item.FindControl("ltlDefaultUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlCreateUrl = (Literal)e.Item.FindControl("ltlCreateUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                TemplateInfo templateInfo = this.sortedlist[templateDir] as TemplateInfo;
                if (templateInfo != null && !string.IsNullOrEmpty(templateInfo.TemplateName))
                {
                    if (!string.IsNullOrEmpty(templateInfo.WebSiteUrl))
                    {
                        ltlTemplateName.Text = string.Format("<a href=\"{0}\" target=_blank>{1}</a>", PageUtils.ParseConfigRootUrl(templateInfo.WebSiteUrl), templateInfo.TemplateName);
                    }
                    else
                    {
                        ltlTemplateName.Text = templateInfo.TemplateName;
                    }

                    ltlDescription.Text = templateInfo.Description;

                    if (!string.IsNullOrEmpty(templateInfo.PicFileName))
                    {
                        string templateUrl = PageUtilityBBS.GetTemplatesUrl(base.PublishmentSystemID, templateDir);
                        ltlSamplePic.Text = string.Format("<a href=\"{0}\" target=_blank><img border=0 src=\"{0}\" /></a>", PageUtilityBBS.GetTemplateUrl(templateUrl, templateInfo.PicFileName));
                    }

                    if (StringUtils.EqualsIgnoreCase(templateDir, base.Additional.TemplateDir))
                    {
                        ltlIsDefault.Text = StringUtilityBBS.GetTrueImageHtml(true);
                        ltlEditUrl.Text = string.Format(@"<a href=""{0}"">管理</a>", BackgroundTemplateList.GetRedirectUrl(base.PublishmentSystemID, templateDir, string.Empty));
                        ltlCreateUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">生成</a>", Modal.ProgressBar.GetOpenWindowStringWithCreateAll(base.PublishmentSystemID));
                    }
                    else
                    {
                        string backgroundUrl = BackgroundTemplate.GetRedirectUrl(base.PublishmentSystemID);

                        ltlDefaultUrl.Text = string.Format(@"<a href=""{0}&Default=True&TemplateDir={1}"" onClick=""javascript:return confirm('此操作将会设置模板“{1}”为当前使用模板，确认吗？');"">使用此模板</a>", backgroundUrl, templateDir);
                        ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&Delete=True&TemplateDir={1}"" onClick=""javascript:return confirm('此操作将会删除此模板“{1}”，确认吗？');"">删除</a>", backgroundUrl, templateDir);
                    }
                }
            }
        }

    }
}