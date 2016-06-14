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

namespace SiteServer.STL.BackgroundPages
{
	public class ConsolePublishmentSystemUrl : BackgroundBasePage
	{
		public DataGrid dgContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "访问地址管理", AppManager.Platform.Permission.Platform_Site);

                ArrayList publishmentSystemArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayListOrderByLevel();
                //去除用户中心
                //publishmentSystemArrayList = PublishmentSystemManager.RemoveUserCenter(publishmentSystemArrayList);
                this.dgContents.DataSource = publishmentSystemArrayList;
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

                Literal ltlName = e.Item.FindControl("ltlName") as Literal;
                Literal ltlDir = e.Item.FindControl("ltlDir") as Literal;
                Literal ltlPublishmentSystemUrl = e.Item.FindControl("ltlPublishmentSystemUrl") as Literal;
                Literal ltlIsMultiDeployment = e.Item.FindControl("ltlIsMultiDeployment") as Literal;
                Literal ltlOuterUrl = e.Item.FindControl("ltlOuterUrl") as Literal;
                Literal ltlInnerUrl = e.Item.FindControl("ltlInnerUrl") as Literal;
                Literal ltlFuncFilesType = e.Item.FindControl("ltlFuncFilesType") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlName.Text = this.GetPublishmentSystemName(publishmentSystemInfo);
                ltlDir.Text = publishmentSystemInfo.PublishmentSystemDir;
                ltlPublishmentSystemUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, string.Empty));

                ltlIsMultiDeployment.Text = publishmentSystemInfo.Additional.IsMultiDeployment ? "内外网分离部署" : "同一台服务器";
                if (publishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    ltlOuterUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", publishmentSystemInfo.Additional.OuterUrl);
                    ltlInnerUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", publishmentSystemInfo.Additional.InnerUrl);
                }
                else
                {
                    ltlOuterUrl.Text = ltlPublishmentSystemUrl.Text;
                }

                ltlFuncFilesType.Text = EFuncFilesTypeUtils.GetText(publishmentSystemInfo.Additional.FuncFilesType);
                
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", ChangePublishmentSystemUrl.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID));
            }
        }

        private string GetPublishmentSystemName(PublishmentSystemInfo publishmentSystemInfo)
		{
            string retval = string.Empty;
            string padding = string.Empty;

            int level = PublishmentSystemManager.GetPublishmentSystemLevel(publishmentSystemInfo.PublishmentSystemID);
            string psLogo = string.Empty;
            if (publishmentSystemInfo.IsHeadquarters)
            {
                psLogo = "siteHQ.gif";
            }
            else
            {
                psLogo = "site.gif";
                if (level > 0 && level < 10)
                {
                    psLogo = string.Format("subsite{0}.gif", level + 1);
                }
            }
            psLogo = PageUtils.GetIconUrl("tree/" + psLogo);

            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            retval = string.Format("<img align='absbottom' border='0' src='{0}'/>&nbsp;<a href='{1}' target='_blank'>{2}</a>", psLogo, publishmentSystemInfo.PublishmentSystemUrl, publishmentSystemInfo.PublishmentSystemName);

            return string.Format("{0}{1}&nbsp;{2}", padding, retval, EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType));
		}
	}
}
