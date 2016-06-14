using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class ConsoleAccount : BackgroundBasePage
	{
		public DataGrid dgContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "系统应用管理", AppManager.Platform.Permission.Platform_Site);

                this.dgContents.DataSource = PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.Weixin);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int publishmentSystemID = (int)e.Item.DataItem;
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo != null)
                {
                    Literal ltlPublishmentSystemName = e.Item.FindControl("ltlPublishmentSystemName") as Literal;
                    Literal ltlPublishmentSystemType = e.Item.FindControl("ltlPublishmentSystemType") as Literal;
                    Literal ltlPublishmentSystemDir = e.Item.FindControl("ltlPublishmentSystemDir") as Literal;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    Literal ltlManage = e.Item.FindControl("ltlManage") as Literal;
                    Literal ltlBinding = e.Item.FindControl("ltlBinding") as Literal;
                    Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;

                    ltlPublishmentSystemName.Text = publishmentSystemInfo.PublishmentSystemName;

                    ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(publishmentSystemInfo.PublishmentSystemType, false);
                    ltlPublishmentSystemDir.Text = publishmentSystemInfo.PublishmentSystemDir;
                    ltlAddDate.Text = DateUtils.GetDateString(NodeManager.GetAddDate(publishmentSystemID, publishmentSystemID));

                    string manageUrl = PageUtils.GetLoadingUrl(PageUtils.GetAdminDirectoryUrl(string.Format("main.aspx?publishmentSystemID={0}", publishmentSystemID)));
                    ltlManage.Text = string.Format(@"<a href=""{0}"" target=""top"">管理</a>", manageUrl);

                    string bindingUrl = ConsoleAccountBinding.GetRedirectUrl(publishmentSystemID, PageUtils.GetWXUrl("console_account.aspx"));

                    AccountInfo accountInfo = WeiXinManager.GetAccountInfo(publishmentSystemID);

                    bool isBinding = WeiXinManager.IsBinding(accountInfo);
                    if (isBinding)
                    {
                        ltlBinding.Text = string.Format(@"<a href=""{0}"" class=""btn btn-success"">已绑定微信</a>", bindingUrl);
                    }
                    else
                    {
                        ltlBinding.Text = string.Format(@"<a href=""{0}"" class=""btn btn-danger"">未绑定微信</a>", bindingUrl);
                    }

                    string urlDelete = PageUtils.GetSTLUrl(string.Format("console_publishmentSystemDelete.aspx?NodeID={0}", publishmentSystemID));
                    ltlDelete.Text = string.Format(@"<a href=""{0}"">删除</a>", urlDelete);
                }
            }
        }
	}
}
