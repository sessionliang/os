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
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class TagStyleCommentsAdd : BackgroundBasePage
	{
        protected RadioButtonList IsPage;
        protected PlaceHolder phPageNum;
        protected TextBox PageNum;
        protected PlaceHolder phTotalNum;
        protected TextBox TotalNum;
        protected RadioButtonList IsLinkToAll;
        protected RadioButtonList IsReference;
        protected RadioButtonList IsDigg;
        protected RadioButtonList IsLocation;
        protected RadioButtonList IsIPAddress;

        private int styleID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
			if (!IsPostBack)
			{
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
                TagStyleCommentsInfo commentsInfo = new TagStyleCommentsInfo(styleInfo.SettingsXML);

                ControlUtils.SelectListItems(this.IsPage, commentsInfo.IsPage.ToString());
                this.phPageNum.Visible = commentsInfo.IsPage;
                this.PageNum.Text = commentsInfo.PageNum.ToString();
                this.phTotalNum.Visible = !commentsInfo.IsPage;
                this.TotalNum.Text = commentsInfo.TotalNum.ToString();
                ControlUtils.SelectListItems(this.IsLinkToAll, commentsInfo.IsLinkToAll.ToString());
                ControlUtils.SelectListItems(this.IsReference, commentsInfo.IsReference.ToString());
                ControlUtils.SelectListItems(this.IsDigg, commentsInfo.IsDigg.ToString());
                ControlUtils.SelectListItems(this.IsLocation, commentsInfo.IsLocation.ToString());
                ControlUtils.SelectListItems(this.IsIPAddress, commentsInfo.IsIPAddress.ToString());

				
			}
		}

        public void IsPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phPageNum.Visible = TranslateUtils.ToBool(this.IsPage.SelectedValue);
            this.phTotalNum.Visible = !this.phPageNum.Visible;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
                if (styleInfo != null)
                {
                    TagStyleCommentsInfo commentsInfo = new TagStyleCommentsInfo(styleInfo.SettingsXML);

                    commentsInfo.IsPage = TranslateUtils.ToBool(this.IsPage.SelectedValue);
                    commentsInfo.PageNum = TranslateUtils.ToInt(this.PageNum.Text);
                    commentsInfo.TotalNum = TranslateUtils.ToInt(this.TotalNum.Text);
                    commentsInfo.IsLinkToAll = TranslateUtils.ToBool(this.IsLinkToAll.SelectedValue);
                    commentsInfo.IsReference = TranslateUtils.ToBool(this.IsReference.SelectedValue);
                    commentsInfo.IsDigg = TranslateUtils.ToBool(this.IsDigg.SelectedValue);
                    commentsInfo.IsLocation = TranslateUtils.ToBool(this.IsLocation.SelectedValue);
                    commentsInfo.IsIPAddress = TranslateUtils.ToBool(this.IsIPAddress.SelectedValue);

                    styleInfo.SettingsXML = commentsInfo.ToString();
                }
                DataProvider.TagStyleDAO.Update(styleInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "修改评论列表样式", string.Format("样式名称:{0}", styleInfo.StyleName));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "评论列表样式修改失败！");
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
