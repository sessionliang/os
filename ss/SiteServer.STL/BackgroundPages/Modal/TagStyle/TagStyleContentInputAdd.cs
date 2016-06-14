using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class TagStyleContentInputAdd : BackgroundBasePage
	{
        protected TextBox StyleName;
        protected DropDownList ChannelID;
        protected RadioButtonList IsChecked;

        protected RadioButtonList IsMail;
        protected PlaceHolder phMail;
        public TextBox MailTo;
        public TextBox MailTitleFormat;

        protected RadioButtonList IsSMS;
        protected PlaceHolder phSMS;
        public TextBox SMSTo;
        public TextBox SMSTitle;

        protected TextBox MessageSuccess;
        protected TextBox MessageFailure;

        protected RadioButtonList IsChannel;
        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;
        protected RadioButtonList IsSuccessHide;
        protected RadioButtonList IsSuccessReload;
        protected RadioButtonList IsCtrlEnter;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                NodeManager.AddListItems(this.ChannelID.Items, base.PublishmentSystemInfo, false, false);

                if (base.GetQueryString("StyleID") != null)
                {
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(styleInfo.SettingsXML);
                        this.StyleName.Text = styleInfo.StyleName;
                        ControlUtils.SelectListItems(this.ChannelID, inputInfo.ChannelID.ToString());
                        ControlUtils.SelectListItems(this.IsChecked, inputInfo.IsChecked.ToString());

                        ControlUtils.SelectListItems(this.IsMail, inputInfo.IsMail.ToString());
                        if (inputInfo.IsMail)
                        {
                            this.phMail.Visible = true;
                        }
                        this.MailTo.Text = inputInfo.MailTo;
                        this.MailTitleFormat.Text = inputInfo.MailTitleFormat;

                        ControlUtils.SelectListItems(this.IsSMS, inputInfo.IsSMS.ToString());
                        if (inputInfo.IsSMS)
                        {
                            this.phSMS.Visible = true;
                        }
                        this.SMSTo.Text = inputInfo.SMSTo;
                        this.SMSTitle.Text = inputInfo.SMSTitle;

                        this.MessageSuccess.Text = inputInfo.MessageSuccess;
                        this.MessageFailure.Text = inputInfo.MessageFailure;

                        ControlUtils.SelectListItems(this.IsChannel, inputInfo.IsChannel.ToString());
                        ControlUtils.SelectListItems(this.IsAnomynous, inputInfo.IsAnomynous.ToString());
                        ControlUtils.SelectListItems(this.IsValidateCode, inputInfo.IsValidateCode.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessHide, inputInfo.IsSuccessHide.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessReload, inputInfo.IsSuccessReload.ToString());
                        ControlUtils.SelectListItems(this.IsCtrlEnter, inputInfo.IsCtrlEnter.ToString());
                    }
                }
				
			}
		}

        public void IsMail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base.IsPostBack)
            {
                this.phMail.Visible = TranslateUtils.ToBool(this.IsMail.SelectedValue);
            }
        }

        public void IsSMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base.IsPostBack)
            {
                this.phSMS.Visible = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
            TagStyleInfo styleInfo = null;
				
			if (base.GetQueryString("StyleID") != null)
			{
				try
				{
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(styleInfo.SettingsXML);

                        styleInfo.StyleName = this.StyleName.Text;
                        inputInfo.ChannelID = TranslateUtils.ToInt(this.ChannelID.SelectedValue);
                        inputInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);

                        inputInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        inputInfo.MailTo = this.MailTo.Text;
                        inputInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        inputInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        inputInfo.SMSTo = this.SMSTo.Text;
                        inputInfo.SMSTitle = this.SMSTitle.Text;

                        inputInfo.MessageSuccess = this.MessageSuccess.Text;
                        inputInfo.MessageFailure = this.MessageFailure.Text;

                        inputInfo.IsChannel = TranslateUtils.ToBool(this.IsChannel.SelectedValue);
                        inputInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        inputInfo.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        inputInfo.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        inputInfo.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        inputInfo.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                        styleInfo.SettingsXML = inputInfo.ToString();
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改内容提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "内容提交样式修改失败！");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlContentInput.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("内容提交样式添加失败，内容提交样式名称已存在！");
				}
				else
				{
					try
					{
                        styleInfo = new TagStyleInfo();
                        TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(string.Empty);

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlContentInput.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        inputInfo.ChannelID = TranslateUtils.ToInt(this.ChannelID.SelectedValue);
                        inputInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);

                        inputInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        inputInfo.MailTo = this.MailTo.Text;
                        inputInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        inputInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        inputInfo.SMSTo = this.SMSTo.Text;
                        inputInfo.SMSTitle = this.SMSTitle.Text;

                        inputInfo.MessageSuccess = this.MessageSuccess.Text;
                        inputInfo.MessageFailure = this.MessageFailure.Text;

                        inputInfo.IsChannel = TranslateUtils.ToBool(this.IsChannel.SelectedValue);
                        inputInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        inputInfo.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        inputInfo.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        inputInfo.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        inputInfo.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                        styleInfo.SettingsXML = inputInfo.ToString();

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加内容提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "内容提交样式添加失败！");
					}
				}
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
