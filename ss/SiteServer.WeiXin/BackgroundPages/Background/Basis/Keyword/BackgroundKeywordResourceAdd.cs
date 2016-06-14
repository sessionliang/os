using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;
using System.Collections.Generic;
using BaiRong.Controls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeywordResourceAdd : BackgroundBasePageWX
    {
        public Literal ltlNav;
        public Literal ltlSite;
        public Button btnContentSelect;
        public Button btnChannelSelect;
        public TextBox tbTitle;
        public TextBox tbTaxis;
        public Literal ltlPreview;
        public TextBox tbSummary;
        public BREditor breContent;
        public TextBox tbNavigationUrl;
        public Literal ltlArrow;
        public Literal ltlScript;

        private int keywordID;
        private int resourceID;
        private int floor;
        private bool isSingle;

        public static string GetRedirectUrl(int publishmentSystemID, int keywordID, int resourceID, int floor, bool isSingle)
        {
            return PageUtils.GetWXUrl(string.Format("background_keywordResourceAdd.aspx?publishmentSystemID={0}&keywordID={1}&resourceID={2}&floor={3}&isSingle={4}", publishmentSystemID, keywordID, resourceID, floor, isSingle));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));
            this.resourceID = TranslateUtils.ToInt(base.GetQueryString("resourceID"));
            this.floor = TranslateUtils.ToInt(base.GetQueryString("floor"));
            this.isSingle = TranslateUtils.ToBool(base.GetQueryString("isSingle"));

            if (!IsPostBack)
            {
                this.ltlPreview.Text = @"
<p class=""js_cover upload_preview"" style=""display:none"">
    <input type=""hidden"" id=""imageUrl"" name=""imageUrl"" value="""" />
    <img src="""" width=""64"">
    <a class=""js_removeCover"" href=""javascript:;"" onclick=""deleteImageUrl();return false;"">删除</a>
</p>";
                this.ltlNav.Text = this.GetNavHtml(EResourceType.Site);

                this.ltlSite.Text = @"
<div id=""titles"" class=""well well-small"" style=""display:none""></div>
<input id=""channelID"" name=""channelID"" type=""hidden"" value="""" />
<input id=""contentID"" name=""contentID"" type=""hidden"" value="""" />";

                if (this.resourceID > 0)
                {
                    KeywordResourceInfo resourceInfo = DataProviderWX.KeywordResourceDAO.GetResourceInfo(this.resourceID);

                    if (resourceInfo.ResourceType == EResourceType.Site)
                    {
                        string siteHtml = MPUtils.GetSitePreivewHtml(base.PublishmentSystemInfo, resourceInfo.ChannelID, resourceInfo.ContentID);
                        if (!string.IsNullOrEmpty(siteHtml))
                        {
                            this.ltlSite.Text = string.Format(@"
<div id=""titles"" class=""well well-small"">{0}</div>
<input id=""channelID"" name=""channelID"" type=""hidden"" value=""{1}"" />
<input id=""contentID"" name=""contentID"" type=""hidden"" value=""{2}"" />", siteHtml, resourceInfo.ChannelID, resourceInfo.ContentID);
                        }
                    }

                    this.tbTitle.Text = resourceInfo.Title;
                    this.tbTaxis.Text = resourceInfo.Taxis.ToString();
                    if (!string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        this.ltlPreview.Text = string.Format(@"
<p class=""js_cover upload_preview"">
    <input type=""hidden"" id=""imageUrl"" name=""imageUrl"" value=""{0}"" />
    <img src=""{1}"" width=""64"">
    <a class=""js_removeCover"" href=""javascript:;"" onclick=""deleteImageUrl();return false;"">删除</a>
</p>", resourceInfo.ImageUrl, PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
                    }
                    this.tbSummary.Text = resourceInfo.Summary;
                    if (resourceInfo.IsShowCoverPic == false)
                    {
                        this.ltlScript.Text += "<script>$('.js_show_cover_pic').click();</script>";
                    }
                    this.breContent.Text = resourceInfo.Content;
                    this.tbNavigationUrl.Text = resourceInfo.NavigationUrl;

                    this.ltlScript.Text += string.Format(@"<script>$('.nav a.{0}').click();</script>", EResourceTypeUtils.GetValue(resourceInfo.ResourceType));
                }

                this.btnContentSelect.Attributes.Add("onclick", "parent." + Modal.ContentSelect.GetOpenWindowString(base.PublishmentSystemID, false, "contentSelect"));
                this.btnChannelSelect.Attributes.Add("onclick", "parent." + SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowString(base.PublishmentSystemID));

                int top = 0;
                if (floor > 1)
                {
                    top = 67 + (floor - 1) * 103;
                }
                this.ltlArrow.Text = string.Format(@"<i class=""arrow arrow_out"" style=""margin-top: {0}px;""></i><i class=""arrow arrow_in"" style=""margin-top: {0}px;""></i>", top);
            }
        }

        private string GetNavHtml(EResourceType resourceType)
        {
            string nav = string.Empty;

            List<EResourceType> list = new List<EResourceType>();
            list.Add(EResourceType.Site);
            list.Add(EResourceType.Content);
            list.Add(EResourceType.Url);

            foreach (EResourceType rType in list)
            {
                nav += string.Format(@"<li class=""{0}""><a href=""javascript:;"" class=""{1}"" resourceType=""{1}"">显示{2}</a></li>", rType == resourceType ? "active" : string.Empty, EResourceTypeUtils.GetValue(rType), EResourceTypeUtils.GetText(rType));
            }

            return nav;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo();
                    resourceInfo.PublishmentSystemID = base.PublishmentSystemID;
                    resourceInfo.KeywordID = this.keywordID;

                    if (this.resourceID > 0)
                    {
                        resourceInfo = DataProviderWX.KeywordResourceDAO.GetResourceInfo(this.resourceID);
                    }

                    resourceInfo.Title = this.tbTitle.Text;
                    resourceInfo.Taxis = Convert.ToInt32(this.tbTaxis.Text);
                    resourceInfo.ResourceType = EResourceTypeUtils.GetEnumType(base.Request.Form["resourceType"]);
                    resourceInfo.ImageUrl = base.Request.Form["imageUrl"];
                    resourceInfo.Summary = this.tbSummary.Text;
                    resourceInfo.IsShowCoverPic = TranslateUtils.ToBool(base.Request.Form["isShowCoverPic"]);
                    resourceInfo.Content = this.breContent.Text;
                    resourceInfo.NavigationUrl = this.tbNavigationUrl.Text;
                    resourceInfo.ChannelID = TranslateUtils.ToInt(base.Request.Form["channelID"]);
                    resourceInfo.ContentID = TranslateUtils.ToInt(base.Request.Form["contentID"]);

                    bool isError = false;

                    if (resourceInfo.ResourceType == EResourceType.Site)
                    {
                        if (resourceInfo.ChannelID == 0)
                        {
                            base.FailMessage("图文回复保存失败，请选择需要显示的微网站页面！");
                            this.ltlScript.Text += string.Format(@"<script>$('.nav a.{0}').click();</script>", EResourceTypeUtils.GetValue(EResourceType.Site));
                            isError = true;
                        }
                    }
                    else if (resourceInfo.ResourceType == EResourceType.Url)
                    {
                        if (string.IsNullOrEmpty(resourceInfo.NavigationUrl))
                        {
                            base.FailMessage("图文回复保存失败，请填写需要链接的网址！");
                            this.ltlScript.Text += string.Format(@"<script>$('.nav a.{0}').click();</script>", EResourceTypeUtils.GetValue(EResourceType.Url));
                            isError = true;
                        }
                    }

                    if (!isError)
                    {
                        if (this.resourceID > 0)
                        {
                            DataProviderWX.KeywordResourceDAO.Update(resourceInfo);

                            StringUtility.AddLog(base.PublishmentSystemID, "修改关键词图文回复");
                            base.SuccessMessage("关键词图文回复修改成功！");
                        }
                        else
                        {
                            this.resourceID = DataProviderWX.KeywordResourceDAO.Insert(resourceInfo);

                            StringUtility.AddLog(base.PublishmentSystemID, "新增关键词图文回复");
                            base.SuccessMessage("关键词图文回复新增成功！");
                        }

                        FileUtilityWX.CreateWeiXinContent(base.PublishmentSystemInfo, this.keywordID, this.resourceID);

                        string redirectUrl = BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, this.resourceID, this.isSingle);
                        this.ltlScript.Text += string.Format(@"<script>setTimeout(""parent.redirect('{0}')"", 1500);</script>", redirectUrl);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "关键词图文回复配置失败！");
                }
            }
        }
    }
}
