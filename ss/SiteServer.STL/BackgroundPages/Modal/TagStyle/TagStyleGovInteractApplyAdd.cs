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
	public class TagStyleGovInteractApplyAdd : BackgroundBasePage
	{
        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;

        private int styleID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));

			if (!IsPostBack)
			{
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                TagStyleGovInteractApplyInfo applyInfo = new TagStyleGovInteractApplyInfo(styleInfo.SettingsXML);

                ControlUtils.SelectListItems(this.IsAnomynous, applyInfo.IsAnomynous.ToString());
                ControlUtils.SelectListItems(this.IsValidateCode, applyInfo.IsValidateCode.ToString());
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                TagStyleGovInteractApplyInfo applyInfo = new TagStyleGovInteractApplyInfo(styleInfo.SettingsXML);

                applyInfo.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                applyInfo.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);

                styleInfo.SettingsXML = applyInfo.ToString();

                DataProvider.TagStyleDAO.Update(styleInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "修改互动交流提交样式", string.Format("样式名称:{0}", styleInfo.StyleName));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "互动交流提交样式修改失败！");
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
