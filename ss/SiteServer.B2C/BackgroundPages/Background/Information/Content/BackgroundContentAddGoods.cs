using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.Core.Office;
using System.Collections.Generic;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundContentAddGoods : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlAutoSave;
        public AuxiliaryControl acAttributes;

        /// <summary>
        /// 品牌模型
        /// </summary>
        public PlaceHolder phBrand;
        public DropDownList BrandNodeID;
        public DropDownList BrandContentID;

        public PlaceHolder phBrandFilter;
        /// <summary>
        /// filter品牌
        /// </summary>
        public DropDownList BrandContent;

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
            GoodsContentInfo contentInfo = null;

            if (this.isAjaxSubmit == false)
            {
                if (contentID == 0)
                {
                    if (nodeInfo != null && nodeInfo.Additional.IsContentAddable == false)
                    {
                        PageUtils.RedirectToErrorPage("此栏目不能添加商品！");
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
                            PageUtils.RedirectToErrorPage("您无此栏目的添加商品权限！");
                            return;
                        }
                    }
                }
                else
                {
                    contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(this.tableName, contentID);

                    if (!base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                    {
                        if (!BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                        {
                            PageUtils.RedirectToLoginPage();
                            return;
                        }
                        else
                        {
                            PageUtils.RedirectToErrorPage("您无此栏目的修改商品权限！");
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

                    #region B2C
                    this.phBrandFilter.Visible = false;
                    this.phBrand.Visible = false;
                    int specifiedBrandNodeID = 0;
                    if (contentInfo != null)//如果父级栏目的filter中，存在自定义品牌，那么加载自定义品牌key-value
                    {
                        
                        if (!this.nodeInfo.Additional.IsBrandSpecified)
                        {
                            this.phBrandFilter.Visible = this.LoadBrand(this.nodeInfo.NodeID, contentInfo.BrandValue);
                        }
                        else if (this.nodeInfo.Additional.IsBrandSpecified && this.nodeInfo.Additional.BrandNodeID > 0)
                        {
                            NodeInfo brandNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeInfo.Additional.BrandNodeID);
                            if (brandNodeInfo != null && EContentModelTypeUtils.Equals(EContentModelType.Brand, brandNodeInfo.ContentModelID))
                            {
                                specifiedBrandNodeID = this.nodeInfo.Additional.BrandNodeID;
                            }
                            this.phBrand.Visible = this.LoadBrand(specifiedBrandNodeID, contentInfo.BrandID);
                        }
                    }
                    else
                    {
                        if (!this.nodeInfo.Additional.IsBrandSpecified)
                        {
                            this.phBrandFilter.Visible = this.LoadBrand(this.nodeInfo.NodeID, string.Empty);

                        }
                        else
                        {
                            NodeInfo brandNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeInfo.Additional.BrandNodeID);
                            if (brandNodeInfo != null && EContentModelTypeUtils.Equals(EContentModelType.Brand, brandNodeInfo.ContentModelID))
                            {
                                specifiedBrandNodeID = this.nodeInfo.Additional.BrandNodeID;
                            }
                            this.phBrand.Visible = this.LoadBrand(specifiedBrandNodeID, 0);
                        }
                    }

                    #endregion

                    //转移
                    if (AdminUtility.HasChannelPermissions(base.PublishmentSystemID, this.nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
                    {
                        this.phTranslate.Visible = base.PublishmentSystemInfo.Additional.IsTranslate;
                        this.divTranslateAdd.Attributes.Add("onclick", SiteServer.CMS.BackgroundPages.Modal.ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, true));

                        ETranslateContentTypeUtils.AddListItems(this.ddlTranslateType, true);
                        ControlUtils.SelectListItems(this.ddlTranslateType, ETranslateContentTypeUtils.GetValue(ETranslateContentType.Copy));
                    }
                    else
                    {
                        this.phTranslate.Visible = false;
                    }

                    //内容属性
                    ArrayList excludeAttributeNames = new ArrayList(TableManager.GetExcludeAttributeNames(this.tableStyle));
                    if (B2CConfigurationManager.GetInstance(this.nodeInfo.NodeID).IsVirtualGoods)
                    {
                        excludeAttributeNames.Add(GoodsContentAttribute.Stock.ToLower());
                    }
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
                            if (styleInfo.IsVisible && !StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GoodsContentAttribute.Stock))
                            {
                                ListItem listItem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                                if (contentID > 0)
                                {
                                    listItem.Selected = TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(styleInfo.AttributeName));
                                }
                                else
                                {
                                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GoodsContentAttribute.IsOnSale))
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
                        formCollection[GoodsContentAttribute.SN] = GoodsManager.GetGoodsSN();
                        if (!string.IsNullOrEmpty(base.GetQueryString("isUploadWord")))
                        {
                            bool isFirstLineTitle = base.GetBoolQueryString("isFirstLineTitle");
                            bool isFirstLineRemove = base.GetBoolQueryString("isFirstLineRemove");
                            bool isClearFormat = base.GetBoolQueryString("isClearFormat");
                            bool isFirstLineIndent = base.GetBoolQueryString("isFirstLineIndent");
                            bool isClearFontSize = base.GetBoolQueryString("isClearFontSize");
                            bool isClearFontFamily = base.GetBoolQueryString("isClearFontFamily");
                            bool isClearImages = base.GetBoolQueryString("isClearImages");
                            int contentLevel = base.GetIntQueryString("contentLevel");
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

        #region B2C

        private bool LoadBrand(int parentNodeID, int brandID)
        {
            this.BrandNodeID.Items.Add(new ListItem("<<请选择>>", string.Empty));
            this.BrandContentID.Items.Add(new ListItem("<<请选择>>", string.Empty));

            bool isBrandExists = NodeManager.AddListItemsForBrand(this.BrandNodeID.Items, base.PublishmentSystemInfo, true, parentNodeID);
            if (this.nodeInfo.Additional.IsBrandSpecified)
            {
                ControlUtils.SelectListItems(this.BrandNodeID, nodeInfo.Additional.BrandNodeID.ToString());
            }

            this.BrandNodeID.Attributes.Add("onchange", "getBrands()");
            this.phBrand.Visible = isBrandExists;

            if (isBrandExists && brandID > 0)
            {
                int brandNodeID = DataProviderB2C.BrandContentDAO.GetNodeID(base.PublishmentSystemID, brandID);
                if (brandNodeID > 0)
                {
                    ControlUtils.SelectListItems(this.BrandNodeID, brandNodeID.ToString());
                    ListItemCollection listItemCollection = DataProviderB2C.BrandContentDAO.GetListItemCollection(base.PublishmentSystemID, brandNodeID, false);
                    foreach (ListItem listItem in listItemCollection)
                    {
                        if (listItem.Value == brandID.ToString())
                        {
                            listItem.Selected = true;
                        }
                        this.BrandContentID.Items.Add(listItem);
                    }
                }
            }
            return isBrandExists;
        }

        private bool LoadBrand(int parentNodeID, string brandValue)
        {
            this.BrandContent.Items.Add(new ListItem("<<请选择>>", string.Empty));
            List<FilterInfo> filterInfoList = DataProviderB2C.FilterDAO.GetFilterInfoList(PublishmentSystemID, parentNodeID);
            bool isExistsBrandFilter = false;
            foreach (FilterInfo filterInfo in filterInfoList)
            {
                if (StringUtils.EqualsIgnoreCase(filterInfo.AttributeName, GoodsContentAttribute.BrandID))
                {
                    if (!filterInfo.IsDefaultValues)
                        isExistsBrandFilter = true;
                    List<FilterItemInfo> filterItemInfoList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);
                    foreach (FilterItemInfo filterItem in filterItemInfoList)
                    {
                        if (StringUtils.EqualsIgnoreCase(brandValue, filterItem.Value))
                        {
                            ListItem selectedItem = new ListItem(filterItem.Title, filterItem.Value);
                            selectedItem.Selected = true;
                            this.BrandContent.Items.Add(selectedItem);
                        }
                        else
                        {
                            this.BrandContent.Items.Add(new ListItem(filterItem.Title, filterItem.Value));
                        }
                    }
                    break;
                }
            }
            this.phBrandFilter.Visible = isExistsBrandFilter;
            return isExistsBrandFilter;
        }

        #endregion

        private int SaveContentInfo(bool isAjaxSubmit, out string errorMessage)
        {
            int savedContentID = 0;
            errorMessage = string.Empty;

            int contentID = TranslateUtils.ToInt(base.Request.Form["savedContentID"]);
            if (contentID == 0)
            {
                GoodsContentInfo contentInfo = new GoodsContentInfo();
                try
                {
                    contentInfo.NodeID = this.nodeInfo.NodeID;
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                    if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                    {
                        errorMessage = string.Format("商品添加失败：系统时间不能为{0}年", DateUtils.SqlMinValue.Year);
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

                    contentInfo.BrandID = TranslateUtils.ToInt(base.Request.Form["BrandContentID"]);
                    contentInfo.BrandValue = this.BrandContent.SelectedValue;

                    if (string.IsNullOrEmpty(contentInfo.SN))
                    {
                        contentInfo.SN = GoodsManager.GetGoodsSN();
                    }

                    SpecManager.SetContentInfoToDefaultSpec(contentInfo);

                    savedContentID = DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    SpecManager.AddDefaultSpecComboListToContent(this.nodeInfo, savedContentID);

                    if (this.phTags.Visible)
                    {
                        TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, savedContentID);
                    }

                    contentInfo.ID = savedContentID;
                }
                catch (Exception ex)
                {
                    LogUtils.AddErrorLog(ex);
                    errorMessage = string.Format("商品添加失败：{0}", ex.Message);
                    return 0;
                }

                if (!isAjaxSubmit)
                {
                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentInfo.ID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, "添加商品", string.Format("栏目:{0},商品标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                    if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Photo))
                    {
                        PageUtils.Redirect(PageUtils.GetB2CUrl(string.Format("background_contentPhotoUpload.aspx?PublishmentSystemID={0}&ContentID={1}&ReturnUrl={2}", base.PublishmentSystemID, contentInfo.ID, base.GetQueryString("ReturnUrl"))));
                    }
                    else
                    {
                        PageUtils.Redirect(BackgroundContentAddAfter.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, contentInfo.ID, this.returnUrl));
                    }
                }
            }
            else
            {
                GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(this.tableName, contentID);

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

                    contentInfo.BrandID = TranslateUtils.ToInt(base.Request.Form["BrandContentID"]);
                    contentInfo.BrandValue = this.BrandContent.SelectedValue;
                    if (string.IsNullOrEmpty(contentInfo.SN))
                    {
                        contentInfo.SN = GoodsManager.GetGoodsSN();
                    }

                    DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    if (this.phTags.Visible)
                    {
                        TagUtils.UpdateTags(tagsLast, contentInfo.Tags, tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, contentID);
                    }

                    if (!isAjaxSubmit)
                    {
                        ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", this.nodeInfo.NodeID, contentInfo.ID), base.Request.Form["translateCollection"], ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue));

                        if (EContentModelTypeUtils.Equals(this.nodeInfo.ContentModelID, EContentModelType.Photo))
                        {
                            PageUtils.Redirect(PageUtils.GetB2CUrl(string.Format("background_contentPhotoUpload.aspx?PublishmentSystemID={0}&ContentID={1}&ReturnUrl={2}", base.PublishmentSystemID, contentInfo.ID, base.GetQueryString("ReturnUrl"))));
                        }

                        #region 更新引用该商品内容的信息
                        //如果不是异步自动保存，那么需要将引用此内容的content修改
                        ArrayList sourceContentIDList = new ArrayList();
                        sourceContentIDList.Add(contentInfo.ID);
                        ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
                        foreach (AuxiliaryTableInfo table in tableArrayList)
                        {
                            ArrayList targetContentIDList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(table.TableENName, sourceContentIDList);
                            foreach (int targetContentID in targetContentIDList)
                            {
                                GoodsContentInfo targetContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(table.TableENName, targetContentID);//DataProvider.ContentDAO.GetContentInfo(ETableStyleUtils.GetEnumType(table.AuxiliaryTableType.ToString()), table.TableENName, targetContentID);
                                if (targetContentInfo != null && targetContentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) == ETranslateContentType.ReferenceContent.ToString())
                                {
                                    PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetContentInfo.PublishmentSystemID);
                                    contentInfo.ID = targetContentID;
                                    contentInfo.PublishmentSystemID = targetContentInfo.PublishmentSystemID;
                                    contentInfo.NodeID = targetContentInfo.NodeID;
                                    contentInfo.SourceID = targetContentInfo.SourceID;
                                    contentInfo.ReferenceID = targetContentInfo.ReferenceID;
                                    contentInfo.SetExtendedAttribute(ContentAttribute.TranslateContentType, targetContentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType));
                                    BaiRongDataProvider.ContentDAO.Update(table.TableENName, contentInfo);

                                    #region 资源转移
                                    //资源：图片，文件，视频
                                    if (contentInfo.ImageUrl != targetContentInfo.ImageUrl)
                                    {
                                        //修改图片
                                        string sourceImageUrl = PathUtility.MapPath(this.PublishmentSystemInfo, contentInfo.ImageUrl);
                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl);

                                    }
                                    else if (contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.ImageUrl)) != targetContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.ImageUrl)))
                                    {
                                        ArrayList sourceImageUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.ImageUrl)));

                                        foreach (string imageUrl in sourceImageUrls)
                                        {
                                            string sourceImageUrl = PathUtility.MapPath(this.PublishmentSystemInfo, imageUrl);
                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl);
                                        }
                                    }
                                    if (contentInfo.FileUrl != targetContentInfo.FileUrl)
                                    {
                                        //修改附件
                                        string sourceFileUrl = PathUtility.MapPath(this.PublishmentSystemInfo, contentInfo.FileUrl);
                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl);

                                    }
                                    else if (contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.FileUrl)) != targetContentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.FileUrl)))
                                    {
                                        ArrayList sourceFileUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.FileUrl)));

                                        foreach (string FileUrl in sourceFileUrls)
                                        {
                                            string sourceFileUrl = PathUtility.MapPath(this.PublishmentSystemInfo, FileUrl);
                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl);
                                        }
                                    }
                                    if (contentInfo.ThumbUrl != targetContentInfo.ThumbUrl)
                                    {
                                        //修改附件
                                        string sourceThumbUrl = PathUtility.MapPath(this.PublishmentSystemInfo, contentInfo.ThumbUrl);
                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceThumbUrl);

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
                    errorMessage = string.Format("商品修改失败：{0}", ex.Message);
                    return 0;
                }

                if (!isAjaxSubmit)
                {
                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, "修改商品", string.Format("栏目:{0},商品标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    PageUtils.Redirect(this.returnUrl);
                }

                savedContentID = contentID;
            }

            return savedContentID;
        }

        private void CopyReferenceFiles(PublishmentSystemInfo targetPublishmentSystemInfo, string sourceUrl)
        {
            string targetUrl = StringUtils.ReplaceFirst(this.PublishmentSystemInfo.PublishmentSystemDir, sourceUrl, targetPublishmentSystemInfo.PublishmentSystemDir);
            if (!FileUtils.IsFileExists(targetUrl))
            {
                FileUtils.CopyFile(sourceUrl, targetUrl, true);
            }
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

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}