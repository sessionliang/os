using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ExpireAdd : BackgroundBasePage
	{
        public RadioButtonList rblIsExpire;
        public PlaceHolder phIsExpire;
        public DateTimeTextBox tbExpireDate;
        public TextBox tbExpireReason;

        private int urlID;
        private string returnUrl;

        public static string GetShowPopWinString(int urlID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("urlID", urlID.ToString());
            arguments.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return JsUtils.OpenWindow.GetOpenWindowString("设置许可限制", "modal_product_expireAdd.aspx", arguments, 550, 600);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.urlID = TranslateUtils.ToInt(base.Request.QueryString["urlID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.rblIsExpire, "开启许可限制", "关闭许可限制");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsExpire, true.ToString());

                if (this.urlID > 0)
                {
                    UrlInfo urlInfo = DataProvider.UrlDAO.GetUrlInfo(this.urlID);
                    if (urlInfo != null)
                    {
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsExpire, urlInfo.IsExpire.ToString());
                        this.tbExpireDate.DateTime = urlInfo.ExpireDate;
                        this.tbExpireReason.Text = urlInfo.ExpireReason;
                    }
                }

                this.rblIsExpire_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsExpire_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsExpire.Visible = TranslateUtils.ToBool(this.rblIsExpire.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
				
			if (this.urlID > 0)
			{
                DataProvider.UrlDAO.Update(TranslateUtils.ToBool(this.rblIsExpire.SelectedValue), this.tbExpireDate.DateTime, this.tbExpireReason.Text, this.urlID);

                isChanged = true;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
			}
		}
	}
}
