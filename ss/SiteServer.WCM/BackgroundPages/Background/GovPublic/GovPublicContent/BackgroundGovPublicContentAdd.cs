using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Core.Office;


using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicContentAdd : BackgroundGovPublicBasePage
    {
        public Literal ltlPageTitle;

        public AuxiliarySingleControl ascTitle;
        public TextBox tbIdentifier;
        public TextBox tbPublisher;
        public TextBox tbDocumentNo;
        public DateTimeTextBox dtbPublishDate;
        public TextBox tbKeywords;
        public DateTimeTextBox dtbEffectDate;
        public TextBox tbDescription;
        public HtmlControl divAddChannel;
        public HtmlControl divAddDepartment;
        public Literal ltlCategoryScript;

        public AuxiliaryControl acAttributes;
        public PlaceHolder phContentAttributes;
        public CheckBoxList ContentAttributes;
        public PlaceHolder phContentGroup;
        public CheckBoxList ContentGroupNameCollection;
        public RadioButtonList ContentLevel;
        public PlaceHolder phTags;
        public TextBox Tags;
        public Literal ltlTags;
        public PlaceHolder phTranslate;
        public HtmlControl divTranslateAdd;
        public DropDownList ddlTranslateType;
        public PlaceHolder phStatus;
        public Button Submit;

        private int contentID;
        private string returnUrl;

        private NodeInfo nodeInfo;
        private string tableName;
        private ETableStyle tableStyle;
        private ArrayList relatedIdentities;

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int nodeID = TranslateUtils.ToInt(Request.QueryString["NodeID"]);
            if (nodeID == 0)
            {
                nodeID = base.PublishmentSystemInfo.Additional.GovPublicNodeID;
            }
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            GovPublicContentInfo contentInfo = null;

            if (this.contentID == 0)
            {
                if (nodeInfo != null && nodeInfo.Additional.IsContentAddable == false)
                {
                    PageUtils.RedirectToErrorPage("此栏目不能添加内容！");
                    return;
                }

                if (!base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentAdd))
                {
                    if (!BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                    {
                        PageUtils.RedirectToLoginPage();
                        return;
                    }
                    else
                    {
                        PageUtils.RedirectToErrorPage("您无此栏目的添加内容权限！");
                        return;
                    }
                }
            }
            else
            {
                contentInfo = DataProvider.GovPublicContentDAO.GetContentInfo(base.PublishmentSystemInfo, this.contentID);
                if (!base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                {
                    if (!BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                    {
                        PageUtils.RedirectToLoginPage();
                        return;
                    }
                    else
                    {
                        PageUtils.RedirectToErrorPage("您无此栏目的修改内容权限！");
                        return;
                    }
                }
            }

            if (!IsPostBack)
            {
                string nodeNames = NodeManager.GetNodeNameNavigationByGovPublic(base.PublishmentSystemID, nodeID);
                int departmentID = 0;
                string departmentName = string.Empty;

                string pageTitle = (contentID == 0) ? "添加信息" : "修改信息";
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContent, pageTitle, nodeNames, AppManager.CMS.Permission.WebSite.GovPublicContent);

                this.ltlPageTitle.Text = pageTitle;
                this.ltlPageTitle.Text += string.Format(@"
<script>
function submitPreview(){{
    var var1 = myForm.action;
    var var2 = myForm.target;
    myForm.action = ""{0}"";
    myForm.target = ""preview"";
    if (UE && UE.getEditor('Content')){{ UE.getEditor('Content').sync(); }}
    myForm.submit();
    myForm.action = var1;
    myForm.target = var2;
}}
</script>
", PageUtility.GetContentPreviewUrl(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.contentID));

                //转移
                if (tableStyle == ETableStyle.BackgroundContent && AdminUtility.HasChannelPermissions(base.PublishmentSystemID, this.nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
                {
                    this.phTranslate.Visible = base.PublishmentSystemInfo.Additional.IsTranslate;
                    this.divTranslateAdd.Attributes.Add("onclick", ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, true));

                    ETranslateContentTypeUtils.AddListItems(this.ddlTranslateType, true);
                    ControlUtils.SelectListItems(this.ddlTranslateType, ETranslateContentTypeUtils.GetValue(ETranslateContentType.Copy));
                }
                else
                {
                    this.phTranslate.Visible = false;
                }

                //内容属性
                ArrayList excludeAttributeNames = TableManager.GetExcludeAttributeNames(this.tableStyle);
                this.acAttributes.AddExcludeAttributeNames(excludeAttributeNames);

                if (excludeAttributeNames.Count == 0)
                {
                    this.phContentAttributes.Visible = false;
                }
                else
                {
                    this.phContentAttributes.Visible = true;
                    foreach (string attributeName in GovPublicContentAttribute.CheckBoxAttributes)
                    {
                        TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, attributeName, this.relatedIdentities);
                        if (styleInfo.IsVisible)
                        {
                            ListItem listItem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                            if (this.contentID > 0)
                            {
                                listItem.Selected = TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(styleInfo.AttributeName));
                            }
                            this.ContentAttributes.Items.Add(listItem);
                        }
                    }
                }

                //内容组
                ArrayList contentGroupNameArrayList = DataProvider.ContentGroupDAO.GetContentGroupNameArrayList(base.PublishmentSystemID);

                if (!base.PublishmentSystemInfo.Additional.IsGroupContent || contentGroupNameArrayList.Count == 0)
                {
                    this.phContentGroup.Visible = false;
                }
                else
                {
                    foreach (string groupName in contentGroupNameArrayList)
                    {
                        ListItem item = new ListItem(groupName, groupName);
                        if (this.contentID > 0)
                        {
                            item.Selected = StringUtils.In(contentInfo.ContentGroupNameCollection, groupName);
                        }
                        this.ContentGroupNameCollection.Items.Add(item);
                    }
                }

                if (!base.PublishmentSystemInfo.Additional.IsRelatedByTags || tableStyle != ETableStyle.BackgroundContent)
                {
                    this.phTags.Visible = false;
                }
                else
                {
                    string tagScript = @"
<script type=""text/javascript"">
function getTags(tag){
	$.get('[url]&tag=' + encodeURIComponent(tag) + '&r=' + Math.random(), function(data) {
		if(data !=''){
			var arr = data.split('|');
			var temp='';
			for(i=0;i<arr.length;i++)
			{
				temp += '<li><a>'+arr[i].replace(tag,'<b>' + tag + '</b>') + '</a></li>';
			}
			var myli='<ul>'+temp+'</ul>';
			$('#tagTips').html(myli);
			$('#tagTips').show();
		}else{
            $('#tagTips').hide();
        }
		$('#tagTips li').click(function () {
			var tag = $('#Tags').val();
			var i = tag.lastIndexOf(' ');
			if (i > 0)
			{
				tag = tag.substring(0, i) + ' ' + $(this).text();
			}else{
				tag = $(this).text();	
			}
			$('#Tags').val(tag);
			$('#tagTips').hide();
		})
	});	
}
$(document).ready(function () {
$('#Tags').keyup(function (e) {
    if (e.keyCode != 40 && e.keyCode != 38) {
        var tag = $('#Tags').val();
		var i = tag.lastIndexOf(' ');
		if (i > 0){ tag = tag.substring(i + 1);}
        if (tag != '' && tag != ' '){
            window.setTimeout(""getTags('"" + tag + ""');"", 200);
        }else{
            $('#tagTips').hide();
        }
    }
}).blur(function () {
	window.setTimeout(""$('#tagTips').hide();"", 200);
})});
</script>
<div id=""tagTips"" class=""inputTips""></div>
";
                    this.ltlTags.Text = tagScript.Replace("[url]", BackgroundService.GetTagsUrl(base.PublishmentSystemID));
                }

                this.divAddChannel.Attributes.Add("onclick", Modal.GovPublicCategoryChannelSelect.GetOpenWindowString(base.PublishmentSystemID));
                this.divAddDepartment.Attributes.Add("onclick", Modal.GovPublicCategoryDepartmentSelect.GetOpenWindowString(base.PublishmentSystemID));

                StringBuilder categoryBuilder = new StringBuilder();
                
                ArrayList categoryClassInfoArrayList = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfoArrayList(base.PublishmentSystemID, ETriState.False, ETriState.True);
                if (categoryClassInfoArrayList.Count > 0)
                {
                    int categoryIndex = 1;
                    foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                    {
                        categoryIndex++;
                        if (categoryIndex % 2 == 0)
                        {
                            categoryBuilder.Append("<tr>");
                        }
                        categoryBuilder.AppendFormat(@"<td height=""30"">{0}分类：</td><td height=""30"">
<div class=""fill_box"" id=""category{1}Container"" style=""display:none"">
                  <div class=""addr_base addr_normal""> <b id=""category{1}Name""></b> <a class=""addr_del"" href=""javascript:;"" onClick=""showCategory{1}('', '0')""></a>
                    <input id=""category{1}ID"" name=""category{1}ID"" value=""0"" type=""hidden"">
                  </div>
                </div>
                <div ID=""divAdd{1}"" class=""btn_pencil"" onclick=""{2}""><span class=""pencil""></span>　修改</div>
                <script language=""javascript"">
			  function showCategory{1}({1}Name, {1}ID){{
				  $('#category{1}Name').html({1}Name);
				  $('#category{1}ID').val({1}ID);
				  if ({1}ID == '0'){{
					$('#category{1}Container').hide();
				  }}else{{
					  $('#category{1}Container').show();
				  }}
			  }}
			  </script>
</td>", categoryClassInfo.ClassName, categoryClassInfo.ClassCode, Modal.GovPublicCategorySelect.GetOpenWindowString(base.PublishmentSystemID, categoryClassInfo.ClassCode));
                        if (categoryIndex % 2 == 1)
                        {
                            categoryBuilder.Append("</tr>");
                        }
                    }
                }

                if (contentID == 0)
                {
                    NameValueCollection formCollection = new NameValueCollection();
                    if (!string.IsNullOrEmpty(base.Request.QueryString["isUploadWord"]))
                    {
                        bool isFirstLineTitle = TranslateUtils.ToBool(base.Request.QueryString["isFirstLineTitle"]);
                        bool isFirstLineRemove = TranslateUtils.ToBool(base.Request.QueryString["isFirstLineRemove"]);
                        bool isClearFormat = TranslateUtils.ToBool(base.Request.QueryString["isClearFormat"]);
                        bool isFirstLineIndent = TranslateUtils.ToBool(base.Request.QueryString["isFirstLineIndent"]);
                        bool isClearFontSize = TranslateUtils.ToBool(base.Request.QueryString["isClearFontSize"]);
                        bool isClearFontFamily = TranslateUtils.ToBool(base.Request.QueryString["isClearFontFamily"]);
                        bool isClearImages = TranslateUtils.ToBool(base.Request.QueryString["isClearImages"]);
                        int contentLevel = TranslateUtils.ToInt(base.Request.QueryString["contentLevel"]);
                        string fileName = base.Request.QueryString["fileName"];

                        formCollection = WordUtils.GetWordNameValueCollection(base.PublishmentSystemID, this.nodeInfo.ContentModelID, isFirstLineTitle, isFirstLineRemove, isClearFormat, isFirstLineIndent, isClearFontSize, isClearFontFamily, isClearImages, contentLevel, fileName);
                    }

                    this.ascTitle.SetParameters(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.tableStyle, this.tableName, ContentAttribute.Title, formCollection, false, base.IsPostBack);
                    this.acAttributes.SetParameters(formCollection, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                }
                //else
                //{
                    
                //    this.Tags.Text = contentInfo.Tags;
                //}

                //if (contentID == 0)
                //{
                //    NameValueCollection formCollection = new NameValueCollection();
                //    this.ascContent.SetParameters(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.GovPublicNodeID, ETableStyle.GovPublicContent, base.PublishmentSystemInfo.AuxiliaryTableForGovPublic, GovPublicContentAttribute.Content, formCollection, false, base.IsPostBack);
                //}
                else
                {
                    departmentID = contentInfo.DepartmentID;
                    departmentName = DepartmentManager.GetDepartmentName(departmentID);

                    foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                    {
                        int categoryID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(categoryClassInfo.ContentAttributeName));
                        if (categoryID > 0)
                        {
                            string categoryName = DataProvider.GovPublicCategoryDAO.GetCategoryName(categoryID);
                            categoryBuilder.AppendFormat(@"<script>showCategory{0}('{1}', '{2}');</script>", categoryClassInfo.ClassCode, categoryName, categoryID);
                        }
                    }

                    this.tbIdentifier.Text = contentInfo.Identifier;
                    this.tbPublisher.Text = contentInfo.Publisher;
                    this.tbDocumentNo.Text = contentInfo.DocumentNo;
                    this.dtbPublishDate.DateTime = contentInfo.PublishDate;
                    this.tbKeywords.Text = contentInfo.Keywords;
                    this.dtbEffectDate.DateTime = contentInfo.EffectDate;
                    this.tbDescription.Text = contentInfo.Description;

                    this.ascTitle.SetParameters(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.tableStyle, this.tableName, ContentAttribute.Title, contentInfo.Attributes, true, base.IsPostBack);

                    this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                    this.Tags.Text = contentInfo.Tags;
                }

                if (departmentID == 0)
                {
                    departmentID = BaiRongDataProvider.AdministratorDAO.GetDepartmentID(AdminManager.Current.UserName);
                    if (departmentID > 0)
                    {
                        departmentName = DepartmentManager.GetDepartmentName(departmentID);
                    }
                }
                
                categoryBuilder.AppendFormat(@"<script>showCategoryChannel('{0}', '{1}');showCategoryDepartment('{2}', '{3}');</script>", nodeNames, nodeID, departmentName, departmentID);

                this.ltlCategoryScript.Text = categoryBuilder.ToString();

                if (base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                {
                    phStatus.Visible = true;

                    int checkedLevel = 0;
                    bool isChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, base.PublishmentSystemID, out checkedLevel);
                    LevelManager.LoadContentLevelToEdit(this.ContentLevel, base.PublishmentSystemInfo, nodeID, contentInfo, isChecked, checkedLevel);
                }
                else
                {
                    phStatus.Visible = false;
                }

                this.Submit.Attributes.Add("onclick", InputParserUtils.GetValidateSubmitOnClickScript("myForm"));

                if (base.PublishmentSystemInfo.Additional.GovPublicIsPublisherRelatedDepartmentID && string.IsNullOrEmpty(this.tbPublisher.Text))
                {
                    this.tbPublisher.Text = departmentName;
                }
            }
            else
            {
                if (contentID == 0)
                {
                    this.ascTitle.SetParameters(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.tableStyle, this.tableName, ContentAttribute.Title, base.Request.Form, false, base.IsPostBack);

                    this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                }
                else
                {
                    this.ascTitle.SetParameters(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.tableStyle, this.tableName, ContentAttribute.Title, base.Request.Form, true, base.IsPostBack);

                    this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                }
                //this.ascContent.SetParameters(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.GovPublicNodeID, ETableStyle.GovPublicContent, base.PublishmentSystemInfo.AuxiliaryTableForGovPublic, GovPublicContentAttribute.Content, base.Request.Form, true, base.IsPostBack);
            }
            base.DataBind();
        }

        public bool IsPublisherRelatedDepartmentID
        {
            get
            {
                return base.PublishmentSystemInfo.Additional.GovPublicIsPublisherRelatedDepartmentID;
            }
        }

        public static string GetRedirectUrlOfAdd(int publishmentSystemID, int nodeID, string returnUrl)
        {
            return PageUtils.GetWCMUrl(string.Format("background_govPublicContentAdd.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemID, nodeID, StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetRedirectUrlOfEdit(int publishmentSystemID, int nodeID, int id, string returnUrl)
        {
            return PageUtils.GetWCMUrl(string.Format("background_govPublicContentAdd.aspx?PublishmentSystemID={0}&NodeID={1}&ID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, id, StringUtils.ValueToUrl(returnUrl)));
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int categoryChannelID = TranslateUtils.ToInt(base.Request["categoryChannelID"]);
                int categoryDepartmentID = TranslateUtils.ToInt(base.Request["categoryDepartmentID"]);
                if (categoryChannelID == 0)
                {
                    base.FailMessage("信息采集失败，必须要选择一个主题分类");
                    return;
                }

                if (categoryDepartmentID == 0)
                {
                    base.FailMessage("信息采集失败，必须要选择一个机构分类");
                    return;
                }
                
                ArrayList categoryClassInfoArrayList = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfoArrayList(base.PublishmentSystemID, ETriState.False, ETriState.True);

                if (this.contentID == 0)
                {
                    GovPublicContentInfo contentInfo = new GovPublicContentInfo();
                    try
                    {
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                        contentInfo.NodeID = categoryChannelID;
                        contentInfo.Description = this.tbDescription.Text;
                        contentInfo.PublishDate = this.dtbPublishDate.DateTime;
                        contentInfo.EffectDate = this.dtbEffectDate.DateTime;
                        contentInfo.IsAbolition = false;
                        contentInfo.AbolitionDate = DateTime.Now;
                        contentInfo.DocumentNo = this.tbDocumentNo.Text;
                        contentInfo.Publisher = this.tbPublisher.Text;
                        contentInfo.Keywords = this.tbKeywords.Text;

                        contentInfo.DepartmentID = categoryDepartmentID;
                        this.SetCategoryAttributes(contentInfo, categoryClassInfoArrayList);
                        contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                        contentInfo.AddUserName = AdminManager.Current.UserName;
                        if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                        {
                            base.FailMessage(string.Format("内容添加失败：系统时间不能为{0}年", DateUtils.SqlMinValue.Year));
                            return;
                        }
                        contentInfo.LastEditUserName = contentInfo.AddUserName;
                        contentInfo.LastEditDate = DateTime.Now;

                        if (this.phContentAttributes.Visible)
                        {
                            foreach (ListItem listItem in this.ContentAttributes.Items)
                            {
                                string value = listItem.Selected.ToString();
                                string attributeName = listItem.Value;
                                contentInfo.SetExtendedAttribute(attributeName, value);
                            }
                        }

                        contentInfo.CheckedLevel = TranslateUtils.ToIntWithNagetive(this.ContentLevel.SelectedValue);
                        contentInfo.IsChecked = contentInfo.CheckedLevel >= base.PublishmentSystemInfo.CheckContentLevel;

                        contentInfo.ContentGroupNameCollection = ControlUtils.SelectedItemsValueToStringCollection(this.ContentGroupNameCollection.Items);
                        StringCollection tagCollection = TagUtils.ParseTagsString(this.Tags.Text);
                        contentInfo.Tags = TranslateUtils.ObjectCollectionToString(tagCollection, " ");

                        contentInfo.Identifier = GovPublicManager.GetIdentifier(base.PublishmentSystemInfo, categoryChannelID, categoryDepartmentID, contentInfo);
                        int contentID = DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                        //更新分类内容数
                        foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                        {
                            int categoryID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(categoryClassInfo.ContentAttributeName));
                            if (categoryID > 0)
                            {
                                DataProvider.GovPublicCategoryDAO.UpdateContentNum(base.PublishmentSystemInfo, categoryClassInfo.ContentAttributeName, categoryID);
                            }
                        }

                        if (contentInfo.IsChecked)
                        {
                            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentInfo.ID, 0);
                            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                        }

                        contentInfo.ID = contentID;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容添加失败：{0}", ex.Message));
                        LogUtils.AddErrorLog(ex);
                        return;
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, categoryChannelID, contentInfo.ID, "添加内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                    PageUtils.Redirect(PageUtils.GetWCMUrl(string.Format("background_govPublicContentAddAfter.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", base.PublishmentSystemID, categoryChannelID, contentInfo.ID, Request.QueryString["ReturnUrl"])));
                }
                else
                {
                    GovPublicContentInfo contentInfo = DataProvider.GovPublicContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    try
                    {
                        string identifier = contentInfo.Identifier;
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                        contentInfo.DepartmentID = categoryDepartmentID;
                        this.SetCategoryAttributes(contentInfo, categoryClassInfoArrayList);

                        contentInfo.Description = this.tbDescription.Text;
                        contentInfo.PublishDate = this.dtbPublishDate.DateTime;
                        contentInfo.EffectDate = this.dtbEffectDate.DateTime;
                        contentInfo.IsAbolition = false;
                        contentInfo.AbolitionDate = DateTime.Now;
                        contentInfo.DocumentNo = this.tbDocumentNo.Text;
                        contentInfo.Publisher = this.tbPublisher.Text;
                        contentInfo.Keywords = this.tbKeywords.Text;
                        
                        NameValueCollection contentAttributeNameWithCategoryID = this.SetCategoryAttributes(contentInfo, categoryClassInfoArrayList);
                        contentInfo.LastEditUserName = AdminManager.Current.UserName;
                        contentInfo.LastEditDate = DateTime.Now;

                        contentInfo.ContentGroupNameCollection = ControlUtils.SelectedItemsValueToStringCollection(this.ContentGroupNameCollection.Items);
                        string tagsLast = contentInfo.Tags;
                        StringCollection tagCollection = TagUtils.ParseTagsString(this.Tags.Text);
                        contentInfo.Tags = TranslateUtils.ObjectCollectionToString(tagCollection, " ");

                        if (this.phContentAttributes.Visible)
                        {
                            foreach (ListItem listItem in this.ContentAttributes.Items)
                            {
                                string value = listItem.Selected.ToString();
                                string attributeName = listItem.Value;
                                contentInfo.SetExtendedAttribute(attributeName, value);
                            }
                        }

                        int checkedLevel = TranslateUtils.ToIntWithNagetive(this.ContentLevel.SelectedValue);
                        if (checkedLevel != LevelManager.LevelInt.NotChange)
                        {
                            if (checkedLevel >= base.PublishmentSystemInfo.CheckContentLevel)
                            {
                                contentInfo.IsChecked = true;
                            }
                            else
                            {
                                contentInfo.IsChecked = false;
                            }
                            contentInfo.CheckedLevel = checkedLevel;
                        }

                        if (string.IsNullOrEmpty(identifier))
                        {
                            identifier = GovPublicManager.GetIdentifier(base.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.DepartmentID, contentInfo);
                        }
                        else if (GovPublicManager.IsIdentifierChanged(categoryChannelID, categoryDepartmentID, this.dtbEffectDate.DateTime, contentInfo))
                        {
                            identifier = GovPublicManager.GetIdentifier(base.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.DepartmentID, contentInfo);
                        }
                        contentInfo.Identifier = identifier;

                        DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                        if (this.phTags.Visible)
                        {
                            TagUtils.UpdateTags(tagsLast, contentInfo.Tags, tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, this.contentID);
                        }

                        ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                        //更新分类内容数
                        foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                        {
                            if (!string.IsNullOrEmpty(contentAttributeNameWithCategoryID[categoryClassInfo.ContentAttributeName]))
                            {
                                int oldCategoryID = TranslateUtils.ToInt(contentAttributeNameWithCategoryID[categoryClassInfo.ContentAttributeName]);
                                int newCategoryID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(categoryClassInfo.ContentAttributeName));

                                if (oldCategoryID > 0)
                                {
                                    DataProvider.GovPublicCategoryDAO.UpdateContentNum(base.PublishmentSystemInfo, categoryClassInfo.ContentAttributeName, oldCategoryID);
                                }
                                if (newCategoryID > 0)
                                {
                                    DataProvider.GovPublicCategoryDAO.UpdateContentNum(base.PublishmentSystemInfo, categoryClassInfo.ContentAttributeName, newCategoryID);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        LogUtils.AddErrorLog(ex);
                        return;
                    }

                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, categoryChannelID, this.contentID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, categoryChannelID, this.contentID, "修改内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    PageUtils.Redirect(this.returnUrl);
                }
            }
        }

        private NameValueCollection SetCategoryAttributes(GovPublicContentInfo contentInfo, ArrayList categoryClassInfoArrayList)
        {
            NameValueCollection contentAttributeNameWithCategoryID = new NameValueCollection();
            foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
            {
                int oldCategoryID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(categoryClassInfo.ContentAttributeName));
                int newCategoryID = TranslateUtils.ToInt(base.Request[string.Format("category{0}ID", categoryClassInfo.ClassCode)]);
                if (oldCategoryID != newCategoryID)
                {
                    contentAttributeNameWithCategoryID.Add(categoryClassInfo.ContentAttributeName, oldCategoryID.ToString());
                    contentInfo.SetExtendedAttribute(categoryClassInfo.ContentAttributeName, newCategoryID.ToString());
                }
            }
            return contentAttributeNameWithCategoryID;
        }

        public string ReturnUrl { get { return this.returnUrl; } }

    }
}
