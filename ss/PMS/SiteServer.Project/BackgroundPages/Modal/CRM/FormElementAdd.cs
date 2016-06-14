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
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using System.Data;
using System.Collections.Generic;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class FormElementAdd : BackgroundBasePage
	{
        public DropDownList ddlPageID;
        public DropDownList ddlGroupID;
        public TextBox tbAttributeName;
        public TextBox DisplayName;
        public TextBox HelpText;
        public TextBox ImageUrl;
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

        private int id;
        private int mobanID;
        private int pageID;
        private int groupID;

        private FormElementInfo elementInfo;

        public static string GetOpenWindowStringToAdd(int mobanID, int pageID, int groupID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("mobanID", mobanID.ToString());
            arguments.Add("pageID", pageID.ToString());
            arguments.Add("groupID", groupID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加表单元素", "modal_formElementAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int id, int mobanID, int pageID, int groupID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("id", id.ToString());
            arguments.Add("mobanID", mobanID.ToString());
            arguments.Add("pageID", pageID.ToString());
            arguments.Add("groupID", groupID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改表单元素", "modal_formElementAdd.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = TranslateUtils.ToInt(Request.QueryString["id"]);
            this.mobanID = TranslateUtils.ToInt(Request.QueryString["mobanID"]);
            this.pageID = TranslateUtils.ToInt(Request.QueryString["pageID"]);
            this.groupID = TranslateUtils.ToInt(Request.QueryString["groupID"]);

            if (this.id != 0)
            {
                this.elementInfo = DataProvider.FormElementDAO.GetFormElementInfo(this.id);
            }
            else
            {
                this.elementInfo = new FormElementInfo();
                this.elementInfo.PageID = this.pageID;
                this.elementInfo.GroupID = this.groupID;
            }

            if (!IsPostBack)
            {
                Dictionary<int, string> pages = DataProvider.FormPageDAO.GetPages(this.mobanID);
                foreach (var val in pages)
                {
                    this.ddlPageID.Items.Add(new ListItem(val.Value, val.Key.ToString()));
                }

                List<FormGroupInfo> groupInfoList = DataProvider.FormGroupDAO.GetFormGroupInfoList(this.pageID);
                foreach (FormGroupInfo groupInfo in groupInfoList)
                {
                    this.ddlGroupID.Items.Add(new ListItem(groupInfo.Title, groupInfo.ID.ToString()));
                }
                this.ddlGroupID.Items.Insert(0, new ListItem("<<不设置>>", "0"));

                this.IsVisible.Items[0].Value = true.ToString();
                this.IsVisible.Items[1].Value = false.ToString();

                this.IsSingleLine.Items[0].Value = true.ToString();
                this.IsSingleLine.Items[1].Value = false.ToString();

                this.IsHorizontal.Items[0].Value = true.ToString();
                this.IsHorizontal.Items[1].Value = false.ToString();

                EInputTypeUtils.AddListItems(this.InputType);

                ETextEditorTypeUtils.AddListItems(this.EditorType);

                ControlUtils.SelectListItems(this.ddlPageID, this.elementInfo.PageID.ToString());
                ControlUtils.SelectListItems(this.ddlGroupID, this.elementInfo.GroupID.ToString());
                this.tbAttributeName.Text = this.elementInfo.AttributeName;
                this.DisplayName.Text = this.elementInfo.DisplayName;
                this.HelpText.Text = this.elementInfo.HelpText;
                this.ImageUrl.Text = this.elementInfo.ImageUrl;
                ControlUtils.SelectListItems(InputType, EInputTypeUtils.GetValue(this.elementInfo.InputType));
                ControlUtils.SelectListItems(this.IsVisible, this.elementInfo.IsVisible.ToString());
                ControlUtils.SelectListItems(this.IsSingleLine, this.elementInfo.IsSingleLine.ToString());
                this.DefaultValue.Text = this.elementInfo.DefaultValue;
                this.IsHorizontal.SelectedValue = this.elementInfo.IsHorizontal.ToString();
                this.Columns.Text = this.elementInfo.Additional.Columns.ToString();

                ControlUtils.SelectListItems(this.EditorType, this.elementInfo.Additional.EditorTypeString);

                this.Height.Text = this.elementInfo.Additional.Height.ToString();
                this.Width.Text = this.elementInfo.Additional.Width.ToString();

                ArrayList elementItemInfoArrayList = DataProvider.FormElementDAO.GetElementItemArrayList(this.elementInfo.ID);
                this.ItemCount.Text = elementItemInfoArrayList.Count.ToString();
                this.MyRepeater.DataSource = this.GetElementItemDataSet(elementItemInfoArrayList.Count, elementItemInfoArrayList);
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
            }

            this.ReFresh(null, EventArgs.Empty);
		}

        private DataSet GetElementItemDataSet(int styleItemCount, ArrayList elementItemInfoArrayList)
        {
            DataSet dataset = new DataSet();

            DataTable dataTable = new DataTable("FormItems");

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "FormElementID";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ItemTitle";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ItemValue";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "IsSelected";
            dataTable.Columns.Add(column);

            for (int i = 0; i < styleItemCount; i++)
            {
                DataRow dataRow = dataTable.NewRow();

                FormElementItemInfo itemInfo = (elementItemInfoArrayList != null && elementItemInfoArrayList.Count > i) ? (FormElementItemInfo)elementItemInfoArrayList[i] : new FormElementItemInfo();

                dataRow["ID"] = itemInfo.ID;
                dataRow["FormElementID"] = itemInfo.FormElementID;
                dataRow["ItemTitle"] = itemInfo.ItemTitle;
                dataRow["ItemValue"] = itemInfo.ItemValue;
                dataRow["IsSelected"] = itemInfo.IsSelected.ToString();

                dataTable.Rows.Add(dataRow);
            }

            dataset.Tables.Add(dataTable);
            return dataset;
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

        public void ReFreshPageID(object sender, EventArgs E)
        {
            if (TranslateUtils.ToInt(this.ddlPageID.SelectedValue) != this.pageID)
            {
                this.ddlGroupID.Items.Clear();

                List<FormGroupInfo> groupInfoList = DataProvider.FormGroupDAO.GetFormGroupInfoList(TranslateUtils.ToInt(this.ddlPageID.SelectedValue));
                foreach (FormGroupInfo groupInfo in groupInfoList)
                {
                    this.ddlGroupID.Items.Add(new ListItem(groupInfo.Title, groupInfo.ID.ToString()));
                }
                this.ddlGroupID.Items.Insert(0, new ListItem("<<不设置>>", "0"));
            }
        }

        public void ReFresh(object sender, EventArgs E)
        {
            this.RowDefaultValue.Visible = this.RowEditorType.Visible = this.RowHeightAndWidth.Visible = this.DateTip.Visible = this.RowItemCount.Visible = this.RowSetItems.Visible = this.RowRepeat.Visible = false;

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
                    ArrayList elementItemInfoArrayList = null;
                    if (elementInfo.ID != 0)
                    {
                        elementItemInfoArrayList = DataProvider.FormElementDAO.GetElementItemArrayList(elementInfo.ID);
                    }
                    MyRepeater.DataSource = this.GetElementItemDataSet(count, elementItemInfoArrayList);
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

            if (elementInfo.ID == 0)//数据库中没有此项及父项的表样式
            {
                isChanged = this.InsertFormElementInfo(inputType);
            }
            else//数据库中有此项的表样式
            {
                isChanged = this.UpdateFormElementInfo(inputType);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_form.aspx?mobanID={0}&pageID={1}", this.mobanID, TranslateUtils.ToInt(this.ddlPageID.SelectedValue)));
            }
		}

        private bool UpdateFormElementInfo(EInputType inputType)
        {
            bool isChanged = false;
            elementInfo.PageID = TranslateUtils.ToInt(this.ddlPageID.SelectedValue);
            elementInfo.GroupID = TranslateUtils.ToInt(this.ddlGroupID.SelectedValue);
            elementInfo.AttributeName = this.tbAttributeName.Text;
            elementInfo.DisplayName = this.DisplayName.Text;
            elementInfo.HelpText = this.HelpText.Text;
            elementInfo.ImageUrl = this.ImageUrl.Text;
            elementInfo.IsVisible = TranslateUtils.ToBool(this.IsVisible.SelectedValue);
            elementInfo.IsSingleLine = TranslateUtils.ToBool(this.IsSingleLine.SelectedValue);
            elementInfo.InputType = inputType;
            elementInfo.DefaultValue = this.DefaultValue.Text;
            elementInfo.IsHorizontal = TranslateUtils.ToBool(this.IsHorizontal.SelectedValue);
            elementInfo.Additional.Columns = TranslateUtils.ToInt(this.Columns.Text);
            elementInfo.Additional.Height = TranslateUtils.ToInt(this.Height.Text);
            elementInfo.Additional.Width = this.Width.Text;

            if (elementInfo.PageID != this.pageID || elementInfo.GroupID != this.groupID)
            {
                elementInfo.Taxis = DataProvider.FormElementDAO.GetNewFormElementInfoTaxis(elementInfo.PageID, elementInfo.GroupID);
            }

            ArrayList elementItemInfoArrayList = null;

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                elementItemInfoArrayList = new ArrayList();

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

                    FormElementItemInfo itemInfo = new FormElementItemInfo(0, elementInfo.ID, ItemTitle.Text, ItemValue.Text, IsSelected.Checked);
                    elementItemInfoArrayList.Add(itemInfo);
                }
            }
            else if (inputType == EInputType.TextEditor)
            {
                elementInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;
            }

            try
            {
                DataProvider.FormElementDAO.Update(elementInfo);

                if (elementItemInfoArrayList != null && elementItemInfoArrayList.Count > 0)
                {
                    DataProvider.FormElementDAO.DeleteStyleItems(elementInfo.ID);
                    DataProvider.FormElementDAO.InsertStyleItems(elementItemInfoArrayList);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "表单元素修改失败：" + ex.Message);
            }
            return isChanged;
        }

        private bool InsertFormElementInfo(EInputType inputType)
        {
            bool isChanged = false;
            this.elementInfo = new FormElementInfo(0, TranslateUtils.ToInt(this.ddlPageID.SelectedValue), TranslateUtils.ToInt(this.ddlGroupID.SelectedValue), this.tbAttributeName.Text, 0, this.DisplayName.Text, this.HelpText.Text, this.ImageUrl.Text, TranslateUtils.ToBool(this.IsVisible.SelectedValue), false, TranslateUtils.ToBool(this.IsSingleLine.SelectedValue), inputType, this.DefaultValue.Text, TranslateUtils.ToBool(this.IsHorizontal.SelectedValue), string.Empty);
            this.elementInfo.Additional.Columns = TranslateUtils.ToInt(this.Columns.Text);
            this.elementInfo.Additional.Height = TranslateUtils.ToInt(this.Height.Text);
            this.elementInfo.Additional.Width = this.Width.Text;
            this.elementInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                elementInfo.StyleItems = new ArrayList();

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

                    FormElementItemInfo itemInfo = new FormElementItemInfo(0, 0, ItemTitle.Text, ItemValue.Text, IsSelected.Checked);
                    elementInfo.StyleItems.Add(itemInfo);
                }
            }
            else if (inputType == EInputType.TextEditor)
            {
                elementInfo.Additional.EditorTypeString = this.EditorType.SelectedValue;
            }

            try
            {
                DataProvider.FormElementDAO.Insert(elementInfo);
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "表单元素添加失败：" + ex.Message);
            }
            return isChanged;
        }
	}
}
