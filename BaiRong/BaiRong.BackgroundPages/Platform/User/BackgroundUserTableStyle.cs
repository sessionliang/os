using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserTableStyle : BackgroundBasePage
    {
        public Literal ltlTitle;
        public Repeater rpTableStyleHidden;
        public Repeater rpTableStyleBasic;

        public Button AddStyle;
        private ArrayList relatedIdentities;

        protected HiddenField hidCurrentTab;

        public static string GetRedirectUrl(string hidCurrentTab)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userTableStyle.aspx?hidCurrentTab={0}", hidCurrentTab));
        }

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userTableStyle.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedIdentities = new ArrayList();
            this.relatedIdentities.Add(0);
            //设置Tab
            this.hidCurrentTab.Value = base.GetQueryString("hidCurrentTab");
            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "用户字段配置", AppManager.User.Permission.Usercenter_Setting);

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

                ArrayList hiddenStyleInfoArrayList = TableStyleManager.GetHiddenUserTableStyleInfoArrayList();
                rpTableStyleHidden.DataSource = hiddenStyleInfoArrayList;
                rpTableStyleHidden.ItemDataBound += new RepeaterItemEventHandler(RpTableStyleHidden_ItemDataBound);
                rpTableStyleHidden.DataBind();


                ArrayList styleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(BaiRongDataProvider.UserDAO.TABLE_NAME, this.relatedIdentities);
                rpTableStyleBasic.DataSource = styleInfoArrayList;
                rpTableStyleBasic.ItemDataBound += new RepeaterItemEventHandler(RpTableStyleBasic_ItemDataBound);
                rpTableStyleBasic.DataBind();

                this.AddStyle.Attributes.Add("onclick", Modal.UserTableStyleAdd.GetOpenWindowString(0, string.Empty, BackgroundUserTableStyle.GetRedirectUrl(base.GetQueryString("hidCurrentTab"))));
            }
        }

        private void DeleteStyle()
        {
            string attributeName = base.GetQueryString("AttributeName");
            if (TableStyleManager.IsExists(0, BaiRongDataProvider.UserDAO.TABLE_NAME, attributeName))
            {
                try
                {
                    TableStyleManager.Delete(0, BaiRongDataProvider.UserDAO.TABLE_NAME, attributeName);
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
            string attributeName = base.GetQueryString("AttributeName");
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

        private void RpTableStyleBasic_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Literal lblAttributeName = (Literal)e.Item.FindControl("AttributeName");
                Literal lblDataType = (Literal)e.Item.FindControl("DataType");
                Literal lblDisplayName = (Literal)e.Item.FindControl("DisplayName");
                Literal lblInputType = (Literal)e.Item.FindControl("InputType");
                Literal lblIsVisible = (Literal)e.Item.FindControl("IsVisible");
                Literal lblIsValidate = (Literal)e.Item.FindControl("IsValidate");
                Literal lblEditStyle = (Literal)e.Item.FindControl("EditStyle");
                Literal lblEditValidate = (Literal)e.Item.FindControl("EditValidate");
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                string showPopWinString = Modal.UserTableMetadataView.GetOpenWindowString(styleInfo.AttributeName);
                lblAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                lblDisplayName.Text = styleInfo.DisplayName;
                lblInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                lblIsVisible.Text = UserUIUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                lblIsValidate.Text = UserUIUtils.GetTrueImageHtml(styleInfo.Additional.IsValidate);

                string redirectUrl = BackgroundUserTableStyle.GetRedirectUrl(base.GetQueryString("hidCurrentTab"));
                showPopWinString = Modal.UserTableStyleAdd.GetOpenWindowString(styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                string editText = "添加";
                if (styleInfo.TableStyleID > 0)//数据库中有样式
                {
                    editText = "修改";
                }
                lblEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.UserTableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                lblEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);


                if (styleInfo.TableStyleID > 0)
                {
                    string urlDelete = string.Format("{0}&DeleteStyle=True&AttributeName={1}", redirectUrl, styleInfo.AttributeName);

                    lblEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlDelete);
                }
                bool isTaxisVisible = true;
                if (TableStyleManager.IsMetadata(ETableStyle.User, styleInfo.AttributeName))
                {
                    isTaxisVisible = false;
                }
                else if (styleInfo.TableStyleID == 0)
                {
                    isTaxisVisible = false;
                }
                else
                {
                    isTaxisVisible = !TableStyleManager.IsExistsInParents(this.relatedIdentities, BaiRongDataProvider.UserDAO.TABLE_NAME, styleInfo.AttributeName);
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

        private void RpTableStyleHidden_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Literal lblAttributeName = (Literal)e.Item.FindControl("AttributeName");
                Literal lblDataType = (Literal)e.Item.FindControl("DataType");
                Literal lblDisplayName = (Literal)e.Item.FindControl("DisplayName");
                Literal lblInputType = (Literal)e.Item.FindControl("InputType");
                Literal lblIsVisible = (Literal)e.Item.FindControl("IsVisible");
                Literal lblIsValidate = (Literal)e.Item.FindControl("IsValidate");
                Literal lblEditStyle = (Literal)e.Item.FindControl("EditStyle");
                Literal lblEditValidate = (Literal)e.Item.FindControl("EditValidate");
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                string showPopWinString = Modal.UserTableMetadataView.GetOpenWindowString(styleInfo.AttributeName);
                lblAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                lblDisplayName.Text = styleInfo.DisplayName;
                lblInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                lblIsVisible.Text = UserUIUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                lblIsValidate.Text = UserUIUtils.GetTrueImageHtml(styleInfo.Additional.IsValidate);

                string redirectUrl = BackgroundUserTableStyle.GetRedirectUrl(base.GetQueryString("hidCurrentTab"));
                showPopWinString = Modal.UserTableStyleAdd.GetOpenWindowString(styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                string editText = "添加";
                if (styleInfo.TableStyleID > 0)//数据库中有样式
                {
                    editText = "修改";
                }
                lblEditStyle.Text = "";// string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.UserTableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, styleInfo.AttributeName, redirectUrl);
                lblEditValidate.Text = "";// string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);


                //if (styleInfo.TableStyleID > 0)
                //{
                //    string urlDelete = string.Format("{0}&DeleteStyle=True&AttributeName={1}", redirectUrl, styleInfo.AttributeName);

                //    lblEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlDelete);
                //}
                bool isTaxisVisible = true;
                if (TableStyleManager.IsMetadata(ETableStyle.User, styleInfo.AttributeName))
                {
                    isTaxisVisible = false;
                }
                else
                {
                    isTaxisVisible = !TableStyleManager.IsExistsInParents(this.relatedIdentities, BaiRongDataProvider.UserDAO.TABLE_NAME, styleInfo.AttributeName);
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
