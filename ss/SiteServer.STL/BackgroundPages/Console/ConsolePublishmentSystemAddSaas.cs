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
using BaiRong.Core.Integration;

namespace SiteServer.STL.BackgroundPages
{
	public class ConsolePublishmentSystemAddSaas : BackgroundBasePage
	{
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public HtmlInputHidden hihAuthPublishmentSystemID;

        public Literal ltlPageTitle;
		public PlaceHolder ChooseSiteTemplate;
		public CheckBox UseSiteTemplate;
		public DataList dlContents;
		public HtmlInputHidden SiteTemplateDir;

        public PlaceHolder CreateSiteParameters;
        public Control RowSiteTemplateName;
        public Label SiteTemplateName;
        public TextBox PublishmentSystemName;
        public Literal ltlPublishmentSystemType;

		public PlaceHolder OperatingError;
		public Literal ltlErrorMessage;

		public Button Previous;
		public Button Next;

        private EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.CMS;
		SortedList sortedlist = new SortedList();
        private string returnUrl = string.Empty;

        public static string GetRedirectUrl(EPublishmentSystemType publishmentSystemType)
        {
            return PageUtils.GetSTLUrl(string.Format("console_publishmentSystemAddSaas.aspx?publishmentSystemType={0}", EPublishmentSystemTypeUtils.GetValue(publishmentSystemType)));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemType = EPublishmentSystemTypeUtils.GetEnumType(base.GetQueryString("publishmentSystemType"));
            this.sortedlist = SiteTemplateManager.Instance.GetSiteTemplateSortedList(this.publishmentSystemType);
            this.returnUrl = base.GetQueryString("returnUrl");

			if (!IsPostBack)
            {
                if (FileConfigManager.Instance.IsSaas)
                {
                    int authPublishmentSystemID = 0;

                    //string token = IntegrationManager.GetIntegrationToken(AdminManager.Current.UserName);
                    //string errorMessage = string.Empty;
                    //IntegrationCloudInfo cloudInfo = IntegrationManager.API_GEXIA_COM.GetIntegrationCloudInfo(token, out errorMessage);

                    //if (cloudInfo.IsSuccess && cloudInfo.UserName == AdminManager.Current.UserName)
                    //{

                    //}

                    PageUtils.RedirectToErrorPage("对不起，旧系统新建站点功能已关闭，请采用新系统");
                    return;

                    //if (ProductPermissionsManager.Current.PublishmentSystemIDList.Count >= 2)
                    //{

                    //}

                    //foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
                    //{
                    //    if (authPublishmentSystemInfo.PublishmentSystemID == 0)
                    //    {
                    //        authPublishmentSystemID = authPublishmentSystemInfo.ID;
                    //    }
                    //}

                    //if (authPublishmentSystemID == 0)
                    //{
                    //    PageUtils.RedirectToErrorPage(string.Format("对不起，您暂无权限创建{0}应用，如需开通此应用请联系公司客服人员", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType)));
                    //    return;
                    //}

                    this.hihAuthPublishmentSystemID.Value = authPublishmentSystemID.ToString();

                    BaiRongDataProvider.TableCollectionDAO.CreateAllAuxiliaryTableIfNotExists();

                    string pageTitle = string.Format("创建{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType));
                    this.ltlPageTitle.Text = pageTitle;
                    base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, pageTitle, AppManager.Platform.Permission.Platform_Site);

                    LicenseManager licenseManager = LicenseManager.Instance;
                    if (licenseManager.IsMaxSiteNumberLimited)
                    {
                        int siteNumber = DataProvider.PublishmentSystemDAO.GetPublishmentSystemCount();
                        if (siteNumber >= licenseManager.MaxSiteNumber)
                        {
                            PageUtils.RedirectToErrorPage(string.Format("受许可限制，您最多能只能建立{0}个网站。", licenseManager.MaxSiteNumber));
                            return;
                        }
                    }

                    ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(this.publishmentSystemType, true);

                    this.UseSiteTemplate.Attributes.Add("onclick", "displaySiteTemplateDiv(this)");

                    BindGrid();

                    if (this.sortedlist.Count > 0)
                    {
                        SetActivePlaceHolder(WizardPlaceHolder.ChooseSiteTemplate, ChooseSiteTemplate);
                    }
                    else
                    {
                        string urlOnline = PageUtils.GetSTLUrl("console_siteTemplateOnline.aspx");
                        base.InfoMessage(string.Format(@"无{0}应用模板可选择，您可以首先 <a href=""{1}"" style=""color:red"">在线下载</a> {0}应用模板之后再创建应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType), urlOnline));
                        this.ChooseSiteTemplate.Visible = false;
                        this.UseSiteTemplate.Checked = false;
                        SetActivePlaceHolder(WizardPlaceHolder.CreateSiteParameters, CreateSiteParameters);
                        this.RowSiteTemplateName.Visible = false;
                    }
                }
			}
		}

        public string GetSiteTemplateName(string siteTemplateDir)
        {
            string retval = string.Empty;
            SiteTemplateInfo siteTemplateInfo = this.sortedlist[siteTemplateDir] as SiteTemplateInfo;
            if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.SiteTemplateName))
            {
                if (!string.IsNullOrEmpty(siteTemplateInfo.WebSiteUrl))
                {

                    retval = string.Format("<a href=\"{0}\" target=_blank>{1}</a>", PageUtils.ParseNavigationUrl(siteTemplateInfo.WebSiteUrl), siteTemplateInfo.SiteTemplateName);
                }
                else
                {
                    retval = siteTemplateInfo.SiteTemplateName;
                }
            }
            return retval;
        }

