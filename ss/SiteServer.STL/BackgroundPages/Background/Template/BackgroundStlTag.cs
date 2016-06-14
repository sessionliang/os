using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;


using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundStlTag : BackgroundBasePage
	{
		public DataGrid dgContents;
		public Button AddStlTag;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
				string tagName = base.GetQueryString("TagName");
			
				try
				{
                    DataProvider.StlTagDAO.Delete(base.PublishmentSystemID, tagName);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除自定义模板语言 ", string.Format("模板标签名:{0}", tagName));
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
            {                
                if (base.PublishmentSystemID != 0)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "自定义模板语言", AppManager.CMS.Permission.WebSite.Template);
                }

                this.dgContents.DataSource = DataProvider.StlTagDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string showPopWinString = StlTagAdd.GetOpenWindowString(base.PublishmentSystemID);
				this.AddStlTag.Attributes.Add("onclick", showPopWinString);
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int publishmentSystemID = TranslateUtils.EvalInt(e.Item.DataItem, "PublishmentSystemID");
                string tagName = TranslateUtils.EvalString(e.Item.DataItem, "TagName");

                Literal ltlEditHtml = (Literal)e.Item.FindControl("ltlEditHtml");
                Literal ltlDeleteHtml = (Literal)e.Item.FindControl("ltlDeleteHtml");

                if (publishmentSystemID == base.PublishmentSystemID)
                {
                    string showPopWinString = StlTagAdd.GetOpenWindowString(base.PublishmentSystemID, tagName);
                    ltlEditHtml.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", showPopWinString);
                }

                bool canEdit = false;
                if (publishmentSystemID != 0)
                {
                    canEdit = true;
                }
                else
                {
                    if (publishmentSystemID == base.PublishmentSystemID)
                    {
                        canEdit = true;
                    }
                    else
                    {
                        canEdit = false;
                    }
                }
                if (canEdit)
                {
                    string deleteUrl = PageUtils.GetSTLUrl(string.Format("background_stlTag.aspx?PublishmentSystemID={0}&Delete=True&TagName={1}", base.PublishmentSystemID, tagName));
                    ltlDeleteHtml.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除自定义标签“{1}”，确认吗？');"">删除</a>", deleteUrl, tagName);
                }
            }
        }

	}
}
