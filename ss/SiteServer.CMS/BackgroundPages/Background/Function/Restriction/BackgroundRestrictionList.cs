using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundRestrictionList : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button AddButton;
        private string type;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.type = base.GetQueryString("Type");

			if (base.GetQueryString("Delete") != null)
			{
                string restriction = base.GetQueryString("Restriction");
			
				try
				{
                    if (this.type == "Black")
                    {
                        StringCollection stringCollection = base.PublishmentSystemInfo.Additional.RestrictionBlackList;
                        stringCollection.Remove(restriction);
                        base.PublishmentSystemInfo.Additional.RestrictionBlackList = stringCollection;
                    }
                    else
                    {
                        StringCollection stringCollection = base.PublishmentSystemInfo.Additional.RestrictionWhiteList;
                        stringCollection.Remove(restriction);
                        base.PublishmentSystemInfo.Additional.RestrictionWhiteList = stringCollection;
                    }

                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "删除页面访问限制", string.Format("{0}:{1}", (this.type == "Black" ? "黑名单" : "白名单"), restriction));
                    
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
			{
                string pageTitle = (this.type == "Black") ? "黑名单" : "白名单";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Restriction, pageTitle, AppManager.CMS.Permission.WebSite.Restriction);

				BindGrid();

                AddButton.Attributes.Add("onclick", Modal.RestrictionAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.type));
			}
		}

		public void BindGrid()
		{
			try
			{
                if (base.PublishmentSystemID == 0)
                {
                    if (this.type == "Black")
                    {
                        this.dgContents.DataSource = ConfigManager.Instance.RestrictionBlackList;
                    }
                    else
                    {
                        this.dgContents.DataSource = ConfigManager.Instance.RestrictionWhiteList;
                    }
                }
                else
                {
                    if (this.type == "Black")
                    {
                        this.dgContents.DataSource = base.PublishmentSystemInfo.Additional.RestrictionBlackList;
                    }
                    else
                    {
                        this.dgContents.DataSource = base.PublishmentSystemInfo.Additional.RestrictionWhiteList;
                    }
                }

                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string restriction = e.Item.DataItem as string;
                Literal listItem = e.Item.FindControl("ListItem") as Literal;
                Literal editUrl = e.Item.FindControl("EditUrl") as Literal;
                Literal deleteUrl = e.Item.FindControl("DeleteUrl") as Literal;
                listItem.Text = restriction;

                string showPopWinString = Modal.RestrictionAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.type, restriction);
                editUrl.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", showPopWinString);

                string urlDelete = PageUtils.GetCMSUrl(string.Format("background_restrictionList.aspx?Delete=True&Type={0}&Restriction={1}&PublishmentSystemID={2}", this.type, restriction, base.PublishmentSystemID));
                deleteUrl.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除IP访问规则“{1}”，确认吗？');\">删除</a>", urlDelete, restriction);
            }
        }

	}
}
