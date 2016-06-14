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
	public class UserTableStyleValidateAdd : BackgroundBasePage
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
            return PageUtility.GetOpenWindowString("设置表单验证", "modal_userTableStyleValidateAdd.aspx", arguments, 450, 430);
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
                this.styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.User, base.PublishmentSystemInfo.GroupSN, this.attributeName, null);
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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            isChanged = this.InsertOrUpdateTableStyleInfo(true);

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.redirectUrl);
            }
		}

        private bool InsertOrUpdateTableStyleInfo(bool isUpdate)
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
                if (isUpdate)
                {
                    TableStyleManager.Update(styleInfo);
                }
                else
                {
                    TableStyleManager.Insert(styleInfo, ETableStyle.User);
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
