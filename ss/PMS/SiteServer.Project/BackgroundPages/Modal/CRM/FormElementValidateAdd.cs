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

namespace SiteServer.Project.BackgroundPages.Modal
{
    public class FormElementValidateAdd : BackgroundBasePage
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

        private FormElementInfo elementInfo;

        public static string GetOpenWindowString(int formElementID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("formElementID", formElementID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置表单验证", "modal_formElementValidateAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int formElementID = TranslateUtils.ToInt(Request.QueryString["formElementID"]);
            this.elementInfo = DataProvider.FormElementDAO.GetFormElementInfo(formElementID);

            if (!IsPostBack)
            {
                this.IsValidate.Items[0].Value = true.ToString();
                this.IsValidate.Items[1].Value = false.ToString();

                ControlUtils.SelectListItems(this.IsValidate, this.elementInfo.Additional.IsValidate.ToString());

                this.IsRequired.Items[0].Value = true.ToString();
                this.IsRequired.Items[1].Value = false.ToString();

                ControlUtils.SelectListItems(this.IsRequired, this.elementInfo.Additional.IsRequired.ToString());

                if (this.elementInfo.InputType == EInputType.Text || this.elementInfo.InputType == EInputType.TextArea)
                {
                    this.phNum.Visible = true;
                }
                else
                {
                    this.phNum.Visible = false;
                }

                this.MinNum.Text = this.elementInfo.Additional.MinNum.ToString();
                this.MaxNum.Text = this.elementInfo.Additional.MaxNum.ToString();

                EInputValidateTypeUtils.AddListItems(this.ValidateType);
                ControlUtils.SelectListItems(this.ValidateType, EInputValidateTypeUtils.GetValue(this.elementInfo.Additional.ValidateType));

                this.RegExp.Text = this.elementInfo.Additional.RegExp;
                this.ErrorMessage.Text = this.elementInfo.Additional.ErrorMessage;

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
            bool isChanged = this.InsertOrUpdateTableStyleInfo(true);

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

        private bool InsertOrUpdateTableStyleInfo(bool isUpdate)
        {
            bool isChanged = false;

            this.elementInfo.Additional.IsValidate = TranslateUtils.ToBool(this.IsValidate.SelectedValue);
            this.elementInfo.Additional.IsRequired = TranslateUtils.ToBool(this.IsRequired.SelectedValue);
            this.elementInfo.Additional.MinNum = TranslateUtils.ToInt(this.MinNum.Text);
            this.elementInfo.Additional.MaxNum = TranslateUtils.ToInt(this.MaxNum.Text);
            this.elementInfo.Additional.ValidateType = EInputValidateTypeUtils.GetEnumType(this.ValidateType.SelectedValue);
            this.elementInfo.Additional.RegExp = this.RegExp.Text.Trim('/');
            this.elementInfo.Additional.ErrorMessage = this.ErrorMessage.Text;

            try
            {
                if (isUpdate)
                {
                    DataProvider.FormElementDAO.Update(this.elementInfo);
                }
                else
                {
                    DataProvider.FormElementDAO.Insert(this.elementInfo);
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
