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
    public class BackgroundCouponAct : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnCouponAdd;
        public Button btnCoupon;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_couponAct.aspx?PublishmentSystemID={0}", publishmentSystemID));
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
                        DataProviderWX.CouponActDAO.Delete(list);
                        base.SuccessMessage("活动删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "活动删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CouponActDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = CouponActAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Coupon, "优惠劵活动", AppManager.WeiXin.Permission.WebSite.Coupon);
                this.spContents.DataBind();

                string urlAdd = BackgroundCouponActAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                this.btnCouponAdd.Attributes.Add("onclick", Modal.CouponAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));

                string urlCoupon = BackgroundCoupon.GetRedirectUrl(base.PublishmentSystemID);
                this.btnCoupon.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlCoupon));

                string urlDelete = PageUtils.AddQueryString(BackgroundCouponAct.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的优惠劵活动", "此操作将删除所选优惠劵活动，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CouponActInfo actInfo = new CouponActInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlStartDate = e.Item.FindControl("ltlStartDate") as Literal;
                Literal ltlEndDate = e.Item.FindControl("ltlEndDate") as Literal;
                Literal ltlUserCount = e.Item.FindControl("ltlUserCount") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlCoupons = e.Item.FindControl("ltlCoupons") as Literal;
                Literal ltlRelate = e.Item.FindControl("ltlRelate") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = actInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(actInfo.KeywordID);
                ltlStartDate.Text = DateUtils.GetDateAndTimeString(actInfo.StartDate);
                ltlEndDate.Text = DateUtils.GetDateAndTimeString(actInfo.EndDate);
                ltlUserCount.Text = actInfo.UserCount.ToString();
                ltlPVCount.Text = actInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!actInfo.IsDisabled);

                List<CouponInfo> couponInfoList = DataProviderWX.CouponDAO.GetCouponInfoList(base.PublishmentSystemID, actInfo.ID);
                foreach (CouponInfo couponInfo in couponInfoList)
                {
                    ltlCoupons.Text += string.Format(@"<a href=""{0}"">{1}</a>&nbsp;&nbsp;" + ",", BackgroundCouponSN.GetRedirectUrl(base.PublishmentSystemID, couponInfo.ID, BackgroundCouponAct.GetRedirectUrl(base.PublishmentSystemID)), couponInfo.Title);
                }
                if (ltlCoupons.Text.Length > 0)
                {
                    ltlCoupons.Text = ltlCoupons.Text.Substring(0, ltlCoupons.Text.Length - 1);
                }
                ltlRelate.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">关联优惠劵</a>", Modal.CouponRelated.GetOpenWindowString(base.PublishmentSystemID, actInfo.ID));

                if (couponInfoList.Count > 0)
                {
                    string urlPreview = CouponManager.GetCouponHoldUrl(base.PublishmentSystemInfo, actInfo.ID);
                    urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                    ltlPreviewUrl.Text = string.Format(@"<a target=""_blank"" href=""{0}"">预览</a>", urlPreview);
                }

                string urlEdit = BackgroundCouponActAdd.GetRedirectUrl(base.PublishmentSystemID, actInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
