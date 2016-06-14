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
    public class BackgroundLotteryWinner : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnSetting;
        public Button btnExport;
        public Button btnReturn;

        private ELotteryType lotteryType;
        private int lotteryID;
        private int awardID;
        private string returnUrl;
        private Dictionary<int, LotteryAwardInfo> awardInfoMap = new Dictionary<int, LotteryAwardInfo>();

        public static string GetRedirectUrl(int publishmentSystemID, ELotteryType lotteryType, int lotteryID, int awardID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_lotteryWinner.aspx?publishmentSystemID={0}&lotteryType={1}&lotteryID={2}&awardID={3}&returnUrl={4}", publishmentSystemID, ELotteryTypeUtils.GetValue(lotteryType), lotteryID, awardID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.lotteryType = ELotteryTypeUtils.GetEnumType(base.Request.QueryString["lotteryType"]);
            this.lotteryID = TranslateUtils.ToInt(base.Request.QueryString["lotteryID"]);
            this.awardID = TranslateUtils.ToInt(base.Request.QueryString["awardID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.LotteryWinnerDAO.Delete(base.PublishmentSystemID, list);             
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.LotteryWinnerDAO.GetSelectString(base.PublishmentSystemID, this.lotteryType, this.lotteryID, this.awardID);
            this.spContents.SortField = LotteryWinnerAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "获奖名单查看", string.Empty);

                this.spContents.DataBind();

                int totalNum = 0;
                int wonNum = 0;
                DataProviderWX.LotteryAwardDAO.GetCount(base.PublishmentSystemID, this.lotteryType, this.lotteryID, out totalNum, out wonNum);
                base.InfoMessage(string.Format("总奖品数：{0}，已中奖人数：{1}，剩余奖品数：{2}", totalNum, wonNum, totalNum - wonNum));

                string urlDelete = PageUtils.AddQueryString(BackgroundLotteryWinner.GetRedirectUrl(base.PublishmentSystemID, this.lotteryType, this.lotteryID, this.awardID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的获奖项", "此操作将删除所选获奖项，确认吗？"));

                this.btnSetting.Attributes.Add("onclick", Modal.WinnerSetting.GetOpenWindowString(base.PublishmentSystemID));

                this.btnExport.Attributes.Add("onclick", Modal.Export.GetOpenWindowStringByLottery(base.PublishmentSystemID, this.lotteryType, this.lotteryID));

                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LotteryWinnerInfo winnerInfo = new LotteryWinnerInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlAward = e.Item.FindControl("ltlAward") as Literal;
                Literal ltlRealName = e.Item.FindControl("ltlRealName") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlAddress = e.Item.FindControl("ltlAddress") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlCashSN = e.Item.FindControl("ltlCashSN") as Literal;
                Literal ltlCashDate = e.Item.FindControl("ltlCashDate") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                LotteryAwardInfo awardInfo = null;
                if (this.awardInfoMap.ContainsKey(winnerInfo.AwardID))
                {
                    awardInfo = this.awardInfoMap[winnerInfo.AwardID];
                }
                else
                {
                    awardInfo = DataProviderWX.LotteryAwardDAO.GetAwardInfo(winnerInfo.AwardID);
                    this.awardInfoMap.Add(winnerInfo.AwardID, awardInfo);
                }
                if (awardInfo != null)
                {
                    ltlAward.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundLotteryWinner.GetRedirectUrl(base.PublishmentSystemID, ELotteryTypeUtils.GetEnumType(winnerInfo.LotteryType), winnerInfo.LotteryID, winnerInfo.AwardID, this.returnUrl), awardInfo.AwardName + "：" + awardInfo.Title);
                }

                ltlRealName.Text = winnerInfo.RealName;
                ltlMobile.Text = winnerInfo.Mobile;
                ltlEmail.Text = winnerInfo.Email;
                ltlAddress.Text = winnerInfo.Address;
                ltlStatus.Text = EWinStatusUtils.GetText(EWinStatusUtils.GetEnumType(winnerInfo.Status));
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(winnerInfo.AddDate);
                ltlCashSN.Text = winnerInfo.CashSN;
                ltlCashDate.Text = DateUtils.GetDateAndTimeString(winnerInfo.CashDate);
            }
        }
    }
}
