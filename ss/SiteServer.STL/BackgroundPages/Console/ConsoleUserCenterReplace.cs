using System;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleUserCenterReplace : BackgroundBasePage
    {
        public Literal PublishmentSystemName;
        public PlaceHolder ChooseSiteTemplate;
        public DataGrid dgContents;
        public HtmlInputHidden SiteTemplateDir;

        public PlaceHolder CreateSiteParameters;
        public Literal ltlSiteTemplateName;
        public RadioButtonList IsDeleteChannels;
        public RadioButtonList IsDeleteTemplates;
        public RadioButtonList IsDeleteFiles;
        public RadioButtonList IsOverride;

        public PlaceHolder OperatingError;
        public Literal ltlErrorMessage;

        public Button Previous;
        public Button Next;

        SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return string.Format("/siteserver/loading.aspx?RedirectType=Loading&RedirectUrl=stl/console_userCenterReplace.aspx?menuID=UserCenter&PublishmentSystemID={0}", publishmentSystemID);
        }

        public string GetSiteTemplateName(string siteTemplateDir)
        {
            string retval = string.Empty;
            SiteTemplateInfo siteTemplateInfo = this.sortedlist[siteTemplateDir] as SiteTemplateInfo;
            if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.SiteTemplateName))
            {
                if (!string.IsNullOrEmpty(siteTemplateInfo.WebSiteUrl))
                {

                    retval = string.Format("<a href=\"{0}\" target=_blank>{1}</a>", PageUtils.ParseConfigRootUrl(siteTemplateInfo.WebSiteUrl), siteTemplateInfo.SiteTemplateName);
                }
                else
                {
                    retval = siteTemplateInfo.SiteTemplateName;
                }
            }
            return retval;
        }

        public string GetDescription(string siteTemplateDir)
        {
            string retval = string.Empty;
            SiteTemplateInfo siteTemplateInfo = this.sortedlist[siteTemplateDir] as SiteTemplateInfo;
            if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.Description))
            {
                retval = siteTemplateInfo.Description;
            }
            return retval;
        }

        public string GetSamplePicHtml(string siteTemplateDir)
        {
            string retval = string.Empty;
            SiteTemplateInfo siteTemplateInfo = this.sortedlist[siteTemplateDir] as SiteTemplateInfo;
            if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.PicFileName))
            {
                string siteTemplateUrl = PageUtility.GetSiteTemplatesUrl(siteTemplateDir);
                retval = string.Format("<a href=\"{0}\" target=_blank><img height=120 width=100 border=0 src=\"{0}\" /></a>", PageUtility.GetSiteTemplateMetadataUrl(siteTemplateUrl, siteTemplateInfo.PicFileName));
            }
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.sortedlist = SiteTemplateManager.Instance.GetSiteTemplateSortedList(BaiRong.Model.EPublishmentSystemType.UserCenter);
            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, " 更换模板", AppManager.Platform.Permission.Platform_Site);

                this.PublishmentSystemName.Text = base.PublishmentSystemInfo.PublishmentSystemName;

                BindGrid();

                if (SiteTemplateManager.Instance.GetSiteTemplateCount() > 0)
                {
                    SetActivePlaceHolder(WizardPlaceHolder.ChooseSiteTemplate, ChooseSiteTemplate);
                }
                else
                {
                    PageUtils.RedirectToErrorPage("无用户中心模板！");
                }
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
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public WizardPlaceHolder CurrentWizardPlaceHolder
        {
            get
            {
                if (ViewState["WizardPlaceHolder"] != null)
                    return (WizardPlaceHolder)ViewState["WizardPlaceHolder"];

                if (SiteTemplateManager.Instance.GetSiteTemplateCount() > 0)
                {
                    return WizardPlaceHolder.ChooseSiteTemplate;
                }
                else
                {
                    return WizardPlaceHolder.CreateSiteParameters;
                }

            }
            set
            {
                ViewState["WizardPlaceHolder"] = value;
            }
        }


        public enum WizardPlaceHolder
        {
            ChooseSiteTemplate,
            CreateSiteParameters,
            OperatingError,
        }

        void SetActivePlaceHolder(WizardPlaceHolder panel, Control controlToShow)
        {
            PlaceHolder currentPlaceHolder = FindControl(CurrentWizardPlaceHolder.ToString()) as PlaceHolder;
            if (currentPlaceHolder != null)
                currentPlaceHolder.Visible = false;

            switch (panel)
            {
                case WizardPlaceHolder.ChooseSiteTemplate:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPlaceHolder.CreateSiteParameters:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPlaceHolder.OperatingError:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
                    break;
                default:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
            }

            controlToShow.Visible = true;
            CurrentWizardPlaceHolder = panel;
        }

        public void NextPlaceHolder(Object sender, EventArgs e)
        {
            switch (CurrentWizardPlaceHolder)
            {
                case WizardPlaceHolder.ChooseSiteTemplate:
                    if (string.IsNullOrEmpty(this.SiteTemplateDir.Value))
                    {
                        base.FailMessage("必须选择一个站的模板进行操作");
                        return;
                    }
                    this.ltlSiteTemplateName.Text = string.Format("{0}（{1}）", this.GetSiteTemplateName(this.SiteTemplateDir.Value), this.SiteTemplateDir.Value);
                    SetActivePlaceHolder(WizardPlaceHolder.CreateSiteParameters, CreateSiteParameters);
                    break;

                case WizardPlaceHolder.CreateSiteParameters:
                    string userKeyPrefix = StringUtils.GUID();
                    string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Value);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "用户中心模板替换", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));

                    PageUtils.Redirect(BackgroundProgressBar.GetRecoveryUrl(base.PublishmentSystemID, this.IsDeleteChannels.SelectedValue, this.IsDeleteTemplates.SelectedValue, this.IsDeleteFiles.SelectedValue, false, siteTemplatePath, this.IsOverride.SelectedValue, this.IsOverride.SelectedValue, userKeyPrefix));
                    break;
            }
        }

        public void PreviousPlaceHolder(Object sender, EventArgs e)
        {
            switch (CurrentWizardPlaceHolder)
            {
                case WizardPlaceHolder.ChooseSiteTemplate:
                    break;

                case WizardPlaceHolder.CreateSiteParameters:
                    SetActivePlaceHolder(WizardPlaceHolder.ChooseSiteTemplate, ChooseSiteTemplate);
                    break;
            }
        }

    }
}
