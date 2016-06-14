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
	public class TagStyleResumeAdd : BackgroundBasePage
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

        protected TextBox MessageSuccess;
        protected TextBox MessageFailure;

        protected RadioButtonList IsAnomynous;

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
                        TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(styleInfo.SettingsXML);
                        this.StyleName.Text = styleInfo.StyleName;
                        
                        ControlUtils.SelectListItems(this.IsMail, resumeInfo.IsMail.ToString());
                        if (resumeInfo.IsMail)
                        {
                            this.phMail.Visible = true;
                        }
                        this.MailTo.Text = resumeInfo.MailTo;
                        this.MailTitleFormat.Text = resumeInfo.MailTitleFormat;

                        ControlUtils.SelectListItems(this.IsSMS, resumeInfo.IsSMS.ToString());
                        if (resumeInfo.IsSMS)
                        {
                            this.phSMS.Visible = true;
                        }
                        this.SMSTo.Text = resumeInfo.SMSTo;
                        this.SMSTitle.Text = resumeInfo.SMSTitle;

                        this.MessageSuccess.Text = resumeInfo.MessageSuccess;
                        this.MessageFailure.Text = resumeInfo.MessageFailure;

                        ControlUtils.SelectListItems(this.IsAnomynous, resumeInfo.IsAnomynous.ToString());
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
                        TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(styleInfo.SettingsXML);

                        styleInfo.StyleName = this.StyleName.Text;

                        resumeInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        resumeInfo.MailTo = this.MailTo.Text;
                        resumeInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        resumeInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        resumeInfo.SMSTo = this.SMSTo.Text;
                        resumeInfo.SMSTitle = this.SMSTitle.Text;

                        resumeInfo.MessageSuccess = this.MessageSuccess.Text;
                        resumeInfo.MessageFailure = this.MessageFailure.Text;

                        resumeInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);

                        styleInfo.SettingsXML = resumeInfo.ToString();
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改简历提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "简历提交样式修改失败！");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlResume.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("简历提交样式添加失败，简历提交样式名称已存在！");
				}
				else
				{
					try
					{
                        styleInfo = new TagStyleInfo();
                        TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(string.Empty);

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlResume.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        resumeInfo.IsMail = TranslateUtils.ToBool(this.IsMail.SelectedValue);
                        resumeInfo.MailTo = this.MailTo.Text;
                        resumeInfo.MailTitleFormat = this.MailTitleFormat.Text;

                        resumeInfo.IsSMS = TranslateUtils.ToBool(this.IsSMS.SelectedValue);
                        resumeInfo.SMSTo = this.SMSTo.Text;
                        resumeInfo.SMSTitle = this.SMSTitle.Text;

                        resumeInfo.MessageSuccess = this.MessageSuccess.Text;
                        resumeInfo.MessageFailure = this.MessageFailure.Text;

                        resumeInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);

                        styleInfo.SettingsXML = resumeInfo.ToString();

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加简历提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "简历提交样式添加失败！");
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
