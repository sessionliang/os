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
    public class BackgroundGatherRuleAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public PlaceHolder GatherRuleBase;
        public TextBox GatherRuleName;
        public DropDownList NodeIDDropDownList;
        public DropDownList Charset;
        public TextBox GatherNum;
        public RadioButtonList IsSaveImage;
        public RadioButtonList IsSetFirstImageAsImageUrl;
        public RadioButtonList IsEmptyContentAllowed;
        public RadioButtonList IsSameTitleAllowed;
        public RadioButtonList IsChecked;
        public RadioButtonList IsAutoCreate;
        public RadioButtonList IsOrderByDesc;

        public PlaceHolder GatherRuleList;
        public CheckBox GatherUrlIsCollection;
        public Control GatherUrlCollectionRow;
        public TextBox GatherUrlCollection;
        public CheckBox GatherUrlIsSerialize;
        public Control GatherUrlSerializeRow;
        public TextBox GatherUrlSerialize;
        public TextBox SerializeFrom;
        public TextBox SerializeTo;
        public TextBox SerializeInterval;
        public CheckBox SerializeIsOrderByDesc;
        public CheckBox SerializeIsAddZero;
        public TextBox UrlInclude;

        public PlaceHolder GatherRuleContent;
        public TextBox ContentTitleStart;
        public TextBox ContentTitleEnd;
        public TextBox ContentContentStart;
        public TextBox ContentContentEnd;
        public TextBox ContentContentStart2;
        public TextBox ContentContentEnd2;
        public TextBox ContentContentStart3;
        public TextBox ContentContentEnd3;
        public TextBox ContentNextPageStart;
        public TextBox ContentNextPageEnd;

        public PlaceHolder GatherRuleOthers;
        public TextBox TitleInclude;
        public TextBox ListAreaStart;
        public TextBox ListAreaEnd;
        public TextBox CookieString;
        public TextBox ContentExclude;
        public CheckBoxList ContentHtmlClearCollection;
        public CheckBoxList ContentHtmlClearTagCollection;
        public TextBox ContentReplaceFrom;
        public TextBox ContentReplaceTo;
        public TextBox ContentChannelStart;
        public TextBox ContentChannelEnd;
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
                string pageTitle = this.isEdit ? "编辑Web页面信息采集规则" : "添加Web页面信息采集规则";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, pageTitle, AppManager.CMS.Permission.WebSite.Gather);

                this.ltlPageTitle.Text = pageTitle;

                ECharsetUtils.AddListItems(this.Charset);
                ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(ECharset.gb2312));
                NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);

                SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);

                if (this.isEdit)
                {
                    GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.theGatherRuleName, base.PublishmentSystemID);
                    GatherRuleName.Text = gatherRuleInfo.GatherRuleName;

                    ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(gatherRuleInfo.Charset));
                    GatherNum.Text = gatherRuleInfo.Additional.GatherNum.ToString();
                    foreach (ListItem item in IsSaveImage.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsSaveImage.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in IsSetFirstImageAsImageUrl.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsSetFirstImageAsImageUrl.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in IsEmptyContentAllowed.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsEmptyContentAllowed.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in this.IsSameTitleAllowed.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsSameTitleAllowed.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in IsChecked.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsChecked.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in IsAutoCreate.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsAutoCreate.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    foreach (ListItem item in IsOrderByDesc.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.Additional.IsOrderByDesc.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    GatherUrlIsCollection.Checked = gatherRuleInfo.GatherUrlIsCollection;
                    GatherUrlCollection.Text = gatherRuleInfo.GatherUrlCollection;
                    GatherUrlIsSerialize.Checked = gatherRuleInfo.GatherUrlIsSerialize;
                    GatherUrlSerialize.Text = gatherRuleInfo.GatherUrlSerialize;
                    SerializeFrom.Text = gatherRuleInfo.SerializeFrom.ToString();
                    SerializeTo.Text = gatherRuleInfo.SerializeTo.ToString();
                    SerializeInterval.Text = gatherRuleInfo.SerializeInterval.ToString();
                    SerializeIsOrderByDesc.Checked = gatherRuleInfo.SerializeIsOrderByDesc;
                    SerializeIsAddZero.Checked = gatherRuleInfo.SerializeIsAddZero;

                    foreach (ListItem item in NodeIDDropDownList.Items)
                    {
                        if (item.Value.Equals(gatherRuleInfo.NodeID.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    UrlInclude.Text = gatherRuleInfo.UrlInclude;
                    TitleInclude.Text = gatherRuleInfo.TitleInclude;
                    ContentExclude.Text = gatherRuleInfo.ContentExclude;
                    ArrayList htmlClearArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.ContentHtmlClearCollection);
                    foreach (ListItem item in this.ContentHtmlClearCollection.Items)
                    {
                        item.Selected = htmlClearArrayList.Contains(item.Value);
                    }
                    ArrayList htmlClearTagArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.ContentHtmlClearTagCollection);
                    foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                    {
                        item.Selected = htmlClearTagArrayList.Contains(item.Value);
                    }
                    ListAreaStart.Text = gatherRuleInfo.ListAreaStart;
                    ListAreaEnd.Text = gatherRuleInfo.ListAreaEnd;
                    CookieString.Text = gatherRuleInfo.CookieString;
                    ContentTitleStart.Text = gatherRuleInfo.ContentTitleStart;
                    ContentTitleEnd.Text = gatherRuleInfo.ContentTitleEnd;
                    ContentContentStart.Text = gatherRuleInfo.ContentContentStart;
                    ContentContentEnd.Text = gatherRuleInfo.ContentContentEnd;
                    ContentContentStart2.Text = gatherRuleInfo.Additional.ContentContentStart2;
                    ContentContentEnd2.Text = gatherRuleInfo.Additional.ContentContentEnd2;
                    ContentContentStart3.Text = gatherRuleInfo.Additional.ContentContentStart3;
                    ContentContentEnd3.Text = gatherRuleInfo.Additional.ContentContentEnd3;
                    ContentReplaceFrom.Text = gatherRuleInfo.Additional.ContentReplaceFrom;
                    ContentReplaceTo.Text = gatherRuleInfo.Additional.ContentReplaceTo;
                    ContentChannelStart.Text = gatherRuleInfo.ContentChannelStart;
                    ContentChannelEnd.Text = gatherRuleInfo.ContentChannelEnd;
                    ContentNextPageStart.Text = gatherRuleInfo.ContentNextPageStart;
                    ContentNextPageEnd.Text = gatherRuleInfo.ContentNextPageEnd;

                    ArrayList contentAttributeArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.ContentAttributes);
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);
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

                    ListItem listItem = new ListItem("点击量", ContentAttribute.Hits.ToLower());
                    if (contentAttributeArrayList.Contains(listItem.Value))
                    {
                        listItem.Selected = true;
                    }
                    this.ContentAttributes.Items.Add(listItem);

                    this.contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherRuleInfo.ContentAttributesXML);

                    this.ContentAttributes_SelectedIndexChanged(null, EventArgs.Empty);

                }
                else
                {
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);
                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Title) || StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, BackgroundContentAttribute.Content)) continue;

                        ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName.ToLower());
                        this.ContentAttributes.Items.Add(listitem);
                    }

                    ListItem listItem = new ListItem("点击量", ContentAttribute.Hits.ToLower());
                    this.ContentAttributes.Items.Add(listItem);
                }

                this.GatherUrl_CheckedChanged(null, null);
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
            GatherRuleList,
            GatherRuleContent,
            GatherRuleOthers,
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
                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl(string.Format("background_gatherRule.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
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

            if (this.isEdit == false)
            {
                ArrayList gatherRuleNameList = DataProvider.GatherRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
                if (gatherRuleNameList.IndexOf(this.GatherRuleName.Text) != -1)
                {
                    errorMessage = "采集规则名称已存在！";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool Validate_GatherList(out string errorMessage)
        {
            if (!this.GatherUrlIsCollection.Checked && !this.GatherUrlIsSerialize.Checked)
            {
                errorMessage = "必须填写起始网页地址！";
                return false;
            }

            if (GatherUrlIsCollection.Checked)
            {
                if (string.IsNullOrEmpty(this.GatherUrlCollection.Text))
                {
                    errorMessage = "必须填写起始网页地址！";
                    return false;
                }
            }

            if (GatherUrlIsSerialize.Checked)
            {
                if (string.IsNullOrEmpty(this.GatherUrlSerialize.Text))
                {
                    errorMessage = "必须填写起始网页地址！";
                    return false;
                }

                if (this.GatherUrlSerialize.Text.IndexOf("*") == -1)
                {
                    errorMessage = "起始网页地址必须带有 * 字符！";
                    return false;
                }

                if (string.IsNullOrEmpty(this.SerializeFrom.Text) || string.IsNullOrEmpty(this.SerializeTo.Text))
                {
                    errorMessage = "必须填写变动数字范围！";
                    return false;
                }
                else
                {
                    if (TranslateUtils.ToInt(this.SerializeFrom.Text) < 0 || TranslateUtils.ToInt(this.SerializeTo.Text) < 0)
                    {
                        errorMessage = "变动数字范围必须大于等于0！";
                        return false;
                    }
                    if (TranslateUtils.ToInt(this.SerializeTo.Text) <= TranslateUtils.ToInt(this.SerializeFrom.Text))
                    {
                        errorMessage = "变动数字范围结束必须大于开始！";
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(this.SerializeInterval.Text))
                {
                    errorMessage = "必须填写数字变动倍数！";
                    return false;
                }
                else
                {
                    if (TranslateUtils.ToInt(this.SerializeInterval.Text) <= 0)
                    {
                        errorMessage = "数字变动倍数必须大于等于1！";
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(this.UrlInclude.Text))
            {
                errorMessage = "必须填写内容地址包含字符串！";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool Validate_GatherContent(out string errorMessage)
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
                        DataProvider.GatherRuleDAO.Delete(this.theGatherRuleName, base.PublishmentSystemID);
                    }
                    else
                    {
                        GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.theGatherRuleName, base.PublishmentSystemID);
                        if (NodeIDDropDownList.SelectedValue != null)
                        {
                            gatherRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                        }
                        gatherRuleInfo.Charset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);
                        gatherRuleInfo.Additional.GatherNum = int.Parse(GatherNum.Text);
                        gatherRuleInfo.Additional.IsSaveImage = TranslateUtils.ToBool(IsSaveImage.SelectedValue);
                        gatherRuleInfo.Additional.IsSetFirstImageAsImageUrl = TranslateUtils.ToBool(IsSetFirstImageAsImageUrl.SelectedValue);
                        gatherRuleInfo.Additional.IsEmptyContentAllowed = TranslateUtils.ToBool(IsEmptyContentAllowed.SelectedValue);
                        gatherRuleInfo.Additional.IsSameTitleAllowed = TranslateUtils.ToBool(IsSameTitleAllowed.SelectedValue);
                        gatherRuleInfo.Additional.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                        gatherRuleInfo.Additional.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);
                        gatherRuleInfo.Additional.IsOrderByDesc = TranslateUtils.ToBool(IsOrderByDesc.SelectedValue);

                        gatherRuleInfo.GatherUrlIsCollection = this.GatherUrlIsCollection.Checked;
                        gatherRuleInfo.GatherUrlCollection = GatherUrlCollection.Text;
                        gatherRuleInfo.GatherUrlIsSerialize = this.GatherUrlIsSerialize.Checked;
                        gatherRuleInfo.GatherUrlSerialize = GatherUrlSerialize.Text;
                        gatherRuleInfo.SerializeFrom = TranslateUtils.ToInt(SerializeFrom.Text);
                        gatherRuleInfo.SerializeTo = TranslateUtils.ToInt(SerializeTo.Text);
                        gatherRuleInfo.SerializeInterval = TranslateUtils.ToInt(SerializeInterval.Text);
                        gatherRuleInfo.SerializeIsOrderByDesc = this.SerializeIsOrderByDesc.Checked;
                        gatherRuleInfo.SerializeIsAddZero = this.SerializeIsAddZero.Checked;

                        gatherRuleInfo.UrlInclude = UrlInclude.Text;
                        gatherRuleInfo.TitleInclude = TitleInclude.Text;
                        gatherRuleInfo.ContentExclude = ContentExclude.Text;

                        ArrayList htmlClearArrayList = new ArrayList();
                        foreach (ListItem item in this.ContentHtmlClearCollection.Items)
                        {
                            if (item.Selected) htmlClearArrayList.Add(item.Value);
                        }
                        gatherRuleInfo.ContentHtmlClearCollection = TranslateUtils.ObjectCollectionToString(htmlClearArrayList);

                        ArrayList htmlClearTagArrayList = new ArrayList();
                        foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                        {
                            if (item.Selected) htmlClearTagArrayList.Add(item.Value);
                        }
                        gatherRuleInfo.ContentHtmlClearTagCollection = TranslateUtils.ObjectCollectionToString(htmlClearTagArrayList);

                        gatherRuleInfo.ListAreaStart = ListAreaStart.Text;
                        gatherRuleInfo.ListAreaEnd = ListAreaEnd.Text;
                        gatherRuleInfo.CookieString = CookieString.Text;
                        gatherRuleInfo.ContentTitleStart = ContentTitleStart.Text;
                        gatherRuleInfo.ContentTitleEnd = ContentTitleEnd.Text;
                        gatherRuleInfo.ContentContentStart = ContentContentStart.Text;
                        gatherRuleInfo.ContentContentEnd = ContentContentEnd.Text;
                        gatherRuleInfo.Additional.ContentContentStart2 = ContentContentStart2.Text;
                        gatherRuleInfo.Additional.ContentContentEnd2 = ContentContentEnd2.Text;
                        gatherRuleInfo.Additional.ContentContentStart3 = ContentContentStart3.Text;
                        gatherRuleInfo.Additional.ContentContentEnd3 = ContentContentEnd3.Text;
                        gatherRuleInfo.Additional.ContentReplaceFrom = ContentReplaceFrom.Text;
                        gatherRuleInfo.Additional.ContentReplaceTo = ContentReplaceTo.Text;
                        gatherRuleInfo.ContentChannelStart = ContentChannelStart.Text;
                        gatherRuleInfo.ContentChannelEnd = ContentChannelEnd.Text;
                        gatherRuleInfo.ContentNextPageStart = ContentNextPageStart.Text;
                        gatherRuleInfo.ContentNextPageEnd = ContentNextPageEnd.Text;

                        ArrayList valueArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.ContentAttributes);
                        gatherRuleInfo.ContentAttributes = TranslateUtils.ObjectCollectionToString(valueArrayList);
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

                                    //采集为空时的默认值
                                    TextBox contentDefault = (TextBox)item.FindControl("ContentDefault");

                                    attributesXML[attributeName + "_ContentStart"] = StringUtils.ValueToUrl(contentStart.Text);
                                    attributesXML[attributeName + "_ContentEnd"] = StringUtils.ValueToUrl(contentEnd.Text);


                                    //采集为空时的默认值
                                    attributesXML[attributeName + "_ContentDefault"] = StringUtils.ValueToUrl(contentDefault.Text);
                                }
                            }
                        }
                        gatherRuleInfo.ContentAttributesXML = TranslateUtils.NameValueCollectionToString(attributesXML);

                        DataProvider.GatherRuleDAO.Update(gatherRuleInfo);
                    }
                }
                else
                {
                    isNeedAdd = true;
                }

                if (isNeedAdd)
                {
                    GatherRuleInfo gatherRuleInfo = new GatherRuleInfo();
                    gatherRuleInfo.GatherRuleName = GatherRuleName.Text;
                    gatherRuleInfo.PublishmentSystemID = base.PublishmentSystemID;
                    if (NodeIDDropDownList.SelectedValue != null)
                    {
                        gatherRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                    }
                    gatherRuleInfo.Charset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);
                    gatherRuleInfo.Additional.GatherNum = int.Parse(GatherNum.Text);
                    gatherRuleInfo.Additional.IsSaveImage = TranslateUtils.ToBool(IsSaveImage.SelectedValue);
                    gatherRuleInfo.Additional.IsSetFirstImageAsImageUrl = TranslateUtils.ToBool(IsSetFirstImageAsImageUrl.SelectedValue);
                    gatherRuleInfo.Additional.IsEmptyContentAllowed = TranslateUtils.ToBool(IsEmptyContentAllowed.SelectedValue);
                    gatherRuleInfo.Additional.IsSameTitleAllowed = TranslateUtils.ToBool(IsSameTitleAllowed.SelectedValue);
                    gatherRuleInfo.Additional.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                    gatherRuleInfo.Additional.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);
                    gatherRuleInfo.Additional.IsOrderByDesc = TranslateUtils.ToBool(IsOrderByDesc.SelectedValue);

                    gatherRuleInfo.GatherUrlIsCollection = this.GatherUrlIsCollection.Checked;
                    gatherRuleInfo.GatherUrlCollection = GatherUrlCollection.Text;
                    gatherRuleInfo.GatherUrlIsSerialize = this.GatherUrlIsSerialize.Checked;
                    gatherRuleInfo.GatherUrlSerialize = GatherUrlSerialize.Text;
                    gatherRuleInfo.SerializeFrom = TranslateUtils.ToInt(SerializeFrom.Text);
                    gatherRuleInfo.SerializeTo = TranslateUtils.ToInt(SerializeTo.Text);
                    gatherRuleInfo.SerializeInterval = TranslateUtils.ToInt(SerializeInterval.Text);
                    gatherRuleInfo.SerializeIsOrderByDesc = this.SerializeIsOrderByDesc.Checked;
                    gatherRuleInfo.SerializeIsAddZero = this.SerializeIsAddZero.Checked;

                    gatherRuleInfo.UrlInclude = UrlInclude.Text;
                    gatherRuleInfo.TitleInclude = TitleInclude.Text;
                    gatherRuleInfo.ContentExclude = ContentExclude.Text;

                    ArrayList htmlClearArrayList = new ArrayList();
                    foreach (ListItem item in this.ContentHtmlClearCollection.Items)
                    {
                        if (item.Selected) htmlClearArrayList.Add(item.Value);
                    }
                    gatherRuleInfo.ContentHtmlClearCollection = TranslateUtils.ObjectCollectionToString(htmlClearArrayList);

                    ArrayList htmlClearTagArrayList = new ArrayList();
                    foreach (ListItem item in this.ContentHtmlClearTagCollection.Items)
                    {
                        if (item.Selected) htmlClearTagArrayList.Add(item.Value);
                    }
                    gatherRuleInfo.ContentHtmlClearTagCollection = TranslateUtils.ObjectCollectionToString(htmlClearTagArrayList);

                    gatherRuleInfo.ListAreaStart = ListAreaStart.Text;
                    gatherRuleInfo.ListAreaEnd = ListAreaEnd.Text;
                    gatherRuleInfo.CookieString = CookieString.Text;
                    gatherRuleInfo.ContentTitleStart = ContentTitleStart.Text;
                    gatherRuleInfo.ContentTitleEnd = ContentTitleEnd.Text;
                    gatherRuleInfo.ContentContentStart = ContentContentStart.Text;
                    gatherRuleInfo.ContentContentEnd = ContentContentEnd.Text;
                    gatherRuleInfo.Additional.ContentContentStart2 = ContentContentStart2.Text;
                    gatherRuleInfo.Additional.ContentContentEnd2 = ContentContentEnd2.Text;
                    gatherRuleInfo.Additional.ContentContentStart3 = ContentContentStart3.Text;
                    gatherRuleInfo.Additional.ContentContentEnd3 = ContentContentEnd3.Text;
                    gatherRuleInfo.Additional.ContentReplaceFrom = ContentReplaceFrom.Text;
                    gatherRuleInfo.Additional.ContentReplaceTo = ContentReplaceTo.Text;
                    gatherRuleInfo.ContentChannelStart = ContentChannelStart.Text;
                    gatherRuleInfo.ContentChannelEnd = ContentChannelEnd.Text;
                    gatherRuleInfo.ContentNextPageStart = ContentNextPageStart.Text;
                    gatherRuleInfo.ContentNextPageEnd = ContentNextPageEnd.Text;
                    gatherRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                    ArrayList valueArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.ContentAttributes);
                    gatherRuleInfo.ContentAttributes = TranslateUtils.ObjectCollectionToString(valueArrayList);
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

                                //采集为空时的默认值
                                TextBox contentDefault = (TextBox)item.FindControl("ContentDefault");

                                attributesXML[attributeName + "_ContentStart"] = StringUtils.ValueToUrl(contentStart.Text);
                                attributesXML[attributeName + "_ContentEnd"] = StringUtils.ValueToUrl(contentEnd.Text);

                                //采集为空时的默认值
                                attributesXML[attributeName + "_ContentDefault"] = StringUtils.ValueToUrl(contentDefault.Text);
                            }
                        }
                    }
                    gatherRuleInfo.ContentAttributesXML = TranslateUtils.NameValueCollectionToString(attributesXML);

                    DataProvider.GatherRuleDAO.Insert(gatherRuleInfo);
                }

                if (isNeedAdd)
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "添加Web页面采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "编辑Web页面采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }

                errorMessage = string.Empty;
                return true;
            }
            catch
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
                        SetActivePanel(WizardPanel.GatherRuleList, GatherRuleList);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
                    }

                    break;

                case WizardPanel.GatherRuleList:

                    if (this.Validate_GatherList(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.GatherRuleContent, GatherRuleContent);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherRuleList, GatherRuleList);
                    }

                    break;

                case WizardPanel.GatherRuleContent:

                    if (this.Validate_GatherContent(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.GatherRuleOthers, GatherRuleOthers);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherRuleContent, GatherRuleContent);
                    }

                    break;

                case WizardPanel.GatherRuleOthers:

                    if (this.Validate_InsertGatherRule(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.Done, Done);
                    }
                    else
                    {
                        ErrorLabel.Text = errorMessage;
                        SetActivePanel(WizardPanel.OperatingError, OperatingError);
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

                case WizardPanel.GatherRuleList:
                    SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
                    break;

                case WizardPanel.GatherRuleContent:
                    SetActivePanel(WizardPanel.GatherRuleList, GatherRuleList);
                    break;

                case WizardPanel.GatherRuleOthers:
                    SetActivePanel(WizardPanel.GatherRuleContent, GatherRuleContent);
                    break;
            }
        }

        public void GatherUrl_CheckedChanged(object sender, EventArgs e)
        {
            this.GatherUrlCollectionRow.Visible = false;
            this.GatherUrlSerializeRow.Visible = false;

            if (this.GatherUrlIsCollection.Checked)
            {
                this.GatherUrlCollectionRow.Visible = true;
            }

            if (this.GatherUrlIsSerialize.Checked)
            {
                this.GatherUrlSerializeRow.Visible = true;
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
                        Literal ltlAttributeName = (Literal)item.FindControl("ltlAttributeName");
                        if (ltlAttributeName.Text == attributeName)
                        {
                            TextBox contentStart = (TextBox)item.FindControl("ContentStart");
                            TextBox contentEnd = (TextBox)item.FindControl("ContentEnd");

                            //采集为空时的默认值
                            TextBox contentDefault = (TextBox)item.FindControl("ContentDefault");

                            this.contentAttributesXML[attributeName + "_ContentStart"] = StringUtils.ValueToUrl(contentStart.Text);
                            this.contentAttributesXML[attributeName + "_ContentEnd"] = StringUtils.ValueToUrl(contentEnd.Text);

                            //采集为空时的默认值
                            this.contentAttributesXML[attributeName + "_ContentDefault"] = StringUtils.ValueToUrl(contentDefault.Text);
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

                Literal ltlAttributeName = (Literal)e.Item.FindControl("ltlAttributeName");
                NoTagText helpStart = (NoTagText)e.Item.FindControl("HelpStart");
                NoTagText helpEnd = (NoTagText)e.Item.FindControl("HelpEnd");
                TextBox contentStart = (TextBox)e.Item.FindControl("ContentStart");
                TextBox contentEnd = (TextBox)e.Item.FindControl("ContentEnd");

                //采集为空时的默认值
                NoTagText helpDefault = (NoTagText)e.Item.FindControl("HelpDefault");
                TextBox contentDefault = (TextBox)e.Item.FindControl("ContentDefault");

                ltlAttributeName.Text = attributeName;

                helpStart.Text = displayName + "的开始字符串";
                helpEnd.Text = displayName + "的结束字符串";

                //采集为空时的默认值
                helpDefault.Text = displayName + "为空时的默认值";
                contentDefault.Text = StringUtils.ValueFromUrl(this.contentAttributesXML[attributeName + "_ContentDefault"]);

                contentStart.Text = StringUtils.ValueFromUrl(this.contentAttributesXML[attributeName + "_ContentStart"]);
                contentEnd.Text = StringUtils.ValueFromUrl(this.contentAttributesXML[attributeName + "_ContentEnd"]);
            }
        }
    }
}
