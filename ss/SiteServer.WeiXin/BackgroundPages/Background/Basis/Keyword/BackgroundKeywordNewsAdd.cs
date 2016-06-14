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

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeywordNewsAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;

        public PlaceHolder phSingle;
        public Literal ltlSingleTitle;
        public Literal ltlSingleImageUrl;
        public Literal ltlSingleSummary;

        public PlaceHolder phMultiple;
        public Literal ltlMultipleTitle;
        public Literal ltlMultipleImageUrl;
        public Literal ltlMultipleEditUrl;
        public Repeater rptMultipleContents;
        public Literal ltlItemEditUrl;

        //public Literal ltlPreview;

        public Literal ltlIFrame;

        private int keywordID;
        private int resourceID;
        private bool isSingle;

        public static string GetRedirectUrl(int publishmentSystemID, int keywordID, int resourceID, bool isSingle)
        {
            return PageUtils.GetWXUrl(string.Format("background_keywordNewsAdd.aspx?publishmentSystemID={0}&keywordID={1}&resourceID={2}&isSingle={3}", publishmentSystemID, keywordID, resourceID, isSingle));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));
            this.resourceID = TranslateUtils.ToInt(base.GetQueryString("resourceID"));
            this.isSingle = TranslateUtils.ToBool(base.GetQueryString("isSingle"));

            if (base.Request.QueryString["deleteResource"] != null)
            {
                int deleteResourceID = TranslateUtils.ToInt(base.Request.QueryString["deleteResourceID"]);

                try
                {
                    DataProviderWX.KeywordResourceDAO.Delete(deleteResourceID);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (!IsPostBack)
            {
                string pageTitle = this.keywordID == 0 ? "添加关键词图文回复" : "修改关键词图文回复";
                this.ltlPageTitle.Text = pageTitle;

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_ImageReply, pageTitle, AppManager.WeiXin.Permission.WebSite.ImageReply);
                this.phSingle.Visible = this.isSingle;
                this.phMultiple.Visible = !this.isSingle;

                if (this.isSingle)
                {
                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo();

                    resourceInfo.ResourceID = 0;
                    resourceInfo.PublishmentSystemID = base.PublishmentSystemID;
                    resourceInfo.KeywordID = this.keywordID;
                    resourceInfo.Title = "标题";
                    resourceInfo.ImageUrl = string.Empty;
                    resourceInfo.Summary = string.Empty;
                    resourceInfo.ResourceType = EResourceType.Content;
                    resourceInfo.IsShowCoverPic = true;
                    resourceInfo.Content = string.Empty;
                    resourceInfo.NavigationUrl = string.Empty;
                    resourceInfo.ChannelID = 0;
                    resourceInfo.ContentID = 0;
                    resourceInfo.Taxis = 0;

                    if (this.resourceID > 0)
                    {
                        resourceInfo = DataProviderWX.KeywordResourceDAO.GetResourceInfo(this.resourceID);
                    }
                    this.ltlSingleTitle.Text = string.Format(@"<a href=""javascript:;"">{0}</a>", resourceInfo.Title);
                    if (string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        this.ltlSingleImageUrl.Text = @"<i class=""appmsg_thumb default"">封面图片</i>";
                    }
                    else
                    {
                        this.ltlSingleImageUrl.Text = string.Format(@"<img class=""js_appmsg_thumb"" src=""{0}"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
                    }
                    ltlSingleSummary.Text = MPUtils.GetSummary(resourceInfo.Summary, resourceInfo.Content);
                }
                else
                {
                    List<KeywordResourceInfo> resourceInfoList = DataProviderWX.KeywordResourceDAO.GetResourceInfoList(this.keywordID);

                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo();

                    resourceInfo.ResourceID = 0;
                    resourceInfo.PublishmentSystemID = base.PublishmentSystemID;
                    resourceInfo.KeywordID = this.keywordID;
                    resourceInfo.Title = "标题";
                    resourceInfo.ImageUrl = string.Empty;
                    resourceInfo.Summary = string.Empty;
                    resourceInfo.ResourceType = EResourceType.Content;
                    resourceInfo.IsShowCoverPic = true;
                    resourceInfo.Content = string.Empty;
                    resourceInfo.NavigationUrl = string.Empty;
                    resourceInfo.ChannelID = 0;
                    resourceInfo.ContentID = 0;
                    resourceInfo.Taxis = 0;

                    if (resourceInfoList.Count <= 1)
                    {
                        resourceInfoList.Add(resourceInfo);
                    }

                    if (resourceInfoList.Count > 1)
                    {
                        resourceInfo = resourceInfoList[0];
                        resourceInfoList.Remove(resourceInfo);
                    }

                    this.ltlMultipleTitle.Text = string.Format(@"<a href=""javascript:;"">{0}</a>", resourceInfo.Title);

                    if (string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        this.ltlMultipleImageUrl.Text = @"<i class=""appmsg_thumb default"">封面图片</i>";
                    }
                    else
                    {
                        this.ltlMultipleImageUrl.Text = string.Format(@"<img class=""js_appmsg_thumb"" src=""{0}"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
                    }
                    this.ltlMultipleEditUrl.Text = string.Format(@"<a class=""icon18_common edit_gray js_edit"" href=""{0}"" target=""resource"">&nbsp;&nbsp;</a>", BackgroundKeywordResourceAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, resourceInfo.ResourceID, 1, false));

                    this.rptMultipleContents.DataSource = resourceInfoList;
                    this.rptMultipleContents.ItemDataBound += rptMultipleContents_ItemDataBound;
                    this.rptMultipleContents.DataBind();

                    this.ltlItemEditUrl.Text = string.Format(@"<a class=""icon18_common edit_gray js_edit"" href=""{0}"" target=""resource"">&nbsp;&nbsp;</a>", BackgroundKeywordResourceAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, 0, resourceInfoList.Count + 2, false));
                }

                //this.ltlPreview.Text = string.Format(@"<button onclick=""{0}"">预览</button>", Modal.KeywordPreview.GetOpenWindowString(base.PublishmentSystemID, this.keywordID));

                this.ltlIFrame.Text = string.Format(@"<iframe frameborder=""0"" id=""resource"" name=""resource"" width=""100%"" height=""1300"" src=""{0}"" scrolling=""no""></iframe>", BackgroundKeywordResourceAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, this.resourceID, 1, this.isSingle));
            }
        }

        void rptMultipleContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            KeywordResourceInfo resourceInfo = e.Item.DataItem as KeywordResourceInfo;

            Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;
            Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
            Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
            Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

            if (string.IsNullOrEmpty(resourceInfo.ImageUrl))
            {
                ltlImageUrl.Text = @"<i class=""appmsg_thumb default"">缩略图</i>";
            }
            else
            {
                ltlImageUrl.Text = string.Format(@"<img class=""js_appmsg_thumb appmsg_thumb"" style=""max-width:78px;max-height:78px;display:block"" src=""{0}"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
            }
            ltlTitle.Text = string.Format(@"<a href=""javascript:;"">{0}</a>", resourceInfo.Title);

            ltlEditUrl.Text = string.Format(@"<a class=""icon18_common edit_gray js_edit"" href=""{0}"" target=""resource"">&nbsp;&nbsp;</a>", BackgroundKeywordResourceAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, resourceInfo.ResourceID, e.Item.ItemIndex + 2, false));

            ltlDeleteUrl.Text = string.Format(@"<a class=""icon18_common del_gray js_del"" href=""{0}&deleteResource=true&deleteResourceID={1}"">&nbsp;&nbsp;</a>", BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, 0, false), resourceInfo.ResourceID);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {

            }
        }
    }
}
