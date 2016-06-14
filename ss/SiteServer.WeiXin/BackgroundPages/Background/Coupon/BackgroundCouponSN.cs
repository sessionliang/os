using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Web;
using System.IO;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCouponSN : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnSetting;
        public Button btnReturn;
        public Button btnUpFile;

        private int couponID;
        private string returnUrl;


        public static string GetRedirectUrl(int publishmentSystemID, int couponID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_couponSN.aspx?publishmentSystemID={0}&couponID={1}&returnUrl={2}", publishmentSystemID, couponID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.couponID = TranslateUtils.ToInt(base.Request.QueryString["couponID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CouponSNDAO.Delete(list);
                        base.SuccessMessage("优惠劵删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "优惠劵删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CouponSNDAO.GetSelectString(base.PublishmentSystemID, this.couponID);
            this.spContents.SortField = CouponAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Coupon, "优惠劵明细", AppManager.WeiXin.Permission.WebSite.Coupon);
                this.spContents.DataBind();

                this.btnAdd.Attributes.Add("onclick", Modal.CouponSNAdd.GetOpenWindowString(base.PublishmentSystemID, this.couponID, 0));

                this.btnSetting.Attributes.Add("onclick", Modal.CouponSNSetting.GetOpenWindowString(base.PublishmentSystemID, this.couponID));

                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));

                this.btnUpFile.Attributes.Add("onclick", Modal.CouponSNAdd.GetOpenUploadWindowString(base.PublishmentSystemID, this.couponID, 1));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CouponSNInfo snInfo = new CouponSNInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlHoldDate = e.Item.FindControl("ltlHoldDate") as Literal;
                Literal ltlHoldMobile = e.Item.FindControl("ltlHoldMobile") as Literal;
                Literal ltlHoldEmail = e.Item.FindControl("ltlHoldEmail") as Literal;
                Literal ltlHoldRealName = e.Item.FindControl("ltlHoldRealName") as Literal;
                Literal ltlCashDate = e.Item.FindControl("ltlCashDate") as Literal;
                Literal ltlCashUserName = e.Item.FindControl("ltlCashUserName") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlSN.Text = snInfo.SN;
                ECouponStatus status = ECouponStatusUtils.GetEnumType(snInfo.Status);
                ltlStatus.Text = ECouponStatusUtils.GetText(status);
                if (status == ECouponStatus.Cash || status == ECouponStatus.Hold)
                {
                    ltlHoldDate.Text = DateUtils.GetDateAndTimeString(snInfo.HoldDate);
                    ltlHoldMobile.Text = snInfo.HoldMobile;
                    ltlHoldEmail.Text = snInfo.HoldEmail;
                    ltlHoldRealName.Text = snInfo.HoldRealName;
                }
                if (status == ECouponStatus.Cash)
                {
                    ltlCashDate.Text = DateUtils.GetDateAndTimeString(snInfo.CashDate);
                    ltlCashUserName.Text = snInfo.HoldRealName;
                }
            }
        }

    }
}
