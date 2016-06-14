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
    public class BackgroundLottery : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        private ELotteryType lotteryType;

        public static string GetRedirectUrl(int publishmentSystemID, ELotteryType lotteryType)
        {
            return PageUtils.GetWXUrl(string.Format("background_lottery.aspx?PublishmentSystemID={0}&lotteryType={1}", publishmentSystemID, ELotteryTypeUtils.GetValue(lotteryType)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.lotteryType = ELotteryTypeUtils.GetEnumType(base.Request.QueryString["lotteryType"]);
            string lotteryName = ELotteryTypeUtils.GetText(this.lotteryType);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.LotteryDAO.Delete(base.PublishmentSystemID, list);

                        base.SuccessMessage(lotteryName + "删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, lotteryName + "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.LotteryDAO.GetSelectString(base.PublishmentSystemID, this.lotteryType);
            this.spContents.SortField = LotteryAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                if (this.lotteryType == ELotteryType.Scratch)
                {
                    base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Scratch, lotteryName, AppManager.WeiXin.Permission.WebSite.Scratch);
                }
                else if (this.lotteryType == ELotteryType.BigWheel)
                {
                    base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_BigWheel, lotteryName, AppManager.WeiXin.Permission.WebSite.BigWheel);
                }
                else if (this.lotteryType == ELotteryType.GoldEgg)
                {
                    base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_GoldEgg, lotteryName, AppManager.WeiXin.Permission.WebSite.GoldEgg);
                }
                else if (this.lotteryType == ELotteryType.Flap)
                {
                    base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Flap, lotteryName, AppManager.WeiXin.Permission.WebSite.Flap);
                }
                else if (this.lotteryType == ELotteryType.YaoYao)
                {
                    base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_YaoYao, lotteryName, AppManager.WeiXin.Permission.WebSite.YaoYao);
                }               

                this.spContents.DataBind();

                string urlAdd = string.Empty;
                if (this.lotteryType == ELotteryType.Scratch)
                {
                    urlAdd = BackgroundScratchAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                }
                else if (this.lotteryType == ELotteryType.BigWheel)
                {
                    urlAdd = BackgroundBigWheelAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                }
                else if (this.lotteryType == ELotteryType.GoldEgg)
                {
                    urlAdd = BackgroundGoldEggAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                }
                else if (this.lotteryType == ELotteryType.Flap)
                {
                    urlAdd = BackgroundFlapAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                }
                else if (this.lotteryType == ELotteryType.YaoYao)
                {
                    urlAdd = BackgroundYaoYaoAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                }
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                string urlDelete = PageUtils.AddQueryString(BackgroundLottery.GetRedirectUrl(base.PublishmentSystemID, this.lotteryType), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的" + lotteryName, "此操作将删除所选" + lotteryName + "，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LotteryInfo lotteryInfo = new LotteryInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlStartDate = e.Item.FindControl("ltlStartDate") as Literal;
                Literal ltlEndDate = e.Item.FindControl("ltlEndDate") as Literal;
                Literal ltlUserCount = e.Item.FindControl("ltlUserCount") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlWinner = e.Item.FindControl("ltlWinner") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = lotteryInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(lotteryInfo.KeywordID);
                ltlStartDate.Text = DateUtils.GetDateAndTimeString(lotteryInfo.StartDate);
                ltlEndDate.Text = DateUtils.GetDateAndTimeString(lotteryInfo.EndDate);
                ltlUserCount.Text = lotteryInfo.UserCount.ToString();
                ltlPVCount.Text = lotteryInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!lotteryInfo.IsDisabled);

                ltlWinner.Text = string.Format(@"<a href=""{0}"">查看获奖名单</a>", BackgroundLotteryWinner.GetRedirectUrl(base.PublishmentSystemID, this.lotteryType, lotteryInfo.ID, 0, BackgroundLottery.GetRedirectUrl(base.PublishmentSystemID, this.lotteryType)));

                string urlPreview = LotteryManager.GetLotteryUrl(lotteryInfo, string.Empty);
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = string.Empty;
                if (this.lotteryType == ELotteryType.Scratch)
                {
                    urlEdit = BackgroundScratchAdd.GetRedirectUrl(base.PublishmentSystemID, lotteryInfo.ID);
                }
                else if (this.lotteryType == ELotteryType.BigWheel)
                {
                    urlEdit = BackgroundBigWheelAdd.GetRedirectUrl(base.PublishmentSystemID, lotteryInfo.ID);
                }
                else if (this.lotteryType == ELotteryType.GoldEgg)
                {
                    urlEdit = BackgroundGoldEggAdd.GetRedirectUrl(base.PublishmentSystemID, lotteryInfo.ID);
                }
                else if (this.lotteryType == ELotteryType.Flap)
                {
                    urlEdit = BackgroundFlapAdd.GetRedirectUrl(base.PublishmentSystemID, lotteryInfo.ID);
                }
                else if (this.lotteryType == ELotteryType.YaoYao)
                {
                    urlEdit = BackgroundYaoYaoAdd.GetRedirectUrl(base.PublishmentSystemID, lotteryInfo.ID);
                }

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
