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

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCoupon : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;
        public Button btnReturn;

        private string actTitle = null;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_coupon.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CouponDAO.Delete(list);
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
            this.spContents.SelectCommand = DataProviderWX.CouponDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = CouponAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Coupon, "优惠劵管理", AppManager.WeiXin.Permission.WebSite.Coupon);

                this.spContents.DataBind();

                this.btnAdd.Attributes.Add("onclick", Modal.CouponAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));

                string urlDelete = PageUtils.AddQueryString(BackgroundCoupon.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的优惠劵", "此操作将删除所选优惠劵，确认吗？"));

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundCouponAct.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                this.actTitle = null;

                CouponInfo couponInfo = new CouponInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlActTitle = e.Item.FindControl("ltlActTitle") as Literal;
                Literal ltlTotalNum = e.Item.FindControl("ltlTotalNum") as Literal;
                Literal ltlHoldNum = e.Item.FindControl("ltlHoldNum") as Literal;
                Literal ltlCashNum = e.Item.FindControl("ltlCashNum") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = couponInfo.Title;
                if (couponInfo.ActID > 0 && this.actTitle == null)
                {
                    this.actTitle = DataProviderWX.CouponActDAO.GetTitle(couponInfo.ActID);
                }
                if (this.actTitle != null)
                {
                    ltlActTitle.Text = actTitle;
                }
                ltlTotalNum.Text = couponInfo.TotalNum.ToString();
                ltlHoldNum.Text = DataProviderWX.CouponSNDAO.GetHoldNum(base.PublishmentSystemID, couponInfo.ID).ToString();
                ltlCashNum.Text = DataProviderWX.CouponSNDAO.GetCashNum(base.PublishmentSystemID, couponInfo.ID).ToString();
                ltlAddDate.Text = DateUtils.GetDateString(couponInfo.AddDate);

                ltlSN.Text = string.Format(@"<a href=""{0}"">优惠劵明细</a>", BackgroundCouponSN.GetRedirectUrl(base.PublishmentSystemID, couponInfo.ID, BackgroundCoupon.GetRedirectUrl(base.PublishmentSystemID)));

                string urlEdit = Modal.CouponAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, couponInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
