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
	public class TableStylesAdd : BackgroundBasePage
	{
        public TextBox AttributeNames;
        public RadioButtonList IsVisible;
        public RadioButtonList IsSingleLine;
        public DropDownList InputType;
        public TextBox DefaultValue;
        public Control DateTip;
        public DropDownList IsHorizontal;
        public TextBox Columns;
        public DropDownList EditorType;
        public TextBox Height;
        public TextBox Width;

        public TextBox ItemCount;
        public Repeater MyRepeater;

        public Control RowDefaultValue;
        public Control RowRepeat;
        public Control RowEditorType;
        public Control RowHeightAndWidth;
        public Control RowItemCount;
        public Control RowSetItems;

        private ArrayList relatedIdentities;
        private string tableName;
        private string redirectUrl;

        private ETableStyle tableStyle;

        public static string GetOpenWindowString(int publishmentSystemID, ArrayList relatedIdentities, string tableName, ETableStyle tableStyle, string redirectUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentities", TranslateUtils.ObjectCollectionToString(relatedIdentities));
            arguments.Add("TableName", tableName);
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(tableStyle));
            arguments.Add("RedirectUrl", StringUtils.ValueToUrl(redirectUrl));
            return PageUtility.GetOpenWindowString("批量添加显示样式", "modal_tableStylesAdd.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedIdentities = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("RelatedIdentities"));
            if (this.relatedIdentities.Count == 0)
            {
                this.relatedIdentities.Add(0);
            }
            this.tableName = base.GetQueryString("TableName");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.redirectUrl = StringUtils.ValueFromUrl(base.GetQueryString("RedirectUrl"));

            if (!IsPostBack)
            {
                this.IsVisible.Items[0].Value = true.ToString();
                this.IsVisible.Items[1].Value = false.ToString();

                this.IsSingleLine.Items[0].Value = true.ToString();
                this.IsSingleLine.Items[1].Value = false.ToString();

                this.IsHorizontal.Items[0].Value = true.ToString();
                this.IsHorizontal.Items[1].Value = false.ToString();

                EInputTypeUtils.AddListItems(this.InputType);

                ETextEditorTypeUtils.AddListItems(this.EditorType);

                TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, string.Empty, this.relatedIdentities);

                ControlUtils.SelectListItems(InputType, EInputTypeUtils.GetValue(styleInfo.InputType));
                ControlUtils.SelectListItems(this.IsVisible, styleInfo.IsVisible.ToString());
                ControlUtils.SelectListItems(this.IsSingleLine, styleInfo.IsSingleLine.ToString());
                this.DefaultValue.Text = styleInfo.DefaultValue;
                this.IsHorizontal.SelectedValue = styleInfo.IsHorizontal.ToString();
                this.Columns.Text = styleInfo.Additional.Columns.ToString();

                ControlUtils.SelectListItems(this.EditorType, styleInfo.Additional.EditorTypeString);

                this.Height.Text = styleInfo.Additional.Height.ToString();
                this.Width.Text = styleInfo.Additional.Width.ToString();

                this.ItemCount.Text = "0";

                
            }

            this.ReFresh(null, EventArgs.Empty);
		}

	    private static void MyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                bool isSelected = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsSelected"));

                CheckBox IsSelectedControl = (CheckBox)e.Item.FindControl("IsSelected");

                IsSelectedControl.Checked = isSelected;
            }
        }


        public void ReFresh(object sender, EventArgs E)
        {
            this.RowDefaultValue.Visible = this.RowEditorType.Visible = this.RowHeightAndWidth.Visible = this.DateTip.Visible = this.RowItemCount.Visible = this.RowSetItems.Visible = this.RowRepeat.Visible = false;
            this.Height.Enabled = true;

            this.DefaultValue.TextMode = TextBoxMode.MultiLine;
            EInputType inputType = EInputTypeUtils.GetEnumType(this.InputType.SelectedValue);
            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                this.RowItemCount.Visible = this.RowSetItems.Visible = true;
                if (inputType == EInputType.CheckBox || inputType == EInputType.Radio)
                {
                    this.RowRepeat.Visible = true;
                }
            }
            else if (inputType == EInputType.TextEditor)
            {
                this.RowDefaultValue.Visible = this.RowEditorType.Visible = this.RowHeightAndWidth.Visible = true;
            }
            else if (inputType == EInputType.TextArea)
            {
                this.RowDefaultValue.Visible = this.RowHeightAndWidth.Visible = true;
            }
            else if (inputType == EInputType.Text)
            {
                this.RowDefaultValue.Visible = this.RowHeightAndWidth.Visible = true;
                this.Height.Enabled = false;
                this.DefaultValue.TextMode = TextBoxMode.SingleLine;
            }
            else if (inputType == EInputType.Date || inputType == EInputType.DateTime)
            {
                this.DateTip.Visible = this.RowDefaultValue.Visible = true;
                this.DefaultValue.TextMode = TextBoxMode.SingleLine;
            }
        }

        public void SetCount_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack)
            {
                int count = TranslateUtils.ToInt(this.ItemCount.Text);
                if (count != 0)
                {
                    ArrayList styleItems = null;
                    MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(count, styleItems);
                    MyRepeater.DataBind();
                }
                else
                {
                    base.FailMessage("选项数目必须为大于0的数字！");
                }
            }
        }


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            EInputType inputType = EInputTypeUtils.GetEnumType(this.InputType.SelectedValue);

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                int itemCount = TranslateUtils.ToInt(this.ItemCount.Text);
                if (itemCount == 0)
                {
                    base.FailMessage("操作失败，选项数目不能为0！");
                    return;
                }
            }

            isChanged = this.InsertTableStyleInfo(inputType);

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.redirectUrl);
            }
		}

        private bool InsertTableStyleInfo(EInputType inputType)
        {
            bool isChanged = false;

            string[] attributeNameArray = this.AttributeNames.Text.Split('\n');

            int relatedIdentity = (int)this.relatedIdentities[0];
            ArrayList styleInfoArrayList = new ArrayList();

            foreach (string itemString in attributeNameArray)
            {
                if (!string.IsNullOrEmpty(itemString))
                {
                    string attributeName = itemString;
                    string displayName = string.Empty;

                    if (StringUtils.Contains(itemString, "(") && StringUtils.Contains(itemString, ")"))
                    {
                        int length = itemString.IndexOf(')') - itemString.IndexOf('(');
                        if (length > 0)
                        {
                            displayName = itemString.Substring(itemString.IndexOf('(') + 1, length);
                            attributeName = itemString.Substring(0, itemString.IndexOf('('));
                        }
                    }
                    attributeName = attributeName.Trim();
                    displayName = displayName.Trim(' ', '(', ')');
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = attributeName;
                    }

                    if (TableStyleManager.IsExists(relatedIdentity, this.tableName, attributeName) || TableStyleManager.IsExistsInParents(this.relatedIdentities, this.tableName, attributeName))
                    {
                        base.FailMessage(string.Format(@"显示样式添加失败：字段名""{0}""已存在", attributeName));
                        return false;
                    }

                    TableStyleInfo styleInfo = new TableStyleInfo(0, relatedIdentity, this.tableName, attributeName, 0, displayName, string.Empty, TranslateUtils.ToBool(this.IsVisible.SelectedValue), false, TranslateUtils.ToBool(this.IsSingleLine.SelectedValue), inputType, this.DefaultValue.Text, TranslateUtils.ToBool(this.IsHorizontal.SelectedValue), string.Empty);
                    styleInfo.Additional.Columns = TranslateUtils.ToInt(this.Columns.Text);
                    styleInfo.Additional.Height = TranslateUtils.ToInt(this.Height.Text);
                    styleInfo.Additional.Width = this.Width.Text;
                    styleInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;

                    if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
                    {
                        styleInfo.StyleItems = new ArrayList();

                        bool isHasSelected = false;
                        foreach (RepeaterItem item in this.MyRepeater.Items)
                        {
                            TextBox ItemTitle = (TextBox)item.FindControl("ItemTitle");
                            TextBox ItemValue = (TextBox)item.FindControl("ItemValue");
                            CheckBox IsSelected = (CheckBox)item.FindControl("IsSelected");

                            if ((inputType != EInputType.SelectMultiple && inputType != EInputType.CheckBox) && isHasSelected && IsSelected.Checked)
                            {
                                base.FailMessage("操作失败，只能有一个初始化时选定项！");
                                return false;
                            }
                            if (IsSelected.Checked) isHasSelected = true;

                            TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, 0, ItemTitle.Text, ItemValue.Text, IsSelected.Checked);
                            styleInfo.StyleItems.Add(itemInfo);
                        }
                    }
                    else if (inputType == EInputType.TextEditor)
                    {
                        styleInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;
                    }

                    styleInfoArrayList.Add(styleInfo);
                }
            }

            try
            {
                ArrayList attributeNames = new ArrayList();
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    attributeNames.Add(styleInfo.AttributeName);
                    TableStyleManager.Insert(styleInfo, tableStyle);
                }
                StringUtility.AddLog(base.PublishmentSystemID, "批量添加表单显示样式", string.Format("类型:{0},字段名:{1}", ETableStyleUtils.GetText(this.tableStyle), TranslateUtils.ObjectCollectionToString(attributeNames)));
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "显示样式添加失败：" + ex.Message);
            }
            
            return isChanged;
        }
	}
}
