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
using View360Manager = SiteServer.WeiXin.Core.View360Manager;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundView360Add : BackgroundBasePageWX
	{
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public PlaceHolder phStep2;
        public TextBox tbContentImageUrl1;
        public TextBox tbContentImageUrl2;
        public TextBox tbContentImageUrl3;
        public TextBox tbContentImageUrl4;
        public TextBox tbContentImageUrl5;
        public TextBox tbContentImageUrl6;
        public Literal ltlContentImageUrl1;
        public Literal ltlContentImageUrl2;
        public Literal ltlContentImageUrl3;
        public Literal ltlContentImageUrl4;
        public Literal ltlContentImageUrl5;
        public Literal ltlContentImageUrl6;

        public HtmlInputHidden imageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int view360ID;

        public static string GetRedirectUrl(int publishmentSystemID, int view360ID)
        {
            return PageUtils.GetWXUrl(string.Format("background_view360Add.aspx?publishmentSystemID={0}&view360ID={1}", publishmentSystemID, view360ID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.view360ID = TranslateUtils.ToInt(base.GetQueryString("view360ID"));

			if (!IsPostBack)
            {
                string pageTitle = this.view360ID > 0 ? "编辑360全景" : "添加360全景";

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_View360, pageTitle, AppManager.WeiXin.Permission.WebSite.View360);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", View360Manager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                string selectImageClick = SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.tbContentImageUrl1.ClientID);
                string uploadImageClick = SiteServer.CMS.BackgroundPages.Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.tbContentImageUrl1.ClientID);
                string cuttingImageClick = SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(base.PublishmentSystemID, this.tbContentImageUrl1.ClientID);
                string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(base.PublishmentSystemID, this.tbContentImageUrl1.ClientID);
                this.ltlContentImageUrl1.Text = string.Format(@"
                      <a class=""btn"" href=""javascript:;"" onclick=""{0};return false;"" title=""选择""><i class=""icon-th""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{1};return false;"" title=""上传""><i class=""icon-arrow-up""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{2};return false;"" title=""裁切""><i class=""icon-crop""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{3};return false;"" title=""预览""><i class=""icon-eye-open""></i></a>", selectImageClick, uploadImageClick, cuttingImageClick, previewImageClick);

                this.ltlContentImageUrl2.Text = this.ltlContentImageUrl1.Text.Replace(this.tbContentImageUrl1.ClientID, this.tbContentImageUrl2.ClientID);
                this.ltlContentImageUrl3.Text = this.ltlContentImageUrl1.Text.Replace(this.tbContentImageUrl1.ClientID, this.tbContentImageUrl3.ClientID);
                this.ltlContentImageUrl4.Text = this.ltlContentImageUrl1.Text.Replace(this.tbContentImageUrl1.ClientID, this.tbContentImageUrl4.ClientID);
                this.ltlContentImageUrl5.Text = this.ltlContentImageUrl1.Text.Replace(this.tbContentImageUrl1.ClientID, this.tbContentImageUrl5.ClientID);
                this.ltlContentImageUrl6.Text = this.ltlContentImageUrl1.Text.Replace(this.tbContentImageUrl1.ClientID, this.tbContentImageUrl6.ClientID);

                if (this.view360ID == 0)
                {
                    this.tbContentImageUrl1.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 1);
                    this.tbContentImageUrl2.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 2);
                    this.tbContentImageUrl3.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 3);
                    this.tbContentImageUrl4.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 4);
                    this.tbContentImageUrl5.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 5);
                    this.tbContentImageUrl6.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty, 6);
                }
                else
                {
                    View360Info view360Info = DataProviderWX.View360DAO.GetView360Info(this.view360ID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(view360Info.KeywordID);
                    this.cbIsEnabled.Checked = !view360Info.IsDisabled;
                    this.tbTitle.Text = view360Info.Title;
                    if (!string.IsNullOrEmpty(view360Info.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, view360Info.ImageUrl));
                    }
                    this.tbSummary.Text = view360Info.Summary;

                    this.tbContentImageUrl1.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl1, 1);
                    this.tbContentImageUrl2.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl2, 2);
                    this.tbContentImageUrl3.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl3, 3);
                    this.tbContentImageUrl4.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl4, 4);
                    this.tbContentImageUrl5.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl5, 5);
                    this.tbContentImageUrl6.Text = View360Manager.GetContentImageUrl(base.PublishmentSystemInfo, view360Info.ContentImageUrl6, 6);

                    this.imageUrl.Value = view360Info.ImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundView360.GetRedirectUrl(base.PublishmentSystemID)));
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

                this.phStep1.Visible = this.phStep2.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.view360ID > 0)
                        {
                            View360Info view360Info = DataProviderWX.View360DAO.GetView360Info(this.view360ID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, view360Info.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                        this.btnSubmit.Text = "确 认";
                    }
                }
                else if (selectedStep == 2)
                {
                    View360Info view360Info = new View360Info();
                    if (this.view360ID > 0)
                    {
                        view360Info = DataProviderWX.View360DAO.GetView360Info(this.view360ID);
                    }
                    view360Info.PublishmentSystemID = base.PublishmentSystemID;

                    view360Info.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.view360ID > 0, this.tbKeywords.Text, EKeywordType.View360, view360Info.KeywordID);
                    view360Info.IsDisabled = !this.cbIsEnabled.Checked;

                    view360Info.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    view360Info.ImageUrl = this.imageUrl.Value; ;
                    view360Info.Summary = this.tbSummary.Text;

                    view360Info.ContentImageUrl1 = this.tbContentImageUrl1.Text;
                    view360Info.ContentImageUrl2 = this.tbContentImageUrl2.Text;
                    view360Info.ContentImageUrl3 = this.tbContentImageUrl3.Text;
                    view360Info.ContentImageUrl4 = this.tbContentImageUrl4.Text;
                    view360Info.ContentImageUrl5 = this.tbContentImageUrl5.Text;
                    view360Info.ContentImageUrl6 = this.tbContentImageUrl6.Text;

                    try
                    {
                        if (this.view360ID > 0)
                        {
                            DataProviderWX.View360DAO.Update(view360Info);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改360全景", string.Format("360全景:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改360全景成功！");
                        }
                        else
                        {
                            this.view360ID = DataProviderWX.View360DAO.Insert(view360Info);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加360全景", string.Format("360全景:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加360全景成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_view360.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "360全景设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
