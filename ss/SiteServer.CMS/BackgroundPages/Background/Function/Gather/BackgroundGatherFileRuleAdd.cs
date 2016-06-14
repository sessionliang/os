using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundGatherFileRuleAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;
		public PlaceHolder GatherRuleBase;
		public TextBox GatherRuleName;
		public TextBox GatherUrl;
        public DropDownList Charset;
        
        public DropDownList IsToFile;

        public PlaceHolder PlaceHolder_File;
        public TextBox FilePath;
        public RadioButtonList IsSaveRelatedFiles;
        public RadioButtonList IsRemoveScripts;
        public PlaceHolder PlaceHolder_File_Directory;
        public TextBox StyleDirectoryPath;
        public TextBox ScriptDirectoryPath;
        public TextBox ImageDirectoryPath;

        public PlaceHolder PlaceHolder_Content;
		public DropDownList NodeIDDropDownList;
        public RadioButtonList IsSaveImage;
        public RadioButtonList IsChecked;
        public RadioButtonList IsAutoCreate;


        public PlaceHolder GatherRuleContent;
		public TextBox ContentTitleStart;
		public TextBox ContentTitleEnd;
        public TextBox ContentExclude;
		public CheckBoxList ContentHtmlClearCollection;
        public CheckBoxList ContentHtmlClearTagCollection;
		public TextBox ContentContentStart;
		public TextBox ContentContentEnd;
        public CheckBoxList ContentAttributes;
        public Repeater ContentAttributesRepeater;

		public PlaceHolder Done;

		public PlaceHolder OperatingError;
		public Label ErrorLabel;

		public Button Previous;
		public Button Next;

		private bool isEdit = false;
		private string theGatherRuleName;
        private NameValueCollection contentAttributesXML;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (base.GetQueryString("GatherRuleName") != null)
			{
				this.isEdit = true;
                this.theGatherRuleName = base.GetQueryString("GatherRuleName");
			}

			if (!Page.IsPostBack)
            {
                string pageTitle = this.isEdit ? "编辑单文件页采集规则" : "添加单文件页采集规则";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, pageTitle, AppManager.CMS.Permission.WebSite.Gather);

                this.ltlPageTitle.Text = pageTitle;

                ECharsetUtils.AddListItems(this.Charset);
                ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(ECharset.gb2312));
                NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);

				SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);

				if (this.isEdit)
				{
                    GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.theGatherRuleName, base.PublishmentSystemID);
					GatherRuleName.Text = gatherFileRuleInfo.GatherRuleName;
                    this.GatherUrl.Text = gatherFileRuleInfo.GatherUrl;
                    ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(gatherFileRuleInfo.Charset));

                    ControlUtils.SelectListItems(this.IsToFile, gatherFileRuleInfo.IsToFile.ToString());
                    this.FilePath.Text = gatherFileRuleInfo.FilePath;
                    ControlUtils.SelectListItems(this.IsSaveRelatedFiles, gatherFileRuleInfo.IsSaveRelatedFiles.ToString());
                    ControlUtils.SelectListItems(this.IsRemoveScripts, gatherFileRuleInfo.IsRemoveScripts.ToString());
                    this.StyleDirectoryPath.Text = gatherFileRuleInfo.StyleDirectoryPath;
                    this.ScriptDirectoryPath.Text = gatherFileRuleInfo.ScriptDirectoryPath;
                    this.ImageDirectoryPath.Text = gatherFileRuleInfo.ImageDirectoryPath;

                    ControlUtils.SelectListItems(this.NodeIDDropDownList, gatherFileRuleInfo.NodeID.ToString());
                    ControlUtils.SelectListItems(this.IsSaveImage, gatherFileRuleInfo.IsSaveImage.ToString());
                    ControlUtils.SelectListItems(this.IsChecked, gatherFileRuleInfo.IsChecked.ToString());
                    ControlUtils.SelectListItems(this.IsAutoCreate, gatherFileRuleInfo.IsAutoCreate.ToString());

                    ContentExclude.Text = gatherFileRuleInfo.ContentExclude;
					ArrayList htmlClearArrayList = TranslateUtils.StringCollectionToArrayList(gatherFileRuleInfo.ContentHtmlClearCollection);
					foreach (ListItem item in this.ContentHtmlClearCollection.Items)
					{
						if (htmlClearArrayList.Contains(item.Value)) item.Selected = true;
					}
                    ArrayList htmlClearTagArrayList = TranslateUtils.StringCollectionToArrayList(gatherFileRuleInfo.ContentHtmlClearTagCollection);
                    foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                    {
                        if (htmlClearTagArrayList.Contains(item.Value)) item.Selected = true;
                    }
					ContentTitleStart.Text = gatherFileRuleInfo.ContentTitleStart;
					ContentTitleEnd.Text = gatherFileRuleInfo.ContentTitleEnd;
					ContentContentStart.Text = gatherFileRuleInfo.ContentContentStart;
					ContentContentEnd.Text = gatherFileRuleInfo.ContentContentEnd;

                    ArrayList contentAttributeArrayList = TranslateUtils.StringCollectionToArrayList(gatherFileRuleInfo.ContentAttributes);
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, null);
                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Title) || StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.Content)) continue;

                        ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName.ToLower());
                        if (contentAttributeArrayList.Contains(listitem.Value))
                        {
                            listitem.Selected = true;
                        }
                        this.ContentAttributes.Items.Add(listitem);
                    }

                    this.contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherFileRuleInfo.ContentAttributesXML);
                    
                    this.ContentAttributes_SelectedIndexChanged(null, EventArgs.Empty);
                    
				}

                this.DropDownList_SelectedIndexChanged(null, EventArgs.Empty);
			}			

			base.SuccessMessage(string.Empty);
		}

        

        private WizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (WizardPanel)ViewState["WizardPanel"];

				return WizardPanel.GatherRuleBase;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}


		private enum WizardPanel
		{
			GatherRuleBase,
			GatherRuleContent,
			OperatingError,
			Done
		}

		void SetActivePanel(WizardPanel panel, Control controlToShow)
		{
			PlaceHolder currentPanel = FindControl(CurrentWizardPanel.ToString()) as PlaceHolder;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case WizardPanel.GatherRuleBase:
					Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
					break;
				case WizardPanel.Done:
					Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl(string.Format("background_gatherFileRule.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
					break;
				case WizardPanel.OperatingError:
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
			CurrentWizardPanel = panel;
		}

		public bool Validate_GatherRuleBase(out string errorMessage)
		{
			if (string.IsNullOrEmpty(this.GatherRuleName.Text))
			{
				errorMessage = "必须填写采集规则名称！";
				return false;
			}

			if (string.IsNullOrEmpty(this.GatherUrl.Text))
			{
                errorMessage = "必须填写采集网页地址！";
				return false;
			}

			if (this.isEdit == false)
			{
                ArrayList gatherRuleNameList = DataProvider.GatherFileRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
				if (gatherRuleNameList.IndexOf(this.GatherRuleName.Text) != -1)
				{
					errorMessage = "采集规则名称已存在！";
					return false;
				}
			}

            if (TranslateUtils.ToBool(this.IsToFile.SelectedValue))
            {
                if (string.IsNullOrEmpty(this.FilePath.Text))
                {
                    errorMessage = "必须填写采集到的文件地址！";
                    return false;
                }
                else
                {
                    bool isOk = false;
                    if (StringUtils.StringStartsWith(this.FilePath.Text, '~') || StringUtils.StringStartsWith(this.FilePath.Text, '@'))
                    {
                        if (!PathUtils.IsDirectoryPath(this.FilePath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        errorMessage = "采集到的文件地址不正确,必须填写有效的文件地址！";
                        return false;
                    }
                }

                if (TranslateUtils.ToBool(this.IsSaveRelatedFiles.SelectedValue))
                {
                    bool isOk = false;
                    if (StringUtils.StringStartsWith(this.StyleDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.StyleDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.StyleDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        errorMessage = "CSS样式保存地址不正确,必须填写有效的文件夹地址！";
                        return false;
                    }

                    isOk = false;
                    if (StringUtils.StringStartsWith(this.ScriptDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.ScriptDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.ScriptDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        errorMessage = "Js脚本保存地址不正确,必须填写有效的文件夹地址！";
                        return false;
                    }
                    
                    isOk = false;
                    if (StringUtils.StringStartsWith(this.ImageDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.ImageDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.ImageDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        errorMessage = "图片保存地址不正确,必须填写有效的文件夹地址！";
                        return false;
                    }
                }
            }

			errorMessage = string.Empty;
			return true;
		}

		public bool Validate_GatherContent(out string errorMessage)
		{
            if (!TranslateUtils.ToBool(this.IsToFile.SelectedValue))
            {
                if (string.IsNullOrEmpty(this.ContentTitleStart.Text) || string.IsNullOrEmpty(this.ContentTitleEnd.Text))
                {
                    errorMessage = "必须填写内容标题规则！";
                    return false;
                }
                else if (string.IsNullOrEmpty(this.ContentContentStart.Text) || string.IsNullOrEmpty(this.ContentContentEnd.Text))
                {
                    errorMessage = "必须填写内容正文规则！";
                    return false;
                }
            }
			errorMessage = string.Empty;
			return true;
		}

		public bool Validate_InsertGatherRule(out string errorMessage)
		{
			try
			{
                bool isNeedAdd = false;
				if (this.isEdit)
				{
                    if (this.theGatherRuleName != this.GatherRuleName.Text)
                    {
                        isNeedAdd = true;
                        DataProvider.GatherDatabaseRuleDAO.Delete(this.theGatherRuleName, base.PublishmentSystemID);
                    }
                    else
                    {
                        GatherFileRuleInfo gatherFileRuleInfo =
                            DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.theGatherRuleName,
                                                                                 base.PublishmentSystemID);
                        gatherFileRuleInfo.GatherUrl = this.GatherUrl.Text;
                        gatherFileRuleInfo.Charset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);

                        gatherFileRuleInfo.IsToFile = TranslateUtils.ToBool(IsToFile.SelectedValue);
                        gatherFileRuleInfo.FilePath = this.FilePath.Text;
                        gatherFileRuleInfo.IsSaveRelatedFiles =
                            TranslateUtils.ToBool(IsSaveRelatedFiles.SelectedValue);
                        gatherFileRuleInfo.IsRemoveScripts =
                            TranslateUtils.ToBool(IsRemoveScripts.SelectedValue);
                        gatherFileRuleInfo.StyleDirectoryPath = this.StyleDirectoryPath.Text;
                        gatherFileRuleInfo.ScriptDirectoryPath = this.ScriptDirectoryPath.Text;
                        gatherFileRuleInfo.ImageDirectoryPath = this.ImageDirectoryPath.Text;

                        if (NodeIDDropDownList.SelectedValue != null)
                        {
                            gatherFileRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                        }
                        gatherFileRuleInfo.IsSaveImage = TranslateUtils.ToBool(IsSaveImage.SelectedValue);
                        gatherFileRuleInfo.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                        gatherFileRuleInfo.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);

                        gatherFileRuleInfo.ContentExclude = ContentExclude.Text;
                        ArrayList htmlClearArrayList = new ArrayList();
                        foreach (ListItem item in this.ContentHtmlClearCollection.Items)
                        {
                            if (item.Selected) htmlClearArrayList.Add(item.Value);
                        }
                        gatherFileRuleInfo.ContentHtmlClearCollection = TranslateUtils.ObjectCollectionToString(htmlClearArrayList);
                        ArrayList htmlClearTagArrayList = new ArrayList();
                        foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                        {
                            if (item.Selected) htmlClearTagArrayList.Add(item.Value);
                        }
                        gatherFileRuleInfo.ContentHtmlClearTagCollection = TranslateUtils.ObjectCollectionToString(htmlClearTagArrayList);
                        gatherFileRuleInfo.ContentTitleStart = ContentTitleStart.Text;
                        gatherFileRuleInfo.ContentTitleEnd = ContentTitleEnd.Text;
                        gatherFileRuleInfo.ContentContentStart = ContentContentStart.Text;
                        gatherFileRuleInfo.ContentContentEnd = ContentContentEnd.Text;

                        ArrayList valueArrayList =
                            ControlUtils.GetSelectedListControlValueArrayList(this.ContentAttributes);
                        gatherFileRuleInfo.ContentAttributes = TranslateUtils.ObjectCollectionToString(valueArrayList);
                        NameValueCollection attributesXML = new NameValueCollection();

                        for (int i = 0; i < valueArrayList.Count; i++)
                        {
                            string attributeName = valueArrayList[i] as string;

                            foreach (RepeaterItem item in this.ContentAttributesRepeater.Items)
                            {
                                if (item.ItemIndex == i)
                                {
                                    TextBox contentStart = (TextBox) item.FindControl("ContentStart");
                                    TextBox contentEnd = (TextBox) item.FindControl("ContentEnd");

                                    attributesXML[attributeName + "_ContentStart"] =
                                        StringUtils.ValueToUrl(contentStart.Text);
                                    attributesXML[attributeName + "_ContentEnd"] =
                                        StringUtils.ValueToUrl(contentEnd.Text);
                                }
                            }
                        }
                        gatherFileRuleInfo.ContentAttributesXML =
                            TranslateUtils.NameValueCollectionToString(attributesXML);

                        DataProvider.GatherFileRuleDAO.Update(gatherFileRuleInfo);
                    }
				}
				else
				{
				    isNeedAdd = true;
				}

                if (isNeedAdd)
                {
                    GatherFileRuleInfo gatherFileRuleInfo = new GatherFileRuleInfo();
                    gatherFileRuleInfo.GatherRuleName = GatherRuleName.Text;
                    gatherFileRuleInfo.PublishmentSystemID = base.PublishmentSystemID;
                    if (NodeIDDropDownList.SelectedValue != null)
                    {
                        gatherFileRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                    }
                    gatherFileRuleInfo.GatherUrl = this.GatherUrl.Text;
                    gatherFileRuleInfo.Charset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);

                    gatherFileRuleInfo.IsToFile = TranslateUtils.ToBool(IsToFile.SelectedValue);
                    gatherFileRuleInfo.FilePath = this.FilePath.Text;
                    gatherFileRuleInfo.IsSaveRelatedFiles = TranslateUtils.ToBool(IsSaveRelatedFiles.SelectedValue);
                    gatherFileRuleInfo.IsRemoveScripts = TranslateUtils.ToBool(IsRemoveScripts.SelectedValue);
                    gatherFileRuleInfo.StyleDirectoryPath = this.StyleDirectoryPath.Text;
                    gatherFileRuleInfo.ScriptDirectoryPath = this.ScriptDirectoryPath.Text;
                    gatherFileRuleInfo.ImageDirectoryPath = this.ImageDirectoryPath.Text;

                    if (NodeIDDropDownList.SelectedValue != null)
                    {
                        gatherFileRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                    }
                    gatherFileRuleInfo.IsSaveImage = TranslateUtils.ToBool(IsSaveImage.SelectedValue);
                    gatherFileRuleInfo.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                    gatherFileRuleInfo.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);

                    gatherFileRuleInfo.ContentExclude = ContentExclude.Text;
                    ArrayList htmlClearArrayList = new ArrayList();
                    foreach (ListItem item in this.ContentHtmlClearCollection.Items)
                    {
                        if (item.Selected) htmlClearArrayList.Add(item.Value);
                    }
                    gatherFileRuleInfo.ContentHtmlClearCollection = TranslateUtils.ObjectCollectionToString(htmlClearArrayList);
                    ArrayList htmlClearTagArrayList = new ArrayList();
                    foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                    {
                        if (item.Selected) htmlClearTagArrayList.Add(item.Value);
                    }
                    gatherFileRuleInfo.ContentHtmlClearTagCollection = TranslateUtils.ObjectCollectionToString(htmlClearTagArrayList);
                    gatherFileRuleInfo.LastGatherDate = DateUtils.SqlMinValue;
                    gatherFileRuleInfo.ContentTitleStart = ContentTitleStart.Text;
                    gatherFileRuleInfo.ContentTitleEnd = ContentTitleEnd.Text;
                    gatherFileRuleInfo.ContentContentStart = ContentContentStart.Text;
                    gatherFileRuleInfo.ContentContentEnd = ContentContentEnd.Text;

                    ArrayList valueArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.ContentAttributes);
                    gatherFileRuleInfo.ContentAttributes = TranslateUtils.ObjectCollectionToString(valueArrayList);
                    NameValueCollection attributesXML = new NameValueCollection();

                    for (int i = 0; i < valueArrayList.Count; i++)
                    {
                        string attributeName = valueArrayList[i] as string;

                        foreach (RepeaterItem item in this.ContentAttributesRepeater.Items)
                        {
                            if (item.ItemIndex == i)
                            {
                                TextBox contentStart = (TextBox)item.FindControl("ContentStart");
                                TextBox contentEnd = (TextBox)item.FindControl("ContentEnd");

                                attributesXML[attributeName + "_ContentStart"] = StringUtils.ValueToUrl(contentStart.Text);
                                attributesXML[attributeName + "_ContentEnd"] = StringUtils.ValueToUrl(contentEnd.Text);
                            }
                        }
                    }
                    gatherFileRuleInfo.ContentAttributesXML = TranslateUtils.NameValueCollectionToString(attributesXML);

                    DataProvider.GatherFileRuleDAO.Insert(gatherFileRuleInfo);
                }

                if (isNeedAdd)
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "添加单文件页采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "编辑单文件页采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }

				errorMessage = string.Empty;
				return true;
			}
			catch(Exception ex)
			{
				errorMessage = "操作失败！";
				return false;
			}
		}


		public void NextPanel(Object sender, EventArgs e)
		{
			string errorMessage;
			switch (CurrentWizardPanel)
			{
				case WizardPanel.GatherRuleBase:

					if (this.Validate_GatherRuleBase(out errorMessage))
					{
                        if (TranslateUtils.ToBool(this.IsToFile.SelectedValue))
                        {
                            if (this.Validate_InsertGatherRule(out errorMessage))
                            {
                                SetActivePanel(WizardPanel.Done, Done);
                            }
                            else
                            {
                                ErrorLabel.Text = errorMessage;
                                SetActivePanel(WizardPanel.OperatingError, OperatingError);
                            }
                        }
                        else
                        {
                            SetActivePanel(WizardPanel.GatherRuleContent, GatherRuleContent);
                        }
					}
					else
					{
                        base.FailMessage(errorMessage);
						SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
					}

					break;

				case WizardPanel.GatherRuleContent:

					if (this.Validate_GatherContent(out errorMessage))
					{
						if (this.Validate_InsertGatherRule(out errorMessage))
						{
							SetActivePanel(WizardPanel.Done, Done);
						}
						else
						{
							ErrorLabel.Text = errorMessage;
							SetActivePanel(WizardPanel.OperatingError, OperatingError);
						}
					}
					else
					{
                        base.FailMessage(errorMessage);
						SetActivePanel(WizardPanel.GatherRuleContent, GatherRuleContent);
					}

					break;

				case WizardPanel.Done:
					break;
			}
		}

		public void PreviousPanel(Object sender, EventArgs e)
		{
			switch (CurrentWizardPanel)
			{
				case WizardPanel.GatherRuleBase:
					break;

				case WizardPanel.GatherRuleContent:
                    SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
					break;
			}
		}

        public void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (TranslateUtils.ToBool(this.IsToFile.SelectedValue))
            {
                this.PlaceHolder_File.Visible = true;
                this.PlaceHolder_Content.Visible = false;
                this.PlaceHolder_File_Directory.Visible = TranslateUtils.ToBool(this.IsSaveRelatedFiles.SelectedValue);
            }
            else
            {
                this.PlaceHolder_File.Visible = false;
                this.PlaceHolder_Content.Visible = true;
            }
		}

        public void ContentAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList valueArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.ContentAttributes);
            if (base.Page.IsPostBack)
            {
                this.contentAttributesXML = new NameValueCollection();

                for (int i = 0; i < valueArrayList.Count; i++)
                {
                    string attributeName = valueArrayList[i] as string;

                    foreach (RepeaterItem item in this.ContentAttributesRepeater.Items)
                    {
                        if (item.ItemIndex == i)
                        {
                            TextBox contentStart = (TextBox) item.FindControl("ContentStart");
                            TextBox contentEnd = (TextBox) item.FindControl("ContentEnd");

                            this.contentAttributesXML[attributeName + "_ContentStart"] = StringUtils.ValueToUrl(contentStart.Text);
                            this.contentAttributesXML[attributeName + "_ContentEnd"] = StringUtils.ValueToUrl(contentEnd.Text);
                        }
                    }
                }
            }

            this.ContentAttributesRepeater.DataSource = valueArrayList;
            this.ContentAttributesRepeater.ItemDataBound += new RepeaterItemEventHandler(ContentAttributesRepeater_ItemDataBound);
            this.ContentAttributesRepeater.DataBind();
        }

        void ContentAttributesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string attributeName = e.Item.DataItem as string;

                string displayName = this.ContentAttributes.Items.FindByValue(attributeName).Text;

                NoTagText helpStart = (NoTagText)e.Item.FindControl("HelpStart") ;
                NoTagText helpEnd = (NoTagText)e.Item.FindControl("HelpEnd");
                TextBox contentStart = (TextBox) e.Item.FindControl("ContentStart");
                TextBox contentEnd = (TextBox) e.Item.FindControl("ContentEnd");

                helpStart.Text = displayName + "的开始字符串";
                helpEnd.Text = displayName + "的结束字符串";

                contentStart.Text = StringUtils.ValueFromUrl(this.contentAttributesXML[attributeName + "_ContentStart"]);
                contentEnd.Text = StringUtils.ValueFromUrl(this.contentAttributesXML[attributeName + "_ContentEnd"]);
            }
        }
	}
}
