using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages.Modal
{
    public class NoticeTemplateEdit : BackgroundBasePage
    {
        public DropDownList ddlEmail;
        public DropDownList ddlPhone;
        public DropDownList ddlMessage;
        public TextBox tbEmailTitle;
        public TextBox tbEmailContent;
        public TextBox tbPhoneContent;
        public TextBox tbMessageTitle;
        public TextBox tbMessageContent;

        public PlaceHolder phEmail;
        public PlaceHolder phPhone;
        public PlaceHolder phMessage;


        public string noticeType;
        public string returnUrl;
        public string templateType;
        UserNoticeSettingInfo info;

        public const string EmailTemplate = "EmailTemplate";
        public const string PhoneTemplate = "PhoneTemplate";
        public const string MessageTemplate = "MessageTemplate";

        public static string GetOpenWindowStringToEdit(string noticeType, string templateType, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("NoticeType", StringUtils.ValueToUrl(noticeType));
            arguments.Add("TemplateType", StringUtils.ValueToUrl(templateType));
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            string title = "选择模板";
            if (templateType == EmailTemplate)
                title = "选择邮件模板";
            else if (templateType == PhoneTemplate)
                title = "选择短信模板";
            else if (templateType == MessageTemplate)
                title = "选择站内信模板";
            return PageUtilityPF.GetOpenWindowString(title, "modal_noticeTemplateEdit.aspx", arguments, 460, 360);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.noticeType = base.GetQueryString("NoticeType");
            this.templateType = base.GetQueryString("TemplateType");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            info = UserNoticeSettingManager.GetUserNoticeSettingInfo(noticeType);

            if (!IsPostBack)
            {
                BindEmailList();
                BindMessageList();
                BindPhoneList();
                
                if (info != null)
                {
                    this.tbEmailContent.Text = info.EmailTemplate;
                    this.tbEmailTitle.Text = info.EmailTitle;
                    this.tbMessageContent.Text = info.MessageTemplate;
                    this.tbMessageTitle.Text = info.MessageTitle;
                    this.tbPhoneContent.Text = info.PhoneTemplate;
                }

                SetActivePlaceHolder(templateType);
            }
        }

        void BindEmailList()
        {
            this.ddlEmail.DataSource = BaiRongDataProvider.UserNoticeTemplateDAO.GetUserNoticeTemplateDS(noticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Email), string.Empty);
            this.ddlEmail.DataTextField = "Name";
            this.ddlEmail.DataValueField = "ID";
            this.ddlEmail.DataBind();
            this.ddlEmail.Items.Insert(0, new ListItem("默认", "default"));
        }

        void BindMessageList()
        {
            this.ddlMessage.DataSource = BaiRongDataProvider.UserNoticeTemplateDAO.GetUserNoticeTemplateDS(noticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Message), string.Empty);
            this.ddlMessage.DataTextField = "Name";
            this.ddlMessage.DataValueField = "ID";
            this.ddlMessage.DataBind();
            this.ddlMessage.Items.Insert(0, new ListItem("默认", "default"));
        }
        void BindPhoneList()
        {
            this.ddlPhone.DataSource = BaiRongDataProvider.UserNoticeTemplateDAO.GetUserNoticeTemplateDS(noticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Phone), string.Empty);
            this.ddlPhone.DataTextField = "Name";
            this.ddlPhone.DataValueField = "ID";
            this.ddlPhone.DataBind();
            this.ddlPhone.Items.Insert(0, new ListItem("默认", "default"));
        }

        protected void ddlEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlEmail.SelectedValue == "default")
            {
                this.tbEmailContent.Text = info.EmailTemplate;
                this.tbEmailTitle.Text = info.EmailTitle;
            }
            else
            {
                UserNoticeTemplateInfo templateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(info.UserNoticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Email));
                if (templateInfo != null)
                {
                    this.tbEmailContent.Text = templateInfo.Content;
                    this.tbEmailTitle.Text = templateInfo.Title;
                }
            }
        }

        protected void ddlPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlPhone.SelectedValue == "default")
            {
                this.tbPhoneContent.Text = info.PhoneTemplate;
            }
            else
            {
                UserNoticeTemplateInfo templateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(info.UserNoticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Phone));
                if (templateInfo != null)
                {
                    this.tbPhoneContent.Text = templateInfo.Content;
                }
            }
        }

        protected void ddlMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlMessage.SelectedValue == "default")
            {
                this.tbMessageContent.Text = info.MessageTemplate;
                this.tbMessageTitle.Text = info.MessageTitle;
            }
            else
            {
                UserNoticeTemplateInfo templateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(info.UserNoticeType, EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Message));
                if (templateInfo != null)
                {
                    this.tbMessageContent.Text = templateInfo.Content;
                    this.tbMessageTitle.Text = templateInfo.Title;
                }
            }
        }

        void SetActivePlaceHolder(string phName)
        {
            switch (phName)
            {
                case EmailTemplate:
                    phEmail.Visible = true;
                    break;
                case PhoneTemplate:
                    phPhone.Visible = true;
                    break;
                case MessageTemplate:
                    phMessage.Visible = true;
                    break;
            }

        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {

                UserNoticeSettingInfo info = UserNoticeSettingManager.GetUserNoticeSettingInfo(noticeType);
                if (info != null)
                {
                    info.EmailTemplate = this.tbEmailContent.Text;
                    info.EmailTitle = this.tbEmailTitle.Text;
                    info.MessageTemplate = this.tbMessageContent.Text;
                    info.MessageTitle = this.tbMessageTitle.Text;
                    info.PhoneTemplate = this.tbPhoneContent.Text;
                    UserNoticeSettingManager.SetUserNoticeSettingInfo(info);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "选择模板");

                base.SuccessMessage("选择模板成功！");
                JsUtils.OpenWindow.CloseModalPage(this);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "选择模板失败！");
            }
        }
    }
}
