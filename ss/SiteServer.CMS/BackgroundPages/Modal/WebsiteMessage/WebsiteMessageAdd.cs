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

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class WebsiteMessageAdd : BackgroundBasePage
	{
        protected TextBox WebsiteMessageName;
        protected RadioButtonList IsChecked;
        protected RadioButtonList IsReply;

        protected TextBox MessageSuccess;
        protected TextBox MessageFailure;

        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;
        protected RadioButtonList IsSuccessHide;
        protected RadioButtonList IsSuccessReload;
        protected RadioButtonList IsCtrlEnter;

        private bool isPreview;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加留言", "modal_websiteMessageAdd.aspx", arguments, 560, 510);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int websiteMessageID, bool isPreview)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
            arguments.Add("IsPreview", isPreview.ToString());
            return PageUtility.GetOpenWindowString("修改留言", "modal_websiteMessageAdd.aspx", arguments, 560, 510);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.isPreview = TranslateUtils.ToBool(base.GetQueryString("IsPreview"));

			if (!IsPostBack)
			{
                if (base.GetQueryString("WebsiteMessageID") != null)
                {
                    int websiteMessageID = TranslateUtils.ToInt(base.GetQueryString("WebsiteMessageID"));
                    WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
                    if (websiteMessageInfo != null)
                    {
                        this.WebsiteMessageName.Text = websiteMessageInfo.WebsiteMessageName;
                        ControlUtils.SelectListItems(this.IsChecked, websiteMessageInfo.IsChecked.ToString());
                        ControlUtils.SelectListItems(this.IsReply, websiteMessageInfo.IsReply.ToString());

                        this.MessageSuccess.Text = websiteMessageInfo.Additional.MessageSuccess;
                        this.MessageFailure.Text = websiteMessageInfo.Additional.MessageFailure;

                        ControlUtils.SelectListItems(this.IsAnomynous, websiteMessageInfo.Additional.IsAnomynous.ToString());
                        ControlUtils.SelectListItems(this.IsValidateCode, websiteMessageInfo.Additional.IsValidateCode.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessHide, websiteMessageInfo.Additional.IsSuccessHide.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessReload, websiteMessageInfo.Additional.IsSuccessReload.ToString());
                        ControlUtils.SelectListItems(this.IsCtrlEnter, websiteMessageInfo.Additional.IsCtrlEnter.ToString());
                    }
                }
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
            bool isNameChanged = true;
            WebsiteMessageInfo websiteMessageInfo = null;
				
			if (base.GetQueryString("WebsiteMessageID") != null)
			{
				try
				{
                    int websiteMessageID = TranslateUtils.ToInt(base.GetQueryString("WebsiteMessageID"));
                    websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
                    if (websiteMessageInfo != null)
                    {
                        if (websiteMessageInfo.WebsiteMessageName == this.WebsiteMessageName.Text)
                        {
                            isNameChanged = false;
                        }
                        else
                        {
                            websiteMessageInfo.WebsiteMessageName = this.WebsiteMessageName.Text;
                        }
                        websiteMessageInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                        websiteMessageInfo.IsReply = TranslateUtils.ToBool(this.IsReply.SelectedValue);

                        websiteMessageInfo.Additional.MessageSuccess = this.MessageSuccess.Text;
                        websiteMessageInfo.Additional.MessageFailure = this.MessageFailure.Text;

                        websiteMessageInfo.Additional.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        websiteMessageInfo.Additional.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        websiteMessageInfo.Additional.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        websiteMessageInfo.Additional.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        websiteMessageInfo.Additional.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);
                    }
                    DataProvider.WebsiteMessageDAO.Update(websiteMessageInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改留言", string.Format("留言:{0}", websiteMessageInfo.WebsiteMessageName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "留言修改失败！");
				}
			}
			else
			{
                ArrayList websiteMessageNameArrayList = DataProvider.WebsiteMessageDAO.GetWebsiteMessageNameArrayList(base.PublishmentSystemID);
                if (websiteMessageNameArrayList.IndexOf(this.WebsiteMessageName.Text) != -1)
				{
                    base.FailMessage("留言添加失败，留言名称已存在！");
				}
				else
				{
					try
					{
                        websiteMessageInfo = new WebsiteMessageInfo();
                        websiteMessageInfo.WebsiteMessageName = this.WebsiteMessageName.Text;
                        websiteMessageInfo.PublishmentSystemID = base.PublishmentSystemID;
                        websiteMessageInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                        websiteMessageInfo.IsReply = TranslateUtils.ToBool(this.IsReply.SelectedValue);

                        websiteMessageInfo.Additional.MessageSuccess = this.MessageSuccess.Text;
                        websiteMessageInfo.Additional.MessageFailure = this.MessageFailure.Text;

                        websiteMessageInfo.Additional.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        websiteMessageInfo.Additional.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        websiteMessageInfo.Additional.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        websiteMessageInfo.Additional.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        websiteMessageInfo.Additional.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                        DataProvider.WebsiteMessageDAO.Insert(websiteMessageInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加留言", string.Format("留言:{0}", websiteMessageInfo.WebsiteMessageName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "留言添加失败！");
					}
				}
			}

			if (isChanged)
			{
                if (this.isPreview)
                {
                    JsUtils.OpenWindow.CloseModalPage(Page);
                }
                else
                {
                    if (isNameChanged)
                    {
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}&RefreshLeft=True", base.PublishmentSystemID)));
                    }
                    else
                    {
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                    }
                }
			}
		}
	}
}
