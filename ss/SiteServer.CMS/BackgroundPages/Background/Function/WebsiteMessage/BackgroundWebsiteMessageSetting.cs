using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundWebsiteMessageSetting : BackgroundBasePage
    {
        public Literal ltlWebsiteMessageName;

        public RadioButtonList rblIsMail;
        public PlaceHolder phMail;
        public RadioButtonList rblMailReceiver;
        public PlaceHolder phMailTo;
        public TextBox tbMailTo;
        public PlaceHolder phMailFiledName;
        public DropDownList ddlMailFiledName;
        public TextBox tbMailTitle;
        public RadioButtonList rblIsMailTemplate;
        public PlaceHolder phMailTemplate;
        public TextBox tbMailContent;

        public RadioButtonList rblIsSMS;
        public PlaceHolder phSMS;
        public RadioButtonList rblSMSReceiver;
        public PlaceHolder phSMSTo;
        public TextBox tbSMSTo;
        public PlaceHolder phSMSFiledName;
        public DropDownList ddlSMSFiledName;
        public RadioButtonList rblIsSMSTemplate;
        public PlaceHolder phSMSTemplate;
        public TextBox tbSMSContent;


        #region 其他设置
        //protected TextBox WebsiteMessageName;
        protected RadioButtonList IsChecked;
        protected RadioButtonList IsReply;

        protected TextBox MessageSuccess;
        protected TextBox MessageFailure;

        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;
        protected RadioButtonList IsSuccessHide;
        protected RadioButtonList IsSuccessReload;
        protected RadioButtonList IsCtrlEnter;
        public PlaceHolder phLoginUrl;
        public TextBox tbLoginUrl;
        #endregion

        public Literal ltlTips1;
        public Literal ltlTips2;

        private WebsiteMessageInfo websiteMessageInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("WebsiteMessageName");

            string WebsiteMessageName = base.GetQueryStringNoSqlAndXss("WebsiteMessageName");

            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(WebsiteMessageName, PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "其他设置", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(base.PublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(base.PublishmentSystemID);
                #endregion

                #region 其他设置
                if (websiteMessageInfo != null)
                {
                    //this.WebsiteMessageName.Text = websiteMessageInfo.WebsiteMessageName;
                    ControlUtils.SelectListItems(this.IsChecked, websiteMessageInfo.IsChecked.ToString());
                    ControlUtils.SelectListItems(this.IsReply, websiteMessageInfo.IsReply.ToString());

                    this.MessageSuccess.Text = websiteMessageInfo.Additional.MessageSuccess;
                    this.MessageFailure.Text = websiteMessageInfo.Additional.MessageFailure;

                    ControlUtils.SelectListItems(this.IsAnomynous, websiteMessageInfo.Additional.IsAnomynous.ToString());
                    ControlUtils.SelectListItems(this.IsValidateCode, websiteMessageInfo.Additional.IsValidateCode.ToString());
                    ControlUtils.SelectListItems(this.IsSuccessHide, websiteMessageInfo.Additional.IsSuccessHide.ToString());
                    ControlUtils.SelectListItems(this.IsSuccessReload, websiteMessageInfo.Additional.IsSuccessReload.ToString());
                    ControlUtils.SelectListItems(this.IsCtrlEnter, websiteMessageInfo.Additional.IsCtrlEnter.ToString());

                    this.tbLoginUrl.Text = websiteMessageInfo.Additional.LoginUrl;
                    this.IsAnomynous_SelectedIndexChanged(null, EventArgs.Empty);
                }
                #endregion

                #region 手机邮件设置
                this.ltlWebsiteMessageName.Text = this.websiteMessageInfo.WebsiteMessageName;

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMail, this.websiteMessageInfo.Additional.IsMail.ToString());
                this.rblIsMail_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblMailReceiver, ETriStateUtils.GetValue(this.websiteMessageInfo.Additional.MailReceiver));
                this.rblMailReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailTo.Text = this.websiteMessageInfo.Additional.MailTo;

                this.ltlTips1.Text = "[addDate]代表提交时间，[ipAddress]代表IP地址，";

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID);
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, websiteMessageInfo.Additional.MailFiledName))
                    {
                        listItem.Selected = true;
                    }
                    this.ddlMailFiledName.Items.Add(listItem);

                    this.ltlTips1.Text += string.Format(@"[{0}]代表{1}，", styleInfo.AttributeName, styleInfo.DisplayName);
                }

                this.ltlTips1.Text = this.ltlTips1.Text.TrimEnd('，');
                this.ltlTips2.Text = this.ltlTips1.Text;

                this.tbMailTitle.Text = this.websiteMessageInfo.Additional.MailTitle;

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMailTemplate, this.websiteMessageInfo.Additional.IsMailTemplate.ToString());
                this.rblIsMailTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailContent.Text = this.websiteMessageInfo.Additional.MailContent;

                if (string.IsNullOrEmpty(this.tbMailContent.Text))
                {
                    this.tbMailContent.Text = MessageManager.GetMailContent(tableStyleInfoArrayList);
                }

                //短信

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMS, this.websiteMessageInfo.Additional.IsSMS.ToString());
                this.rblIsSMS_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblSMSReceiver, ETriStateUtils.GetValue(this.websiteMessageInfo.Additional.SMSReceiver));
                this.rblSMSReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSTo.Text = this.websiteMessageInfo.Additional.SMSTo;

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, websiteMessageInfo.Additional.SMSFiledName))
                    {
                        listItem.Selected = true;
                    }
                    this.ddlSMSFiledName.Items.Add(listItem);
                }

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMSTemplate, this.websiteMessageInfo.Additional.IsSMSTemplate.ToString());
                this.rblIsSMSTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSContent.Text = this.websiteMessageInfo.Additional.SMSContent;

                if (string.IsNullOrEmpty(this.tbSMSContent.Text))
                {
                    this.tbSMSContent.Text = MessageManager.GetSMSContent(tableStyleInfoArrayList);
                }
                #endregion
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                #region 其他设置

                //websiteMessageInfo.WebsiteMessageName = this.WebsiteMessageName.Text;
                websiteMessageInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                websiteMessageInfo.IsReply = TranslateUtils.ToBool(this.IsReply.SelectedValue);

                websiteMessageInfo.Additional.MessageSuccess = this.MessageSuccess.Text;
                websiteMessageInfo.Additional.MessageFailure = this.MessageFailure.Text;

                websiteMessageInfo.Additional.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                websiteMessageInfo.Additional.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                websiteMessageInfo.Additional.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                websiteMessageInfo.Additional.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                websiteMessageInfo.Additional.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                websiteMessageInfo.Additional.LoginUrl = this.tbLoginUrl.Text;
                #endregion

                #region 手机邮件设置
                this.websiteMessageInfo.Additional.IsMail = TranslateUtils.ToBool(this.rblIsMail.SelectedValue);
                this.websiteMessageInfo.Additional.MailReceiver = ETriStateUtils.GetEnumType(this.rblMailReceiver.SelectedValue);
                this.websiteMessageInfo.Additional.MailTo = this.tbMailTo.Text;
                this.websiteMessageInfo.Additional.MailFiledName = this.ddlMailFiledName.SelectedValue;
                this.websiteMessageInfo.Additional.MailTitle = this.tbMailTitle.Text;
                this.websiteMessageInfo.Additional.IsMailTemplate = TranslateUtils.ToBool(this.rblIsMailTemplate.SelectedValue);
                this.websiteMessageInfo.Additional.MailContent = this.tbMailContent.Text;

                this.websiteMessageInfo.Additional.IsSMS = TranslateUtils.ToBool(this.rblIsSMS.SelectedValue);
                this.websiteMessageInfo.Additional.SMSReceiver = ETriStateUtils.GetEnumType(this.rblSMSReceiver.SelectedValue);
                this.websiteMessageInfo.Additional.SMSTo = this.tbSMSTo.Text;
                this.websiteMessageInfo.Additional.SMSFiledName = this.ddlSMSFiledName.SelectedValue;
                this.websiteMessageInfo.Additional.IsSMSTemplate = TranslateUtils.ToBool(this.rblIsSMSTemplate.SelectedValue);
                this.websiteMessageInfo.Additional.SMSContent = this.tbSMSContent.Text;
                #endregion

                try
                {
                    DataProvider.WebsiteMessageDAO.Update(this.websiteMessageInfo);
                    base.SuccessMessage("邮件/短信发送设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "邮件/短信发送设置修改失败," + ex.Message);
                }
            }
        }

        public void rblMailReceiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phMailTo.Visible = this.phMailFiledName.Visible = false;

            ETriState mailReceiver = ETriStateUtils.GetEnumType(this.rblMailReceiver.SelectedValue);
            if (mailReceiver == ETriState.True)
            {
                this.phMailTo.Visible = true;
            }
            else if (mailReceiver == ETriState.False)
            {
                this.phMailFiledName.Visible = true;
            }
            else if (mailReceiver == ETriState.All)
            {
                this.phMailTo.Visible = this.phMailFiledName.Visible = true;
            }
        }

        public void rblIsMail_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phMail.Visible = TranslateUtils.ToBool(this.rblIsMail.SelectedValue);
        }

        public void rblIsMailTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phMailTemplate.Visible = TranslateUtils.ToBool(this.rblIsMailTemplate.SelectedValue);
        }

        public void rblSMSReceiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phSMSTo.Visible = this.phSMSFiledName.Visible = false;

            ETriState smsReceiver = ETriStateUtils.GetEnumType(this.rblSMSReceiver.SelectedValue);
            if (smsReceiver == ETriState.True)
            {
                this.phSMSTo.Visible = true;
            }
            else if (smsReceiver == ETriState.False)
            {
                this.phSMSFiledName.Visible = true;
            }
            else if (smsReceiver == ETriState.All)
            {
                this.phSMSTo.Visible = this.phSMSFiledName.Visible = true;
            }
        }

        public void rblIsSMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phSMS.Visible = TranslateUtils.ToBool(this.rblIsSMS.SelectedValue);
        }

        public void rblIsSMSTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phSMSTemplate.Visible = TranslateUtils.ToBool(this.rblIsSMSTemplate.SelectedValue);
        }

        protected void IsAnomynous_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phLoginUrl.Visible = !TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
        }
    }
}
