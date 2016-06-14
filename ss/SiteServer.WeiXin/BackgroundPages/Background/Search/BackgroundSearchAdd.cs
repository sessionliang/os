using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Controls;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundSearchAdd : BackgroundBasePageWX
	{
        public Literal ltlPageTitle; 

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;
         
        public PlaceHolder phStep2;
        public Literal ltlContentImageUrl;
        public CheckBox cbIsOutsiteSearch;
        public CheckBox cbIsNavigation;
        public TextBox tbNavTitleColor;
        public TextBox tbNavImageColor;

        public Literal ltlSearchNavs;

        public PlaceHolder phStep3;
        public TextBox tbImageAreaTitle;
        public TextBox tbTextAreaTitle;
        public Button btnImageChannelSelect;
        public Button btnTextChannelSelect;
          
        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int searchID;
       
        public static string GetRedirectUrl(int publishmentSystemID, int searchID)
        {
            return PageUtils.GetWXUrl(string.Format("background_searchAdd.aspx?publishmentSystemID={0}&searchID={1}", publishmentSystemID, searchID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }
         
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.searchID = TranslateUtils.ToInt(base.GetQueryString("searchID"));

			if (!IsPostBack)
            {
                string pageTitle = this.searchID > 0 ? "编辑微搜索" : "添加微搜索";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Search, pageTitle, AppManager.WeiXin.Permission.WebSite.Search);
                this.ltlPageTitle.Text = pageTitle;
               
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", SearchManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", SearchManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));

                this.ltlSearchNavs.Text = string.Format(@"
itemController.openFunctionSelect = function(itemIndex){{
    var openString = ""{0}"".replace(""itemIndexValue"", itemIndex);
    eval(openString);
}};
itemController.openChannelSelect = function(itemIndex){{
    var openString = ""{1}"".replace(""itemIndexValue"", itemIndex);
    eval(openString);
}};
itemController.openContentSelect = function(itemIndex){{
    var openString = ""{2}"".replace(""itemIndexValue"", itemIndex);
    eval(openString);
}};
itemController.openImageCssClassSelect = function(itemIndex){{
    var openString = ""{3}"".replace(""itemIndexValue"", itemIndex);
    eval(openString);
}};
", Modal.FunctionSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectFunction", "itemIndexValue"), SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectChannel", "itemIndexValue"), Modal.ContentSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectContent", "itemIndexValue"), Modal.ImageCssClassSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectImageCssClass", "itemIndexValue"));
                 
                if (this.searchID == 0)
                {
                    this.ltlSearchNavs.Text += "itemController.itemCount = 2;itemController.items = [{navigationType : 'Url', imageCssClass : 'fa fa-angle-double-down'}, {navigationType : 'Url', imageCssClass : 'fa fa-angle-double-down'}];";
                }
                else
                {
                    SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(this.searchID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(searchInfo.KeywordID);
                    this.cbIsEnabled.Checked = !searchInfo.IsDisabled;
                    this.tbTitle.Text = searchInfo.Title;
                    if (!string.IsNullOrEmpty(searchInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, searchInfo.ImageUrl));
                    }
                    this.tbSummary.Text = searchInfo.Summary;

                    this.cbIsOutsiteSearch.Checked = searchInfo.IsOutsiteSearch;
                    this.cbIsNavigation.Checked = searchInfo.IsNavigation;
                    this.tbNavTitleColor.Text = searchInfo.NavTitleColor;
                    this.tbNavImageColor.Text = searchInfo.NavImageColor;
                    if (!string.IsNullOrEmpty(searchInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, searchInfo.ContentImageUrl));
                    }

                    List<SearchNavigationInfo> searchNavigationInfoList = DataProviderWX.SearchNavigationDAO.GetSearchNavigationInfoList(base.PublishmentSystemID, this.searchID);
                    StringBuilder itemBuilder = new StringBuilder();
                    foreach (SearchNavigationInfo searchNavigationInfo in searchNavigationInfoList)
                    {
                        string searchPageTitle = string.Empty;

                        if (searchNavigationInfo.NavigationType == ENavigationTypeUtils.GetValue(ENavigationType.Url))
                        {
                            searchPageTitle = string.Empty;
                        }
                        else if (searchNavigationInfo.NavigationType == ENavigationTypeUtils.GetValue(ENavigationType.Function))
                        {
                            searchPageTitle = KeywordManager.GetFunctionName(EKeywordTypeUtils.GetEnumType(searchNavigationInfo.KeywordType), searchNavigationInfo.FunctionID);
                        }
                        else if (searchNavigationInfo.NavigationType == ENavigationTypeUtils.GetValue(ENavigationType.Site))
                        { 
                            if (searchNavigationInfo.ContentID > 0)
                            {
                                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, searchNavigationInfo.ChannelID);
                                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, searchNavigationInfo.ChannelID);
                                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, searchNavigationInfo.ContentID);

                                string pageUrl = PageUtilityWX.GetContentUrl(base.PublishmentSystemInfo, contentInfo);
                                searchPageTitle = string.Format(@"内容页：{0}", contentInfo.Title);
                                //title2 = string.Format(@"内容页：{0}&nbsp;<a href=""{1}""  target=""blank"">查看</a>", contentInfo.Title, pageUrl);
                            }
                            else
                            { 
                                string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, searchNavigationInfo.ChannelID);
                                string pageUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, searchNavigationInfo.ChannelID), base.PublishmentSystemInfo.Additional.VisualType);
                                searchPageTitle = string.Format(@"栏目页：{0}", nodeNames);
                                //title2 = string.Format(@"栏目页：{0}&nbsp;<a href=""{1}""  target=""blank"">查看</a>", nodeNames, pageUrl);
                            }
                        }

                        itemBuilder.AppendFormat("{{id: '{0}', title: '{1}',pageTitle: '{2}', url: '{3}', imageCssClass: '{4}',navigationType:'{5}',keywordType:'{6}',functionID:'{7}',channelID:'{8}',contentID:'{9}'}},", searchNavigationInfo.ID, searchNavigationInfo.Title, searchPageTitle, searchNavigationInfo.Url, searchNavigationInfo.ImageCssClass, searchNavigationInfo.NavigationType, searchNavigationInfo.KeywordType, searchNavigationInfo.FunctionID, searchNavigationInfo.ChannelID, searchNavigationInfo.ContentID);
                    }

                    if (itemBuilder.Length > 0) itemBuilder.Length--;

                    this.ltlSearchNavs.Text += string.Format(@"
itemController.itemCount = {0};itemController.items = [{1}];", searchNavigationInfoList.Count, itemBuilder.ToString());

                    this.imageUrl.Value = searchInfo.ImageUrl;
                    this.contentImageUrl.Value = searchInfo.ContentImageUrl;

                    this.tbImageAreaTitle.Text = searchInfo.ImageAreaTitle;
                    this.tbTextAreaTitle.Text = searchInfo.TextAreaTitle;

                    if (searchInfo.ImageAreaChannelID > 0)
                    {
                        string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, searchInfo.ImageAreaChannelID);
                        this.ltlSearchNavs.Text += string.Format(@"
$(document).ready(function() {{
    selectChannel(1, '{0}', {1});
}});
", nodeNames, searchInfo.ImageAreaChannelID);
                    }
                    if (searchInfo.TextAreaChannelID > 0)
                    {
                        string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, searchInfo.TextAreaChannelID);
                        this.ltlSearchNavs.Text += string.Format(@"
$(document).ready(function() {{
    selectChannel(2, '{0}', {1});
}});
", nodeNames, searchInfo.TextAreaChannelID);
                    }
                }

                this.btnImageChannelSelect.Attributes.Add("onclick", SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectChannel", "1"));
                this.btnTextChannelSelect.Attributes.Add("onclick", SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowStringByItemIndex(base.PublishmentSystemID, "selectChannel", "2"));

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundSearch.GetRedirectUrl(base.PublishmentSystemID)));
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                int selectedStep = 0;
                if (this.phStep1.Visible)
                {
                    selectedStep = 1;
                }
                else if (this.phStep2.Visible)
                {
                    selectedStep = 2;
                }
                else if (this.phStep3.Visible)
                {
                    selectedStep = 3;
                }
                
                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.searchID > 0)
                        {
                            SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(this.searchID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, searchInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                    }

                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                        this.phStep1.Visible = true;
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 2)
                {
                    bool isItemReady = true;

                    if (isItemReady)
                    {
                        int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);

                        if (itemCount > 0)
                        {
                            List<int> itemIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemID"]);
                            List<string> imageCssClassList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemImageCssClass"]);
                            List<string> keywordTypeList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemKeywordType"]);
                            List<int> functionIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemFunctionID"]);
                            List<int> channelIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemChannelID"]);
                            List<int> contentIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemContentID"]);

                            List<string> titleList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemTitle"]);
                            List<string> navigationTypeList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemNavigationType"]);
                            List<string> urlList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemUrl"]);

                            List<SearchNavigationInfo> navigationInfoList = new List<SearchNavigationInfo>();
                            for (int i = 0; i < itemCount; i++)
                            {
                                SearchNavigationInfo navigationInfo = new SearchNavigationInfo { ID = itemIDList[i], PublishmentSystemID = base.PublishmentSystemID, SearchID = this.searchID, Title = titleList[i], Url = urlList[i], ImageCssClass = imageCssClassList[i], NavigationType = navigationTypeList[i], KeywordType = keywordTypeList[i], FunctionID = functionIDList[i], ChannelID = channelIDList[i], ContentID = contentIDList[i] };

                                if (string.IsNullOrEmpty(navigationInfo.Title))
                                {
                                    base.FailMessage("保存失败，导航链接名称为必填项");
                                    isItemReady = false;
                                }
                                if (string.IsNullOrEmpty(navigationInfo.ImageCssClass))
                                {
                                    base.FailMessage("保存失败，导航链接图标为必填项");
                                    isItemReady = false;
                                }
                                if (navigationInfo.NavigationType == ENavigationTypeUtils.GetValue(ENavigationType.Url) && string.IsNullOrEmpty(navigationInfo.Url))
                                {
                                    base.FailMessage("保存失败，导航链接地址为必填项");
                                    isItemReady = false;
                                }

                                navigationInfoList.Add(navigationInfo);
                            }

                            if (isItemReady)
                            {
                                DataProviderWX.SearchNavigationDAO.DeleteAllNotInIDList(base.PublishmentSystemID, this.searchID, itemIDList);

                                foreach (SearchNavigationInfo navigationInfo in navigationInfoList)
                                {
                                    if (navigationInfo.ID > 0)
                                    {
                                        DataProviderWX.SearchNavigationDAO.Update(navigationInfo);
                                    }
                                    else
                                    {
                                        DataProviderWX.SearchNavigationDAO.Insert(navigationInfo);
                                    }
                                }
                            }
                        }
                    }

                    if (isItemReady)
                    {
                        this.phStep3.Visible = true;
                        this.btnSubmit.Text = "确 认";
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }

                }
                else if (selectedStep == 3)
                {  
                    SearchInfo searchInfo = new SearchInfo();
                    if (this.searchID > 0)
                    {
                        searchInfo = DataProviderWX.SearchDAO.GetSearchInfo (this.searchID);
                    }
                    searchInfo.PublishmentSystemID = base.PublishmentSystemID;
                    searchInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.searchID > 0, this.tbKeywords.Text, EKeywordType.Search , searchInfo.KeywordID);
                    searchInfo.IsDisabled = !this.cbIsEnabled.Checked;
                    searchInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    searchInfo.ImageUrl = this.imageUrl.Value; ;
                    searchInfo.Summary = this.tbSummary.Text;
                    searchInfo.ContentImageUrl = this.contentImageUrl.Value;

                    searchInfo.IsOutsiteSearch = this.cbIsOutsiteSearch.Checked;
                    searchInfo.IsNavigation = this.cbIsNavigation.Checked;
                    searchInfo.NavTitleColor = this.tbNavTitleColor.Text;
                    searchInfo.NavImageColor = this.tbNavImageColor.Text;
                    
                    searchInfo.ImageAreaTitle = this.tbImageAreaTitle.Text;
                    searchInfo.ImageAreaChannelID = TranslateUtils.ToInt(base.Request.Form["imageChannelID"]);
                    searchInfo.TextAreaTitle = this.tbTextAreaTitle.Text;
                    searchInfo.TextAreaChannelID = TranslateUtils.ToInt(base.Request.Form["textChannelID"]);

                    try
                    {
                        if (this.searchID > 0)
                        {
                            DataProviderWX.SearchDAO.Update(searchInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微搜索", string.Format("微搜索:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微搜索成功！");
                        }
                        else
                        {
                            this.searchID = DataProviderWX.SearchDAO.Insert(searchInfo);

                            DataProviderWX.SearchNavigationDAO.UpdateSearchID(base.PublishmentSystemID, this.searchID);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微搜索", string.Format("微搜索:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微搜索成功！");
                        }

                        string redirectUrl = BackgroundSearch.GetRedirectUrl(base.PublishmentSystemID);
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微搜索设置失败！");
                    }
                }
			}
		}
	}
}
