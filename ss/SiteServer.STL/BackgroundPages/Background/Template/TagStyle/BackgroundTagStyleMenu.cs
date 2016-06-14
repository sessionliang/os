using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStyleMenu : BackgroundBasePage
	{
		public DataGrid dgContents;

		public bool IsDefault(string isDefault)
		{
            return TranslateUtils.ToBool(isDefault);
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("Delete") != null)
            {
                int MenuDisplayID = base.GetIntQueryString("MenuDisplayID");

                try
                {
                    DataProvider.MenuDisplayDAO.Delete(MenuDisplayID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除菜单显示方式");
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("SetDefault") != null)
            {
                int MenuDisplayID = base.GetIntQueryString("MenuDisplayID");

                try
                {
                    DataProvider.MenuDisplayDAO.SetDefault(MenuDisplayID);
                    base.SuccessMessage();
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "操作失败");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, AppManager.CMS.LeftMenu.Template.ID_TagStyle, "下拉菜单样式", AppManager.CMS.Permission.WebSite.Template);

                this.dgContents.DataSource = DataProvider.MenuDisplayDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.DataBind();
            }
		}
	}
}
