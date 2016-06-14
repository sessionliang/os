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
	public class TagStyleGovPublicApplyAdd : BackgroundBasePage
	{
        protected TextBox StyleName;

        protected RadioButtonList IsMail;
        protected PlaceHolder phMail;
        public TextBox MailTo;
        public TextBox MailTitleFormat;

        protected RadioButtonList IsSMS;
        protected PlaceHolder phSMS;
        public TextBox SMSTo;
        public TextBox SMSTitle;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                if (base.GetQueryString("StyleID") != null)
                {
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleGovPublicApplyInfo applyInfo = new TagStyleGovPublicApplyInfo(styleInfo.SettingsXML);
                        this.StyleName.Text = styleInfo.StyleName;

                        ControlUtils.SelectListItems(this.IsMail, applyInfo.IsMail.ToString());
                        if (applyInfo.IsMail)
                        {
                            this.phMail.Visible = true;
                        }
                        this.MailTo.Text = applyInfo.MailTo;
                        this.MailTitleFormat.Text = applyInfo.MailTitleFormat;

                        ControlUtils.SelectListItems(this.IsSMS, applyInfo.IsSMS.ToString());
                        if (applyInfo.IsSMS)
                        {
                            this.phSMS.Visible = true;
                        }
                        this.SMSTo.Text = applyInfo.SMSTo;
                        this.SMSTitle.Text = applyInfo.SMSTitle;
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
                        TagStyleGovPublicApplyInfo applyInfo = new TagStyleGovPublicApplyInfo(styleInfo.SettingsXML);

                        styleInfo.StyleName = this.StyleName.Text;

                        applyInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        applyInfo.MailTo = this.MailTo.Text;
                        applyInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        applyInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        applyInfo.SMSTo = this.SMSTo.Text;
                        applyInfo.SMSTitle = this.SMSTitle.Text;

                        styleInfo.SettingsXML = applyInfo.ToString();
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改依申请公开提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "依申请公开提交样式修改失败！");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlGovPublicApply.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("依申请公开提交样式添加失败，依申请公开提交样式名称已存在！");
				}
				else
				{
					try
					{
                        styleInfo = new TagStyleInfo();
                        TagStyleGovPublicApplyInfo applyInfo = new TagStyleGovPublicApplyInfo(string.Empty);

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlGovPublicApply.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        applyInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        applyInfo.MailTo = this.MailTo.Text;
                        applyInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        applyInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        applyInfo.SMSTo = this.SMSTo.Text;
                        applyInfo.SMSTitle = this.SMSTitle.Text;

                        styleInfo.SettingsXML = applyInfo.ToString();

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加依申请公开提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "依申请公开提交样式添加失败！");
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
