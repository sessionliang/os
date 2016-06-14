using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCollectItem : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnReturn;
        private int collectID;
        private string returnUrl;
        private int collectItemID;

        public static string GetRedirectUrl(int publishmentSystemID, int collectID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_collectItem.aspx?publishmentSystemID={0}&collectID={1}&returnUrl={2}", publishmentSystemID, collectID, StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int collectItemID, int collectID)
        {
            return PageUtils.GetWXUrl(string.Format("background_collectItem.aspx?publishmentSystemID={0}&collectItemID={1}&collectID={2}", publishmentSystemID, collectItemID, collectID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.collectID = TranslateUtils.ToInt(base.Request.QueryString["collectID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);
            this.collectItemID = TranslateUtils.ToInt(base.Request.QueryString["collectItemID"]);
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CollectItemDAO.Delete(base.PublishmentSystemID, list);
                        base.SuccessMessage("征集参赛选项删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "征集参赛选项删除失败！");
                    }
                }
            }

            if (this.collectItemID > 0)
            {
                try
                {
                    DataProviderWX.CollectItemDAO.Audit(base.PublishmentSystemID, this.collectItemID);
                    base.SuccessMessage("征集参赛选项审核成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "征集参赛选项审核失败！");
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CollectItemDAO.GetSelectString(base.PublishmentSystemID, this.collectID);
            this.spContents.SortField = CollectItemAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Collect, "参赛记录", AppManager.WeiXin.Permission.WebSite.Collect);
                this.spContents.DataBind();

                string urlDelete = PageUtils.AddQueryString(BackgroundCollectItem.GetRedirectUrl(base.PublishmentSystemID, this.collectID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的征集活动参赛选项", "此操作将删除所选征集活动参赛选项，确认吗？"));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CollectItemInfo collectItemInfo = new CollectItemInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlIsChecked = e.Item.FindControl("ltlIsChecked") as Literal;
                Literal ltlVoteNum = e.Item.FindControl("ltlVoteNum") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlItemTitle.Text = collectItemInfo.Title;
                ltlDescription.Text = collectItemInfo.Description;
                ltlMobile.Text = collectItemInfo.Mobile;
                ltlVoteNum.Text = collectItemInfo.VoteNum.ToString(); ;
                ltlIsChecked.Text = StringUtils.GetTrueOrFalseImageHtml(collectItemInfo.IsChecked);
                string urlEdit = BackgroundCollectItem.GetRedirectUrl(base.PublishmentSystemID, collectItemInfo.ID, collectItemInfo.CollectID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">审核</a>", urlEdit);

            }
        }
    }
}
