using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundInnerLink : BackgroundBasePage
	{
		public DataGrid dgContents;
		public Button AddInnerLink;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                string innerLinkName = base.GetQueryString("InnerLinkName");
			
				try
				{
                    DataProvider.InnerLinkDAO.Delete(innerLinkName, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除站内链接", string.Format("站内链接:{0}", innerLinkName));
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_InnerLink, "站内链接管理", AppManager.CMS.Permission.WebSite.InnerLink);

                this.dgContents.DataSource = DataProvider.InnerLinkDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.DataBind();

				this.AddInnerLink.Attributes.Add("onclick", Modal.InnerLinkAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
			}
		}

		public string GetEditHtml(string innerLinkName)
		{
			string retval = string.Empty;
			bool canEdit = false;
            if (base.PublishmentSystemID != 0)
			{
                if (DataProvider.InnerLinkDAO.IsExactExists(innerLinkName, base.PublishmentSystemID))
				{
					canEdit = true;
				}
			}
			else
			{
				canEdit = true;
			}
			if (canEdit)
			{
				retval = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.InnerLinkAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, innerLinkName));
			}
			return retval;
		}

        public string GetLinkUrl(string linkUrl)
        {
            return PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(linkUrl));
        }

        public string GetDeleteHtml(string innerLinkName)
		{
			string retval = string.Empty;
			bool canEdit = false;
            if (base.PublishmentSystemID != 0)
			{
                if (DataProvider.InnerLinkDAO.IsExactExists(innerLinkName, base.PublishmentSystemID))
				{
					canEdit = true;
				}
			}
			else
			{
				canEdit = true;
			}
			if (canEdit)
			{
                string urlInnerLink = PageUtils.GetCMSUrl(string.Format("background_innerLink.aspx?PublishmentSystemID={0}&Delete=True&InnerLinkName={1}", base.PublishmentSystemID, innerLinkName));
                retval = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除内部链接“{1}”，确认吗？');\">删除</a>", urlInnerLink, innerLinkName);
			}
			return retval;
		}
	}
}