		public void BindGrid()
		{
			try
			{
				ArrayList directoryArrayList = new ArrayList();
				foreach (string directoryName in this.sortedlist.Keys)
				{
                    string directoryPath = PathUtility.GetSiteTemplatesPath(directoryName);
					DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
					directoryArrayList.Add(dirInfo);
				}

                this.dlContents.DataSource = directoryArrayList;
                this.dlContents.ItemDataBound += dlContents_ItemDataBound;
                this.dlContents.DataBind();
			}
			catch (Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        void dlContents_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DirectoryInfo dirInfo = (DirectoryInfo)e.Item.DataItem;

                Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlRadio = e.Item.FindControl("ltlRadio") as Literal;

                SiteTemplateInfo siteTemplateInfo = this.sortedlist[dirInfo.Name] as SiteTemplateInfo;
                if (siteTemplateInfo != null && !string.IsNullOrEmpty(siteTemplateInfo.SiteTemplateName))
                {
                    string checkedStr = string.Empty;
                    if (StringUtils.EqualsIgnoreCase(base.GetQueryString("siteTemplate"), dirInfo.Name))
                    {
                        checkedStr = "checked";
                        this.SiteTemplateDir.Value = dirInfo.Name;
                    }

                    string templateSN = dirInfo.Name.ToUpper().Substring(2);
                    if (!string.IsNullOrEmpty(siteTemplateInfo.WebSiteUrl))
                    {
                        templateSN = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.ParseConfigRootUrl(siteTemplateInfo.WebSiteUrl), templateSN);
                    }

                    ltlRadio.Text = string.Format(@"
<label class=""radio lead"" onClick=""$('#SiteTemplateDir').val($(this).find('input').val());"">
  <input type=""radio"" name=""choose"" id=""choose{0}"" value=""{1}"" {2}>
  {3}
</label>", e.Item.ItemIndex + 1, dirInfo.Name, checkedStr, templateSN);

                    if (!string.IsNullOrEmpty(siteTemplateInfo.PicFileName))
                    {
                        string siteTemplateUrl = PageUtility.GetSiteTemplatesUrl(dirInfo.Name);
                        ltlImageUrl.Text = string.Format(@"<img class=""cover"" src=""{0}"" width=""180"" class=""img-polaroid""><p></p>", PageUtility.GetSiteTemplateMetadataUrl(siteTemplateUrl, siteTemplateInfo.PicFileName));
                    }

                    if (!string.IsNullOrEmpty(siteTemplateInfo.Description))
                    {
                        ltlDescription.Text = siteTemplateInfo.Description + "<p></p>";
                    }
                }
            }
        }

		public WizardPlaceHolder CurrentWizardPlaceHolder
		{
			get
			{
				if (ViewState["WizardPlaceHolder"] != null)
					return (WizardPlaceHolder)ViewState["WizardPlaceHolder"];

                if (this.sortedlist.Count > 0)
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

		public int Validate_PublishmentSystemInfo(out string errorMessage)
		{
		    try
			{
                int parentPublishmentSystemID = 0;
                string publishmentSystemDir = string.Empty;

                publishmentSystemDir = StringUtils.GetShortGUID().Substring(0, 5);

                ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(parentPublishmentSystemID);
                if (arraylist.IndexOf(publishmentSystemDir.ToLower()) != -1)
                {
                    publishmentSystemDir = StringUtils.GetShortGUID().Substring(0, 5);
                }
                if (arraylist.IndexOf(publishmentSystemDir.ToLower()) != -1)
                {
                    publishmentSystemDir = StringUtils.GetShortGUID().Substring(0, 5);
                }

				NodeInfo nodeInfo = new NodeInfo();

                nodeInfo.NodeName = nodeInfo.NodeIndexName = "首页";
				nodeInfo.NodeType = ENodeType.BackgroundPublishNode;
                nodeInfo.ContentModelID = EContentModelTypeUtils.GetValue(EContentModelTypeUtils.GetEnumTypeByPublishmentSystemType(this.publishmentSystemType));
                
                string publishmentSystemUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, publishmentSystemDir);

                string auxiliaryTableForContent = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.BackgroundContent);

                string auxiliaryTableForGoods = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.GoodsContent);

                string auxiliaryTableForBrand = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.BrandContent);

