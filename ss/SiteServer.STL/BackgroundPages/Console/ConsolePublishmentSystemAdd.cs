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

namespace SiteServer.STL.BackgroundPages
{
    public class ConsolePublishmentSystemAdd : BackgroundBasePage
    {
        protected override bool IsSinglePage
        {
            get { return true; }
        }

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
        public RadioButtonList IsHeadquarters;
        public PlaceHolder phNotIsHeadquarters;
        public DropDownList ParentPublishmentSystemID;
        public TextBox PublishmentSystemDir;

        public PlaceHolder phNodeRelated;
        public DropDownList Charset;
        public Control RowIsImportContents;
        public CheckBox IsImportContents;
        public Control RowIsImportTableStyles;
        public CheckBox IsImportTableStyles;
        public Control RowIsUserSiteTemplateAuxiliaryTables;
        public RadioButtonList IsUserSiteTemplateAuxiliaryTables;
        public PlaceHolder phAuxiliaryTable;
        public DropDownList AuxiliaryTableForContent;
        public PlaceHolder phB2CTables;
        public DropDownList AuxiliaryTableForGoods;
        public DropDownList AuxiliaryTableForBrand;
        public PlaceHolder phWCMTables;
        public DropDownList AuxiliaryTableForGovPublic;
        public DropDownList AuxiliaryTableForGovInteract;
        public DropDownList AuxiliaryTableForVote;
        public DropDownList AuxiliaryTableForJob;
        public RadioButtonList IsCheckContentUseLevel;
        public Control CheckContentLevelRow;
        public DropDownList CheckContentLevel;

        public PlaceHolder OperatingError;
        public Literal ltlErrorMessage;

        public Button Previous;
        public Button Next;

