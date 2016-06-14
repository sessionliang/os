using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundAnnouncement : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Button AddButton;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_announcement.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.AnnouncementDAO.Delete(id);
                    base.SuccessMessage("成功删除公告");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除公告失败，{0}", ex.Message));
                }
            }
            else if (base.GetQueryString("ID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int id = base.GetIntQueryString("ID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.AnnouncementDAO.UpdateTaxisToDown(base.PublishmentSystemID, id);
                }
                else
                {
                    DataProvider.AnnouncementDAO.UpdateTaxisToUp(base.PublishmentSystemID, id);
                }
                PageUtils.Redirect(BackgroundAnnouncement.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "公告管理", AppManager.BBS.Permission.BBS_Template);

                MyDataGrid.DataSource = DataProvider.AnnouncementDAO.GetAnnouncements(base.PublishmentSystemID);
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                MyDataGrid.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.AnnouncementAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AnnouncementInfo announcementInfo = e.Item.DataItem as AnnouncementInfo;

                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", announcementInfo.LinkUrl, announcementInfo.Title);

                string announcementUrl = BackgroundAnnouncement.GetRedirectUrl(base.PublishmentSystemID);

                hlUpLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Up=True", announcementUrl, announcementInfo.ID);
                hlDownLinkButton.NavigateUrl = string.Format("{0}&ID={1}&Down=True", announcementUrl, announcementInfo.ID);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.AnnouncementAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, announcementInfo.ID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onClick=""javascript:return confirm('此操作将删除公告“{2}”，确认吗？');"">删除</a>", BackgroundAnnouncement.GetRedirectUrl(base.PublishmentSystemID), announcementInfo.ID, announcementInfo.Title);
            }
        }
    }
}
