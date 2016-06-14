using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class KeywordPreview : BackgroundBasePage
	{
        public TextBox tbWeiXin;

        private int keywordID;

        public static string GetOpenWindowString(int publishmentSystemID, int keywordID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("keywordID", keywordID.ToString());
            return PageUtilityWX.GetOpenWindowString("预览", "modal_keywordPreview.aspx", arguments, 400, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.keywordID = TranslateUtils.ToInt(base.GetQueryString("keywordID"));

			if (!IsPostBack)
			{
                //this.tbWeiXin.Text = keywordInfo.Keywords;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(this.keywordID);

                base.SuccessMessage("发送预览成功，请留意您的手机微信提醒");

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}
	}
}