        private EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.CMS;
        SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl(EPublishmentSystemType publishmentSystemType)
        {
            return PageUtils.GetSTLUrl(string.Format("console_publishmentSystemAdd.aspx?publishmentSystemType={0}", EPublishmentSystemTypeUtils.GetValue(publishmentSystemType)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemType = EPublishmentSystemTypeUtils.GetEnumType(base.GetQueryString("publishmentSystemType"));
            this.sortedlist = SiteTemplateManager.Instance.GetSiteTemplateSortedList(this.publishmentSystemType);

            if (!IsPostBack)
            {
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

                int hqSiteID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                if (hqSiteID == 0)
                {
                    this.IsHeadquarters.SelectedValue = "True";
                    this.phNotIsHeadquarters.Visible = false;
                }
                else
                {
                    this.IsHeadquarters.Enabled = false;
                }

                ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(this.publishmentSystemType, true);

                this.phWCMTables.Visible = publishmentSystemType == EPublishmentSystemType.WCM;
                this.phB2CTables.Visible = EPublishmentSystemTypeUtils.IsB2C(publishmentSystemType);

                this.ParentPublishmentSystemID.Items.Add(new ListItem("<无上级应用>", "0"));
                ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDWithoutUserCenterArrayList();
                ArrayList mySystemInfoArrayList = new ArrayList();
                Hashtable parentWithChildren = new Hashtable();
                foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (publishmentSystemInfo.IsHeadquarters == false)
                    {
                        if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                        {
                            mySystemInfoArrayList.Add(publishmentSystemInfo);
                        }
                        else
                        {
                            ArrayList children = new ArrayList();
                            if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                            {
                                children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                            }
                            children.Add(publishmentSystemInfo);
                            parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                        }
                    }
                }
                foreach (PublishmentSystemInfo publishmentSystemInfo in mySystemInfoArrayList)
                {
                    AddSite(this.ParentPublishmentSystemID, publishmentSystemInfo, parentWithChildren, 0);
                }
                ControlUtils.SelectListItems(this.ParentPublishmentSystemID, "0");

                this.phNodeRelated.Visible = EPublishmentSystemTypeUtils.IsNodeRelated(this.publishmentSystemType);

                ECharsetUtils.AddListItems(this.Charset);
                ControlUtils.SelectListItems(this.Charset, ECharsetUtils.GetValue(ECharset.utf_8));

                ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForContent.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForGoods.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BrandContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForBrand.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GovPublicContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForGovPublic.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GovInteractContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForGovInteract.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.VoteContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForVote.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.JobContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    this.AuxiliaryTableForJob.Items.Add(li);
                }

                IsCheckContentUseLevel.Items.Add(new ListItem("默认审核机制", false.ToString()));
                IsCheckContentUseLevel.Items.Add(new ListItem("多级审核机制", true.ToString()));
                ControlUtils.SelectListItems(this.IsCheckContentUseLevel, false.ToString());

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
                    this.RowSiteTemplateName.Visible = this.RowIsImportContents.Visible = this.RowIsImportTableStyles.Visible = this.RowIsUserSiteTemplateAuxiliaryTables.Visible = false;
                    this.phAuxiliaryTable.Visible = true;
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

        private void AddSite(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName + string.Format("({0})", children.Count), publishmentSystemInfo.PublishmentSystemID.ToString()));
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddSite(listControl, subSiteInfo, parentWithChildren, level);
                }
            }
            else
            {
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString()));
            }
        }

        public void IsHeadquarters_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phNotIsHeadquarters.Visible = !TranslateUtils.ToBool(this.IsHeadquarters.SelectedValue);
        }

        public void IsUserSiteTemplateAuxiliaryTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.IsUserSiteTemplateAuxiliaryTables.SelectedValue))
            {
                this.phAuxiliaryTable.Visible = false;
            }
            else
            {
                this.phAuxiliaryTable.Visible = true;
            }
        }

        public void IsCheckContentUseLevel_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            if (EBooleanUtils.Equals(this.IsCheckContentUseLevel.SelectedValue, EBoolean.True))
            {
                this.CheckContentLevelRow.Visible = true;
            }
            else
            {
                this.CheckContentLevelRow.Visible = false;
            }
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

                    }

                    string templateSN = dirInfo.Name.ToUpper().Substring(2);
                    if (!string.IsNullOrEmpty(siteTemplateInfo.WebSiteUrl))
                    {
                        templateSN = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.ParseConfigRootUrl(siteTemplateInfo.WebSiteUrl), templateSN);
                    }
                    if (e.Item.ItemIndex == 0)
                    {
                        ltlRadio.Text = string.Format(@"
<label class=""radio lead"" onClick=""$('#SiteTemplateDir').val($(this).find('input').val());"">
  <input type=""radio"" name=""choose"" id=""choose{0}"" value=""{1}"" {2} checked=""checked"">
  {3}
</label>", e.Item.ItemIndex + 1, dirInfo.Name, checkedStr, templateSN);
                        this.SiteTemplateDir.Value = dirInfo.Name;
                    }
                    else
                    {
                        ltlRadio.Text = string.Format(@"
<label class=""radio lead"" onClick=""$('#SiteTemplateDir').val($(this).find('input').val());"">
  <input type=""radio"" name=""choose"" id=""choose{0}"" value=""{1}"" {2}>
  {3}
</label>", e.Item.ItemIndex + 1, dirInfo.Name, checkedStr, templateSN);

                    }


                    if (!string.IsNullOrEmpty(siteTemplateInfo.PicFileName))
                    {
                        string siteTemplateUrl = PageUtility.GetSiteTemplatesUrl(dirInfo.Name);
                        ltlImageUrl.Text = string.Format(@"<img class=""cover"" src=""{0}"" width=""180""><p></p>", PageUtility.GetSiteTemplateMetadataUrl(siteTemplateUrl, siteTemplateInfo.PicFileName));
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
                bool isHQ = TranslateUtils.ToBool(this.IsHeadquarters.SelectedValue);
                int parentPublishmentSystemID = 0;
                string publishmentSystemDir = string.Empty;

                if (isHQ == false)
                {
                    if (DirectoryUtils.IsSystemDirectory(PublishmentSystemDir.Text))
                    {
                        errorMessage = "文件夹名称不能为系统文件夹名称！";
                        return 0;
                    }

                    parentPublishmentSystemID = TranslateUtils.ToInt(this.ParentPublishmentSystemID.SelectedValue);
                    publishmentSystemDir = this.PublishmentSystemDir.Text;

                    ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(parentPublishmentSystemID);
                    if (arraylist.IndexOf(publishmentSystemDir.ToLower()) != -1)
                    {
                        errorMessage = "已存在相同的发布路径！";
                        return 0;
                    }

                    if (!DirectoryUtils.IsDirectoryNameCompliant(publishmentSystemDir))
                    {
                        errorMessage = "文件夹名称不符合系统要求！";
                        return 0;
                    }
                }

                NodeInfo nodeInfo = new NodeInfo();

                nodeInfo.NodeName = nodeInfo.NodeIndexName = "首页";
                nodeInfo.NodeType = ENodeType.BackgroundPublishNode;
                nodeInfo.ContentModelID = EContentModelTypeUtils.GetValue(EContentModelTypeUtils.GetEnumTypeByPublishmentSystemType(this.publishmentSystemType));

                string publishmentSystemUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, publishmentSystemDir);

                string groupSN = GroupSNManager.GetCurrentGroupSN();

                #region 多个用户中心（已注释）
                ////只有创建用户中心的时候，才有GroupSN；其他应用创建，默认关联到一个用户中心或者通过下拉列表选择用户中心，同时也可以更改用户中心
                //if (EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.UserCenter, this.publishmentSystemType))
                //{
                //    //用户中心
                //    groupSN = GroupSNManager.GetCurrentGroupSN();
                //}
                //else
                //{
                //    //其他应用
                //    if (EPublishmentSystemTypeUtils.IsEnabled(this.publishmentSystemType))
                //    {
                //        //唯一用户中心
                //        PublishmentSystemInfo defaultUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                //        if (defaultUserCenter != null)
                //            groupSN = defaultUserCenter.GroupSN;
                //    }
                //} 
                #endregion

                PublishmentSystemInfo psInfo = BaseTable.GetDefaultPublishmentSystemInfo(PageUtils.FilterXSS(this.PublishmentSystemName.Text), this.publishmentSystemType, AuxiliaryTableForContent.SelectedValue, AuxiliaryTableForGoods.SelectedValue, AuxiliaryTableForBrand.SelectedValue, AuxiliaryTableForGovPublic.SelectedValue, AuxiliaryTableForGovInteract.SelectedValue, AuxiliaryTableForVote.SelectedValue, AuxiliaryTableForJob.SelectedValue, publishmentSystemDir, publishmentSystemUrl, parentPublishmentSystemID, groupSN);

                if (psInfo.ParentPublishmentSystemID > 0)
                {
                    PublishmentSystemInfo parentPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psInfo.ParentPublishmentSystemID);
                    psInfo.PublishmentSystemUrl = PageUtils.Combine(parentPublishmentSystemInfo.PublishmentSystemUrl, psInfo.PublishmentSystemDir);
                }

                psInfo.IsHeadquarters = isHQ;

                psInfo.Additional.Charset = this.Charset.SelectedValue;
                psInfo.IsCheckContentUseLevel = TranslateUtils.ToBool(this.IsCheckContentUseLevel.SelectedValue);
                if (psInfo.IsCheckContentUseLevel)
                {
                    psInfo.CheckContentLevel = TranslateUtils.ToInt(this.CheckContentLevel.SelectedValue);
                }

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

                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(thePublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(thePublishmentSystemID);
                #endregion

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, string.Format("新建{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType)), string.Format("应用名称:{0}", PageUtils.FilterXSS(this.PublishmentSystemName.Text)));

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
                    if (this.UseSiteTemplate.Checked)
                    {
                        this.RowSiteTemplateName.Visible = this.RowIsImportContents.Visible = this.RowIsImportTableStyles.Visible = this.RowIsUserSiteTemplateAuxiliaryTables.Visible = true;
                        this.phAuxiliaryTable.Visible = true;
                        this.SiteTemplateName.Text = string.Format("{0}（{1}）", this.GetSiteTemplateName(this.SiteTemplateDir.Value), this.SiteTemplateDir.Value);

                        string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Value);
                        string filePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Configuration);
                        PublishmentSystemInfo publishmentSystemInfo = ConfigurationIE.GetPublishmentSytemInfo(filePath);

                        this.PublishmentSystemName.Text = publishmentSystemInfo.PublishmentSystemName;
                        this.PublishmentSystemDir.Text = publishmentSystemInfo.PublishmentSystemDir;
                        PublishmentSystemInfoExtend extend = new PublishmentSystemInfoExtend(publishmentSystemInfo.SettingsXML);
                        if (!string.IsNullOrEmpty(extend.Charset))
                        {
                            this.Charset.SelectedValue = extend.Charset;
                        }
                    }
                    else
                    {
                        this.RowSiteTemplateName.Visible = this.RowIsImportContents.Visible = this.RowIsImportTableStyles.Visible = this.RowIsUserSiteTemplateAuxiliaryTables.Visible = false;
                        this.phAuxiliaryTable.Visible = true;
                    }
                    SetActivePlaceHolder(WizardPlaceHolder.CreateSiteParameters, CreateSiteParameters);
                    break;

                case WizardPlaceHolder.CreateSiteParameters:
                    string errorMessage;
                    int thePublishmentSystemID = Validate_PublishmentSystemInfo(out errorMessage);
                    if (thePublishmentSystemID > 0)
                    {
                        string url = BackgroundProgressBar.GetCreatePublishmentSystemUrl(thePublishmentSystemID, this.UseSiteTemplate.Checked, this.IsImportContents.Checked, this.IsImportTableStyles.Checked, this.SiteTemplateDir.Value, bool.Parse(this.IsUserSiteTemplateAuxiliaryTables.SelectedValue), StringUtils.GUID(), string.Empty);
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
