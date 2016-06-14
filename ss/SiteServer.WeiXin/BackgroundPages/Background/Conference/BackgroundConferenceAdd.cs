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
    public class BackgroundConferenceAdd : BackgroundBasePageWX
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
        public Literal ltlBackgroundImageUrl;
        public TextBox tbConferenceName;
        public TextBox tbAddress;
        public TextBox tbDuration;
        public BREditor breIntroduction;

        public PlaceHolder phStep3;
        public CheckBox cbIsAgenda;
        public TextBox tbAgendaTitle;
        public Literal ltlAgendaScript;

        public PlaceHolder phStep4;
        public CheckBox cbIsGuest;
        public TextBox tbGuestTitle;
        public Literal ltlGuestScript;

        public PlaceHolder phStep5;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden backgroundImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int conferenceID;

        public static string GetRedirectUrl(int publishmentSystemID, int conferenceID)
        {
            return PageUtils.GetWXUrl(string.Format("background_conferenceAdd.aspx?publishmentSystemID={0}&conferenceID={1}", publishmentSystemID, conferenceID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.conferenceID = TranslateUtils.ToInt(base.GetQueryString("conferenceID"));

			if (!IsPostBack)
            {
                string pageTitle = this.conferenceID > 0 ? "编辑会议（活动）" : "添加会议（活动）";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Conference, pageTitle, AppManager.WeiXin.Permission.WebSite.Conference);
                this.ltlPageTitle.Text = pageTitle;
                
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", ConferenceManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlBackgroundImageUrl.Text = string.Format(@"{0}<hr /><img id=""preview_backgroundImageUrl"" src=""{1}"" width=""370"" align=""middle"" />", ComponentsManager.GetBackgroundImageSelectHtml(string.Empty), ComponentsManager.GetBackgroundImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", ConferenceManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));

                string selectImageClick = SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, "itemPicUrl_");
                string uploadImageClick = SiteServer.CMS.BackgroundPages.Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, "itemPicUrl_");
                string cuttingImageClick = SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(base.PublishmentSystemID, "itemPicUrl_");
                string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(base.PublishmentSystemID, "itemPicUrl_");
                this.ltlGuestScript.Text = string.Format(@"guestController.selectImageClickString = ""{0}"";guestController.uploadImageClickString = ""{1}"";guestController.cuttingImageClickString = ""{2}"";guestController.previewImageClickString = ""{3}"";", selectImageClick, uploadImageClick, cuttingImageClick, previewImageClick);

                if (this.conferenceID == 0)
                {
                    this.ltlAgendaScript.Text += "agendaController.agendaCount = 2;agendaController.items = [{}, {}];";
                    this.ltlGuestScript.Text += "guestController.guestCount = 2;guestController.items = [{}, {}];";
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
                else
                {
                    ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(this.conferenceID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(conferenceInfo.KeywordID);
                    this.cbIsEnabled.Checked = !conferenceInfo.IsDisabled;
                    this.dtbStartDate.DateTime = conferenceInfo.StartDate;
                    this.dtbEndDate.DateTime = conferenceInfo.EndDate;
                    this.tbTitle.Text = conferenceInfo.Title;
                    if (!string.IsNullOrEmpty(conferenceInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, conferenceInfo.ImageUrl));
                    }
                    this.tbSummary.Text = conferenceInfo.Summary;
                    if (!string.IsNullOrEmpty(conferenceInfo.BackgroundImageUrl))
                    {
                        this.ltlBackgroundImageUrl.Text = string.Format(@"{0}<hr /><img id=""preview_backgroundImageUrl"" src=""{1}"" width=""370"" align=""middle"" />", ComponentsManager.GetBackgroundImageSelectHtml(conferenceInfo.BackgroundImageUrl), ComponentsManager.GetBackgroundImageUrl(base.PublishmentSystemInfo, conferenceInfo.BackgroundImageUrl));
                    }

                    this.tbConferenceName.Text = conferenceInfo.ConferenceName;
                    this.tbAddress.Text = conferenceInfo.Address;
                    this.tbDuration.Text = conferenceInfo.Duration;
                    this.breIntroduction.Text = conferenceInfo.Introduction;

                    this.cbIsAgenda.Checked = conferenceInfo.IsAgenda;
                    this.tbAgendaTitle.Text = conferenceInfo.AgendaTitle;
                    List<ConferenceAgendaInfo> agendaInfoList = new List<ConferenceAgendaInfo>();
                    agendaInfoList = TranslateUtils.JsonToObject(conferenceInfo.AgendaList, agendaInfoList) as List<ConferenceAgendaInfo>;
                    if (agendaInfoList != null)
                    {
                        StringBuilder agendaBuilder = new StringBuilder();
                        foreach (ConferenceAgendaInfo agendaInfo in agendaInfoList)
                        {
                            agendaBuilder.AppendFormat(@"{{dateTime: '{0}', title: '{1}', summary: '{2}'}},", agendaInfo.dateTime, agendaInfo.title, agendaInfo.summary);
                        }
                        if (agendaBuilder.Length > 0) agendaBuilder.Length--;

                        this.ltlAgendaScript.Text += string.Format(@"agendaController.agendaCount = {0};agendaController.items = [{1}];", agendaInfoList.Count, agendaBuilder.ToString());
                    }
                    else
                    {
                        this.ltlAgendaScript.Text += "agendaController.agendaCount = 0;agendaController.items = [{}];";
                    }

                    this.cbIsGuest.Checked = conferenceInfo.IsGuest;
                    this.tbGuestTitle.Text = conferenceInfo.GuestTitle;
                    List<ConferenceGuestInfo> guestInfoList = new List<ConferenceGuestInfo>();
                    guestInfoList = TranslateUtils.JsonToObject(conferenceInfo.GuestList, guestInfoList) as List<ConferenceGuestInfo>;
                    if (guestInfoList != null)
                    {
                        StringBuilder guestBuilder = new StringBuilder();
                        foreach (ConferenceGuestInfo guestInfo in guestInfoList)
                        {
                            guestBuilder.AppendFormat(@"{{displayName: '{0}', position: '{1}', picUrl: '{2}'}},", guestInfo.displayName, guestInfo.position, guestInfo.picUrl);
                        }
                        if (guestBuilder.Length > 0) guestBuilder.Length--;

                        this.ltlGuestScript.Text += string.Format(@"guestController.guestCount = {0};guestController.items = [{1}];", guestInfoList.Count, guestBuilder.ToString());
                    }
                    else
                    {
                        this.ltlGuestScript.Text += "guestController.guestCount = 0;guestController.items = [{}];";
                    }

                    this.tbEndTitle.Text = conferenceInfo.EndTitle;
                    this.tbEndSummary.Text = conferenceInfo.EndSummary;
                    if (!string.IsNullOrEmpty(conferenceInfo.EndImageUrl))
                    {
                        this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, conferenceInfo.EndImageUrl));
                    }

                    this.imageUrl.Value = conferenceInfo.ImageUrl;
                    this.backgroundImageUrl.Value = conferenceInfo.BackgroundImageUrl;
                    this.endImageUrl.Value = conferenceInfo.EndImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundConference.GetRedirectUrl(base.PublishmentSystemID)));
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

                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = this.phStep4.Visible = this.phStep5.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.conferenceID > 0)
                        {
                            ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(this.conferenceID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, conferenceInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                    bool isItemReady = true;
                    int agendaCount = TranslateUtils.ToInt(base.Request.Form["agendaCount"]);

                    if (this.cbIsAgenda.Checked && agendaCount < 2)
                    {
                        base.FailMessage("微会议保存失败，至少需要设置两个日程项");
                        isItemReady = false;
                    }
                    else
                    {
                        List<string> dateTimeList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemDateTime"]);
                        List<string> titleList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemTitle"]);
                        List<string> summaryList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemSummary"]);
                        List<ConferenceAgendaInfo> agendaInfoList = new List<ConferenceAgendaInfo>();

                        for (int i = 0; i < agendaCount; i++)
                        {
                            string dateTime = dateTimeList[i];
                            string title = titleList[i];
                            string summary = summaryList[i];

                            if (string.IsNullOrEmpty(dateTime) || string.IsNullOrEmpty(title))
                            {
                                base.FailMessage("微会议保存失败，日程项不能为空");
                                isItemReady = false;
                            }

                            ConferenceAgendaInfo agendaInfo = new ConferenceAgendaInfo { dateTime = dateTime, title = title, summary = summary };

                            agendaInfoList.Add(agendaInfo);
                        }

                        if (isItemReady)
                        {
                            Page.Session.Add("BackgroundConferenceAdd.AgendaList", TranslateUtils.ObjectToJson(agendaInfoList));
                        }
                    }

                    if (isItemReady)
                    {
                        this.phStep4.Visible = true;
                    }
                    else
                    {
                        this.phStep3.Visible = true;
                    }
                }
                else if (selectedStep == 4)
                {
                    bool isItemReady = true;
                    int guestCount = TranslateUtils.ToInt(base.Request.Form["guestCount"]);

                    if (this.cbIsGuest.Checked && guestCount < 2)
                    {
                        base.FailMessage("微会议保存失败，至少需要设置两个嘉宾项");
                        isItemReady = false;
                    }
                    else
                    {
                        List<string> displayNameList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemDisplayName"]);
                        List<string> positionList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemPosition"]);
                        List<string> picUrlList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemPicUrl"]);
                        List<ConferenceGuestInfo> guestInfoList = new List<ConferenceGuestInfo>();

                        for (int i = 0; i < guestCount; i++)
                        {
                            string displayName = displayNameList[i];
                            string position = positionList[i];
                            string picUrl = picUrlList[i];

                            if (string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(position))
                            {
                                base.FailMessage("微会议保存失败，嘉宾项不能为空");
                                isItemReady = false;
                            }

                            ConferenceGuestInfo guestInfo = new ConferenceGuestInfo { displayName = displayName, position = position, picUrl = picUrl };

                            guestInfoList.Add(guestInfo);
                        }

                        if (isItemReady)
                        {
                            Page.Session.Add("BackgroundConferenceAdd.GuestList", TranslateUtils.ObjectToJson(guestInfoList));
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
                    ConferenceInfo conferenceInfo = new ConferenceInfo();
                    if (this.conferenceID > 0)
                    {
                        conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(this.conferenceID);
                    }

                    conferenceInfo.PublishmentSystemID = base.PublishmentSystemID;

                    conferenceInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.conferenceID > 0, this.tbKeywords.Text, EKeywordType.Conference, conferenceInfo.KeywordID);
                    conferenceInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    conferenceInfo.StartDate = this.dtbStartDate.DateTime;
                    conferenceInfo.EndDate = this.dtbEndDate.DateTime;
                    conferenceInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    conferenceInfo.ImageUrl = this.imageUrl.Value; ;
                    conferenceInfo.Summary = this.tbSummary.Text;

                    conferenceInfo.BackgroundImageUrl = this.backgroundImageUrl.Value;
                    conferenceInfo.ConferenceName = this.tbConferenceName.Text;
                    conferenceInfo.Address = this.tbAddress.Text;
                    conferenceInfo.Duration = this.tbDuration.Text;
                    conferenceInfo.Introduction = this.breIntroduction.Text;

                    conferenceInfo.IsAgenda = this.cbIsAgenda.Checked;
                    conferenceInfo.AgendaTitle = this.tbAgendaTitle.Text;
                    conferenceInfo.AgendaList = Page.Session["BackgroundConferenceAdd.AgendaList"] as string;
                    Page.Session.Remove("BackgroundConferenceAdd.AgendaList");

                    conferenceInfo.IsGuest = this.cbIsGuest.Checked;
                    conferenceInfo.GuestTitle = this.tbGuestTitle.Text;
                    conferenceInfo.GuestList = Page.Session["BackgroundConferenceAdd.GuestList"] as string;
                    Page.Session.Remove("BackgroundConferenceAdd.GuestList");

                    conferenceInfo.EndTitle = this.tbEndTitle.Text;
                    conferenceInfo.EndImageUrl = this.endImageUrl.Value;
                    conferenceInfo.EndSummary = this.tbEndSummary.Text;

                    try
                    {
                        if (this.conferenceID > 0)
                        {
                            DataProviderWX.ConferenceDAO.Update(conferenceInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改会议（活动）", string.Format("会议（活动）:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改会议（活动）成功！");
                        }
                        else
                        {
                            this.conferenceID = DataProviderWX.ConferenceDAO.Insert(conferenceInfo);

                            //DataProviderWX.ConferenceItemDAO.UpdateConferenceID(base.PublishmentSystemID, this.conferenceID);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加会议（活动）", string.Format("会议（活动）:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加会议（活动）成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_conference.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "会议（活动）设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
