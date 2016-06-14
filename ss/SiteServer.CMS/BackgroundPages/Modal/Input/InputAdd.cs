using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class InputAdd : BackgroundBasePage
    {
        protected TextBox InputName;
        protected RadioButtonList IsChecked;
        protected RadioButtonList IsReply;

        protected TextBox MessageSuccess;
        protected TextBox MessageFailure;

        protected RadioButtonList IsAnomynous;
        protected RadioButtonList IsValidateCode;
        protected RadioButtonList IsSuccessHide;
        protected RadioButtonList IsSuccessReload;
        protected RadioButtonList IsCtrlEnter;

        //对表单校验唯一
        protected PlaceHolder phIsUnique;
        protected RadioButtonList rtlIsUnique;
        protected DropDownList ddlUniquePro;

        private bool isPreview;
        private int itemID;
        private string itemUrl;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加提交表单", "modal_inputAdd.aspx", arguments, 560, 510);
        }

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ItemID", itemID.ToString());
            return PageUtility.GetOpenWindowString("添加提交表单", "modal_inputAdd.aspx", arguments, 560, 510);
        }


        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int inputID, bool isPreview)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("IsPreview", isPreview.ToString());
            return PageUtility.GetOpenWindowString("修改提交表单", "modal_inputAdd.aspx", arguments, 560, 510);
        }
        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int inputID, bool isPreview, int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("IsPreview", isPreview.ToString());
            arguments.Add("ItemID", itemID.ToString());
            return PageUtility.GetOpenWindowString("修改提交表单", "modal_inputAdd.aspx", arguments, 560, 510);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.itemID = TranslateUtils.ToInt(base.GetQueryString("ItemID"));
            this.isPreview = TranslateUtils.ToBool(base.GetQueryString("IsPreview"));

            if (base.GetQueryString("ItemID") != null)
                this.itemUrl = "background_inputMainContent.aspx?itemID=" + this.itemID + "&";
            else
                this.itemUrl = "background_input.aspx?";

            if (!IsPostBack)
            {
                this.rtlIsUnique.SelectedValue = false.ToString();

                if (base.GetQueryString("InputID") != null)
                {
                    int inputID = TranslateUtils.ToInt(base.GetQueryString("InputID"));
                    InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
                    if (inputInfo != null)
                    {
                        this.InputName.Text = inputInfo.InputName;
                        ControlUtils.SelectListItems(this.IsChecked, inputInfo.IsChecked.ToString());
                        ControlUtils.SelectListItems(this.IsReply, inputInfo.IsReply.ToString());

                        this.MessageSuccess.Text = inputInfo.Additional.MessageSuccess;
                        this.MessageFailure.Text = inputInfo.Additional.MessageFailure;

                        ControlUtils.SelectListItems(this.IsAnomynous, inputInfo.Additional.IsAnomynous.ToString());
                        ControlUtils.SelectListItems(this.IsValidateCode, inputInfo.Additional.IsValidateCode.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessHide, inputInfo.Additional.IsSuccessHide.ToString());
                        ControlUtils.SelectListItems(this.IsSuccessReload, inputInfo.Additional.IsSuccessReload.ToString());
                        ControlUtils.SelectListItems(this.IsCtrlEnter, inputInfo.Additional.IsCtrlEnter.ToString());

                        #region 加载校验重复设置选项
                        this.phIsUnique.Visible = inputInfo.Additional.IsUnique;
                        ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, inputInfo.InputID);
                        ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedIdentities);
                        foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                        {
                            this.ddlUniquePro.Items.Add(new ListItem(styleInfo.DisplayName, styleInfo.AttributeName));
                        }
                        ControlUtils.SelectListItems(this.rtlIsUnique, inputInfo.Additional.IsUnique.ToString());
                        #endregion
                    }
                }
                rtlIsUnique_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            bool isNameChanged = true;
            InputInfo inputInfo = null;


            if (base.GetQueryString("InputID") != null)
            {
                try
                {
                    int inputID = TranslateUtils.ToInt(base.GetQueryString("InputID"));
                    inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
                    if (inputInfo != null)
                    {
                        if (inputInfo.InputName == this.InputName.Text)
                        {
                            isNameChanged = false;
                        }
                        else
                        {
                            inputInfo.InputName = this.InputName.Text;
                        }
                        inputInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                        inputInfo.IsReply = TranslateUtils.ToBool(this.IsReply.SelectedValue);

                        inputInfo.Additional.MessageSuccess = this.MessageSuccess.Text;
                        inputInfo.Additional.MessageFailure = this.MessageFailure.Text;

                        inputInfo.Additional.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        inputInfo.Additional.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        inputInfo.Additional.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        inputInfo.Additional.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        inputInfo.Additional.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                        //设置表单验证重复
                        inputInfo.Additional.IsUnique = TranslateUtils.ToBool(this.rtlIsUnique.SelectedValue);
                        inputInfo.Additional.UniquePro = this.ddlUniquePro.SelectedValue;
                    }
                    DataProvider.InputDAO.Update(inputInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改提交表单", string.Format("提交表单:{0}", inputInfo.InputName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "提交表单修改失败！");
                }
            }
            else
            {
                InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(base.PublishmentSystemID);
                if (this.itemID == pinfo.ItemID)
                {
                    base.FailMessage("提交表单添加失败，请选择分类进行表单添加！");
                    return;
                }
                ArrayList inputNameArrayList = DataProvider.InputDAO.GetInputNameArrayList(base.PublishmentSystemID);
                if (inputNameArrayList.IndexOf(this.InputName.Text) != -1)
                {
                    base.FailMessage("提交表单添加失败，提交表单名称已存在！");
                }
                else
                {
                    try
                    {
                        inputInfo = new InputInfo();
                        inputInfo.InputName = this.InputName.Text;
                        inputInfo.PublishmentSystemID = base.PublishmentSystemID;
                        inputInfo.IsChecked = TranslateUtils.ToBool(this.IsChecked.SelectedValue);
                        inputInfo.IsReply = TranslateUtils.ToBool(this.IsReply.SelectedValue);
                        inputInfo.ClassifyID = this.itemID;

                        inputInfo.Additional.MessageSuccess = this.MessageSuccess.Text;
                        inputInfo.Additional.MessageFailure = this.MessageFailure.Text;

                        inputInfo.Additional.IsAnomynous = TranslateUtils.ToBool(this.IsAnomynous.SelectedValue);
                        inputInfo.Additional.IsValidateCode = TranslateUtils.ToBool(this.IsValidateCode.SelectedValue);
                        inputInfo.Additional.IsSuccessHide = TranslateUtils.ToBool(this.IsSuccessHide.SelectedValue);
                        inputInfo.Additional.IsSuccessReload = TranslateUtils.ToBool(this.IsSuccessReload.SelectedValue);
                        inputInfo.Additional.IsCtrlEnter = TranslateUtils.ToBool(this.IsCtrlEnter.SelectedValue);

                        //设置表单验证重复
                        inputInfo.Additional.IsUnique = TranslateUtils.ToBool(this.rtlIsUnique.SelectedValue);
                        inputInfo.Additional.UniquePro = this.ddlUniquePro.SelectedValue;

                        DataProvider.InputDAO.Insert(inputInfo);

                        //修改分类下表单的数量
                        DataProvider.InputClassifyDAO.UpdateInputCount(base.PublishmentSystemID, this.itemID, 1);
                        //修改全部分类下表单的数量
                        DataProvider.InputClassifyDAO.UpdateCountByAll(base.PublishmentSystemID, pinfo.ItemID);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加提交表单", string.Format("提交表单:{0}", inputInfo.InputName));

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "提交表单添加失败！");
                    }
                }
            }

            if (isChanged)
            {
                if (this.isPreview)
                {
                    JsUtils.OpenWindow.CloseModalPage(Page);
                }
                else
                {
                    if (isNameChanged)
                    {
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format(this.itemUrl + "PublishmentSystemID={0}&RefreshLeft=True", base.PublishmentSystemID)));
                    }
                    else
                    {
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format(this.itemUrl + "PublishmentSystemID={0}", base.PublishmentSystemID)));
                    }
                }
            }
        }

        protected void rtlIsUnique_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsUnique.Visible = TranslateUtils.ToBool(this.rtlIsUnique.SelectedValue);
        }
    }
}
