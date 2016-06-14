using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundOrderMessage : BackgroundBasePage
    {
        public HyperLink hlAdd;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.OrderMessageDAO.Delete(arraylist);
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
            this.spContents.SelectCommand = DataProvider.OrderMessageDAO.GetSelectString();
            this.spContents.SortField = DataProvider.OrderMessageDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                this.hlAdd.NavigateUrl = BackgroundOrderMessageAdd.GetAddUrl("background_orderMessage.aspx");

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert("background_orderMessage.aspx?Delete=True", "IDCollection", "IDCollection", "请选择需要删除的消息！", "此操作将删除所选消息，确定吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderMessageInfo messageInfo = new OrderMessageInfo(e.Item.DataItem);

                Literal ltlIndex = e.Item.FindControl("ltlIndex") as Literal;
                Literal ltlMessageName = e.Item.FindControl("ltlMessageName") as Literal;
                Literal ltlIsSMS = e.Item.FindControl("ltlIsSMS") as Literal;
                Literal ltlTemplateSMS = e.Item.FindControl("ltlTemplateSMS") as Literal;
                Literal ltlIsEmail = e.Item.FindControl("ltlIsEmail") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);
                ltlMessageName.Text = messageInfo.MessageName;

                ltlIsSMS.Text = StringUtils.GetTrueImageHtml(messageInfo.IsSMS);
                ltlTemplateSMS.Text = messageInfo.TemplateSMS;
                ltlIsEmail.Text = StringUtils.GetTrueImageHtml(messageInfo.IsEmail);

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundOrderMessageAdd.GetEditUrl(messageInfo.ID, "background_orderMessage.aspx"));
            }
        }
    }
}
