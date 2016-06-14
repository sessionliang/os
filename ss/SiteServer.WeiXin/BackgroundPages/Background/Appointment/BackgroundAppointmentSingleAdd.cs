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
    public class BackgroundAppointmentSingleAdd : BackgroundBasePageWX
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
        public TextBox tbItemTitle;
        public CheckBox cbIsDescription;
        public TextBox tbDescriptionTitle;
        public TextBox tbDescription;
        public Literal ltlTopImageUrl;
        public Literal ltlResultTopImageUrl;
        public HtmlInputHidden topImageUrl;
        public HtmlInputHidden resultTopImageUrl;

        public CheckBox cbIsImageUrl;
        public TextBox tbImageUrlTitle;
        public TextBox tbContentImageUrl;
        public Literal ltlContentImageUrl;

        public CheckBox cbIsVideoUrl;
        public TextBox tbVideoUrlTitle;
        public TextBox tbContentVideoUrl;
        public Literal ltlContentVideoUrl;

        public CheckBox cbIsImageUrlCollection;
        public TextBox tbImageUrlCollectionTitle;
        public Literal ltlScript;
        public HtmlInputHidden imageUrlCollection;
        public HtmlInputHidden largeImageUrlCollection;
        
        public CheckBox cbIsMap;
        public TextBox tbMapTitle;
        public TextBox tbMapAddress;
        public Button btnMapAddress;
        public Literal ltlMap;
        public PlaceHolder phMap;
        
        public CheckBox cbIsTel;
        public TextBox tbTelTitle;
        public TextBox tbTel;
        public PlaceHolder phTel;

        public PlaceHolder phStep3;
        public Literal ltlAwardItems;
        public CheckBox cbIsFormRealName;
        public TextBox tbFormRealNameTitle;
        public CheckBox cbIsFormMobile;
        public TextBox tbFormMobileTitle;
        public CheckBox cbIsFormEmail;
        public TextBox tbFormEmailTitle;
        public CheckBox cbIsFormAddress;
        public TextBox tbFormAddressTitle;

        public PlaceHolder phStep4;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int appointmentID ;
        private int appointmentItemID ;
        public static string GetRedirectUrl(int publishmentSystemID, int appointmentID, int appointmentItemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_appointmentSingleAdd.aspx?publishmentSystemID={0}&appointmentID={1}&appointmentItemID={2}", publishmentSystemID, appointmentID, appointmentItemID));
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

            string selectImageClick = SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.tbContentImageUrl.ClientID);
            string uploadImageClick = SiteServer.CMS.BackgroundPages.Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.tbContentImageUrl.ClientID);
            string cuttingImageClick = SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(base.PublishmentSystemID, this.tbContentImageUrl.ClientID);
            string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(base.PublishmentSystemID, this.tbContentImageUrl.ClientID);
            this.ltlContentImageUrl.Text = string.Format(@"
                      <a class=""btn"" href=""javascript:;"" onclick=""{0};return false;"" title=""选择""><i class=""icon-th""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{1};return false;"" title=""上传""><i class=""icon-arrow-up""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{2};return false;"" title=""裁切""><i class=""icon-crop""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{3};return false;"" title=""预览""><i class=""icon-eye-open""></i></a>", selectImageClick, uploadImageClick, cuttingImageClick, previewImageClick);

            string selectVideoClick = SiteServer.CMS.BackgroundPages.Modal.SelectVideo.GetOpenWindowString(base.PublishmentSystemInfo, this.tbContentVideoUrl.ClientID);
            string uploadVideoClick = SiteServer.CMS.BackgroundPages.Modal.UploadVideo.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.tbContentVideoUrl.ClientID);
            string previewVideoClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewVideoByUrl(base.PublishmentSystemID, this.tbContentVideoUrl.ClientID);
            this.ltlContentVideoUrl.Text = string.Format(@"
                      <a class=""btn"" href=""javascript:;"" onclick=""{0};return false;"" title=""选择""><i class=""icon-th""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{1};return false;"" title=""上传""><i class=""icon-arrow-up""></i></a>
                      <a class=""btn"" href=""javascript:;"" onclick=""{2};return false;"" title=""预览""><i class=""icon-eye-open""></i></a>", selectVideoClick, uploadVideoClick, previewVideoClick);
             
			if (!IsPostBack)
            {
                string pageTitle = this.appointmentID > 0 ? "编辑微预约" : "添加微预约";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Appointment, pageTitle, AppManager.WeiXin.Permission.WebSite.Appointment);
                this.ltlPageTitle.Text = pageTitle;
                  
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlTopImageUrl.Text = string.Format(@"<img id=""preview_topImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetItemTopImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlResultTopImageUrl.Text = string.Format(@"<img id=""preview_resultTopImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetContentResultTopImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));
                
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));
                 
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
                        if (!string.IsNullOrEmpty(appointmentInfo.ContentResultTopImageUrl))
                        {
                            this.ltlResultTopImageUrl.Text = string.Format(@"<img id=""preview_resultTopImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.ContentResultTopImageUrl));
                        }

                        this.tbSummary.Text = appointmentInfo.Summary;

                        this.tbEndTitle.Text = appointmentInfo.EndTitle;
                        this.tbEndSummary.Text = appointmentInfo.EndSummary;
                        if (!string.IsNullOrEmpty(appointmentInfo.EndImageUrl))
                        {
                            this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentInfo.EndImageUrl));
                        }

                        this.imageUrl.Value = appointmentInfo.ImageUrl;
                        this.contentImageUrl.Value = appointmentInfo.ContentImageUrl;
                        this.resultTopImageUrl.Value = appointmentInfo.ContentResultTopImageUrl;
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
                            this.tbFormEmailTitle.Text = "电话";
                        }
                        else
                        {
                            this.cbIsFormEmail.Checked = false;
                            this.tbFormEmailTitle.Text = appointmentInfo.FormEmailTitle;
                        }
                        #endregion

                        this.appointmentItemID = DataProviderWX.AppointmentItemDAO.GetItemID(base.PublishmentSystemID, this.appointmentID);

                        List<ConfigExtendInfo> configExtendInfoList = DataProviderWX.ConfigExtendDAO.GetConfigExtendInfoList(this.PublishmentSystemID, appointmentInfo.ID, EKeywordTypeUtils.GetValue(EKeywordType.Appointment));
                        StringBuilder itemBuilder=new StringBuilder();
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

                if (this.appointmentItemID > 0)
                {
                    AppointmentItemInfo appointmentItemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(this.appointmentItemID);
                    if (appointmentItemInfo != null)
                    {
                        this.tbItemTitle.Text = appointmentItemInfo.Title;
                        this.topImageUrl.Value = appointmentItemInfo.TopImageUrl;
                        this.cbIsDescription.Checked = appointmentItemInfo.IsDescription;
                        this.tbDescriptionTitle.Text = appointmentItemInfo.DescriptionTitle;
                        this.tbDescription.Text = appointmentItemInfo.Description;
                        this.cbIsImageUrl.Checked = appointmentItemInfo.IsImageUrl;
                        this.tbImageUrlTitle.Text = appointmentItemInfo.ImageUrlTitle;
                        this.tbContentImageUrl.Text = appointmentItemInfo.ImageUrl;
                        this.cbIsVideoUrl.Checked = appointmentItemInfo.IsVideoUrl;
                        this.tbVideoUrlTitle.Text = appointmentItemInfo.VideoUrlTitle;
                        this.tbContentVideoUrl.Text = appointmentItemInfo.VideoUrl;
                        this.cbIsImageUrlCollection.Checked = appointmentItemInfo.IsImageUrlCollection;
                        this.tbImageUrlCollectionTitle.Text = appointmentItemInfo.ImageUrlCollectionTitle;
                        this.imageUrlCollection.Value = appointmentItemInfo.ImageUrlCollection;
                        this.largeImageUrlCollection.Value = appointmentItemInfo.LargeImageUrlCollection;
                        this.cbIsMap.Checked = appointmentItemInfo.IsMap;
                        this.tbMapTitle.Text = appointmentItemInfo.MapTitle;
                        this.tbMapAddress.Text = appointmentItemInfo.MapAddress;
                        this.cbIsTel.Checked = appointmentItemInfo.IsTel;
                        this.tbTelTitle.Text = appointmentItemInfo.TelTitle;
                        this.tbTel.Text = appointmentItemInfo.Tel;


                        if (!string.IsNullOrEmpty(appointmentItemInfo.TopImageUrl))
                        {
                            this.ltlTopImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentItemInfo.TopImageUrl));
                        }
                        if (!string.IsNullOrEmpty(appointmentItemInfo.MapAddress))
                        { 
                            this.ltlMap.Text = string.Format(@"<iframe style=""width:100%;height:100%;background-color:#ffffff;margin-bottom:15px;"" scrolling=""auto"" frameborder=""0"" width=""100%"" height=""100%"" src=""{0}""></iframe>", MapManager.GetMapUrl(this.tbMapAddress.Text));
                        }
                        if (!string.IsNullOrEmpty(appointmentItemInfo.ImageUrlCollection))
                        { 
                            StringBuilder scriptBuilder = new StringBuilder();
                            scriptBuilder.AppendFormat(@"
addImage('{0}','{1}');
", appointmentItemInfo.ImageUrlCollection, appointmentItemInfo.LargeImageUrlCollection);

                            this.ltlScript.Text = string.Format(@"
$(document).ready(function(){{
	{0}
}});
", scriptBuilder.ToString());
                        }
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

			    this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.appointmentID > 0)
                        {
                            AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(this.appointmentID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, appointmentInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                    }
                    
                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 2)
                {
                    bool isItemReady = true;
                    AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo();
                    appointmentItemInfo.PublishmentSystemID = base.PublishmentSystemID;
                    if (this.appointmentItemID > 0)
                    {
                        appointmentItemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(this.appointmentItemID);
                    }

                    appointmentItemInfo.AppointmentID = this.appointmentID;
                    appointmentItemInfo.Title =PageUtils.FilterXSS(this.tbItemTitle.Text);
                    appointmentItemInfo.TopImageUrl = this.topImageUrl.Value;
                    appointmentItemInfo.IsDescription = this.cbIsDescription.Checked;
                    appointmentItemInfo.DescriptionTitle = this.tbDescriptionTitle.Text;
                    appointmentItemInfo.Description = this.tbDescription.Text;
                    appointmentItemInfo.IsImageUrl = this.cbIsImageUrl.Checked;
                    appointmentItemInfo.ImageUrlTitle = this.tbImageUrlTitle.Text;
                    appointmentItemInfo.ImageUrl = this.tbContentImageUrl.Text;
                    appointmentItemInfo.IsVideoUrl = this.cbIsVideoUrl.Checked;
                    appointmentItemInfo.VideoUrlTitle = this.tbVideoUrlTitle.Text;
                    appointmentItemInfo.VideoUrl = this.tbContentVideoUrl.Text;
                    appointmentItemInfo.IsImageUrlCollection = this.cbIsImageUrlCollection.Checked;
                    appointmentItemInfo.ImageUrlCollectionTitle = this.tbImageUrlCollectionTitle.Text;
                    appointmentItemInfo.ImageUrlCollection = this.imageUrlCollection.Value;
                    appointmentItemInfo.LargeImageUrlCollection = this.largeImageUrlCollection.Value;
                    appointmentItemInfo.IsMap = this.cbIsMap.Checked;
                    appointmentItemInfo.MapTitle = this.tbMapTitle.Text;
                    appointmentItemInfo.MapAddress = this.tbMapAddress.Text;
                    appointmentItemInfo.IsTel = this.cbIsTel.Checked;
                    appointmentItemInfo.TelTitle = this.tbTelTitle.Text;
                    appointmentItemInfo.Tel = this.tbTel.Text;

                    try
                    {
                        if (this.appointmentItemID > 0)
                        {
                            DataProviderWX.AppointmentItemDAO.Update(appointmentItemInfo);
                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改预约项目", string.Format("预约项目:{0}", this.tbTitle.Text));
                        }
                        else
                        {
                            this.appointmentItemID = DataProviderWX.AppointmentItemDAO.Insert(appointmentItemInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "新增预约项目", string.Format("预约项目:{0}", this.tbTitle.Text));
                        }
                    }
                    catch (Exception ex)
                    {
                        isItemReady = false;
                        base.FailMessage(ex, "微预约项目设置失败！");
                    }

                    if (isItemReady)
                    {
                        this.phStep3.Visible = true;
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 3)
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
                        ConfigExtendInfo configExtendInfo = new ConfigExtendInfo { ID = itemIDList[i], PublishmentSystemID = base.PublishmentSystemID, KeywordType =EKeywordTypeUtils.GetValue( EKeywordType.Appointment), FunctionID = this.appointmentID, AttributeName = attributeNameList[i], IsVisible = isVisibleList[i] };

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
                        this.phStep4.Visible = true;
                        this.btnSubmit.Text = "确 认";
                    }
                    else
                    {
                        this.phStep3.Visible = true;
                    }

                }
                else if (selectedStep==4)
                {
                    AppointmentInfo appointmentInfo = new AppointmentInfo();
                    appointmentInfo.PublishmentSystemID = base.PublishmentSystemID;

                    if (this.appointmentID > 0)
                    {
                        appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(this.appointmentID);
                        DataProviderWX.KeywordDAO.Update(base.PublishmentSystemID, appointmentInfo.KeywordID, EKeywordType.Appointment, EMatchType.Exact, this.tbKeywords.Text, !this.cbIsEnabled.Checked);
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
                    appointmentInfo.ContentResultTopImageUrl = this.resultTopImageUrl.Value;
                    appointmentInfo.Summary = this.tbSummary.Text;
                    appointmentInfo.ContentIsSingle = true;
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

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微预约", string.Format("微预约:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微预约成功！");
                        }
                        else
                        {
                            this.appointmentID = DataProviderWX.AppointmentDAO.Insert(appointmentInfo);
                            DataProviderWX.AppointmentItemDAO.UpdateAppointmentID(base.PublishmentSystemID, this.appointmentID);
                            DataProviderWX.ConfigExtendDAO.UpdateFuctionID(base.PublishmentSystemID, this.appointmentID);
                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微预约", string.Format("微预约:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微预约成功！");
                        }

                        base.AddWaitAndRedirectScript(PageUtils.GetWXUrl(string.Format("background_appointment.aspx?publishmentSystemID={0}", base.PublishmentSystemID)));
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
  
        public void Preview_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            { 
                this.ltlMap.Text = string.Format(@"<iframe style=""width:100%;height:100%;background-color:#ffffff;margin-bottom:15px;"" scrolling=""auto"" frameborder=""0"" width=""100%"" height=""100%"" src=""{0}""></iframe>", MapManager.GetMapUrl(this.tbMapAddress.Text));
            }
        }
    }
}
