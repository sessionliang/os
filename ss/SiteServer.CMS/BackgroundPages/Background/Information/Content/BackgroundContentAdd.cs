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
using BaiRong.Core.Net;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentAdd : BackgroundBasePage
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
        public PlaceHolder phTranslate;
        public HtmlControl divTranslateAdd;
        public DropDownList ddlTranslateType;
        public PlaceHolder phStatus;

        public Button Submit;

        private NodeInfo nodeInfo;
        private ArrayList relatedIdentities;
        private string returnUrl;
        private bool isAjaxSubmit;
        private ETableStyle tableStyle;
        private string tableName;

        protected override bool IsSinglePage { get { return true; } }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl");

            int nodeID = base.GetIntQueryString("NodeID");
            int contentID = base.GetIntQueryString("ID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.isAjaxSubmit = TranslateUtils.ToBool(base.GetQueryString("isAjaxSubmit"));

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

                    this.ltlPageTitle.Text = pageTitle;
                    this.ltlPageTitle.Text += string.Format(@"
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

                    //转移
                    if (AdminUtility.HasChannelPermissions(base.PublishmentSystemID, this.nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
                    {
                        this.phTranslate.Visible = base.PublishmentSystemInfo.Additional.IsTranslate;
                        this.divTranslateAdd.Attributes.Add("onclick", Modal.ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, true));

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
                                else
                                {
                                    if (TranslateUtils.ToBool(styleInfo.DefaultValue))
                                    {
                                        listItem.Selected = true;
                                    }
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
                        if (!string.IsNullOrEmpty(base.GetQueryStringNoSqlAndXss("contentLevel")))
                        {
                            checkedLevel = TranslateUtils.ToIntWithNagetive(base.GetQueryString("contentLevel"));
                            if (checkedLevel != LevelManager.LevelInt.NotChange)
                            {
                                if (checkedLevel >= base.PublishmentSystemInfo.CheckContentLevel)
                                {
                                    isChecked = true;
                                }
                                else
                                {
                                    isChecked = false;
                                }
                            }
                        }

                        LevelManager.LoadContentLevelToEdit(this.ContentLevel, base.PublishmentSystemInfo, nodeID, contentInfo, isChecked, checkedLevel);
                    }
                    else
                    {
                        phStatus.Visible = false;
                    }

                    this.Submit.Attributes.Add("onclick", InputParserUtils.GetValidateSubmitOnClickScript("myForm", true, "autoCheckKeywords()"));
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
            int contentID = 0;
            if (isAjaxSubmit)
                contentID = TranslateUtils.ToInt(base.Request.Form["savedContentID"]);
            else
                contentID = base.GetIntQueryString("ID");
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

                    //自动保存的时候，不保存编辑器的图片
                    InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes, !this.isAjaxSubmit);

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

                    //判断是不是有审核权限
                    int checkedLevelOfUser = 0;
                    bool isCheckedOfUser = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, contentInfo.NodeID, out checkedLevelOfUser);
                    if (LevelManager.IsCheckable(base.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isCheckedOfUser, checkedLevelOfUser))
                    {
                        //添加审核记录
                        BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, base.PublishmentSystemID, contentInfo.NodeID, new ArrayList() { savedContentID }, 0, true, AdminManager.Current.UserName, contentInfo.IsChecked, contentInfo.CheckedLevel, "");
                    }

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

                #region 更新内容搜索联想词结果数
                //UpdateSearchWordResult();
                #endregion

                if (!isAjaxSubmit)
                {
                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentInfo.ID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, "添加内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                    if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Photo))
                    {
                        PageUtils.Redirect(BackgroundContentPhotoUpload.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, base.GetQueryString("ReturnUrl")));
                    }
                    else if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Teleplay))
                    {
                        PageUtils.Redirect(BackgroundContentTeleplayUpload.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, base.GetQueryString("ReturnUrl")));
                    }
                    else
                    {
                        PageUtils.Redirect(BackgroundContentAddAfter.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, this.returnUrl));
                    }
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

                    //自动保存的时候，不保存编辑器的图片
                    InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes, !this.isAjaxSubmit);

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

                    #region 更新内容搜索联想词结果数
                    //UpdateSearchWordResult();
                    #endregion

                    if (!isAjaxSubmit)
                    {
                        ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                        if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Photo))
                        {
                            PageUtils.Redirect(BackgroundContentPhotoUpload.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, base.GetQueryString("ReturnUrl")));
                        }
                        else if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Teleplay))
                        {
                            PageUtils.Redirect(BackgroundContentTeleplayUpload.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, base.GetQueryString("ReturnUrl")));
                        }

                        #region 更新引用该内容的信息
                        //如果不是异步自动保存，那么需要将引用此内容的content修改
                        ArrayList sourceContentIDList = new ArrayList();
                        sourceContentIDList.Add(contentInfo.ID);
                        ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent, EAuxiliaryTableType.JobContent, EAuxiliaryTableType.VoteContent);
                        foreach (AuxiliaryTableInfo table in tableArrayList)
                        {
                            ArrayList targetContentIDList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(table.TableENName, sourceContentIDList);
                            foreach (int targetContentID in targetContentIDList)
                            {
                                ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyleUtils.GetEnumType(table.AuxiliaryTableType.ToString()), table.TableENName, targetContentID);
                                if (targetContentInfo != null && targetContentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) == ETranslateContentType.ReferenceContent.ToString())
                                {
                                    contentInfo.ID = targetContentID;
                                    contentInfo.PublishmentSystemID = targetContentInfo.PublishmentSystemID;
                                    contentInfo.NodeID = targetContentInfo.NodeID;
                                    contentInfo.SourceID = targetContentInfo.SourceID;
                                    contentInfo.ReferenceID = targetContentInfo.ReferenceID;
                                    contentInfo.SetExtendedAttribute(ContentAttribute.TranslateContentType, targetContentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType));
                                    BaiRongDataProvider.ContentDAO.Update(table.TableENName, contentInfo);


                                    #region 资源转移
                                    //资源：图片，文件，视频
                                    PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetContentInfo.PublishmentSystemID);
                                    BackgroundContentInfo bgContentInfo = contentInfo as BackgroundContentInfo;
                                    BackgroundContentInfo bgTargetContentInfo = targetContentInfo as BackgroundContentInfo;
                                    if (bgContentInfo.ImageUrl != bgTargetContentInfo.ImageUrl)
                                    {
                                        //修改图片
                                        string sourceImageUrl = PathUtility.MapPath(this.PublishmentSystemInfo, bgContentInfo.ImageUrl);
                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl);

                                    }
                                    else if (bgContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl)) != bgTargetContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl)))
                                    {
                                        ArrayList sourceImageUrls = TranslateUtils.StringCollectionToArrayList(bgContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl)));

                                        foreach (string imageUrl in sourceImageUrls)
                                        {
                                            string sourceImageUrl = PathUtility.MapPath(this.PublishmentSystemInfo, imageUrl);
                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl);
                                        }
                                    }
                                    if (bgContentInfo.FileUrl != bgTargetContentInfo.FileUrl)
                                    {
                                        //修改附件
                                        string sourceFileUrl = PathUtility.MapPath(this.PublishmentSystemInfo, bgContentInfo.FileUrl);
                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl);

                                    }
                                    else if (bgContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl)) != bgTargetContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl)))
                                    {
                                        ArrayList sourceFileUrls = TranslateUtils.StringCollectionToArrayList(bgContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl)));

                                        foreach (string FileUrl in sourceFileUrls)
                                        {
                                            string sourceFileUrl = PathUtility.MapPath(this.PublishmentSystemInfo, FileUrl);
                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion

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

                    PageUtils.Redirect(this.returnUrl);
                }
                savedContentID = contentID;
            }

            return savedContentID;
        }

        private void UpdateSearchWordResult()
        {
            try
            {
                string cacheKey = string.Format("{0}_{1}", SearchwordInfo.CACHE_UPDATE_TIME, base.PublishmentSystemID);
                bool update = false;
                if (CacheUtils.Get(cacheKey) == null)
                {
                    update = true;
                }
                else if ((DateTime.Now - TranslateUtils.ToDateTime(CacheUtils.Get(cacheKey).ToString(), DateTime.Now)).TotalMinutes >= 12 * 60)
                {
                    update = true;
                }

                if (update)
                {
                    new Action(() =>
                    {
                        //更新结果数
                        DataProvider.SearchwordDAO.UpdateSearchResultCountAll(base.PublishmentSystemID);
                        //设置更新时间
                        CacheUtils.Max(cacheKey, DateTime.Now.ToString());
                    }).BeginInvoke(null, null);
                }
            }
            catch { }
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
        private void CopyReferenceFiles(PublishmentSystemInfo targetPublishmentSystemInfo, string sourceUrl)
        {
            string targetUrl = StringUtils.ReplaceFirst(this.PublishmentSystemInfo.PublishmentSystemDir, sourceUrl, targetPublishmentSystemInfo.PublishmentSystemDir);
            if (!FileUtils.IsFileExists(targetUrl))
            {
                FileUtils.CopyFile(sourceUrl, targetUrl, true);
            }
        }
        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
