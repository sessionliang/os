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
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class TableMetadataView : BackgroundBasePage
	{
        public PlaceHolder phAttribute;

        public Label lblAttributeName;
        public Label AuxiliaryTableENName;
        public Label DataType;
        public Label DataLength;
        public Label DataScale;
        public Label DataDigit;
        public Label CanBeNull;
        public Label DBDefaultValue;

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

        private EAuxiliaryTableType tableType = EAuxiliaryTableType.BackgroundContent;
        private string tableName;
        private string attributeName;
		private ArrayList relatedIdentities;

        public static string GetOpenWindowString(EAuxiliaryTableType tableType, string tableName, string attributeName, ArrayList relatedIdentities)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableType", EAuxiliaryTableTypeUtils.GetValue(tableType));
            arguments.Add("TableName", tableName);
            arguments.Add("AttributeName", attributeName);
            arguments.Add("RelatedIdentities", TranslateUtils.ObjectCollectionToString(relatedIdentities));
            return PageUtility.GetOpenWindowString("辅助表字段查看", "modal_tableMetadataView.aspx", arguments, 480, 510, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableType = EAuxiliaryTableTypeUtils.GetEnumType(base.GetQueryString("TableType"));
            this.tableName = base.GetQueryString("TableName");
            this.attributeName = base.GetQueryString("AttributeName");
            this.relatedIdentities = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("RelatedIdentities"));

			if (!IsPostBack)
			{
                TableMetadataInfo metadataInfo = TableManager.GetTableMetadataInfo(this.tableName, this.attributeName);

                if (metadataInfo != null)
                {
                    this.lblAttributeName.Text = metadataInfo.AttributeName;
                    this.AuxiliaryTableENName.Text = metadataInfo.AuxiliaryTableENName;
                    this.DataType.Text = metadataInfo.DataType.ToString();
                    this.DataLength.Text = metadataInfo.DataLength.ToString();
                    this.CanBeNull.Text = metadataInfo.CanBeNull.ToString();
                    this.DBDefaultValue.Text = metadataInfo.DBDefaultValue;

                    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType), metadataInfo.AuxiliaryTableENName, metadataInfo.AttributeName, this.relatedIdentities);

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

                    if (metadataInfo.IsSystem)
                    {
                        this.RowDefaultValue.Visible = this.RowIsHorizontal.Visible = this.RowSetItems.Visible = false;
                    }

                    this.DisplayName.Text = styleInfo.DisplayName;
                    this.HelpText.Text = styleInfo.HelpText;
                    this.IsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                    this.IsValidate.Text = StringUtils.GetTrueImageHtml(styleInfo.Additional.IsValidate);
                    this.InputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                    this.DefaultValue.Text = styleInfo.DefaultValue;
                    this.IsHorizontal.Text = StringUtils.GetBoolText(styleInfo.IsHorizontal);

                    ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                    this.MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(styleItems.Count, styleItems);
                    this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                    this.MyRepeater.DataBind();
                }
                else
                {
                    base.FailMessage("此字段为虚拟字段，在数据库中不存在！");
                    this.phAttribute.Visible = false;
                }
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
                IsSelectedControl.Text = StringUtils.GetTrueImageHtml(isSelected.ToString());
            }
        }
	}
}
