using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections;

using SiteServer.BBS.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundRestrictionList : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button AddButton;

        private string type;

        public static string GetRedirectUrl(int publishmentSystemID, string type)
        {
            return PageUtils.GetBBSUrl(string.Format("background_restrictionList.aspx?publishmentSystemID={0}&type={1}", publishmentSystemID, type));
        }

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
                        StringCollection stringCollection = base.Additional.RestrictionBlackList;
                        stringCollection.Remove(restriction);

                        base.Additional.RestrictionBlackList = stringCollection;
                    }
                    else
                    {
                        StringCollection stringCollection = base.Additional.RestrictionWhiteList;
                        stringCollection.Remove(restriction);

                        base.Additional.RestrictionWhiteList = stringCollection;
                    }

                    ConfigurationManager.Update(base.PublishmentSystemID);
                    
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
			{
                string pageTitle = this.type == "Black" ? "黑名单" : "白名单";
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, pageTitle, AppManager.BBS.Permission.BBS_Settings);

				BindGrid();
                AddButton.Attributes.Add("onclick", Modal.RestrictionAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.type));
			}
		}

        public void BindGrid()
		{
			try
			{
                if (this.type == "Black")
                {
                    this.dgContents.DataSource = base.Additional.RestrictionBlackList;
                }
                else
                {
                    this.dgContents.DataSource = base.Additional.RestrictionWhiteList;
                }

                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                deleteUrl.Text = string.Format("<a href=\"{0}&Delete=True&Restriction={1}\" onClick=\"javascript:return confirm('此操作将删除IP访问规则“{1}”，确认吗？');\">删除</a>", BackgroundRestrictionList.GetRedirectUrl(base.PublishmentSystemID, this.type), restriction);
            }
        }

	}
}
