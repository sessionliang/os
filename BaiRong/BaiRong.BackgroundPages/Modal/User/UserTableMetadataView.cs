using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


namespace BaiRong.BackgroundPages.Modal
{
	public class UserTableMetadataView : BackgroundBasePage
	{
        public PlaceHolder phAttribute;

        public Label lblAttributeName;

        public Label DisplayName;
        public Label HelpText;
        public Label IsVisible;
        public Label IsValidate;
        public Label InputType;
        public Label DefaultValue;
        public Label IsHorizontal;
        public Repeater MyRepeater;

        public Control RowDefaultValue;
        public Control RowIsHorizontal;
        public Control RowSetItems;

        private string attributeName;

        public static string GetOpenWindowString(int publishmentSystemID, string attributeName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("AttributeName", attributeName);
            return PageUtilityPF.GetOpenWindowString("用户字段查看", "modal_userTableMetadataView.aspx", arguments, 480, 410, true);
        }

        public static string GetOpenWindowString( string attributeName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AttributeName", attributeName);
            return PageUtilityPF.GetOpenWindowString("用户字段查看", "modal_userTableMetadataView.aspx", arguments, 480, 410, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.attributeName = base.GetQueryString("AttributeName");

			if (!IsPostBack)
			{
                TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, this.attributeName, null);

                if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.SelectMultiple || styleInfo.InputType == EInputType.SelectOne)
                {
                    this.RowDefaultValue.Visible = this.RowIsHorizontal.Visible = false;
                    this.RowSetItems.Visible = true;
                    if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio)
                    {
                        this.RowIsHorizontal.Visible = true;
                    }
                }
                else if (styleInfo.InputType == EInputType.Text || styleInfo.InputType == EInputType.TextArea || styleInfo.InputType == EInputType.TextEditor)
                {
                    this.RowDefaultValue.Visible = true;
                    this.RowSetItems.Visible = this.RowIsHorizontal.Visible = false;
                }
                else
                {
                    this.RowDefaultValue.Visible = this.RowIsHorizontal.Visible = this.RowSetItems.Visible = false;
                }

                this.lblAttributeName.Text = styleInfo.AttributeName;
                this.DisplayName.Text = styleInfo.DisplayName;
                this.HelpText.Text = styleInfo.HelpText;
                this.IsVisible.Text = UserUIUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                this.IsValidate.Text = UserUIUtils.GetTrueImageHtml(styleInfo.Additional.IsValidate);
                this.InputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                this.DefaultValue.Text = styleInfo.DefaultValue;
                this.IsHorizontal.Text = StringUtils.GetBoolText(styleInfo.IsHorizontal);

                ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                this.MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(styleItems.Count, styleItems);
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
			}
		}

	    static void MyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string itemTitle = TranslateUtils.EvalString(e.Item.DataItem, "ItemTitle");
                string itemValue = TranslateUtils.EvalString(e.Item.DataItem, "ItemValue");
                bool isSelected = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsSelected"));

                Label ItemTitleControl = (Label)e.Item.FindControl("ItemTitle");
                Label ItemValueControl = (Label)e.Item.FindControl("ItemValue");
                Label IsSelectedControl = (Label)e.Item.FindControl("IsSelected");

                ItemTitleControl.Text = itemTitle;
                ItemValueControl.Text = itemValue;
                IsSelectedControl.Text = UserUIUtils.GetTrueImageHtml(isSelected.ToString());
            }
        }
	}
}
