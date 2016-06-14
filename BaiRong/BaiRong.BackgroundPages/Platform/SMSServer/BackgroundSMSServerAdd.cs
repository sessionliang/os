using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Cryptography;
using BaiRong.Core;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServerAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlEditParams;
        public HiddenField hfEditParams;

        //基础信息
        public PlaceHolder SmsServerBase;
        public TextBox SmsServerName;
        public TextBox SmsServerEName;

        //公共参数
        public PlaceHolder SmsServerParams;

        //发送短信参数
        public PlaceHolder SmsServerSend;
        public TextBox tbUrl_Send;
        public TextBox tbParams_Send;
        public DropDownList ddlSendRequestType_Send;
        public DropDownList ddlSendParamsType_Send;
        public DropDownList ddlReturnContentType_Send;
        public TextBox tbOKFlag_Send;
        public TextBox tbOKValue_Send;
        public TextBox tbMsgFlag_Send;

        //查询剩余短信数
        public PlaceHolder SmsServerLastSms;
        public TextBox tbUrl_LastSms;
        public TextBox tbParams_LastSms;
        public DropDownList ddlSendRequestType_LastSms;
        public DropDownList ddlSendParamsType_LastSms;
        public DropDownList ddlReturnContentType_LastSms;
        public TextBox tbOKFlag_LastSms;
        public TextBox tbOKValue_LastSms;
        public TextBox tbRetrunValueKey_LastSms;
        public TextBox tbMsgFlag_LastSms;

        //新增模板
        public PlaceHolder SmsServerInsert;
        public TextBox tbUrl_Insert;
        public TextBox tbParams_Insert;
        public DropDownList ddlSendRequestType_Insert;
        public DropDownList ddlSendParamsType_Insert;
        public DropDownList ddlReturnContentType_Insert;
        public TextBox tbOKFlag_Insert;
        public TextBox tbOKValue_Insert;
        public TextBox tbRetrunValueKey_Insert;
        public TextBox tbMsgFlag_Insert;

        //更新模板
        public PlaceHolder SmsServerUpdate;
        public TextBox tbUrl_Update;
        public TextBox tbParams_Update;
        public DropDownList ddlSendRequestType_Update;
        public DropDownList ddlSendParamsType_Update;
        public DropDownList ddlReturnContentType_Update;
        public TextBox tbOKFlag_Update;
        public TextBox tbOKValue_Update;
        public TextBox tbRetrunValueKey_Update;
        public TextBox tbMsgFlag_Update;

        //删除模板
        public PlaceHolder SmsServerDelete;
        public TextBox tbUrl_Delete;
        public TextBox tbParams_Delete;
        public DropDownList ddlSendRequestType_Delete;
        public DropDownList ddlSendParamsType_Delete;
        public DropDownList ddlReturnContentType_Delete;
        public TextBox tbOKFlag_Delete;
        public TextBox tbOKValue_Delete;
        public TextBox tbMsgFlag_Delete;

        //查询模板
        public PlaceHolder SmsServerSearch;
        public TextBox tbUrl_Search;
        public TextBox tbParams_Search;
        public DropDownList ddlSendRequestType_Search;
        public DropDownList ddlSendParamsType_Search;
        public DropDownList ddlReturnContentType_Search;
        public TextBox tbOKFlag_Search;
        public TextBox tbOKValue_Search;
        public TextBox tbMsgFlag_Search;
        public TextBox tbRetrunValueKey_Search;


        public PlaceHolder Done;

        public PlaceHolder OperatingError;
        public Label ErrorLabel;

        public Button Previous;
        public Button Next;

        private bool isEdit = false;
        private string theSmsServerEName;

        public static string GetRedirectUrl(string SmsServerEName)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_SMSServerAdd.aspx?SmsServerEName={0}", PageUtils.FilterSqlAndXss(SmsServerEName)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("SmsServerEName") != null)
            {
                this.isEdit = true;
                this.theSmsServerEName = PageUtils.FilterSqlAndXss(base.GetQueryString("SmsServerEName"));
            }

            StringBuilder sbEditParams = new StringBuilder();
            NameValueCollection hfParams = TranslateUtils.ToNameValueCollection(this.hfEditParams.Value);
            if (isEdit && string.IsNullOrEmpty(this.hfEditParams.Value))
            {
                SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(this.theSmsServerEName);
                NameValueCollection smsServerParams = smsServerInfo.ParamCollection;
                foreach (string key in smsServerParams.Keys)
                {
                    hfParams.Add("smsServerParams_" + key, smsServerParams[key]);
                    this.hfEditParams.Value = TranslateUtils.NameValueCollectionToString(hfParams);
                }
            }
            foreach (string key in hfParams.Keys)
            {
                if (key.IndexOf("smsServerParams_") == 0)
                {
                    sbEditParams.AppendFormat("<tr><td>{0} : <input name='{0}' id='{0}' value='{1}' />&nbsp;&nbsp;<a href='javascript:void(0);' onclick='removeFormData({0})'>移除</a></td><td></td></tr>", key, hfParams[key]);

                }
            }
            if (sbEditParams.Length > 0)
            {
                ltlEditParams.Visible = true;
                ltlEditParams.Text = sbEditParams.ToString();
            }

            if (!Page.IsPostBack)
            {
                string pageTitle = this.isEdit ? "编辑短信服务商" : "添加短信服务商";
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "手机短信设置", AppManager.Platform.Permission.platform_SMS);

                LoadSendRequestType(this.ddlSendRequestType_Send);
                LoadSendParamsType(this.ddlSendParamsType_Send);
                LoadReturnContentType(this.ddlReturnContentType_Send);


                LoadSendRequestType(this.ddlSendRequestType_LastSms);
                LoadSendParamsType(this.ddlSendParamsType_LastSms);
                LoadReturnContentType(this.ddlReturnContentType_LastSms);

                LoadSendRequestType(this.ddlSendRequestType_Insert);
                LoadSendParamsType(this.ddlSendParamsType_Insert);
                LoadReturnContentType(this.ddlReturnContentType_Insert);

                LoadSendRequestType(this.ddlSendRequestType_Update);
                LoadSendParamsType(this.ddlSendParamsType_Update);
                LoadReturnContentType(this.ddlReturnContentType_Update);

                LoadSendRequestType(this.ddlSendRequestType_Delete);
                LoadSendParamsType(this.ddlSendParamsType_Delete);
                LoadReturnContentType(this.ddlReturnContentType_Delete);

                LoadSendRequestType(this.ddlSendRequestType_Search);
                LoadSendParamsType(this.ddlSendParamsType_Search);
                LoadReturnContentType(this.ddlReturnContentType_Search);

                this.ltlPageTitle.Text = pageTitle;


                SetActivePanel(WizardPanel.SmsServerBase, SmsServerBase);

                if (this.isEdit)
                {
                    SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(this.theSmsServerEName);
                    if (smsServerInfo != null)
                    {
                        this.SmsServerName.Text = smsServerInfo.SMSServerName;
                        this.SmsServerEName.Text = smsServerInfo.SMSServerEName;

                        NameValueCollection smsServerParams = smsServerInfo.ParamCollection;
                        foreach (string key in smsServerParams.Keys)
                        {
                            sbEditParams.AppendFormat("<tr><td>{0} : <input name='{0}' id='{0}' value='{1}' />&nbsp;&nbsp;<a href='javascript:void(0);' onclick='removeFormData({0})'>移除</a></td><td></td></tr>", key, smsServerParams[key]);
                        }
                        ltlEditParams.Visible = true;
                        ltlEditParams.Text = sbEditParams.ToString();

                        this.tbUrl_Send.Text = smsServerInfo.Additional.SendUrl;
                        this.tbParams_Send.Text = smsServerInfo.Additional.SendParams;
                        this.ddlSendRequestType_Send.SelectedValue = smsServerInfo.Additional.SendSettings["SendRequestType"];
                        this.ddlSendParamsType_Send.SelectedValue = smsServerInfo.Additional.SendSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_Send.SelectedValue = smsServerInfo.Additional.SendSettings["ReturnContentType"];
                        this.tbOKFlag_Send.Text = smsServerInfo.Additional.SendSettings["OKFlag"];
                        this.tbOKValue_Send.Text = smsServerInfo.Additional.SendSettings["OKValue"];
                        this.tbMsgFlag_Send.Text = smsServerInfo.Additional.SendSettings["MsgFlag"];

                        this.tbUrl_LastSms.Text = smsServerInfo.Additional.LastSmsSearchUrl;
                        this.tbParams_LastSms.Text = smsServerInfo.Additional.LastSmsSearchParams;
                        this.ddlSendRequestType_LastSms.SelectedValue = smsServerInfo.Additional.LastSmsSearchSettings["SendRequestType"];
                        this.ddlSendParamsType_LastSms.SelectedValue = smsServerInfo.Additional.LastSmsSearchSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_LastSms.SelectedValue = smsServerInfo.Additional.LastSmsSearchSettings["ReturnContentType"];
                        this.tbOKFlag_LastSms.Text = smsServerInfo.Additional.LastSmsSearchSettings["OKFlag"];
                        this.tbOKValue_LastSms.Text = smsServerInfo.Additional.LastSmsSearchSettings["OKValue"];
                        this.tbRetrunValueKey_LastSms.Text = smsServerInfo.Additional.LastSmsSearchSettings["RetrunValueKey"];
                        this.tbMsgFlag_LastSms.Text = smsServerInfo.Additional.LastSmsSearchSettings["MsgFlag"];

                        this.tbUrl_Insert.Text = smsServerInfo.Additional.InsertTemplateUrl;
                        this.tbParams_Insert.Text = smsServerInfo.Additional.InsertTemplateParams;
                        this.ddlSendRequestType_Insert.SelectedValue = smsServerInfo.Additional.InsertTemplateSettings["SendRequestType"];
                        this.ddlSendParamsType_Insert.SelectedValue = smsServerInfo.Additional.InsertTemplateSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_Insert.SelectedValue = smsServerInfo.Additional.InsertTemplateSettings["ReturnContentType"];
                        this.tbOKFlag_Insert.Text = smsServerInfo.Additional.InsertTemplateSettings["OKFlag"];
                        this.tbOKValue_Insert.Text = smsServerInfo.Additional.InsertTemplateSettings["OKValue"];
                        this.tbRetrunValueKey_Insert.Text = smsServerInfo.Additional.InsertTemplateSettings["RetrunValueKey"];
                        this.tbMsgFlag_Insert.Text = smsServerInfo.Additional.InsertTemplateSettings["MsgFlag"];

                        this.tbUrl_Update.Text = smsServerInfo.Additional.UpdateTemplateUrl;
                        this.tbParams_Update.Text = smsServerInfo.Additional.UpdateTemplateParams;
                        this.ddlSendRequestType_Update.SelectedValue = smsServerInfo.Additional.UpdateTemplateSettings["SendRequestType"];
                        this.ddlSendParamsType_Update.SelectedValue = smsServerInfo.Additional.UpdateTemplateSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_Update.SelectedValue = smsServerInfo.Additional.UpdateTemplateSettings["ReturnContentType"];
                        this.tbOKFlag_Update.Text = smsServerInfo.Additional.UpdateTemplateSettings["OKFlag"];
                        this.tbOKValue_Update.Text = smsServerInfo.Additional.UpdateTemplateSettings["OKValue"];
                        this.tbRetrunValueKey_Update.Text = smsServerInfo.Additional.UpdateTemplateSettings["RetrunValueKey"];
                        this.tbMsgFlag_Update.Text = smsServerInfo.Additional.UpdateTemplateSettings["MsgFlag"];

                        this.tbUrl_Delete.Text = smsServerInfo.Additional.DeleteTemplateUrl;
                        this.tbParams_Delete.Text = smsServerInfo.Additional.DeleteTemplateParams;
                        this.ddlSendRequestType_Delete.SelectedValue = smsServerInfo.Additional.DeleteTemplateSettings["SendRequestType"];
                        this.ddlSendParamsType_Delete.SelectedValue = smsServerInfo.Additional.DeleteTemplateSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_Delete.SelectedValue = smsServerInfo.Additional.DeleteTemplateSettings["ReturnContentType"];
                        this.tbOKFlag_Delete.Text = smsServerInfo.Additional.DeleteTemplateSettings["OKFlag"];
                        this.tbOKValue_Delete.Text = smsServerInfo.Additional.DeleteTemplateSettings["OKValue"];
                        this.tbMsgFlag_Delete.Text = smsServerInfo.Additional.DeleteTemplateSettings["MsgFlag"];

                        this.tbUrl_Search.Text = smsServerInfo.Additional.SearchTemplateUrl;
                        this.tbParams_Search.Text = smsServerInfo.Additional.SearchTemplateParams;
                        this.ddlSendRequestType_Search.SelectedValue = smsServerInfo.Additional.SearchTemplateSettings["SendRequestType"];
                        this.ddlSendParamsType_Search.SelectedValue = smsServerInfo.Additional.SearchTemplateSettings
                            ["SendParamsType"];
                        this.ddlReturnContentType_Search.SelectedValue = smsServerInfo.Additional.SearchTemplateSettings["ReturnContentType"];
                        this.tbOKFlag_Search.Text = smsServerInfo.Additional.SearchTemplateSettings["OKFlag"];
                        this.tbOKValue_Search.Text = smsServerInfo.Additional.SearchTemplateSettings["OKValue"];
                        this.tbMsgFlag_Search.Text = smsServerInfo.Additional.SearchTemplateSettings["MsgFlag"];
                        this.tbRetrunValueKey_Search.Text = smsServerInfo.Additional.SearchTemplateSettings["RetrunValueKey"];
                    }

                }
            }

            base.SuccessMessage(string.Empty);
        }

        private void LoadReturnContentType(DropDownList dropDownList)
        {
            if (dropDownList != null)
            {
                EDataFormatUtils.AddListItems(dropDownList);
                dropDownList.Items.Insert(0, new ListItem("-请选择-", ""));
                dropDownList.Items.Remove(EDataFormatUtils.GetListItem(EDataFormat.String, false));
            }
        }

        private void LoadSendParamsType(DropDownList dropDownList)
        {
            if (dropDownList != null)
            {
                EDataFormatUtils.AddListItems(dropDownList);
                dropDownList.Items.Insert(0, new ListItem("-请选择-", ""));
            }
        }

        private void LoadSendRequestType(DropDownList dropDownList)
        {
            if (dropDownList != null)
            {
                dropDownList.Items.Add(new ListItem("-请选择-", ""));
                dropDownList.Items.Add(new ListItem("Get", "Get"));
                dropDownList.Items.Add(new ListItem("Post", "Post"));
            }
        }


        private WizardPanel CurrentWizardPanel
        {
            get
            {
                if (ViewState["WizardPanel"] != null)
                    return (WizardPanel)ViewState["WizardPanel"];

                return WizardPanel.SmsServerBase;
            }
            set
            {
                ViewState["WizardPanel"] = value;
            }
        }


        private enum WizardPanel
        {
            SmsServerBase,
            SmsServerParams,
            SmsServerSend,
            SmsServerLastSms,
            SmsServerInsert,
            SmsServerUpdate,
            SmsServerDelete,
            SmsServerSearch,
            Done
        }

        void SetActivePanel(WizardPanel panel, Control controlToShow)
        {
            PlaceHolder currentPanel = FindControl(CurrentWizardPanel.ToString()) as PlaceHolder;
            if (currentPanel != null)
                currentPanel.Visible = false;

            switch (panel)
            {
                case WizardPanel.SmsServerBase:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    break;
                case WizardPanel.SmsServerParams:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerSend:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerLastSms:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerInsert:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerUpdate:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerDelete:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                case WizardPanel.SmsServerSearch:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
                    break;
                default:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Text = "完成";
                    Next.Enabled = false;
                    break;
            }

            controlToShow.Visible = true;
            CurrentWizardPanel = panel;
        }

        private bool Validate_SmsServerBase(out string errorMessage)
        {
            if (string.IsNullOrEmpty(this.SmsServerName.Text))
            {
                errorMessage = "必须填写短信服务商名称！";
                return false;
            }

            if (string.IsNullOrEmpty(this.SmsServerEName.Text))
            {
                errorMessage = "必须填写短信服务商英文名称！";
                return false;
            }
            if (this.isEdit == false)
            {
                if (BaiRongDataProvider.SMSServerDAO.IsExists(this.SmsServerEName.Text))
                {
                    errorMessage = "短信服务商已存在！";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerParams(out string errorMessage)
        {

            errorMessage = string.Empty;
            StringBuilder sbEditParams = new StringBuilder();
            NameValueCollection smsServerParams = Request.Form;
            NameValueCollection hfParams = new NameValueCollection();
            foreach (string key in smsServerParams.Keys)
            {
                if (key.IndexOf("smsServerParams_") == 0)
                {
                    sbEditParams.AppendFormat("<tr><td>{0} : <input name='{0}' id='{0}' value='{1}' />&nbsp;&nbsp;<a href='javascript:void(0);' onclick='removeFormData({0})'>移除</a></td><td></td></tr>", key, smsServerParams[key]);
                    hfParams.Add(key, smsServerParams[key]);
                }
            }
            ltlEditParams.Visible = true;
            ltlEditParams.Text = sbEditParams.ToString();
            hfEditParams.Value = TranslateUtils.NameValueCollectionToString(hfParams);

            return true;
        }

        private bool Validate_SmsServerSend(out string errorMessage)
        {

            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerLastSms(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerInsert(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerUpdate(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerDelete(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_SmsServerSearch(out string errorMessage)
        {
            ProcessData(out errorMessage);
            errorMessage = string.Empty;
            return true;
        }

        public void NextPanel(Object sender, EventArgs e)
        {
            string errorMessage;
            switch (CurrentWizardPanel)
            {
                case WizardPanel.SmsServerBase:

                    if (this.Validate_SmsServerBase(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerParams, SmsServerParams);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerBase, SmsServerBase);
                    }

                    break;

                case WizardPanel.SmsServerParams:

                    if (this.Validate_SmsServerParams(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerSend, SmsServerSend);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerParams, SmsServerParams);
                    }

                    break;

                case WizardPanel.SmsServerSend:

                    if (this.Validate_SmsServerSend(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerSend, SmsServerSend);
                    }

                    break;

                case WizardPanel.SmsServerLastSms:

                    if (this.Validate_SmsServerLastSms(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerInsert, SmsServerInsert);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }

                    break;

                case WizardPanel.SmsServerInsert:

                    if (this.Validate_SmsServerInsert(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerUpdate, SmsServerUpdate);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }

                    break;

                case WizardPanel.SmsServerUpdate:

                    if (this.Validate_SmsServerUpdate(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerDelete, SmsServerDelete);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }

                    break;

                case WizardPanel.SmsServerDelete:

                    if (this.Validate_SmsServerDelete(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.SmsServerSearch, SmsServerSearch);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }

                    break;

                case WizardPanel.SmsServerSearch:

                    if (this.Validate_SmsServerSearch(out errorMessage))
                    {
                        base.SuccessMessage("设置成功");
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.SmsServerLastSms, SmsServerLastSms);
                    }

                    break;

                case WizardPanel.Done:
                    break;
            }
        }

        public void PreviousPanel(Object sender, EventArgs e)
        {
            switch (CurrentWizardPanel)
            {
                case WizardPanel.SmsServerBase:
                    break;

                case WizardPanel.SmsServerParams:
                    SetActivePanel(WizardPanel.SmsServerBase, SmsServerBase);
                    break;

                case WizardPanel.SmsServerSend:
                    SetActivePanel(WizardPanel.SmsServerParams, SmsServerParams);
                    break;

                case WizardPanel.SmsServerLastSms:
                    SetActivePanel(WizardPanel.SmsServerSend, SmsServerSend);
                    break;

                case WizardPanel.SmsServerInsert:
                    SetActivePanel(WizardPanel.SmsServerInsert, SmsServerInsert);
                    break;

                case WizardPanel.SmsServerUpdate:
                    SetActivePanel(WizardPanel.SmsServerUpdate, SmsServerUpdate);
                    break;

                case WizardPanel.SmsServerDelete:
                    SetActivePanel(WizardPanel.SmsServerDelete, SmsServerDelete);
                    break;

                case WizardPanel.SmsServerSearch:
                    SetActivePanel(WizardPanel.SmsServerSearch, SmsServerSearch);
                    break;
            }
        }

        private bool ProcessData(out string errorMessage)
        {
            try
            {
                bool isNeedAdd = false;
                if (this.isEdit)
                {
                    if (this.theSmsServerEName != this.SmsServerEName.Text)
                    {
                        isNeedAdd = true;
                        BaiRongDataProvider.SMSServerDAO.Delete(this.theSmsServerEName);
                    }
                    else
                    {
                        SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(this.theSmsServerEName);
                        //基本信息
                        smsServerInfo.SMSServerName = this.SmsServerName.Text.Trim();
                        smsServerInfo.SMSServerEName = this.SmsServerEName.Text.Trim();
                        smsServerInfo.IsEnable = true;
                        //公告参数
                        NameValueCollection hfParams = TranslateUtils.ToNameValueCollection(hfEditParams.Value);
                        NameValueCollection smsServerParams = new NameValueCollection();
                        foreach (string key in hfParams)
                        {
                            if (key.IndexOf("smsServerParams_") == 0)
                            {
                                smsServerParams.Add(key.Substring("smsServerParams_".Length), hfParams[key]);
                            }
                        }
                        smsServerInfo.ParamCollection = smsServerParams;

                        //=============扩展属性=============
                        //发送设置
                        smsServerInfo.Additional.SendUrl = this.tbUrl_Send.Text.Trim();
                        smsServerInfo.Additional.SendParams = this.tbParams_Send.Text.Trim();
                        NameValueCollection SendSettings = new NameValueCollection();
                        SendSettings.Add("SendRequestType", this.ddlSendRequestType_Send.SelectedValue);
                        SendSettings.Add("SendParamsType", this.ddlSendParamsType_Send.SelectedValue);
                        SendSettings.Add("ReturnContentType", this.ddlReturnContentType_Send.SelectedValue);
                        SendSettings.Add("OKFlag", this.tbOKFlag_Send.Text.Trim());
                        SendSettings.Add("OKValue", this.tbOKValue_Send.Text.Trim());
                        SendSettings.Add("MsgFlag", this.tbMsgFlag_Send.Text.Trim());
                        smsServerInfo.Additional.SendSettings = SendSettings;

                        //查询剩余短信条数
                        smsServerInfo.Additional.LastSmsSearchUrl = this.tbUrl_LastSms.Text.Trim();
                        smsServerInfo.Additional.LastSmsSearchParams = this.tbParams_LastSms.Text.Trim();
                        NameValueCollection LastSmsSearchSettings = new NameValueCollection();
                        LastSmsSearchSettings.Add("SendRequestType", this.ddlSendRequestType_LastSms.SelectedValue);
                        LastSmsSearchSettings.Add("SendParamsType", this.ddlSendParamsType_LastSms.SelectedValue);
                        LastSmsSearchSettings.Add("ReturnContentType", this.ddlReturnContentType_LastSms.SelectedValue);
                        LastSmsSearchSettings.Add("OKFlag", this.tbOKFlag_LastSms.Text.Trim());
                        LastSmsSearchSettings.Add("OKValue", this.tbOKValue_LastSms.Text.Trim());
                        LastSmsSearchSettings.Add("RetrunValueKey", this.tbRetrunValueKey_LastSms.Text.Trim());
                        LastSmsSearchSettings.Add("MsgFlag", this.tbMsgFlag_LastSms.Text.Trim());
                        smsServerInfo.Additional.LastSmsSearchSettings = LastSmsSearchSettings;

                        //新增模板
                        smsServerInfo.Additional.InsertTemplateUrl = this.tbUrl_Insert.Text.Trim();
                        smsServerInfo.Additional.InsertTemplateParams = this.tbParams_Insert.Text.Trim();
                        NameValueCollection InsertTemplateSettings = new NameValueCollection();
                        InsertTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Insert.SelectedValue);
                        InsertTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Insert.SelectedValue);
                        InsertTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Insert.SelectedValue);
                        InsertTemplateSettings.Add("OKFlag", this.tbOKFlag_Insert.Text.Trim());
                        InsertTemplateSettings.Add("OKValue", this.tbOKValue_Insert.Text.Trim());
                        InsertTemplateSettings.Add("RetrunValueKey", this.tbRetrunValueKey_Insert.Text.Trim());
                        InsertTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Insert.Text.Trim());
                        smsServerInfo.Additional.InsertTemplateSettings = InsertTemplateSettings;

                        //修改模板
                        smsServerInfo.Additional.UpdateTemplateUrl = this.tbUrl_Update.Text.Trim();
                        smsServerInfo.Additional.UpdateTemplateParams = this.tbParams_Update.Text.Trim();
                        NameValueCollection UpdateTemplateSettings = new NameValueCollection();
                        UpdateTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Update.SelectedValue);
                        UpdateTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Update.SelectedValue);
                        UpdateTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Update.SelectedValue);
                        UpdateTemplateSettings.Add("OKFlag", this.tbOKFlag_Update.Text.Trim());
                        UpdateTemplateSettings.Add("OKValue", this.tbOKValue_Update.Text.Trim());
                        UpdateTemplateSettings.Add("RetrunValueKey", this.tbRetrunValueKey_Update.Text.Trim());
                        UpdateTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Update.Text.Trim());
                        smsServerInfo.Additional.UpdateTemplateSettings = UpdateTemplateSettings;

                        //删除模板
                        smsServerInfo.Additional.DeleteTemplateUrl = this.tbUrl_Delete.Text.Trim();
                        smsServerInfo.Additional.DeleteTemplateParams = this.tbParams_Delete.Text.Trim();
                        NameValueCollection DeleteTemplateSettings = new NameValueCollection();
                        DeleteTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Delete.SelectedValue);
                        DeleteTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Delete.SelectedValue);
                        DeleteTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Delete.SelectedValue);
                        DeleteTemplateSettings.Add("OKFlag", this.tbOKFlag_Delete.Text.Trim());
                        DeleteTemplateSettings.Add("OKValue", this.tbOKValue_Delete.Text.Trim());
                        DeleteTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Delete.Text.Trim());
                        smsServerInfo.Additional.DeleteTemplateSettings = DeleteTemplateSettings;

                        //查询模板
                        smsServerInfo.Additional.SearchTemplateUrl = this.tbUrl_Search.Text.Trim();
                        smsServerInfo.Additional.SearchTemplateParams = this.tbParams_Search.Text.Trim();
                        NameValueCollection SearchTemplateSettings = new NameValueCollection();
                        SearchTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Search.SelectedValue);
                        SearchTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Search.SelectedValue);
                        SearchTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Search.SelectedValue);
                        SearchTemplateSettings.Add("OKFlag", this.tbOKFlag_Search.Text.Trim());
                        SearchTemplateSettings.Add("OKValue", this.tbOKValue_Search.Text.Trim());
                        SearchTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Search.Text.Trim());
                        smsServerInfo.Additional.SearchTemplateSettings = SearchTemplateSettings;

                        BaiRongDataProvider.SMSServerDAO.Update(smsServerInfo);
                        SMSServerManager.IsChange = true;
                    }
                }
                else
                {
                    isNeedAdd = true;
                }

                if (isNeedAdd)
                {
                    SMSServerInfo smsServerInfo = new SMSServerInfo();
                    //基本信息
                    smsServerInfo.SMSServerName = this.SmsServerName.Text.Trim();
                    smsServerInfo.SMSServerEName = this.SmsServerEName.Text.Trim();
                    smsServerInfo.IsEnable = true;
                    //公告参数
                    NameValueCollection hfParams = TranslateUtils.ToNameValueCollection(hfEditParams.Value);
                    NameValueCollection smsServerParams = new NameValueCollection();
                    foreach (string key in hfParams)
                    {
                        if (key.IndexOf("smsServerParams_") == 0)
                        {
                            smsServerParams.Add(key.Substring("smsServerParams_".Length), hfParams[key]);
                        }
                    }
                    smsServerInfo.ParamCollection = smsServerParams;

                    //=============扩展属性=============
                    //发送设置
                    smsServerInfo.Additional.SendUrl = this.tbUrl_Send.Text.Trim();
                    smsServerInfo.Additional.SendParams = this.tbParams_Send.Text.Trim();
                    NameValueCollection SendSettings = new NameValueCollection();
                    SendSettings.Add("SendRequestType", this.ddlSendRequestType_Send.SelectedValue);
                    SendSettings.Add("SendParamsType", this.ddlSendParamsType_Send.SelectedValue);
                    SendSettings.Add("ReturnContentType", this.ddlReturnContentType_Send.SelectedValue);
                    SendSettings.Add("OKFlag", this.tbOKFlag_Send.Text.Trim());
                    SendSettings.Add("OKValue", this.tbOKValue_Send.Text.Trim());
                    SendSettings.Add("MsgFlag", this.tbMsgFlag_Send.Text.Trim());
                    smsServerInfo.Additional.SendSettings = SendSettings;

                    //查询剩余短信条数
                    smsServerInfo.Additional.LastSmsSearchUrl = this.tbUrl_LastSms.Text.Trim();
                    smsServerInfo.Additional.LastSmsSearchParams = this.tbParams_LastSms.Text.Trim();
                    NameValueCollection LastSmsSearchSettings = new NameValueCollection();
                    LastSmsSearchSettings.Add("SendRequestType", this.ddlSendRequestType_LastSms.SelectedValue);
                    LastSmsSearchSettings.Add("SendParamsType", this.ddlSendParamsType_LastSms.SelectedValue);
                    LastSmsSearchSettings.Add("ReturnContentType", this.ddlReturnContentType_LastSms.SelectedValue);
                    LastSmsSearchSettings.Add("OKFlag", this.tbOKFlag_LastSms.Text.Trim());
                    LastSmsSearchSettings.Add("OKValue", this.tbOKValue_LastSms.Text.Trim());
                    LastSmsSearchSettings.Add("RetrunValueKey", this.tbRetrunValueKey_LastSms.Text.Trim());
                    LastSmsSearchSettings.Add("MsgFlag", this.tbMsgFlag_LastSms.Text.Trim());
                    smsServerInfo.Additional.LastSmsSearchSettings = LastSmsSearchSettings;

                    //新增模板
                    smsServerInfo.Additional.InsertTemplateUrl = this.tbUrl_Insert.Text.Trim();
                    smsServerInfo.Additional.InsertTemplateParams = this.tbParams_Insert.Text.Trim();
                    NameValueCollection InsertTemplateSettings = new NameValueCollection();
                    InsertTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Insert.SelectedValue);
                    InsertTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Insert.SelectedValue);
                    InsertTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Insert.SelectedValue);
                    InsertTemplateSettings.Add("OKFlag", this.tbOKFlag_Insert.Text.Trim());
                    InsertTemplateSettings.Add("OKValue", this.tbOKValue_Insert.Text.Trim());
                    InsertTemplateSettings.Add("RetrunValueKey", this.tbRetrunValueKey_Insert.Text.Trim());
                    InsertTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Insert.Text.Trim());
                    smsServerInfo.Additional.InsertTemplateSettings = InsertTemplateSettings;

                    //修改模板
                    smsServerInfo.Additional.UpdateTemplateUrl = this.tbUrl_Update.Text.Trim();
                    smsServerInfo.Additional.UpdateTemplateParams = this.tbParams_Update.Text.Trim();
                    NameValueCollection UpdateTemplateSettings = new NameValueCollection();
                    UpdateTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Update.SelectedValue);
                    UpdateTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Update.SelectedValue);
                    UpdateTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Update.SelectedValue);
                    UpdateTemplateSettings.Add("OKFlag", this.tbOKFlag_Update.Text.Trim());
                    UpdateTemplateSettings.Add("OKValue", this.tbOKValue_Update.Text.Trim());
                    UpdateTemplateSettings.Add("RetrunValueKey", this.tbRetrunValueKey_Update.Text.Trim());
                    UpdateTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Update.Text.Trim());
                    smsServerInfo.Additional.UpdateTemplateSettings = UpdateTemplateSettings;

                    //删除模板
                    smsServerInfo.Additional.DeleteTemplateUrl = this.tbUrl_Delete.Text.Trim();
                    smsServerInfo.Additional.DeleteTemplateParams = this.tbParams_Delete.Text.Trim();
                    NameValueCollection DeleteTemplateSettings = new NameValueCollection();
                    DeleteTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Delete.SelectedValue);
                    DeleteTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Delete.SelectedValue);
                    DeleteTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Delete.SelectedValue);
                    DeleteTemplateSettings.Add("OKFlag", this.tbOKFlag_Delete.Text.Trim());
                    DeleteTemplateSettings.Add("OKValue", this.tbOKValue_Delete.Text.Trim());
                    DeleteTemplateSettings.Add("RetrunValueKey", "");
                    DeleteTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Delete.Text.Trim());
                    smsServerInfo.Additional.DeleteTemplateSettings = DeleteTemplateSettings;

                    //查询模板
                    smsServerInfo.Additional.SearchTemplateUrl = this.tbUrl_Search.Text.Trim();
                    smsServerInfo.Additional.SearchTemplateParams = this.tbParams_Search.Text.Trim();
                    NameValueCollection SearchTemplateSettings = new NameValueCollection();
                    SearchTemplateSettings.Add("SendRequestType", this.ddlSendRequestType_Search.SelectedValue);
                    SearchTemplateSettings.Add("SendParamsType", this.ddlSendParamsType_Search.SelectedValue);
                    SearchTemplateSettings.Add("ReturnContentType", this.ddlReturnContentType_Search.SelectedValue);
                    SearchTemplateSettings.Add("OKFlag", this.tbOKFlag_Search.Text.Trim());
                    SearchTemplateSettings.Add("OKValue", this.tbOKValue_Search.Text.Trim());
                    SearchTemplateSettings.Add("RetrunValueKey", this.tbRetrunValueKey_Search.Text.Trim());
                    SearchTemplateSettings.Add("MsgFlag", this.tbMsgFlag_Search.Text.Trim());
                    smsServerInfo.Additional.SearchTemplateSettings = SearchTemplateSettings;

                    BaiRongDataProvider.SMSServerDAO.Insert(smsServerInfo);
                    SMSServerManager.IsChange = true;
                }

                if (isNeedAdd)
                {
                    LogUtils.AddLog(AdminManager.Current.UserName, "添加短信服务商", string.Format("短信服务商:{0}", SmsServerName.Text));
                }
                else
                {
                    LogUtils.AddLog(AdminManager.Current.UserName, "编辑短信服务商", string.Format("短信服务商:{0}", SmsServerName.Text));
                }

                errorMessage = string.Empty;
                return true;
            }
            catch
            {
                errorMessage = "操作失败！";
                return false;
            }
        }
    }
}
