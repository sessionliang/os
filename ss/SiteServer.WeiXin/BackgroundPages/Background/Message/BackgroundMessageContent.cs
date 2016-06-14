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
    public class BackgroundMessageContent : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnReturn;

        private int messageID;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int messageID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_messageContent.aspx?publishmentSystemID={0}&messageID={1}&returnUrl={2}", publishmentSystemID, messageID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.messageID = TranslateUtils.ToInt(base.Request.QueryString["messageID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.MessageContentDAO.Delete(base.PublishmentSystemID, list);
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
            this.spContents.SelectCommand = DataProviderWX.MessageContentDAO.GetSelectString(base.PublishmentSystemID, this.messageID);
            this.spContents.SortField = MessageContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Message, "留言查看", AppManager.WeiXin.Permission.WebSite.Message);
                this.spContents.DataBind();

                string urlDelete = PageUtils.AddQueryString(BackgroundMessageContent.GetRedirectUrl(base.PublishmentSystemID, this.messageID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的留言项", "此操作将删除所选留言项，确认吗？"));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MessageContentInfo contentInfo = new MessageContentInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
                Literal ltlIsReply = e.Item.FindControl("ltlIsReply") as Literal;
                Literal ltlWXOpenID = e.Item.FindControl("ltlWXOpenID") as Literal;
                Literal ltlIPAddress = e.Item.FindControl("ltlIPAddress") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                ltlDisplayName.Text = contentInfo.DisplayName;
                ltlContent.Text = contentInfo.Content;
                ltlIsReply.Text = contentInfo.IsReply ? "评论" : "留言";
                ltlWXOpenID.Text = contentInfo.WXOpenID;
                ltlIPAddress.Text = contentInfo.IPAddress;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
            }
        }
    }
}
