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
	public class BackgroundInputMailSMS : BackgroundBasePage
	{
        public Literal ltlInputName;

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

        private InputInfo inputInfo;
        public int GetItemID()
        {
            return this.inputInfo.ClassifyID;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("InputID");

            int inputID = TranslateUtils.ToInt(base.GetQueryString("InputID"));
            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "邮件/短信发送设置", AppManager.CMS.Permission.WebSite.Input);

                this.ltlInputName.Text = this.inputInfo.InputName;

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMail, this.inputInfo.Additional.IsMail.ToString());
                this.rblIsMail_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblMailReceiver, ETriStateUtils.GetValue(this.inputInfo.Additional.MailReceiver));
                this.rblMailReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailTo.Text = this.inputInfo.Additional.MailTo;

                this.ltlTips1.Text = "[addDate]代表提交时间，[ipAddress]代表IP地址，";

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, this.inputInfo.InputID);
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, inputInfo.Additional.MailFiledName))
                    {
                        listItem.Selected = true;
                    }
                    this.ddlMailFiledName.Items.Add(listItem);

                    this.ltlTips1.Text += string.Format(@"[{0}]代表{1}，", styleInfo.AttributeName, styleInfo.DisplayName);
                }

                this.ltlTips1.Text = this.ltlTips1.Text.TrimEnd('，');
                this.ltlTips2.Text = this.ltlTips1.Text;
                
                this.tbMailTitle.Text = this.inputInfo.Additional.MailTitle;

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMailTemplate, this.inputInfo.Additional.IsMailTemplate.ToString());
                this.rblIsMailTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbMailContent.Text = this.inputInfo.Additional.MailContent;

                if (string.IsNullOrEmpty(this.tbMailContent.Text))
                {
                    this.tbMailContent.Text = MessageManager.GetMailContent(tableStyleInfoArrayList);
                }

                //短信

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMS, this.inputInfo.Additional.IsSMS.ToString());
                this.rblIsSMS_SelectedIndexChanged(null, EventArgs.Empty);

                ControlUtils.SelectListItemsIgnoreCase(this.rblSMSReceiver, ETriStateUtils.GetValue(this.inputInfo.Additional.SMSReceiver));
                this.rblSMSReceiver_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSTo.Text = this.inputInfo.Additional.SMSTo;

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, inputInfo.Additional.SMSFiledName))
                    {
                        listItem.Selected = true;
                    }
                    this.ddlSMSFiledName.Items.Add(listItem);
                }

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSMSTemplate, this.inputInfo.Additional.IsSMSTemplate.ToString());
                this.rblIsSMSTemplate_SelectedIndexChanged(null, EventArgs.Empty);

                this.tbSMSContent.Text = this.inputInfo.Additional.SMSContent;

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
                this.inputInfo.Additional.IsMail = TranslateUtils.ToBool(this.rblIsMail.SelectedValue);
                this.inputInfo.Additional.MailReceiver = ETriStateUtils.GetEnumType(this.rblMailReceiver.SelectedValue);
                this.inputInfo.Additional.MailTo = this.tbMailTo.Text;
                this.inputInfo.Additional.MailFiledName = this.ddlMailFiledName.SelectedValue;
                this.inputInfo.Additional.MailTitle = this.tbMailTitle.Text;
                this.inputInfo.Additional.IsMailTemplate = TranslateUtils.ToBool(this.rblIsMailTemplate.SelectedValue);
                this.inputInfo.Additional.MailContent = this.tbMailContent.Text;

                this.inputInfo.Additional.IsSMS = TranslateUtils.ToBool(this.rblIsSMS.SelectedValue);
                this.inputInfo.Additional.SMSReceiver = ETriStateUtils.GetEnumType(this.rblSMSReceiver.SelectedValue);
                this.inputInfo.Additional.SMSTo = this.tbSMSTo.Text;
                this.inputInfo.Additional.SMSFiledName = this.ddlSMSFiledName.SelectedValue;
                this.inputInfo.Additional.IsSMSTemplate = TranslateUtils.ToBool(this.rblIsSMSTemplate.SelectedValue);
                this.inputInfo.Additional.SMSContent = this.tbSMSContent.Text;

                try
                {
                    DataProvider.InputDAO.Update(this.inputInfo);
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
