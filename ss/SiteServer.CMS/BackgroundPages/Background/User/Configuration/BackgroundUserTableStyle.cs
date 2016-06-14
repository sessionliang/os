using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundUserTableStyle : BackgroundBasePage
	{
        public Literal ltlTitle;
		public DataGrid MyDataGrid;

        public Button AddStyle;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_userTableStyle.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_User, "用户字段管理", AppManager.CMS.Permission.WebSite.User);

                //this.ltlTitle.Text = string.Format("{0}用户字段定义", BaiRongDataProvider.UserTypeDAO.GetTypeName(this.typeID));
                //删除样式
                if (base.GetQueryString("DeleteStyle") != null)
                {
                    DeleteStyle();
                }
                else if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }

                ArrayList styleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(base.PublishmentSystemInfo.GroupSN);
                MyDataGrid.DataSource = styleInfoArrayList;
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                MyDataGrid.DataBind();

                this.AddStyle.Attributes.Add("onclick", Modal.UserTableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, string.Empty, BackgroundUserTableStyle.GetRedirectUrl(base.PublishmentSystemID)));
			}
		}

        private void DeleteStyle()
        {
            string attributeName = base.GetQueryString("AttributeName");
            if (TableStyleManager.IsExists(0, base.PublishmentSystemInfo.GroupSN, attributeName))
            {
                try
                {
                    TableStyleManager.Delete(0, base.PublishmentSystemInfo.GroupSN, attributeName);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
        }

        private void SetTaxis()
        {
            int tableStyleID = TranslateUtils.ToInt(base.GetQueryString("TableStyleID"));
            TableStyleInfo styleInfo = BaiRongDataProvider.TableStyleDAO.GetTableStyleInfo(tableStyleID);
            string direction = base.GetQueryString("Direction");

            switch (direction.ToUpper())
            {
                case "UP":
                    BaiRongDataProvider.TableStyleDAO.TaxisDown(tableStyleID);
                    break;
                case "DOWN":
                    BaiRongDataProvider.TableStyleDAO.TaxisUp(tableStyleID);
                    break;
                default:
                    break;
            }
            base.SuccessMessage("排序成功！");
        }

        private void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Label lblAttributeName = (Label)e.Item.FindControl("AttributeName");
                Label lblDataType = (Label)e.Item.FindControl("DataType");
                Label lblDisplayName = (Label)e.Item.FindControl("DisplayName");
                Label lblInputType = (Label)e.Item.FindControl("InputType");
                Label lblIsVisible = (Label)e.Item.FindControl("IsVisible");
                Label lblIsValidate = (Label)e.Item.FindControl("IsValidate");
                Label lblEditStyle = (Label)e.Item.FindControl("EditStyle");
                Label lblEditValidate = (Label)e.Item.FindControl("EditValidate");
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                string showPopWinString = Modal.UserTableMetadataView.GetOpenWindowString(base.PublishmentSystemID, styleInfo.AttributeName);
                lblAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                lblDisplayName.Text = styleInfo.DisplayName;
                lblInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                lblIsVisible.Text = UserUIUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                lblIsValidate.Text = UserUIUtils.GetTrueImageHtml(styleInfo.Additional.IsValidate);

                string redirectUrl = BackgroundUserTableStyle.GetRedirectUrl(base.PublishmentSystemID);
                showPopWinString = Modal.UserTableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                string editText = "修改";
                lblEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.UserTableStyleValidateAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                lblEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                string urlDelete = string.Format("{0}&DeleteStyle=True&AttributeName={1}", redirectUrl, styleInfo.AttributeName);

                lblEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlDelete);

                bool isTaxisVisible = true;
                if (TableStyleManager.IsMetadata(ETableStyle.User, styleInfo.AttributeName))
                {
                    isTaxisVisible = false;
                }
                else
                {
                    isTaxisVisible = !TableStyleManager.IsExistsInParents(null, base.PublishmentSystemInfo.GroupSN, styleInfo.AttributeName);
                }

                if (!isTaxisVisible)
                {
                    upLinkButton.Visible = downLinkButton.Visible = false;
                }
                else
                {
                    upLinkButton.NavigateUrl = string.Format("{0}&SetTaxis=True&TableStyleID={1}&Direction=UP", redirectUrl, styleInfo.TableStyleID);
                    downLinkButton.NavigateUrl = string.Format("{0}&SetTaxis=True&TableStyleID={1}&Direction=DOWN", redirectUrl, styleInfo.TableStyleID);
                }
            }
        }
	}
}
