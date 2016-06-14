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
using MessageManager = SiteServer.WeiXin.Core.MessageManager;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundMessageAdd : BackgroundBasePageWX
	{
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public PlaceHolder phStep2;
        public TextBox tbContentDescription;
        public Literal ltlContentImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int messageID;

        public static string GetRedirectUrl(int publishmentSystemID, int messageID)
        {
            return PageUtils.GetWXUrl(string.Format("background_messageAdd.aspx?publishmentSystemID={0}&messageID={1}", publishmentSystemID, messageID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.messageID = TranslateUtils.ToInt(base.GetQueryString("messageID"));

			if (!IsPostBack)
            {
                string pageTitle = this.messageID > 0 ? "编辑微留言" : "添加微留言";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Message, pageTitle, AppManager.WeiXin.Permission.WebSite.Message);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", MessageManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", MessageManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.messageID > 0)
                {
                    MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(this.messageID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(messageInfo.KeywordID);
                    this.cbIsEnabled.Checked = !messageInfo.IsDisabled;
                    this.tbTitle.Text = messageInfo.Title;
                    if (!string.IsNullOrEmpty(messageInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, messageInfo.ImageUrl));
                    }
                    this.tbSummary.Text = messageInfo.Summary;
                    if (!string.IsNullOrEmpty(messageInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, messageInfo.ContentImageUrl));
                    }

                    this.tbContentDescription.Text = messageInfo.ContentDescription;

                    this.imageUrl.Value = messageInfo.ImageUrl;
                    this.contentImageUrl.Value = messageInfo.ContentImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundMessage.GetRedirectUrl(base.PublishmentSystemID)));
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
                        if (this.messageID > 0)
                        {
                            MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(this.messageID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, messageInfo.KeywordID,PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                    MessageInfo messageInfo = new MessageInfo();
                    if (this.messageID > 0)
                    {
                        messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(this.messageID);
                    }
                    messageInfo.PublishmentSystemID = base.PublishmentSystemID;

                    messageInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.messageID > 0,PageUtils.FilterXSS(this.tbKeywords.Text), EKeywordType.Message, messageInfo.KeywordID);
                    messageInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    messageInfo.Title =PageUtils.FilterXSS(this.tbTitle.Text);
                    messageInfo.ImageUrl = this.imageUrl.Value; ;
                    messageInfo.Summary = this.tbSummary.Text;

                    messageInfo.ContentImageUrl = this.contentImageUrl.Value;
                    messageInfo.ContentDescription = this.tbContentDescription.Text;

                    try
                    {
                        if (this.messageID > 0)
                        {
                            DataProviderWX.MessageDAO.Update(messageInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微留言", string.Format("微留言:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微留言成功！");
                        }
                        else
                        {
                            this.messageID = DataProviderWX.MessageDAO.Insert(messageInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微留言", string.Format("微留言:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微留言成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_message.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微留言设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
