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
    public class ConsoleUserCenter : BackgroundBasePage
    {
        public DataGrid dgContents;
        private int hqSiteID = 0;
        private int userCenterID = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.userCenterID = TranslateUtils.ToInt(PageUtils.FilterXSS(base.GetQueryString("UserCenterID")));
            SetDefaultUserCenter();
            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "系统应用管理", AppManager.Platform.Permission.Platform_Site);

                this.hqSiteID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();

                this.dgContents.DataSource = PublishmentSystemManager.GetUserCenterIDArrayListOrderByLevel();
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
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;
                    Literal ltlChangeDefault = e.Item.FindControl("ltlChangeDefault") as Literal;

                    ltlPublishmentSystemName.Text = publishmentSystemInfo.PublishmentSystemName;
                    ltlPublishmentSystemType.Text = EPublishmentSystemTypeUtils.GetHtml(publishmentSystemInfo.PublishmentSystemType, false);
                    ltlAddDate.Text = DateUtils.GetDateString(NodeManager.GetAddDate(publishmentSystemID, publishmentSystemID));

                    //if (publishmentSystemInfo.Additional.IsDefaultUserCenter)
                    //{
                    //    ltlChangeDefault.Text = "默认用户中心";
                    //}
                    //else
                    //{
                    //    ltlChangeDefault.Text = string.Format("<a href='{0}?default=true&UserCenterID={1}'>设置默认</a>", this.PageUrl, publishmentSystemID);
                    //}

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

        private void SetDefaultUserCenter()
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("default")) && base.GetQueryString("default") == "true")
            {
                if (this.userCenterID > 0)
                {
                    PublishmentSystemInfo userCenterInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.userCenterID);
                    if (userCenterInfo != null && EPublishmentSystemTypeUtils.Equals(userCenterInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                    {
                        //取消其他默认设置
                        PublishmentSystemInfo preUserCenterInfo = PublishmentSystemManager.GetDefaultUserCenter();
                        if (preUserCenterInfo != null)
                        {
                            preUserCenterInfo.Additional.IsDefaultUserCenter = false;
                            preUserCenterInfo.SettingsXML = userCenterInfo.Additional.ToString();
                            DataProvider.PublishmentSystemDAO.Update(preUserCenterInfo);
                        }

                        //设置默认
                        userCenterInfo.Additional.IsDefaultUserCenter = true;
                        userCenterInfo.SettingsXML = userCenterInfo.Additional.ToString();
                        DataProvider.PublishmentSystemDAO.Update(userCenterInfo);

                        base.SuccessMessage("设置默认用户中心成功");
                    }
                }
            }
        }

        public string PageUrl
        {
            get
            {
                return PageUtils.GetSTLUrl(string.Format("console_userCenter.aspx"));
            }
        }
    }
}
