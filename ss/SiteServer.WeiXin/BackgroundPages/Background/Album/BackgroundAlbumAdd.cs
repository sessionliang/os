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

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundAlbumAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public HtmlInputHidden imageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int albumID;

        public static string GetRedirectUrl(int publishmentSystemID, int albumID)
        {
            return PageUtils.GetWXUrl(string.Format("background_albumAdd.aspx?publishmentSystemID={0}&albumID={1}", publishmentSystemID, albumID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.albumID = TranslateUtils.ToInt(base.GetQueryString("albumID"));

            if (!IsPostBack)
            {
                string pageTitle = this.albumID > 0 ? "编辑微相册" : "添加微相册";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Album, pageTitle, AppManager.WeiXin.Permission.WebSite.Album);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AlbumManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.albumID > 0)
                {
                    AlbumInfo albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(this.albumID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(albumInfo.KeywordID);
                    this.cbIsEnabled.Checked = !albumInfo.IsDisabled;
                    this.tbTitle.Text = albumInfo.Title;
                    if (!string.IsNullOrEmpty(albumInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, albumInfo.ImageUrl));
                    }
                    this.tbSummary.Text = albumInfo.Summary;

                    this.imageUrl.Value = albumInfo.ImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundAlbum.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isConflict = false;
                string conflictKeywords = string.Empty;
                if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                {
                    if (this.albumID > 0)
                    {
                        AlbumInfo albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(this.albumID);
                        isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, albumInfo.KeywordID,PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                    else
                    {
                        isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                }

                if (isConflict)
                {
                    base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                }
                else
                {
                    AlbumInfo albumInfo = new AlbumInfo();
                    if (this.albumID > 0)
                    {
                        albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(this.albumID);
                    }
                    albumInfo.PublishmentSystemID = base.PublishmentSystemID;

                    albumInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.albumID > 0, this.tbKeywords.Text, EKeywordType.Album, albumInfo.KeywordID);
                    albumInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    albumInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    albumInfo.ImageUrl = this.imageUrl.Value; ;
                    albumInfo.Summary = this.tbSummary.Text;

                    try
                    {
                        if (this.albumID > 0)
                        {
                            DataProviderWX.AlbumDAO.Update(albumInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微相册", string.Format("微相册:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微相册成功！");
                        }
                        else
                        {
                            this.albumID = DataProviderWX.AlbumDAO.Insert(albumInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微相册", string.Format("微相册:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微相册成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_albumContent.aspx?publishmentSystemID={0}&albumID={1}", base.PublishmentSystemID, this.albumID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微相册设置失败！");
                    }
                }
            }
        }
    }
}
