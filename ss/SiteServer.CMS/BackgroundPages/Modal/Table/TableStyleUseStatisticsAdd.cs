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
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class TableStyleUseStatisticsAdd : BackgroundBasePage
    {
        public TextBox tbAttributeName;
        public TextBox tbDisplayName;
        public TextBox tbHelpText;
        public RadioButtonList rblIsVisible;
        public RadioButtonList rblIsSingleLine;
        public PlaceHolder phIsFormatString;
        public DropDownList ddlInputType;
        public RadioButtonList rblIsFormatString;
        public TextBox tbDefaultValue;
        public Control DateTip;
        public DropDownList ddlIsHorizontal;
        public TextBox tbColumns;
        public DropDownList ddlEditorType;
        public DropDownList ddlRelatedFieldID;
        public DropDownList ddlRelatedFieldStyle;
        public TextBox tbHeight;
        public TextBox tbWidth;
        public RadioButtonList rblUseStatistics;

        public DropDownList ddlItemType;
        public PlaceHolder phItemCount;
        public TextBox tbItemCount;
        public TextBox tbItemValues;
        public Repeater MyRepeater;

        public Control rowRepeat;
        public Control rowEditorType;
        public Control rowRelatedField;
        public Control rowHeightAndWidth;
        public Control rowItemsType;
        public Control rowItemsRapid;
        public Control rowItems;

        private int tableStyleID;
        private ArrayList relatedIdentities;
        private string tableName;
        private string attributeName;
        private string redirectUrl;

        private TableStyleInfo styleInfo;
        private ETableStyle tableStyle;

        public static string GetOpenWindowString(int publishmentSystemID, int tableStyleID, ArrayList relatedIdentities, string tableName, string attributeName, ETableStyle tableStyle, string redirectUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TableStyleID", tableStyleID.ToString());
            arguments.Add("RelatedIdentities", TranslateUtils.ObjectCollectionToString(relatedIdentities));
            arguments.Add("TableName", tableName);
            arguments.Add("AttributeName", attributeName);
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(tableStyle));
            arguments.Add("RedirectUrl", StringUtils.ValueToUrl(redirectUrl));
            return PageUtility.GetOpenWindowString("修改显示样式", "modal_tableStyleUseStatisticsAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyleID = TranslateUtils.ToInt(base.GetQueryString("TableStyleID"));
            this.relatedIdentities = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("RelatedIdentities"));
            if (this.relatedIdentities.Count == 0)
            {
                this.relatedIdentities.Add(0);
            }
            this.tableName = base.GetQueryString("TableName");
            this.attributeName = base.GetQueryString("AttributeName");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.redirectUrl = StringUtils.ValueFromUrl(base.GetQueryString("RedirectUrl"));

            if (this.tableStyleID != 0)
            {
                this.styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyleID);
            }
            else
            {
                this.styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, this.attributeName, this.relatedIdentities);
            }

            if (!IsPostBack)
            {
                this.rblIsVisible.Items[0].Value = true.ToString();
                this.rblIsVisible.Items[1].Value = false.ToString();

                this.rblIsSingleLine.Items[0].Value = true.ToString();
                this.rblIsSingleLine.Items[1].Value = false.ToString();

                this.rblIsFormatString.Items[0].Value = true.ToString();
                this.rblIsFormatString.Items[1].Value = false.ToString();

                this.ddlIsHorizontal.Items[0].Value = true.ToString();
                this.ddlIsHorizontal.Items[1].Value = false.ToString();
                 
                EBooleanUtils.AddListItems(this.rblUseStatistics, "是", "否");
                ControlUtils.SelectListItems(this.rblUseStatistics, EBoolean.False.ToString());

                if (this.styleInfo.Additional.IsUseStatistics)
                    EStatisticsInputTypeUtils.AddListItems(this.ddlInputType);
                else
                EInputTypeUtils.AddListItems(this.ddlInputType);

                ETextEditorTypeUtils.AddListItems(this.ddlEditorType);

                ArrayList arraylist = DataProvider.RelatedFieldDAO.GetRelatedFieldInfoArrayList(base.PublishmentSystemID);
                foreach (RelatedFieldInfo rfInfo in arraylist)
                {
                    ListItem listItem = new ListItem(rfInfo.RelatedFieldName, rfInfo.RelatedFieldID.ToString());
                    this.ddlRelatedFieldID.Items.Add(listItem);
                }

                ERelatedFieldStyleUtils.AddListItems(this.ddlRelatedFieldStyle);

                string isDefault = base.GetQueryString("AttributeName");
                if (this.styleInfo.TableStyleID != 0 || (isDefault.Equals("IsHot") || isDefault.Equals("IsRecommend") || isDefault.Equals("IsColor") || isDefault.Equals("IsTop")))
                {
                    ddlItemType.SelectedValue = false.ToString();
                }
                else
                {
                    ddlItemType.SelectedValue = true.ToString();
                }

                this.tbAttributeName.Text = this.styleInfo.AttributeName;
                this.tbDisplayName.Text = this.styleInfo.DisplayName;
                this.tbHelpText.Text = this.styleInfo.HelpText;
                ControlUtils.SelectListItems(this.ddlInputType, EInputTypeUtils.GetValue(this.styleInfo.InputType));
                ControlUtils.SelectListItems(this.rblIsVisible, this.styleInfo.IsVisible.ToString());
                ControlUtils.SelectListItems(this.rblIsSingleLine, this.styleInfo.IsSingleLine.ToString());
                ControlUtils.SelectListItems(this.rblIsFormatString, this.styleInfo.Additional.IsFormatString.ToString());
                ControlUtils.SelectListItems(this.rblUseStatistics, this.styleInfo.Additional.IsUseStatistics.ToString());
                this.tbDefaultValue.Text = this.styleInfo.DefaultValue;
                this.ddlIsHorizontal.SelectedValue = this.styleInfo.IsHorizontal.ToString();
                this.tbColumns.Text = this.styleInfo.Additional.Columns.ToString();

                ControlUtils.SelectListItems(this.ddlEditorType, this.styleInfo.Additional.EditorTypeString);

                ControlUtils.SelectListItems(this.ddlRelatedFieldID, this.styleInfo.Additional.RelatedFieldID.ToString());
                ControlUtils.SelectListItems(this.ddlRelatedFieldStyle, this.styleInfo.Additional.RelatedFieldStyle);

                this.tbHeight.Text = this.styleInfo.Additional.Height.ToString();
                this.tbWidth.Text = this.styleInfo.Additional.Width.ToString();

                ArrayList styleItems = this.styleInfo.StyleItems;
                if (styleItems == null)
                {
                    styleItems = TableStyleManager.GetStyleItemArrayList(this.styleInfo.TableStyleID);
                }
                this.tbItemCount.Text = styleItems.Count.ToString();
                this.MyRepeater.DataSource = TableStyleManager.GetStyleItemDataSet(styleItems.Count, styleItems);
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
                if (this.MyRepeater.Items.Count > 0)
                {
                    this.ddlItemType.SelectedValue = false.ToString();
                }

                
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

        public void rblUseStatistics_SelectedIndexChanged(object sender, EventArgs E)
        {
            this.ddlInputType.Items.Clear();
            if (TranslateUtils.ToBool(rblUseStatistics.SelectedValue))
            {
                EStatisticsInputTypeUtils.AddListItems(this.ddlInputType);
            }
            else
                EInputTypeUtils.AddListItems(this.ddlInputType);
            this.ReFresh(sender, E);
        }

        public void ReFresh(object sender, EventArgs E)
        {
            this.rowEditorType.Visible = this.rowRelatedField.Visible = this.rowHeightAndWidth.Visible = this.DateTip.Visible = this.rowItemsType.Visible = this.rowItemsRapid.Visible = this.rowItems.Visible = this.rowRepeat.Visible = this.phItemCount.Visible = this.phIsFormatString.Visible = false;

            if (!string.IsNullOrEmpty(this.attributeName))
            {
                this.tbAttributeName.Enabled = false;
            }

            EInputType inputType = EInputTypeUtils.GetEnumType(this.ddlInputType.SelectedValue);
            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                this.rowItemsType.Visible = true;
                bool isRapid = TranslateUtils.ToBool(this.ddlItemType.SelectedValue);
                if (isRapid)
                {
                    this.rowItemsRapid.Visible = true;
                    this.phItemCount.Visible = false;
                    this.rowItems.Visible = false;
                }
                else
                {
                    this.rowItemsRapid.Visible = false;
                    this.phItemCount.Visible = true;
                    this.rowItems.Visible = true;
                }
                if (inputType == EInputType.CheckBox || inputType == EInputType.Radio)
                {
                    this.rowRepeat.Visible = true;
                }
            }
            else if (inputType == EInputType.TextEditor)
            {
                this.rowEditorType.Visible = this.rowHeightAndWidth.Visible = true;
            }
            else if (inputType == EInputType.TextArea)
            {
                this.rowHeightAndWidth.Visible = true;
            }
            else if (inputType == EInputType.Text)
            {
                this.phIsFormatString.Visible = true;
                this.rowHeightAndWidth.Visible = true;
            }
            else if (inputType == EInputType.Date || inputType == EInputType.DateTime)
            {
                this.DateTip.Visible = true;
            }
            else if (inputType == EInputType.RelatedField)
            {
                this.rowRelatedField.Visible = true;
            }
        }

        public void SetCount_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack)
            {
                int count = TranslateUtils.ToInt(this.tbItemCount.Text);
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


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            EInputType inputType = EInputTypeUtils.GetEnumType(this.ddlInputType.SelectedValue);

            if (inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                bool isRapid = TranslateUtils.ToBool(this.ddlItemType.SelectedValue);
                if (!isRapid)
                {
                    int itemCount = TranslateUtils.ToInt(this.tbItemCount.Text);
                    if (itemCount == 0)
                    {
                        base.FailMessage("操作失败，选项数目不能为0！");
                        return;
                    }
                }
            }

            if (styleInfo.TableStyleID == 0 && styleInfo.RelatedIdentity == 0)//数据库中没有此项及父项的表样式
            {
                isChanged = this.InsertTableStyleInfo(inputType);
            }
            else if (styleInfo.RelatedIdentity != (int)this.relatedIdentities[0])//数据库中没有此项的表样式，但是有父项的表样式
            {
                isChanged = this.InsertTableStyleInfo(inputType);
            }
            else//数据库中有此项的表样式
            {
                isChanged = this.UpdateTableStyleInfo(inputType);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.redirectUrl);
            }
        }

        private bool UpdateTableStyleInfo(EInputType inputType)
        {
            bool isChanged = false;
            styleInfo.AttributeName = this.tbAttributeName.Text;
            styleInfo.DisplayName = PageUtils.FilterXSS(this.tbDisplayName.Text);
            styleInfo.HelpText = this.tbHelpText.Text;
            styleInfo.IsVisible = TranslateUtils.ToBool(this.rblIsVisible.SelectedValue);
            styleInfo.IsSingleLine = TranslateUtils.ToBool(this.rblIsSingleLine.SelectedValue);
            styleInfo.InputType = inputType;
            styleInfo.DefaultValue = this.tbDefaultValue.Text;
            styleInfo.IsHorizontal = TranslateUtils.ToBool(this.ddlIsHorizontal.SelectedValue);

            styleInfo.Additional.Columns = TranslateUtils.ToInt(this.tbColumns.Text);
            styleInfo.Additional.Height = TranslateUtils.ToInt(this.tbHeight.Text);
            styleInfo.Additional.Width = this.tbWidth.Text;
            styleInfo.Additional.EditorTypeString = this.ddlEditorType.SelectedValue;
            styleInfo.Additional.IsFormatString = TranslateUtils.ToBool(this.rblIsFormatString.SelectedValue);
            styleInfo.Additional.RelatedFieldID = TranslateUtils.ToInt(this.ddlRelatedFieldID.SelectedValue);
            styleInfo.Additional.RelatedFieldStyle = this.ddlRelatedFieldStyle.SelectedValue;

            ArrayList styleItems = null;

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                styleItems = new ArrayList();

                bool isRapid = TranslateUtils.ToBool(this.ddlItemType.SelectedValue);
                if (isRapid)
                {
                    ArrayList itemArrayList = TranslateUtils.StringCollectionToArrayList(this.tbItemValues.Text);
                    foreach (string itemValue in itemArrayList)
                    {
                        TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, itemValue, itemValue, false);
                        styleItems.Add(itemInfo);
                    }
                }
                else
                {
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
            }

            try
            {
                TableStyleManager.Update(styleInfo);
                TableStyleManager.DeleteAndInsertStyleItems(styleInfo.TableStyleID, styleItems);
                StringUtility.AddLog(base.PublishmentSystemID, "修改表单显示样式", string.Format("类型:{0},字段名:{1}", ETableStyleUtils.GetText(this.tableStyle), styleInfo.AttributeName));
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

            int relatedIdentity = (int)this.relatedIdentities[0];

            if (TableStyleManager.IsExists(relatedIdentity, this.tableName, this.tbAttributeName.Text))
            //|| TableStyleManager.IsExistsInParents(this.relatedIdentities, this.tableName, this.tbAttributeName.Text)      
            {
                base.FailMessage(string.Format(@"显示样式添加失败：字段名""{0}""已存在", this.tbAttributeName.Text));
                return false;
            }

            if (TableStyleManager.IsMetadata(this.tableStyle, this.tbAttributeName.Text))
            {
                this.styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, this.tbAttributeName.Text, this.relatedIdentities);
            }
            else
            {
                this.styleInfo = new TableStyleInfo();
            }

            this.styleInfo.RelatedIdentity = relatedIdentity;
            this.styleInfo.TableName = this.tableName;
            this.styleInfo.AttributeName = this.tbAttributeName.Text;
            this.styleInfo.DisplayName = PageUtils.FilterXSS(this.tbDisplayName.Text);
            this.styleInfo.HelpText = this.tbHelpText.Text;
            this.styleInfo.IsVisible = TranslateUtils.ToBool(this.rblIsVisible.SelectedValue);
            this.styleInfo.IsSingleLine = TranslateUtils.ToBool(this.rblIsSingleLine.SelectedValue);
            this.styleInfo.InputType = inputType;
            this.styleInfo.DefaultValue = this.tbDefaultValue.Text;
            this.styleInfo.IsHorizontal = TranslateUtils.ToBool(this.ddlIsHorizontal.SelectedValue);

            this.styleInfo.Additional.Columns = TranslateUtils.ToInt(this.tbColumns.Text);
            this.styleInfo.Additional.Height = TranslateUtils.ToInt(this.tbHeight.Text);
            this.styleInfo.Additional.Width = this.tbWidth.Text;
            this.styleInfo.Additional.IsFormatString = TranslateUtils.ToBool(this.rblIsFormatString.SelectedValue);
            this.styleInfo.Additional.EditorTypeString = this.ddlEditorType.SelectedValue;
            this.styleInfo.Additional.RelatedFieldID = TranslateUtils.ToInt(this.ddlRelatedFieldID.SelectedValue);
            this.styleInfo.Additional.RelatedFieldStyle = this.ddlRelatedFieldStyle.SelectedValue;
            this.styleInfo.Additional.IsUseStatistics = TranslateUtils.ToBool(this.rblUseStatistics.SelectedValue);

            if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)
            {
                styleInfo.StyleItems = new ArrayList();

                bool isRapid = TranslateUtils.ToBool(this.ddlItemType.SelectedValue);
                if (isRapid)
                {
                    ArrayList itemArrayList = TranslateUtils.StringCollectionToArrayList(this.tbItemValues.Text);
                    foreach (string itemValue in itemArrayList)
                    {
                        TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, itemValue, itemValue, false);
                        styleInfo.StyleItems.Add(itemInfo);
                    }
                }
                else
                {
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
            }

            try
            {
                TableStyleManager.Insert(styleInfo, tableStyle);
                StringUtility.AddLog(base.PublishmentSystemID, "添加表单显示样式", string.Format("类型:{0},字段名:{1}", ETableStyleUtils.GetText(this.tableStyle), styleInfo.AttributeName));
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
