using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundNavigation : BackgroundBasePage
    {
        public Literal ltlTabs;
        public DataGrid dgContents;
        public Button AddButton;

        private ENavType navType;

        public static string GetRedirectUrl(int publishmentSystemID, ENavType navType)
        {
            return PageUtils.GetBBSUrl(string.Format("background_navigation.aspx?publishmentSystemID={0}&navType={1}", publishmentSystemID, ENavTypeUtils.GetValue(navType)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.navType = ENavTypeUtils.GetEnumType(base.GetQueryString("navType"));

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.NavigationDAO.Delete(id);
                    base.SuccessMessage("成功删除导航");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除导航失败，{0}", ex.Message));
                }
            }

            if (base.GetQueryString("ID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int id = base.GetIntQueryString("ID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.NavigationDAO.UpdateTaxisToDown(base.PublishmentSystemID, id, this.navType);
                }
                else
                {
                    DataProvider.NavigationDAO.UpdateTaxisToUp(base.PublishmentSystemID, id, this.navType);
                }
                PageUtils.Redirect(BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, this.navType));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "导航设置", AppManager.BBS.Permission.BBS_Template);

                if (this.navType == ENavType.Header)
                {
                    this.ltlTabs.Text = string.Format(@"
<li class=""active""><a href=""javascript:;"">主导航</a></li>
<li><a href=""{0}"">下级导航</a></li>
<li><a href=""{1}"">页尾导航</a></li>
", BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Secondary), BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Footer));
                }
                else if (this.navType == ENavType.Secondary)
                {
                    this.ltlTabs.Text = string.Format(@"
<li><a href=""{0}"">主导航</a></li>
<li class=""active""><a href=""javascript:;"">下级导航</a></li>
<li><a href=""{1}"">页尾导航</a></li>
", BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Header), BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Footer));
                }
                else if (this.navType == ENavType.Footer)
                {
                    this.ltlTabs.Text = string.Format(@"
<li><a href=""{0}"">主导航</a></li>
<li><a href=""{1}"">下级导航</a></li>
<li class=""active""><a href=""javascript:;"">页尾导航</a></li>
", BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Header), BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, ENavType.Secondary));
                }

                List<NavigationInfo> list = DataProvider.NavigationDAO.GetNavigations(base.PublishmentSystemID, this.navType);

                if (list.Count == 0 && this.navType == ENavType.Header)
                {
                    DataProvider.NavigationDAO.CreateDefaultNavigation(base.PublishmentSystemID);
                    list = DataProvider.NavigationDAO.GetNavigations(base.PublishmentSystemID, this.navType);
                }
                this.dgContents.DataSource = list;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.NavigationAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.navType));
            }
        }

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NavigationInfo navInfo = e.Item.DataItem as NavigationInfo;

                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlLinkUrl = e.Item.FindControl("ltlLinkUrl") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                string backgroundUrl = BackgroundNavigation.GetRedirectUrl(base.PublishmentSystemID, this.navType);

                ltlTitle.Text = navInfo.Title;
                ltlLinkUrl.Text = navInfo.LinkUrl;
                hlUpLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Up=True", backgroundUrl, navInfo.ID);
                hlDownLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Down=True", backgroundUrl, navInfo.ID);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.NavigationAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, navInfo.ID, this.navType));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onClick=""javascript:return confirm('此操作将删除公告“{2}”，确认吗？');"">删除</a>", backgroundUrl, navInfo.ID, navInfo.Title);
            }
        }
    }
}
