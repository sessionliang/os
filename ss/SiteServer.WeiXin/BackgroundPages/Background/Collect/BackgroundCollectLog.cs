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
    public class BackgroundCollectLog : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnReturn;

        private Dictionary<int, string> idTitleMap;
        private int collectID;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int collectID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_collectLog.aspx?publishmentSystemID={0}&collectID={1}&returnUrl={2}", publishmentSystemID, collectID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.collectID = TranslateUtils.ToInt(base.Request.QueryString["collectID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CollectLogDAO.Delete(list);
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
            this.spContents.SelectCommand = DataProviderWX.CollectLogDAO.GetSelectString(this.collectID);
            this.spContents.SortField = CollectLogAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Collect, "投票记录", AppManager.WeiXin.Permission.WebSite.Collect);
                List<CollectItemInfo> itemInfoList = DataProviderWX.CollectItemDAO.GetCollectItemInfoList(this.collectID);
                this.idTitleMap = new Dictionary<int, string>();
                foreach (CollectItemInfo itemInfo in itemInfoList)
                {
                    this.idTitleMap[itemInfo.ID] = itemInfo.Title;
                }

                this.spContents.DataBind();

                string urlDelete = PageUtils.AddQueryString(BackgroundCollectLog.GetRedirectUrl(base.PublishmentSystemID, this.collectID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的投票项", "此操作将删除所选投票项，确认吗？"));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CollectLogInfo logInfo = new CollectLogInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlItemID = e.Item.FindControl("ltlItemID") as Literal;
                Literal ltlIPAddress = e.Item.FindControl("ltlIPAddress") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                if (this.idTitleMap.ContainsKey(logInfo.ItemID))
                {
                    ltlItemID.Text = this.idTitleMap[logInfo.ItemID];
                }

                ltlIPAddress.Text = logInfo.IPAddress;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(logInfo.AddDate);
            }
        }
    }
}
