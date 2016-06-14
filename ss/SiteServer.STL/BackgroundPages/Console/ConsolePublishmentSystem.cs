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
    public class ConsolePublishmentSystem : BackgroundBasePage
    {
        public DataGrid dgContents;
        private int hqSiteID = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("PublishmentSystemID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("PublishmentSystemID"));

                if (base.GetQueryString("Up") != null && StringUtils.EqualsIgnoreCase(base.GetQueryString("Up"), "true"))
                {
                    //上升
                    DataProvider.PublishmentSystemDAO.UpdateTaxisToUp(publishmentSystemID);
                    //清楚缓存
                    PublishmentSystemManager.ClearCache(false);
                }
                else
                {
                    //下降
                    DataProvider.PublishmentSystemDAO.UpdateTaxisToDown(publishmentSystemID);
                    //清楚缓存
                    PublishmentSystemManager.ClearCache(false);
                }

            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "系统应用管理", AppManager.Platform.Permission.Platform_Site);

                this.hqSiteID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();

                this.dgContents.DataSource = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
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
                    Literal ltlChangeType = e.Item.FindControl("ltlChangeType") as Literal;
                    Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;

                    Literal ltUpLink = e.Item.FindControl("ltUpLink") as Literal;
                    Literal ltDownLink = e.Item.FindControl("ltDownLink") as Literal;

                    ltlPublishmentSystemName.Text = this.GetPublishmentSystemNameHtml(publishmentSystemInfo);
                    ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(publishmentSystemInfo.PublishmentSystemType, false);
                    ltlPublishmentSystemDir.Text = publishmentSystemInfo.PublishmentSystemDir;
                    ltlAddDate.Text = DateUtils.GetDateString(NodeManager.GetAddDate(publishmentSystemID, publishmentSystemID));
                    ltUpLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升""/></a>", PageUtils.GetSTLUrl(string.Format("console_publishmentSystem.aspx?PublishmentSystemID={0}&Up=True", publishmentSystemID)));
                    ltDownLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降""/></a>", PageUtils.GetSTLUrl(string.Format("console_publishmentSystem.aspx?PublishmentSystemID={0}&Down=True", publishmentSystemID)));

                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0 && (this.hqSiteID == 0 || publishmentSystemID == this.hqSiteID))
                    {
                        ltlChangeType.Text = this.GetChangeHtml(publishmentSystemID, publishmentSystemInfo.IsHeadquarters);
                    }

                    if (publishmentSystemInfo.IsHeadquarters == false)
                    {
                        string urlDelete = PageUtils.GetSTLUrl(string.Format("console_publishmentSystemDelete.aspx?NodeID={0}", publishmentSystemID));
                        ltlDelete.Text = string.Format(@"<a href=""{0}"">删除</a>", urlDelete);
                    }
                }
            }
        }

        private string GetPublishmentSystemNameHtml(PublishmentSystemInfo publishmentSystemInfo)
        {
            string retval = string.Empty;
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

            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            return string.Format("{0}<img align='absbottom' border='0' src='{1}'/>&nbsp;<a href='{2}' target='_blank'>{3}{4}</a>", padding, psLogo, publishmentSystemInfo.PublishmentSystemUrl, publishmentSystemInfo.PublishmentSystemName, string.IsNullOrEmpty(publishmentSystemInfo.GroupSN) ? string.Empty : string.Format("({0})", publishmentSystemInfo.GroupSN));
        }

        private string GetChangeHtml(int publishmentSystemID, bool isHeadquarters)
        {
            string showPopWinString = ChangePublishmentSystemType.GetOpenWindowString(publishmentSystemID);

            if (isHeadquarters == false)
            {
                return string.Format("<a href=\"javascript:;\" onClick=\"{0}\">转移到根目录</a>", showPopWinString);
            }
            else
            {
                return string.Format("<a href=\"javascript:;\" onClick=\"{0}\">转移到子目录</a>", showPopWinString);
            }
        }
    }
}
