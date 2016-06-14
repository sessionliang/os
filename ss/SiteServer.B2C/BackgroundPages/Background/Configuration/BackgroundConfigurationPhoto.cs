using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Web.UI.HtmlControls;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundConfigurationPhoto : BackgroundBasePage
	{
        public TextBox PhotoSmallWidth;
        public TextBox PhotoSmallHeight;
        public TextBox PhotoMiddleWidth;
        public TextBox PhotoMiddleHeight;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            //base.VerifyWebsitePermissions(PredefinedCMSPermissions.WebSitePermisson.WebsiteConfigration);

			if (!IsPostBack)
			{
                this.PhotoSmallWidth.Text = base.PublishmentSystemInfo.Additional.PhotoSmallWidth.ToString();
                this.PhotoSmallHeight.Text = base.PublishmentSystemInfo.Additional.PhotoSmallHeight.ToString();

                this.PhotoMiddleWidth.Text = base.PublishmentSystemInfo.Additional.PhotoMiddleWidth.ToString();
                this.PhotoMiddleHeight.Text = base.PublishmentSystemInfo.Additional.PhotoMiddleHeight.ToString();
			}
		}

		public void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.PhotoSmallWidth = TranslateUtils.ToInt(this.PhotoSmallWidth.Text, base.PublishmentSystemInfo.Additional.PhotoSmallWidth);
                base.PublishmentSystemInfo.Additional.PhotoSmallHeight = TranslateUtils.ToInt(this.PhotoSmallHeight.Text, base.PublishmentSystemInfo.Additional.PhotoSmallHeight);

                base.PublishmentSystemInfo.Additional.PhotoMiddleWidth = TranslateUtils.ToInt(this.PhotoMiddleWidth.Text, base.PublishmentSystemInfo.Additional.PhotoMiddleWidth);
                base.PublishmentSystemInfo.Additional.PhotoMiddleHeight = TranslateUtils.ToInt(this.PhotoMiddleHeight.Text, base.PublishmentSystemInfo.Additional.PhotoMiddleHeight);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改商品图片尺寸设置");

                    base.SuccessMessage("商品图片尺寸设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "商品图片尺寸设置修改失败！");
				}
			}
		}
	}
}
