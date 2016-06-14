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
	public class TableStyleValidateAdd : BackgroundBasePage
	{
        public RadioButtonList IsValidate;
        public PlaceHolder phValidate;
        public RadioButtonList IsRequired;
        public PlaceHolder phNum;
        public TextBox MinNum;
        public TextBox MaxNum;
        public DropDownList ValidateType;
        public PlaceHolder phRegExp;
        public TextBox RegExp;
        public TextBox ErrorMessage;

        private int tableStyleID;
        private ArrayList relatedIdentities;
        private string tableName;
        private string attributeName;
        private string redirectUrl;

        private TableStyleInfo styleInfo;
        private ETableStyle tableStyle;

        public static string GetOpenWindowString(int tableStyleID, ArrayList relatedIdentities, string tableName, string attributeName, ETableStyle tableStyle, string redirectUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableStyleID", tableStyleID.ToString());
            arguments.Add("RelatedIdentities", TranslateUtils.ObjectCollectionToString(relatedIdentities));
            arguments.Add("TableName", tableName);
            arguments.Add("AttributeName", attributeName);
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(tableStyle));
            arguments.Add("RedirectUrl", StringUtils.ValueToUrl(redirectUrl));
            return PageUtility.GetOpenWindowString("设置表单验证", "modal_tableStyleValidateAdd.aspx", arguments, 450, 460);
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
                this.IsValidate.Items[0].Value = true.ToString();
                this.IsValidate.Items[1].Value = false.ToString();

                ControlUtils.SelectListItems(this.IsValidate, this.styleInfo.Additional.IsValidate.ToString());

                this.IsRequired.Items[0].Value = true.ToString();
                this.IsRequired.Items[1].Value = false.ToString();

                ControlUtils.SelectListItems(this.IsRequired, this.styleInfo.Additional.IsRequired.ToString());

                if (this.styleInfo.InputType == EInputType.Text || this.styleInfo.InputType == EInputType.TextArea)
                {
                    this.phNum.Visible = true;
                }
                else
                {
                    this.phNum.Visible = false;
                }

                this.MinNum.Text = this.styleInfo.Additional.MinNum.ToString();
                this.MaxNum.Text = this.styleInfo.Additional.MaxNum.ToString();

                EInputValidateTypeUtils.AddListItems(this.ValidateType);
                ControlUtils.SelectListItems(this.ValidateType, EInputValidateTypeUtils.GetValue(this.styleInfo.Additional.ValidateType));

                this.RegExp.Text = this.styleInfo.Additional.RegExp;
                this.ErrorMessage.Text = this.styleInfo.Additional.ErrorMessage;

                this.Validate_SelectedIndexChanged(null, EventArgs.Empty);

                
            }
		}

        public void Validate_SelectedIndexChanged(object sender, EventArgs E)
        {
            if (EBooleanUtils.Equals(EBoolean.False, this.IsValidate.SelectedValue))
            {
                this.phValidate.Visible = false;
            }
            else
            {
                this.phValidate.Visible = true;
            }

            EInputValidateType type = EInputValidateTypeUtils.GetEnumType(this.ValidateType.SelectedValue);
            if (type == EInputValidateType.Custom)
            {
                this.phRegExp.Visible = true;
            }
            else
            {
                this.phRegExp.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = this.InsertOrUpdateTableStyleInfo();

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.redirectUrl);
            }
		}

        private bool InsertOrUpdateTableStyleInfo()
        {
            bool isChanged = false;

            this.styleInfo.Additional.IsValidate = TranslateUtils.ToBool(this.IsValidate.SelectedValue);
            this.styleInfo.Additional.IsRequired = TranslateUtils.ToBool(this.IsRequired.SelectedValue);
            this.styleInfo.Additional.MinNum = TranslateUtils.ToInt(this.MinNum.Text);
            this.styleInfo.Additional.MaxNum = TranslateUtils.ToInt(this.MaxNum.Text);
            this.styleInfo.Additional.ValidateType = EInputValidateTypeUtils.GetEnumType(this.ValidateType.SelectedValue);
            this.styleInfo.Additional.RegExp = this.RegExp.Text.Trim('/');
            this.styleInfo.Additional.ErrorMessage = this.ErrorMessage.Text;

            try
            {
                if (this.tableStyleID == 0)//数据库中没有此项的表样式，但是有父项的表样式
                {
                    int relatedIdentity = (int)this.relatedIdentities[0];
                    this.styleInfo.RelatedIdentity = relatedIdentity;
                    styleInfo.TableStyleID = TableStyleManager.Insert(this.styleInfo, this.tableStyle);
                }

                if (styleInfo.TableStyleID > 0)
                {
                    TableStyleManager.Update(styleInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改表单验证", string.Format("类型:{0},字段:{1}", ETableStyleUtils.GetText(this.tableStyle), this.styleInfo.AttributeName));
                }
                else
                {
                    TableStyleManager.Insert(styleInfo, tableStyle);
                    StringUtility.AddLog(base.PublishmentSystemID, "新增表单验证", string.Format("类型:{0},字段:{1}", ETableStyleUtils.GetText(this.tableStyle), this.styleInfo.AttributeName));
                }
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "设置表单验证失败：" + ex.Message);
            }
            return isChanged;
        }
	}
}
