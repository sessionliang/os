﻿using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundContentGroup : BackgroundBasePage
	{
		public DataGrid dgContents;
		public Button AddGroup;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_contentGroup.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                string groupName = base.GetQueryString("GroupName");
			
				try
				{
					DataProvider.ContentGroupDAO.Delete(groupName, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除内容组", string.Format("内容组:{0}", groupName));
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_Category, "内容组管理", AppManager.CMS.Permission.WebSite.Category);

                if (base.GetQueryString("SetTaxis") != null)
                {
                    string groupName = base.GetQueryString("GroupName");
                    string direction = base.GetQueryString("Direction");

                    switch (direction.ToUpper())
                    {
                        case "UP":
                            DataProvider.ContentGroupDAO.UpdateTaxisToUp(base.PublishmentSystemID, groupName);
                            break;
                        case "DOWN":
                            DataProvider.ContentGroupDAO.UpdateTaxisToDown(base.PublishmentSystemID, groupName);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("排序成功！");
                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl(string.Format("background_contentGroup.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                }

                this.dgContents.DataSource = DataProvider.ContentGroupDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string showPopWinString = Modal.ContentGroupAdd.GetOpenWindowString(base.PublishmentSystemID);
				AddGroup.Attributes.Add("onclick", showPopWinString);
			}
		}

        public string GetContentsHtml(string groupName)
        {
            string urlGroup = PageUtils.GetCMSUrl(string.Format("background_contentsGroup.aspx?publishmentSystemID={0}&contentGroupName={1}", base.PublishmentSystemID, groupName));
            return string.Format("<a href=\"{0}\">查看内容</a>", urlGroup);
        }

		public string GetEditHtml(string groupName)
		{
            string showPopWinString = Modal.ContentGroupAdd.GetOpenWindowString(base.PublishmentSystemID, groupName);
            return string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", showPopWinString);
		}

		public string GetDeleteHtml(string groupName)
		{
            string urlGroup = PageUtils.GetCMSUrl(string.Format("background_contentGroup.aspx?PublishmentSystemID={0}&Delete=True&GroupName={1}", base.PublishmentSystemID, groupName));
            return string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除内容组“{1}”，确认吗？');\">删除</a>", urlGroup, groupName);
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string groupName = TranslateUtils.EvalString(e.Item.DataItem, "ContentGroupName");

                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                upLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_contentGroup.aspx?PublishmentSystemID={0}&SetTaxis=True&GroupName={1}&Direction=UP", base.PublishmentSystemID, groupName));
                downLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_contentGroup.aspx?PublishmentSystemID={0}&SetTaxis=True&GroupName={1}&Direction=DOWN", base.PublishmentSystemID, groupName));
            }
        }
	}
}
