using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundPromotion : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button btnAdd;
        public Button btnEnable;
        public Button btnDisable;

        private List<int> validPromotionIDList;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("delete")))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.PromotionDAO.Delete(list);
                        base.SuccessMessage("打折促销删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "打折促销删除失败！");
                    }
                }
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("enable")))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.PromotionDAO.UpdateIsEnabled(list, true);
                        base.SuccessMessage("打折促销启用成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "打折促销启用失败！");
                    }
                }
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("disable")))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.PromotionDAO.UpdateIsEnabled(list, false);
                        base.SuccessMessage("打折促销禁用成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "打折促销禁用失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderB2C.PromotionDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = PromotionAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.B2C.LeftMenu.ID_ConfigrationB2C, "打折促销设置", string.Empty);

                this.validPromotionIDList = DataProviderB2C.PromotionDAO.GetEnabledPromotionIDList(base.PublishmentSystemID);
                this.spContents.DataBind();

                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundPromotionAdd.GetRedirectUrlToAdd(base.PublishmentSystemID)));
                this.btnEnable.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(string.Format("background_promotion.aspx?publishmentSystemID={0}&enable=true", base.PublishmentSystemID), "IDCollection", "IDCollection", "此操作将启用所选促销活动，确认吗？"));
                this.btnDisable.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(string.Format("background_promotion.aspx?publishmentSystemID={0}&disable=true", base.PublishmentSystemID), "IDCollection", "IDCollection", "此操作将禁用所选促销活动，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PromotionInfo promotionInfo = new PromotionInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlPromotionName = e.Item.FindControl("ltlPromotionName") as Literal;
                Literal ltlStartDate = e.Item.FindControl("ltlStartDate") as Literal;
                Literal ltlEndDate = e.Item.FindControl("ltlEndDate") as Literal;
                Literal ltlTags = e.Item.FindControl("ltlTags") as Literal;
                Literal ltlTarget = e.Item.FindControl("ltlTarget") as Literal;
                Literal ltlIf = e.Item.FindControl("ltlIf") as Literal;
                Literal ltlPromotion = e.Item.FindControl("ltlPromotion") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsValid = e.Item.FindControl("ltlIsValid") as Literal;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;
                Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlPromotionName.Text = promotionInfo.PromotionName;
                ltlStartDate.Text = DateUtils.GetDateAndTimeString(promotionInfo.StartDate);
                ltlEndDate.Text = DateUtils.GetDateAndTimeString(promotionInfo.EndDate);
                ltlTags.Text = promotionInfo.Tags;
                ltlTarget.Text = EPromotionTargetUtils.GetText(EPromotionTargetUtils.GetEnumType(promotionInfo.Target));
                ltlAddDate.Text = DateUtils.GetDateString(promotionInfo.AddDate);

                StringBuilder builder = new StringBuilder();
                if (promotionInfo.IfAmount > 0)
                {
                    builder.AppendFormat("满{0}元 ", promotionInfo.IfAmount);
                }
                if (promotionInfo.IfCount > 0)
                {
                    builder.AppendFormat("满{0}件 ", promotionInfo.IfCount);
                }
                ltlIf.Text = builder.ToString();
                builder.Length = 0;
                if (promotionInfo.ReturnAmount > 0)
                {
                    builder.AppendFormat("减{0}元{1} ", promotionInfo.ReturnAmount, promotionInfo.IsReturnMultiply ? "（上不封顶）" : string.Empty);
                }
                if (promotionInfo.Discount > 0)
                {
                    builder.AppendFormat("打{0}折 ", Convert.ToString(promotionInfo.Discount * 10).TrimEnd('0').TrimEnd('.'));
                }
                if (promotionInfo.IsShipmentFree)
                {
                    builder.Append("免运费 ");
                }
                if (promotionInfo.IsGift)
                {
                    builder.AppendFormat(@"送礼物（<a href=""{0}"" target=""_blank"">{1}</a>） ", promotionInfo.GiftUrl, promotionInfo.GiftName);
                }
                ltlPromotion.Text = builder.ToString();

                ltlIsValid.Text = this.validPromotionIDList.Contains(promotionInfo.ID) ? "有效促销" : "已失效";

                ltlEdit.Text = string.Format(@"<a href=""{0}"">修改</a>", BackgroundPromotionAdd.GetRedirectUrlToEdit(base.PublishmentSystemID, promotionInfo.ID));

                ltlDelete.Text = string.Format(@"<a href=""background_promotion.aspx?publishmentSystemID={0}&delete=true&IDCollection={1}"">删除</a>", base.PublishmentSystemID, promotionInfo.ID);
            }
        }
    }
}