                string auxiliaryTableForVote = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.VoteContent);

                string auxiliaryTableForJob = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.JobContent);

                string groupSN = GroupSNManager.GetCurrentGroupSN();

                PublishmentSystemInfo psInfo = BaseTable.GetDefaultPublishmentSystemInfo(this.PublishmentSystemName.Text, this.publishmentSystemType, auxiliaryTableForContent, auxiliaryTableForGoods, auxiliaryTableForBrand, string.Empty, string.Empty, auxiliaryTableForVote, auxiliaryTableForJob, publishmentSystemDir, publishmentSystemUrl, parentPublishmentSystemID, groupSN);

                if (psInfo.ParentPublishmentSystemID > 0)
                {
                    PublishmentSystemInfo parentPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psInfo.ParentPublishmentSystemID);
                    psInfo.PublishmentSystemUrl = PageUtils.Combine(parentPublishmentSystemInfo.PublishmentSystemUrl, psInfo.PublishmentSystemDir);
                }

                psInfo.IsHeadquarters = false;

                psInfo.Additional.Charset = ECharsetUtils.GetValue(ECharset.utf_8);
                psInfo.IsCheckContentUseLevel = false;

                int thePublishmentSystemID = DataProvider.NodeDAO.InsertPublishmentSystemInfo(nodeInfo, psInfo);

                if (PermissionsManager.Current.IsSystemAdministrator && !PermissionsManager.Current.IsConsoleAdministrator)
                {
                    List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                    if (publishmentSystemIDList == null)
                    {
                        publishmentSystemIDList = new List<int>();
                    }
                    publishmentSystemIDList.Add(thePublishmentSystemID);
                    BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemIDCollection(AdminManager.Current.UserName, TranslateUtils.ObjectCollectionToString(publishmentSystemIDList));
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, string.Format("新建{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType)), string.Format("应用名称:{0}", this.PublishmentSystemName.Text));

                //if (isHQ == EBoolean.False)
                //{
                //    string configFilePath = PathUtility.MapPath(psInfo, "@/web.config");
                //    if (FileUtils.IsFileExists(configFilePath))
                //    {
                //        FileUtility.UpdateWebConfig(configFilePath, psInfo.Additional.Charset);
                //    }
                //    else
                //    {
                //        FileUtility.CreateWebConfig(configFilePath, psInfo.Additional.Charset);
                //    }
                //}
                errorMessage = string.Empty;
                return thePublishmentSystemID;
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				return 0;
			}
		}

		public void NextPlaceHolder(Object sender, EventArgs e)
		{
		    switch (CurrentWizardPlaceHolder)
			{
				case WizardPlaceHolder.ChooseSiteTemplate:

                    if (this.UseSiteTemplate.Checked && string.IsNullOrEmpty(this.SiteTemplateDir.Value))
                    {
                        base.FailMessage("请选择一个应用模板创建您的微信应用");
                        return;
                    }

                    if (this.UseSiteTemplate.Checked)
                    {
                        this.RowSiteTemplateName.Visible = true;
                        this.SiteTemplateName.Text = string.Format("{0}（{1}）", this.GetSiteTemplateName(this.SiteTemplateDir.Value), this.SiteTemplateDir.Value);
                    }
                    else
                    {
                        this.RowSiteTemplateName.Visible = false;
                    }
                    SetActivePlaceHolder(WizardPlaceHolder.CreateSiteParameters, CreateSiteParameters);
					break;

                case WizardPlaceHolder.CreateSiteParameters:
			        string errorMessage;
			        int thePublishmentSystemID = Validate_PublishmentSystemInfo(out errorMessage);
			        if (thePublishmentSystemID > 0)
                    {
                        string url = BackgroundProgressBar.GetCreatePublishmentSystemUrl(thePublishmentSystemID, this.UseSiteTemplate.Checked, true, true, this.SiteTemplateDir.Value, false, StringUtils.GUID(), this.returnUrl);
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        this.ltlErrorMessage.Text = errorMessage;
                        SetActivePlaceHolder(WizardPlaceHolder.OperatingError, OperatingError);
                    }
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
                    base.AddScript("displaySiteTemplateDiv(document.all.UseSiteTemplate);");
                    SetActivePlaceHolder(WizardPlaceHolder.ChooseSiteTemplate, ChooseSiteTemplate);
					break;
			}
		}

	}
}
