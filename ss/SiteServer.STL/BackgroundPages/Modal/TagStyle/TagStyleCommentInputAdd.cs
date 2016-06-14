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
	public class TagStyleCommentInputAdd : BackgroundBasePage
	{
        protected RadioButtonList IsChecked;
        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;
        protected RadioButtonList IsSuccessHide;
        protected RadioButtonList IsSuccessReload;

        private int styleID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
			if (!IsPostBack)
			{
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
                if (styleInfo != null)
                {
                    TagStyleCommentInputInfo inputInfo = new TagStyleCommentInputInfo(styleInfo.SettingsXML);
                    ControlUtils.SelectListItems(this.IsChecked, inputInfo.IsChecked.ToString());
                    ControlUtils.SelectListItems(this.IsAnomynous, inputInfo.IsAnomynous.ToString());
                    ControlUtils.SelectListItems(this.IsValidateCode, inputInfo.IsValidateCode.ToString());
                    ControlUtils.SelectListItems(this.IsSuccessHide, inputInfo.IsSuccessHide.ToString());
                    ControlUtils.SelectListItems(this.IsSuccessReload, inputInfo.IsSuccessReload.ToString());
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                if (styleInfo != null)
                {
                    TagStyleCommentInputInfo inputInfo = new TagStyleCommentInputInfo(styleInfo.SettingsXML);

                    inputInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                    inputInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                    inputInfo.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                    inputInfo.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                    inputInfo.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);

                    styleInfo.SettingsXML = inputInfo.ToString();
                }
                DataProvider.TagStyleDAO.Update(styleInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "修改评论提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "评论提交样式修改失败！");
            }
			
			if (isChanged)
			{
                //InnerTemplateManager.CreateFiles(base.PublishmentSystemInfo, styleInfo);
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
