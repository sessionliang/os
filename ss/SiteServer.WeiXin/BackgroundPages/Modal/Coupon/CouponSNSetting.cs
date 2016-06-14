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
    public class CouponSNSetting : BackgroundBasePage
	{
        public DropDownList ddlStatus;

        private int couponID;

        public static string GetOpenWindowString(int publishmentSystemID, int couponID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("couponID", couponID.ToString());
            return PageUtilityWX.GetOpenWindowStringWithCheckBoxValue("�����Ż݄�����", "modal_couponSNSetting.aspx", arguments, "IDCollection", "��ѡ����Ҫ���õ�SN��", 400, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.couponID = TranslateUtils.ToInt(base.GetQueryString("couponID"));

			if (!IsPostBack)
			{
                ECouponStatusUtils.AddListItems(this.ddlStatus);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {

                DataProviderWX.CouponSNDAO.UpdateStatus(ECouponStatusUtils.GetEnumType(this.ddlStatus.SelectedValue), TranslateUtils.StringCollectionToIntList(base.Request.QueryString["IDCollection"]));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "ʧ�ܣ�" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}
	}
}
