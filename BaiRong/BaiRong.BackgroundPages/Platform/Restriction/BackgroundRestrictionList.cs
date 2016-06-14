using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections;
using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages
{
    public class BackgroundRestrictionList : BackgroundBasePage
	{
		public DataGrid MyDataGrid;
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
                        StringCollection stringCollection = ConfigManager.Instance.RestrictionBlackList;
                        stringCollection.Remove(restriction);
                        ConfigManager.Instance.RestrictionBlackList = stringCollection;
                    }
                    else
                    {
                        StringCollection stringCollection = ConfigManager.Instance.RestrictionWhiteList;
                        stringCollection.Remove(restriction);
                        ConfigManager.Instance.RestrictionWhiteList = stringCollection;
                    }
                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                    
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
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Restriction, pageTitle, AppManager.Platform.Permission.Platform_Restriction);

				BindGrid();

                AddButton.Attributes.Add("onclick", Modal.RestrictionAdd.GetOpenWindowStringToAdd(0, this.type));
			}
		}

		public void BindGrid()
		{
			try
			{
                if (this.type == "Black")
                {
                    MyDataGrid.DataSource = ConfigManager.Instance.RestrictionBlackList;
                }
                else
                {
                    MyDataGrid.DataSource = ConfigManager.Instance.RestrictionWhiteList;
                }
                
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
				MyDataGrid.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string restriction = e.Item.DataItem as string;
                Literal listItem = e.Item.FindControl("ListItem") as Literal;
                Literal editUrl = e.Item.FindControl("EditUrl") as Literal;
                Literal deleteUrl = e.Item.FindControl("DeleteUrl") as Literal;
                listItem.Text = restriction;

                string showPopWinString = Modal.RestrictionAdd.GetOpenWindowStringToEdit(0, this.type, restriction);
                editUrl.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", showPopWinString);

                string urlDelete = PageUtils.GetPlatformUrl(string.Format("background_restrictionList.aspx?Delete=True&Type={0}&Restriction={1}", this.type, restriction));
                deleteUrl.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除IP访问规则“{1}”，确认吗？');\">删除</a>", urlDelete, restriction);
            }
        }

	}
}
