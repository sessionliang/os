using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Controls;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundAppointmentMultipleAdd : BackgroundBasePageWX
	{
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public DateTimeTextBox dtbStartDate;
        public DateTimeTextBox dtbEndDate;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public PlaceHolder phStep2;
        public TextBox tbContentDescription;
        public Literal ltlContentImageUrl;
        public Literal ltlContentResultTopImageUrl;
         
        public PlaceHolder phStep3;
        public Repeater rptContents;
        public SqlPager spContents;
        public Button btnAdd;
        //public Button btnDelete;

        public PlaceHolder phStep4;
        public Literal ltlAwardItems;
        public CheckBox cbIsFormRealName;
        public TextBox tbFormRealNameTitle;
        public CheckBox cbIsFormMobile;
        public TextBox tbFormMobileTitle;
        public CheckBox cbIsFormEmail;
        public TextBox tbFormEmailTitle;
        public CheckBox cbIsFormAddress;
        public TextBox tbFormAddressTitle;

        public PlaceHolder phStep5;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden contentResultTopImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int appointmentID = 0;
        private int appointmentItemID;

        public static string GetRedirectUrl(int publishmentSystemID, int appointmentID, int appointmentItemID)
        {
            //return PageUtils.GetWXUrl(string.Format("background_appointmentMultipleAdd.aspx?publishmentSystemID={0}&appointmentID={1}", publishmentSystemID, appointmentID));
            return PageUtils.GetWXUrl(string.Format("background_appointmentMultipleAdd.aspx?publishmentSystemID={0}&appointmentID={1}&appointmentItemID={2}", publishmentSystemID, appointmentID, appointmentItemID));
        }
         
        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
         
            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.appointmentID = TranslateUtils.ToInt(base.GetQueryString("appointmentID"));
            this.appointmentItemID = TranslateUtils.ToInt(base.GetQueryString("appointmentItemID"));  

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.AppointmentItemDAO.GetSelectString(base.PublishmentSystemID, this.appointmentID);
            this.spContents.SortField = AlbumAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
            {
                 
                string pageTitle = this.appointmentID > 0 ? "编辑微预约" : "添加微预约";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Appointment, pageTitle, AppManager.WeiXin.Permission.WebSite.Appointment);
 
                this.ltlPageTitle.Text = pageTitle;
                  
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentResultTopImageUrl.Text = string.Format(@"<img id=""preview_contentResultTopImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetContentResultTopImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));

                this.spContents.DataBind();
                 
                this.btnAdd.Attributes.Add("onclick",Modal.AppointmentItemAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID,this.appointmentID, 0));

                //string urlDelete = PageUtils.AddQueryString(BackgroundAppointmentMultipleAdd.GetRedirectUrl(base.PublishmentSystemID, this.appointmentID,this.tbTitle.Text), "Delete", "True");
                //this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的微预约项目", "此操作将删除所选微预约项目，确认吗？"));
               
                if (this.appointmentID == 0)
                { 
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
                else
                {
                    AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(this.appointmentID);

                    if (appointmentInfo != null)
                    {
                        this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(appointmentInfo.KeywordID);
                        this.cbIsEnabled.Checked = !appointmentInfo.IsDisabled;
                        this.dtbStartDate.DateTime = appointmentInfo.StartDate;
                        this.dtbEndDate.DateTime = appointmentInfo.EndDate;
                        this.tbTitle.Text = appointmentInfo.Title;
                        if (!string.IsNullOrEmpty(appointmentInfo.ImageUrl))
                        {
                            this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.ImageUrl));
                        }
                        this.tbSummary.Text = appointmentInfo.Summary;
                        if (!string.IsNullOrEmpty(appointmentInfo.ContentImageUrl))
                        {
                            this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.ContentImageUrl));
                        }
                        if (!string.IsNullOrEmpty(appointmentInfo.ContentResultTopImageUrl))
                        {
                            this.ltlContentResultTopImageUrl.Text = string.Format(@"<img id=""preview_contentResultTopImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.ContentResultTopImageUrl));
                        }

                        this.tbContentDescription.Text = appointmentInfo.ContentDescription;

                        this.tbEndTitle.Text = appointmentInfo.EndTitle;
                        this.tbEndSummary.Text = appointmentInfo.EndSummary;
                        if (!string.IsNullOrEmpty(appointmentInfo.EndImageUrl))
                        {
                            this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.EndImageUrl));
                        }

                        this.imageUrl.Value = appointmentInfo.ImageUrl;
                        this.contentImageUrl.Value = appointmentInfo.ContentImageUrl;
                        this.contentResultTopImageUrl.Value = appointmentInfo.ContentResultTopImageUrl;
                        this.endImageUrl.Value = appointmentInfo.EndImageUrl;
                        #region 拓展属性
                        #region 姓名
                        if (appointmentInfo.IsFormRealName == "True")
                        {
                            this.cbIsFormRealName.Checked = true;
                            this.tbFormRealNameTitle.Text = appointmentInfo.FormRealNameTitle;
                        }
                        else if (string.IsNullOrEmpty(appointmentInfo.IsFormRealName))
                        {
                            this.cbIsFormRealName.Checked = true;
                            this.tbFormRealNameTitle.Text = "姓名";
                        }
                        else
                        {
                            this.cbIsFormRealName.Checked = false;
                            this.tbFormRealNameTitle.Text = appointmentInfo.FormRealNameTitle;
                        }
                        #endregion
                        #region 电话
                        if (appointmentInfo.IsFormMobile == "True")
                        {
                            this.cbIsFormMobile.Checked = true;
                            this.tbFormMobileTitle.Text = appointmentInfo.FormMobileTitle;
                        }
                        else if (string.IsNullOrEmpty(appointmentInfo.IsFormMobile))
                        {
                            this.cbIsFormMobile.Checked = true;
                            this.tbFormMobileTitle.Text = "电话";
                        }
                        else
                        {
                            this.cbIsFormMobile.Checked = false;
                            this.tbFormMobileTitle.Text = appointmentInfo.FormMobileTitle;
                        }
                        #endregion
                        #region 邮箱
                        if (appointmentInfo.IsFormEmail == "True")
                        {
                            this.cbIsFormEmail.Checked = true;
                            this.tbFormEmailTitle.Text = appointmentInfo.FormEmailTitle;
                        }
                        else if (string.IsNullOrEmpty(appointmentInfo.IsFormEmail))
                        {
                            this.cbIsFormEmail.Checked = true;
                            this.tbFormEmailTitle.Text = "邮箱";
                        }
                        else
                        {
                            this.cbIsFormEmail.Checked = false;
                            this.tbFormEmailTitle.Text = appointmentInfo.FormEmailTitle;
                        }
                        #endregion

                        this.appointmentItemID = DataProviderWX.AppointmentItemDAO.GetItemID(base.PublishmentSystemID, this.appointmentID);

                        List<ConfigExtendInfo> configExtendInfoList = DataProviderWX.ConfigExtendDAO.GetConfigExtendInfoList(this.PublishmentSystemID, appointmentInfo.ID, EKeywordTypeUtils.GetValue(EKeywordType.Appointment));
                        StringBuilder itemBuilder = new StringBuilder();
                        foreach (ConfigExtendInfo configExtendInfo in configExtendInfoList)
                        {
                            if (string.IsNullOrEmpty(configExtendInfo.IsVisible))
                            {
                                configExtendInfo.IsVisible = "checked=checked";
                            }
                            else if (configExtendInfo.IsVisible == "True")
                            {
                                configExtendInfo.IsVisible = "checked=checked";
                            }
                            else
                            {
                                configExtendInfo.IsVisible = "";
                            }
                            itemBuilder.AppendFormat(@"{{id: '{0}', attributeName: '{1}',isVisible:'{2}'}},", configExtendInfo.ID, configExtendInfo.AttributeName, configExtendInfo.IsVisible);
                        }
                        if (itemBuilder.Length > 0) itemBuilder.Length--;
                        this.ltlAwardItems.Text = string.Format(@"itemController.itemCount = {0};itemController.items = [{1}];", configExtendInfoList.Count, itemBuilder.ToString());
                        #endregion
                    }
                }
               
                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundAppointment.GetRedirectUrl(base.PublishmentSystemID)));
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                int selectedStep = 0;
                if (this.phStep1.Visible)
                {
                    selectedStep = 1;
                }
                else if (this.phStep2.Visible)
                {
                    selectedStep = 2;
                }
                else if (this.phStep3.Visible)
                {
                    selectedStep = 3;
                }
                else if (this.phStep4.Visible)
                {
                    selectedStep = 4;
                }
                else if (this.phStep5.Visible)
                {
                    selectedStep = 5;
                }

			    this.phStep1.Visible = false;
			    this.phStep2.Visible = false;
			    this.phStep3.Visible = false;
			    this.phStep4.Visible = false;
                this.phStep5.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.appointmentID > 0)
                        {
                            AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(this.appointmentID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, appointmentInfo.KeywordID, this.tbKeywords.Text, out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, this.tbKeywords.Text, out conflictKeywords);
                        }
                    }
                    
                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                        this.phStep1.Visible = true;
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 2)
                {
                    this.phStep3.Visible = true;
                }
                else if (selectedStep == 3)
                {
                    this.phStep4.Visible = true;
                }
                else if (selectedStep == 4)
                {
                    bool isItemReady = true;
                    int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);

                    List<int> itemIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemID"]);
                    List<string> attributeNameList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemAttributeName"]);

                    string itemIsVisible = "off";
                    if (!string.IsNullOrEmpty(base.Request.Form["itemIsVisible"]))
                    {
                        itemIsVisible = base.Request.Form["itemIsVisible"];
                    }

                    List<string> isVisibleList = TranslateUtils.StringCollectionToStringList(itemIsVisible);

                    if (isVisibleList.Count < itemIDList.Count)
                    {
                        for (int i = isVisibleList.Count; i < itemIDList.Count; i++)
                        {
                            isVisibleList.Add("off");
                        }
                    }

                    List<ConfigExtendInfo> configExtendInfoList = new List<ConfigExtendInfo>();
                    for (int i = 0; i < itemCount; i++)
                    {
                        ConfigExtendInfo configExtendInfo = new ConfigExtendInfo { ID = itemIDList[i], PublishmentSystemID = base.PublishmentSystemID, KeywordType = EKeywordTypeUtils.GetValue(EKeywordType.Appointment), FunctionID = this.appointmentID, AttributeName = attributeNameList[i], IsVisible = isVisibleList[i] };

                        if (string.IsNullOrEmpty(configExtendInfo.AttributeName))
                        {
                            base.FailMessage("保存失败，属性名称为必填项");
                            isItemReady = false;
                        }
                        if (string.IsNullOrEmpty(configExtendInfo.IsVisible))
                        {
                            base.FailMessage("保存失败，是否必填为显示项");
                            isItemReady = false;
                        }

                        if (configExtendInfo.IsVisible == "on")
                        {
                            configExtendInfo.IsVisible = "True";
                        }
                        else
                        {
                            configExtendInfo.IsVisible = "False";
                        }

                        configExtendInfoList.Add(configExtendInfo);
                    }

                    if (isItemReady)
                    {
                        DataProviderWX.ConfigExtendDAO.DeleteAllNotInIDList(base.PublishmentSystemID, this.appointmentID, itemIDList);

                        foreach (ConfigExtendInfo configExtendInfo in configExtendInfoList)
                        {
                            if (configExtendInfo.ID > 0)
                            {
                                DataProviderWX.ConfigExtendDAO.Update(configExtendInfo);
                            }
                            else
                            {
                                DataProviderWX.ConfigExtendDAO.Insert(configExtendInfo);
                            }
                        }
                    }

                    if (isItemReady)
                    {
                        this.phStep5.Visible = true;
                        this.btnSubmit.Text = "确 认";
                    }
                    else
                    {
                        this.phStep4.Visible = true;
                    }
                }
                else if (selectedStep == 5)
                {
                    AppointmentInfo appointmentInfo = new AppointmentInfo();
                    appointmentInfo.PublishmentSystemID = base.PublishmentSystemID;

                    if (this.appointmentID > 0)
                    {
                        appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(this.appointmentID);
                        DataProviderWX.KeywordDAO.Update(base.PublishmentSystemID, appointmentInfo.KeywordID,
                            EKeywordType.Appointment, EMatchType.Exact, this.tbKeywords.Text, !this.cbIsEnabled.Checked);
                    }
                    else
                    {
                        KeywordInfo keywordInfo = new KeywordInfo();

                        keywordInfo.KeywordID = 0;
                        keywordInfo.PublishmentSystemID = base.PublishmentSystemID;
                        keywordInfo.Keywords = this.tbKeywords.Text;
                        keywordInfo.IsDisabled = !this.cbIsEnabled.Checked;
                        keywordInfo.KeywordType = EKeywordType.Appointment;
                        keywordInfo.MatchType = EMatchType.Exact;
                        keywordInfo.Reply = string.Empty;
                        keywordInfo.AddDate = DateTime.Now;
                        keywordInfo.Taxis = 0;

                        appointmentInfo.KeywordID = DataProviderWX.KeywordDAO.Insert(keywordInfo);
                    }

                    appointmentInfo.StartDate = this.dtbStartDate.DateTime;
                    appointmentInfo.EndDate = this.dtbEndDate.DateTime;
                    appointmentInfo.Title = this.tbTitle.Text;
                    appointmentInfo.ImageUrl = this.imageUrl.Value;
                    ;
                    appointmentInfo.Summary = this.tbSummary.Text;

                    appointmentInfo.ContentImageUrl = this.contentImageUrl.Value;
                    appointmentInfo.ContentDescription = this.tbContentDescription.Text;
                    appointmentInfo.ContentResultTopImageUrl = this.contentResultTopImageUrl.Value;
                    appointmentInfo.ContentIsSingle = false;

                    appointmentInfo.EndTitle = this.tbEndTitle.Text;
                    appointmentInfo.EndImageUrl = this.endImageUrl.Value;
                    appointmentInfo.EndSummary = this.tbEndSummary.Text;

                    appointmentInfo.IsFormRealName = this.cbIsFormRealName.Checked ? "True" : "False";
                    appointmentInfo.FormRealNameTitle = this.tbFormRealNameTitle.Text;
                    appointmentInfo.IsFormMobile = this.cbIsFormMobile.Checked ? "True" : "False";
                    appointmentInfo.FormMobileTitle = this.tbFormMobileTitle.Text;
                    appointmentInfo.IsFormEmail = this.cbIsFormEmail.Checked ? "True" : "False";
                    appointmentInfo.FormEmailTitle = this.tbFormEmailTitle.Text;

                    try
                    {
                        if (this.appointmentID > 0)
                        {
                            DataProviderWX.AppointmentDAO.Update(appointmentInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微预约",
                                string.Format("微预约:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微预约成功！");
                        }
                        else
                        {
                            this.appointmentID = DataProviderWX.AppointmentDAO.Insert(appointmentInfo);

                            DataProviderWX.AppointmentItemDAO.UpdateAppointmentID(base.PublishmentSystemID,
                                this.appointmentID);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微预约",
                                string.Format("微预约:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微预约成功！");
                        }

                        base.AddWaitAndRedirectScript(
                            PageUtils.GetWXUrl(string.Format("background_appointment.aspx?publishmentSystemID={0}",
                                base.PublishmentSystemID)));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微预约设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
 
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlMapAddress = e.Item.FindControl("ltlMapAddress") as Literal;
                Literal ltlTel = e.Item.FindControl("ltlTel") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = appointmentItemInfo.Title;
                ltlMapAddress.Text = appointmentItemInfo.MapAddress;
                ltlTel.Text = appointmentItemInfo.Tel;

                string urlEdit = Modal.AppointmentItemAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.appointmentID, appointmentItemInfo.ID); 
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", urlEdit);
            }
        }

        public string GetIDCollection()
        {
            return base.Request.QueryString["IDCollection"];
        }
         
	}
}
