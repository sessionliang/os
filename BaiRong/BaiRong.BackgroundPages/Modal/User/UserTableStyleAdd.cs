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
    public class UserTableStyleAdd : BackgroundBasePage
    {
        public TextBox tbAttributeName;
        public TextBox DisplayName;
        public TextBox HelpText;
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

        private int tableStyleID;
        private string attributeName;
        private string redirectUrl;

        private TableStyleInfo styleInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int tableStyleID, string attributeName, string redirectUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TableStyleID", tableStyleID.ToString());
            arguments.Add("AttributeName", attributeName);
            arguments.Add("RedirectUrl", StringUtils.ValueToUrl(redirectUrl));
            string title = string.Empty;
            if (tableStyleID > 0)
            {
                title = "修改显示样式";
            }
            else
            {
                title = "添加显示样式";
            }
            return PageUtilityPF.GetOpenWindowString(title, "modal_userTableStyleAdd.aspx", arguments);
        }

        public static string GetOpenWindowString(int tableStyleID, string attributeName, string redirectUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableStyleID", tableStyleID.ToString());
            arguments.Add("AttributeName", attributeName);
            arguments.Add("RedirectUrl", StringUtils.ValueToUrl(redirectUrl));
            string title = string.Empty;
            if (tableStyleID > 0)
            {
                title = "修改显示样式";
            }
            else
            {
                title = "添加显示样式";
            }
            return PageUtilityPF.GetOpenWindowString(title, "modal_userTableStyleAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyleID = TranslateUtils.ToInt(base.GetQueryString("TableStyleID"));
            this.attributeName = base.GetQueryString("AttributeName");
            this.redirectUrl = StringUtils.ValueFromUrl(base.GetQueryString("RedirectUrl"));

            if (this.tableStyleID != 0)
            {
                this.styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyleID);
            }
            else
            {
                this.styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, this.attributeName, null);
            }



            if (!IsPostBack)
            {
                this.IsVisible.Items[0].Value = true.ToString();
                this.IsVisible.Items[1].Value = false.ToString();

                this.IsSingleLine.Items[0].Value = true.ToString();
                this.IsSingleLine.Items[1].Value = false.ToString();

                this.IsHorizontal.Items[0].Value = true.ToString();
                this.IsHorizontal.Items[1].Value = false.ToString();

                EInputTypeUtils.AddListItemsForUser(this.InputType);

                ETextEditorTypeUtils.AddListItems(this.EditorType);

                this.tbAttributeName.Text = this.styleInfo.AttributeName;
                this.DisplayName.Text = this.styleInfo.DisplayName;
                this.HelpText.Text = this.styleInfo.HelpText;
                ControlUtils.SelectListItems(InputType, EInputTypeUtils.GetValue(this.styleInfo.InputType));
                ControlUtils.SelectListItems(this.IsVisible, this.styleInfo.IsVisible.ToString());
                ControlUtils.SelectListItems(this.IsSingleLine, this.styleInfo.IsSingleLine.ToString());
                this.DefaultValue.Text = this.styleInfo.DefaultValue;
                this.IsHorizontal.SelectedValue = this.styleInfo.IsHorizontal.ToString();
                this.Columns.Text = this.styleInfo.Additional.Columns.ToString();

                ControlUtils.SelectListItems(this.EditorType, this.styleInfo.Additional.EditorTypeString);

                this.Height.Text = this.styleInfo.Additional.Height.ToString();
                this.Width.Text = this.styleInfo.Additional.Width.ToString();

                ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(this.styleInfo.TableStyleID);
                if (styleItems.Count == 0)
                {
                    //获取默认值
                    TableStyleInfo tableStyleTmp = TableStyleManager.GetUserTableStyleInfo(this.styleInfo.TableName, this.styleInfo.AttributeName);
                    if (tableStyleTmp != null && tableStyleTmp.StyleItems != null)
                    {
                        styleItems = tableStyleTmp.StyleItems;
                    }                    
                }

                this.ItemCount.Text = styleItems.Count.ToString();
                this.MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(styleItems.Count, styleItems);
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
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

            if (!string.IsNullOrEmpty(this.attributeName))
            {
                this.tbAttributeName.Enabled = false;
            }

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
                    if (styleInfo.TableStyleID != 0)
                    {
                        styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                    }
                    MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(count, styleItems);
                    MyRepeater.DataBind();
                }
                else
                {
                    base.FailMessage("选项数目必须为大于0的数字！");
                }
            }
        }


        public override void Submit_OnClick(object sender, EventArgs E)
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

            if (styleInfo.TableStyleID == 0)//数据库中没有此项及父项的表样式
            {
                isChanged = this.InsertTableStyleInfo(inputType);
            }
            else//数据库中有此项的表样式
            {
                isChanged = this.UpdateTableStyleInfo(inputType);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

        private bool UpdateTableStyleInfo(EInputType inputType)
        {
            bool isChanged = false;
            styleInfo.AttributeName = this.tbAttributeName.Text;
            styleInfo.DisplayName = this.DisplayName.Text;
            styleInfo.HelpText = this.HelpText.Text;
            styleInfo.IsVisible = TranslateUtils.ToBool(this.IsVisible.SelectedValue);
            styleInfo.IsSingleLine = TranslateUtils.ToBool(this.IsSingleLine.SelectedValue);
            styleInfo.InputType = inputType;
            styleInfo.DefaultValue = this.DefaultValue.Text;
            styleInfo.IsHorizontal = TranslateUtils.ToBool(this.IsHorizontal.SelectedValue);
            styleInfo.Additional.Columns = TranslateUtils.ToInt(this.Columns.Text);
            styleInfo.Additional.Height = TranslateUtils.ToInt(this.Height.Text);
            styleInfo.Additional.Width = this.Width.Text;

            ArrayList styleItems = null;

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                styleItems = new ArrayList();

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

                    TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, ItemTitle.Text, ItemValue.Text, IsSelected.Checked);
                    styleItems.Add(itemInfo);
                }
            }
            else if (inputType == EInputType.TextEditor)
            {
                styleInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;
            }

            try
            {
                TableStyleManager.Update(styleInfo);
                TableStyleManager.DeleteAndInsertStyleItems(styleInfo.TableStyleID, styleItems);
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "显示样式修改失败：" + ex.Message);
            }
            return isChanged;
        }

        private bool InsertTableStyleInfo(EInputType inputType)
        {
            bool isChanged = false;
            this.styleInfo = new TableStyleInfo(0, 0, BaiRongDataProvider.UserDAO.TABLE_NAME, this.tbAttributeName.Text, 0, this.DisplayName.Text, this.HelpText.Text, TranslateUtils.ToBool(this.IsVisible.SelectedValue), false, TranslateUtils.ToBool(this.IsSingleLine.SelectedValue), inputType, this.DefaultValue.Text, TranslateUtils.ToBool(this.IsHorizontal.SelectedValue), string.Empty);
            this.styleInfo.Additional.Columns = TranslateUtils.ToInt(this.Columns.Text);
            this.styleInfo.Additional.Height = TranslateUtils.ToInt(this.Height.Text);
            this.styleInfo.Additional.Width = this.Width.Text;
            this.styleInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;

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

            try
            {
                TableStyleManager.Insert(styleInfo, ETableStyle.User);
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
