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
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CouponRelated : BackgroundBasePage
	{
        public CheckBoxList cblCoupon;

        private int actID;

        public static string GetOpenWindowString(int publishmentSystemID, int actID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("actID", actID.ToString());
            return PageUtilityWX.GetOpenWindowString("¹ØÁªÓÅ»Ý„»", "modal_couponRelated.aspx", arguments, 400, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.actID = TranslateUtils.ToInt(base.GetQueryString("actID"));

            List<int> actIDList = DataProviderWX.CouponActDAO.GetActIDList(base.PublishmentSystemID);

			if (!IsPostBack)
			{
                List<CouponInfo> allCouponInfoList = DataProviderWX.CouponDAO.GetAllCouponInfoList(base.PublishmentSystemID);
                foreach (CouponInfo couponInfo in allCouponInfoList)
                {
                    if (!actIDList.Contains(couponInfo.ActID) || couponInfo.ActID == 0 || couponInfo.ActID == this.actID)
                    {
                        ListItem listItem = new ListItem(couponInfo.Title, couponInfo.ID.ToString());
                        if (couponInfo.ActID == this.actID)
                        {
                            listItem.Selected = true;
                        }
                        this.cblCoupon.Items.Add(listItem);
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                foreach (ListItem listItem in this.cblCoupon.Items)
                {
                    int couponID = TranslateUtils.ToInt(listItem.Value);
                    int updateActID = listItem.Selected ? this.actID : 0;
                    DataProviderWX.CouponDAO.UpdateActID(couponID, updateActID);
                }

                //DataProviderWX.CouponSNDAO.UpdateStatus(ECouponStatusUtils.GetEnumType(this.ddlStatus.SelectedValue), TranslateUtils.StringCollectionToIntList(base.Request.QueryString["IDCollection"]));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "Ê§°Ü£º" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}
	}
}
