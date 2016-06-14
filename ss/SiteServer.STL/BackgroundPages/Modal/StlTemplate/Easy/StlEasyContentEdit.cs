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
using System.Web;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlEasyContentEdit : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlAutoSave;
        public AuxiliaryControl acAttributes;
        public PlaceHolder phContentAttributes;
        public CheckBoxList ContentAttributes;
        public PlaceHolder phContentGroup;
        public CheckBoxList ContentGroupNameCollection;
        public RadioButtonList ContentLevel;
        public PlaceHolder phTags;
        public TextBox Tags;
        public Literal ltlTags;
        public PlaceHolder phStatus;

        public Button Submit;

        private NodeInfo nodeInfo;
        private ArrayList relatedIdentities;
        private bool isAjaxSubmit;
        private ETableStyle tableStyle;
        private string tableName;

        protected override bool IsSinglePage { get { return true; } }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ID", contentID.ToString());

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("modal_stlEasyContentEdit.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");

            int nodeID = int.Parse(Request.QueryString["NodeID"]);
            int contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.isAjaxSubmit = TranslateUtils.ToBool(Request.QueryString["isAjaxSubmit"]);

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            ContentInfo contentInfo = null;

            if (this.isAjaxSubmit == false)
            {
                if (contentID == 0)
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
                    contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);
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
                    string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeInfo.NodeID);
                    string pageTitle = (contentID == 0) ? string.Format("添加{0}", ContentModelManager.GetContentModelInfo(base.PublishmentSystemInfo, nodeInfo.ContentModelID).ModelName) : string.Format("编辑{0}", ContentModelManager.GetContentModelInfo(base.PublishmentSystemInfo, nodeInfo.ContentModelID).ModelName);
                    base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Content, pageTitle, nodeNames, string.Empty);

                    this.ltlPageTitle.Text = string.Format(@"
<script language=""javascript"" type=""text/javascript"">
function submitPreview(){{
    var var1 = myForm.action;
    var var2 = myForm.target;
    myForm.action = ""{0}"";
    myForm.target = ""preview"";
    if (UE){{
        $.each(UE.instants,function(index,editor){{
            editor.sync();
        }});
    }}
    myForm.submit();
    myForm.action = var1;
    myForm.target = var2;
}}
</script>
", PageUtility.GetContentPreviewUrl(base.PublishmentSystemInfo, this.nodeInfo.NodeID, contentID));

                    if (base.PublishmentSystemInfo.Additional.IsAutoSaveContent && base.PublishmentSystemInfo.Additional.AutoSaveContentInterval > 0)
                    {
                        this.ltlPageTitle.Text += string.Format(@"
<input type=""hidden"" id=""savedContentID"" name=""savedContentID"" value=""{0}"">
<script language=""javascript"" type=""text/javascript"">setInterval(""autoSave()"",{1});</script>
", contentID, base.PublishmentSystemInfo.Additional.AutoSaveContentInterval * 1000);
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
                        foreach (string attributeName in excludeAttributeNames)
                        {
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, attributeName, this.relatedIdentities);
                            if (styleInfo.IsVisible)
                            {
                                ListItem listItem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                                if (contentID > 0)
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
                            if (contentID > 0)
                            {
                                item.Selected = StringUtils.In(contentInfo.ContentGroupNameCollection, groupName);
                            }
                            this.ContentGroupNameCollection.Items.Add(item);
                        }
                    }

                    //标签
                    if (!base.PublishmentSystemInfo.Additional.IsRelatedByTags)
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

                    if (contentID == 0)
                    {
                        NameValueCollection formCollection = new NameValueCollection();
                        if (!string.IsNullOrEmpty(base.GetQueryString("isUploadWord")))
                        {
                            bool isFirstLineTitle = TranslateUtils.ToBool(base.GetQueryString("isFirstLineTitle"));
                            bool isFirstLineRemove = TranslateUtils.ToBool(base.GetQueryString("isFirstLineRemove"));
                            bool isClearFormat = TranslateUtils.ToBool(base.GetQueryString("isClearFormat"));
                            bool isFirstLineIndent = TranslateUtils.ToBool(base.GetQueryString("isFirstLineIndent"));
                            bool isClearFontSize = TranslateUtils.ToBool(base.GetQueryString("isClearFontSize"));
                            bool isClearFontFamily = TranslateUtils.ToBool(base.GetQueryString("isClearFontFamily"));
                            bool isClearImages = TranslateUtils.ToBool(base.GetQueryString("isClearImages"));
                            int contentLevel = TranslateUtils.ToInt(base.GetQueryString("contentLevel"));
                            string fileName = base.GetQueryString("fileName");

                            formCollection = WordUtils.GetWordNameValueCollection(base.PublishmentSystemID, this.nodeInfo.ContentModelID, isFirstLineTitle, isFirstLineRemove, isClearFormat, isFirstLineIndent, isClearFontSize, isClearFontFamily, isClearImages, contentLevel, fileName);
                        }

                        this.acAttributes.SetParameters(formCollection, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                    }
                    else
                    {
                        this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                        this.Tags.Text = contentInfo.Tags;
                    }

                    if (base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                    {
                        phStatus.Visible = true;

                        int checkedLevel = 0;
                        bool isChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, this.nodeInfo.NodeID, out checkedLevel);
                        LevelManager.LoadContentLevelToEdit(this.ContentLevel, base.PublishmentSystemInfo, nodeID, contentInfo, isChecked, checkedLevel);
                    }
                    else
                    {
                        phStatus.Visible = false;
                    }

                    this.Submit.Attributes.Add("onclick", InputParserUtils.GetValidateSubmitOnClickScript("myForm"));
                }
                else
                {
                    if (contentID == 0)
                    {
                        this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                    }
                    else
                    {
                        this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                    }
                }
                base.DataBind();
            }
            else
            {
                bool success = false;
                string errorMessage = string.Empty;
                int savedContentID = this.SaveContentInfo(true, out errorMessage);
                if (savedContentID > 0)
                {
                    success = true;
                }

                string jsonString = string.Format(@"{{success:'{0}',savedContentID:'{1}'}}", success.ToString().ToLower(), savedContentID);

                PageUtils.ResponseToJSON(jsonString);
            }
        }

        private int SaveContentInfo(bool isAjaxSubmit, out string errorMessage)
        {
            int savedContentID = 0;
            errorMessage = string.Empty;

            int contentID = TranslateUtils.ToInt(base.Request.Form["savedContentID"]);
            if (contentID == 0)
            {
                ContentInfo contentInfo = ContentUtility.GetContentInfo(this.tableStyle);
                try
                {
                    contentInfo.NodeID = this.nodeInfo.NodeID;
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                    if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                    {
                        errorMessage = string.Format("内容添加失败：系统时间不能为{0}年", DateUtils.SqlMinValue.Year);
                        return 0;
                    }
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;

                    InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                    StringCollection tagCollection = null;

                    if (isAjaxSubmit)
                    {
                        contentInfo.ContentGroupNameCollection = base.Request.Form[ContentAttribute.ContentGroupNameCollection];
                        tagCollection = TagUtils.ParseTagsString(base.Request.Form[ContentAttribute.Tags]);

                        contentInfo.CheckedLevel = LevelManager.LevelInt.CaoGao;
                        contentInfo.IsChecked = false;
                    }
                    else
                    {
                        contentInfo.ContentGroupNameCollection = ControlUtils.SelectedItemsValueToStringCollection(this.ContentGroupNameCollection.Items);
                        tagCollection = TagUtils.ParseTagsString(this.Tags.Text);

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
                    }
                    contentInfo.Tags = TranslateUtils.ObjectCollectionToString(tagCollection, " ");

                    savedContentID = DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);
                    
                    if (this.phTags.Visible)
                    {
                        TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, savedContentID);
                    }

                    contentInfo.ID = savedContentID;
                }
                catch (Exception ex)
                {
                    LogUtils.AddErrorLog(ex);
                    errorMessage = string.Format("内容添加失败：{0}", ex.Message);
                    return 0;
                }

                if (!isAjaxSubmit)
                {
                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentInfo.ID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, "添加内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));
                }
            }
            else
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);
                try
                {
                    string tagsLast = contentInfo.Tags;

                    contentInfo.LastEditUserName = AdminManager.Current.UserName;
                    contentInfo.LastEditDate = DateTime.Now;

                    InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                    StringCollection tagCollection = null;
                    if (isAjaxSubmit)
                    {
                        contentInfo.ContentGroupNameCollection = base.Request.Form[ContentAttribute.ContentGroupNameCollection];
                        tagCollection = TagUtils.ParseTagsString(base.Request.Form[ContentAttribute.Tags]);
                    }
                    else
                    {
                        contentInfo.ContentGroupNameCollection = ControlUtils.SelectedItemsValueToStringCollection(this.ContentGroupNameCollection.Items);
                        tagCollection = TagUtils.ParseTagsString(this.Tags.Text);

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
                    }
                    contentInfo.Tags = TranslateUtils.ObjectCollectionToString(tagCollection, " ");

                    DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    if (this.phTags.Visible)
                    {
                        TagUtils.UpdateTags(tagsLast, contentInfo.Tags, tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, contentID);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.AddErrorLog(ex);
                    errorMessage = string.Format("内容修改失败：{0}", ex.Message);
                    return 0;
                }

                if (!isAjaxSubmit)
                {
                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, "修改内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    JsUtils.Layer.CloseModalLayer(base.Page);
                }

                savedContentID = contentID;
            }

            return savedContentID;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string errorMessage = string.Empty;
                int savedContentID = this.SaveContentInfo(false, out errorMessage);
                if (savedContentID == 0)
                {
                    base.FailMessage(errorMessage);
                }
            }
        }
    }
}
