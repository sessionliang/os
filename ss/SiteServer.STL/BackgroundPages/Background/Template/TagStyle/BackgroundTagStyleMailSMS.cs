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

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStyleMailSMS : BackgroundBasePage
	{
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

        public Literal ltlTips1;
        public Literal ltlTips2;

        private int styleID;
        private ETableStyle tableStyle;
        private ITagStyleMailSMSBaseInfo mailSMSInfo;

        public static string GetRedirectUrl(int publishmentSystemID, int styleID, ETableStyle tableStyle, int relatedIdentity)
        {
            return PageUtils.GetSTLUrl(string.Format("background_tagStyleMailSMS.aspx?PublishmentSystemID={0}&StyleID={1}&TableStyle={2}&RelatedIdentity={3}", publishmentSystemID, styleID, ETableStyleUtils.GetValue(tableStyle), relatedIdentity));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.styleID = base.GetIntQueryString("StyleID");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            int relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            TagStyleInfo tagStyleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
            if (tableStyle == ETableStyle.GovInteractContent)
            {
                this.mailSMSInfo = new TagStyleGovInteractApplyInfo(tagStyleInfo.SettingsXML);
            }

			if (!IsPostBack)
			{
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMail, this.mailSMSInfo.IsMail.ToString());
                this.rblIsMail_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblMailReceiver, ETriStateUtils.GetValue(this.mailSMSInfo.MailReceiver));
                this.rblMailReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailTo.Text = this.mailSMSInfo.MailTo;

                this.ltlTips1.Text = string.Format("[{0}]代表提交时间，[{1}]代表查询码，", ContentAttribute.AddDate, GovInteractContentAttribute.QueryCode);

                ArrayList tableStyleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(base.PublishmentSystemInfo, tableStyle, relatedIdentity);
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (styleInfo.IsVisible)
                    {
                        ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, this.mailSMSInfo.MailFiledName))
                        {
                            listItem.Selected = true;
                        }
                        this.ddlMailFiledName.Items.Add(listItem);

                        this.ltlTips1.Text += string.Format(@"[{0}]代表{1}，", styleInfo.AttributeName, styleInfo.DisplayName);
                    }
                }

                this.ltlTips1.Text = this.ltlTips1.Text.TrimEnd('，');
                this.ltlTips2.Text = this.ltlTips1.Text;

                this.tbMailTitle.Text = this.mailSMSInfo.MailTitle;

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMailTemplate, this.mailSMSInfo.IsMailTemplate.ToString());
                this.rblIsMailTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailContent.Text = this.mailSMSInfo.MailContent;

                if (string.IsNullOrEmpty(this.tbMailContent.Text))
                {
                    this.tbMailContent.Text = MessageManager.GetMailContent(tableStyleInfoArrayList);
                }

                //短信

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMS, this.mailSMSInfo.IsSMS.ToString());
                this.rblIsSMS_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblSMSReceiver, ETriStateUtils.GetValue(this.mailSMSInfo.SMSReceiver));
                this.rblSMSReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSTo.Text = this.mailSMSInfo.SMSTo;

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (styleInfo.IsVisible)
                    {
                        ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, this.mailSMSInfo.SMSFiledName))
                        {
                            listItem.Selected = true;
                        }
                        this.ddlSMSFiledName.Items.Add(listItem);
                    }
                }

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMSTemplate, this.mailSMSInfo.IsSMSTemplate.ToString());
                this.rblIsSMSTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSContent.Text = this.mailSMSInfo.SMSContent;

                if (string.IsNullOrEmpty(this.tbSMSContent.Text))
                {
                    this.tbSMSContent.Text = MessageManager.GetSMSContent(tableStyleInfoArrayList);
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.mailSMSInfo.IsMail = TranslateUtils.ToBool(this.rblIsMail.SelectedValue);
                this.mailSMSInfo.MailReceiver = ETriStateUtils.GetEnumType(this.rblMailReceiver.SelectedValue);
                this.mailSMSInfo.MailTo = this.tbMailTo.Text;
                this.mailSMSInfo.MailFiledName = this.ddlMailFiledName.SelectedValue;
                this.mailSMSInfo.MailTitle = this.tbMailTitle.Text;
                this.mailSMSInfo.IsMailTemplate = TranslateUtils.ToBool(this.rblIsMailTemplate.SelectedValue);
                this.mailSMSInfo.MailContent = this.tbMailContent.Text;

                this.mailSMSInfo.IsSMS = TranslateUtils.ToBool(this.rblIsSMS.SelectedValue);
                this.mailSMSInfo.SMSReceiver = ETriStateUtils.GetEnumType(this.rblSMSReceiver.SelectedValue);
                this.mailSMSInfo.SMSTo = this.tbSMSTo.Text;
                this.mailSMSInfo.SMSFiledName = this.ddlSMSFiledName.SelectedValue;
                this.mailSMSInfo.IsSMSTemplate = TranslateUtils.ToBool(this.rblIsSMSTemplate.SelectedValue);
                this.mailSMSInfo.SMSContent = this.tbSMSContent.Text;

                try
                {
                    TagStyleInfo tagStyleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
                    tagStyleInfo.SettingsXML = this.mailSMSInfo.ToString();
                    DataProvider.TagStyleDAO.Update(tagStyleInfo);
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
	}
}
