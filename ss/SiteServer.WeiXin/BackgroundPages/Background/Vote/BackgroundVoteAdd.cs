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
	public class BackgroundVoteAdd : BackgroundBasePageWX
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
        public DropDownList ddlContentIsImageOption;
        public DropDownList ddlContentIsCheckBox;
        public Literal ltlContentImageUrl;
        public Literal ltlVoteItems;

        public PlaceHolder phStep3;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int voteID;

        public static string GetRedirectUrl(int publishmentSystemID, int voteID)
        {
            return PageUtils.GetWXUrl(string.Format("background_voteAdd.aspx?publishmentSystemID={0}&voteID={1}", publishmentSystemID, voteID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.voteID = TranslateUtils.ToInt(base.GetQueryString("voteID"));

			if (!IsPostBack)
            {
                string pageTitle = this.voteID > 0 ? "编辑投票活动" : "添加投票活动";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Vote, pageTitle, AppManager.WeiXin.Permission.WebSite.Vote);
                this.ltlPageTitle.Text = pageTitle;

                ListItem listItem = new ListItem("文字类型投票", "false");
                this.ddlContentIsImageOption.Items.Add(listItem);
                listItem = new ListItem("图文类型投票", "true");
                this.ddlContentIsImageOption.Items.Add(listItem);

                this.ddlContentIsImageOption.Attributes.Add("onchange", "itemController.isImageOptionChange(this)");
                EBooleanUtils.AddListItems(this.ddlContentIsCheckBox, "多选", "单选");
                ControlUtils.SelectListItems(this.ddlContentIsCheckBox, false.ToString());

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", VoteManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", VoteManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", VoteManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));

                string selectImageClick = SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, "itemImageUrl_");
                string uploadImageClick = SiteServer.CMS.BackgroundPages.Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, "itemImageUrl_");
                string cuttingImageClick = SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(base.PublishmentSystemID, "itemImageUrl_");
                string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(base.PublishmentSystemID, "itemImageUrl_");
                this.ltlVoteItems.Text = string.Format(@"itemController.selectImageClickString = ""{0}"";itemController.uploadImageClickString = ""{1}"";itemController.cuttingImageClickString = ""{2}"";itemController.previewImageClickString = ""{3}"";", selectImageClick, uploadImageClick, cuttingImageClick, previewImageClick);

                if (this.voteID == 0)
                {
                    this.ltlVoteItems.Text += "itemController.isImageOption = false;itemController.itemCount = 2;itemController.items = [{}, {}];";
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
                else
                {
                    VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(this.voteID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(voteInfo.KeywordID);
                    this.cbIsEnabled.Checked = !voteInfo.IsDisabled;
                    this.dtbStartDate.DateTime = voteInfo.StartDate;
                    this.dtbEndDate.DateTime = voteInfo.EndDate;
                    this.tbTitle.Text = voteInfo.Title;
                    if (!string.IsNullOrEmpty(voteInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, voteInfo.ImageUrl));
                    }
                    this.tbSummary.Text = voteInfo.Summary;
                    if (!string.IsNullOrEmpty(voteInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, voteInfo.ContentImageUrl));
                    }

                    this.tbContentDescription.Text = voteInfo.ContentDescription;
                    //ControlUtils.SelectListItems(this.ddlContentResultVisible, voteInfo.ContentResultVisible);
                    ControlUtils.SelectListItems(this.ddlContentIsImageOption, voteInfo.ContentIsImageOption.ToString().ToLower());
                    ControlUtils.SelectListItems(this.ddlContentIsCheckBox, voteInfo.ContentIsCheckBox.ToString());

                    List<VoteItemInfo> voteItemInfoList = DataProviderWX.VoteItemDAO.GetVoteItemInfoList(this.voteID);
                    StringBuilder itemBuilder = new StringBuilder();
                    foreach (VoteItemInfo itemInfo in voteItemInfoList)
                    {
                        itemBuilder.AppendFormat(@"{{id: '{0}', title: '{1}', imageUrl: '{2}', navigationUrl: '{3}', voteNum: '{4}'}},", itemInfo.ID, itemInfo.Title, itemInfo.ImageUrl, itemInfo.NavigationUrl, itemInfo.VoteNum);
                    }
                    if (itemBuilder.Length > 0) itemBuilder.Length--;

                    this.ltlVoteItems.Text += string.Format(@"
itemController.isImageOption = {0};itemController.itemCount = {1};itemController.items = [{2}];", voteInfo.ContentIsImageOption.ToString().ToLower(), voteItemInfoList.Count, itemBuilder.ToString());

                    this.tbEndTitle.Text = voteInfo.EndTitle;
                    this.tbEndSummary.Text = voteInfo.EndSummary;
                    if (!string.IsNullOrEmpty(voteInfo.EndImageUrl))
                    {
                        this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, voteInfo.EndImageUrl));
                    }

                    this.imageUrl.Value = voteInfo.ImageUrl;
                    this.contentImageUrl.Value = voteInfo.ContentImageUrl;
                    this.endImageUrl.Value = voteInfo.EndImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundVote.GetRedirectUrl(base.PublishmentSystemID)));
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

                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.voteID > 0)
                        {
                            VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(this.voteID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, voteInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                    bool isItemReady = true;
                    int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);

                    if (itemCount < 2)
                    {
                        base.FailMessage("投票保存失败，至少设置两个投票项");
                        isItemReady = false;
                    }
                    else
                    {
                        bool isImageOption = TranslateUtils.ToBool(this.ddlContentIsImageOption.SelectedValue);

                        List<int> itemIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemID"]);
                        List<string> titleList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemTitle"]);
                        List<string> imageUrlList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemImageUrl"]);
                        List<string> navigationUrlList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemNavigationUrl"]);
                        List<int> voteNumList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemVoteNum"]);
                        List<VoteItemInfo> voteItemInfoList = new List<VoteItemInfo>();
                        for (int i = 0; i < itemCount; i++)
                        {
                            string imageUrl = string.Empty;
                            if (isImageOption)
                            {
                                imageUrl = imageUrlList[i];
                            }
                            VoteItemInfo itemInfo = new VoteItemInfo { ID = itemIDList[i], VoteID = this.voteID, PublishmentSystemID = base.PublishmentSystemID, Title = titleList[i], ImageUrl = imageUrl, NavigationUrl = navigationUrlList[i], VoteNum = voteNumList[i] };

                            if (isImageOption && string.IsNullOrEmpty(itemInfo.ImageUrl))
                            {
                                base.FailMessage("投票保存失败，图片地址为必填项");
                                isItemReady = false;
                            }
                            if (string.IsNullOrEmpty(itemInfo.Title))
                            {
                                base.FailMessage("投票保存失败，选项标题为必填项");
                                isItemReady = false;
                            }

                            voteItemInfoList.Add(itemInfo);
                        }

                        if (isItemReady)
                        {
                            //DataProviderWX.VoteItemDAO.DeleteAll(base.PublishmentSystemID, this.voteID);
                            
                            foreach (VoteItemInfo itemInfo in voteItemInfoList)
                            {
                               VoteItemInfo newVoteItemInfo=DataProviderWX.VoteItemDAO.GetVoteItemInfo(itemInfo.ID);
                               if (itemInfo.ID>0)
                                {
                                    itemInfo.VoteNum = newVoteItemInfo.VoteNum;
                                    DataProviderWX.VoteItemDAO.Update(itemInfo);
                                }
                                else
                                {
                                    DataProviderWX.VoteItemDAO.Insert(itemInfo);
                                }
                            }
                        }
                    }

                    if (isItemReady)
                    {
                        this.phStep3.Visible = true;
                        this.btnSubmit.Text = "确 认";
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 3)
                {
                    VoteInfo voteInfo = new VoteInfo();
                    if (this.voteID > 0)
                    {
                        voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(this.voteID);
                    }
                    voteInfo.PublishmentSystemID = base.PublishmentSystemID;

                    voteInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.voteID > 0, this.tbKeywords.Text, EKeywordType.Vote, voteInfo.KeywordID);
                    voteInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    voteInfo.StartDate = this.dtbStartDate.DateTime;
                    voteInfo.EndDate = this.dtbEndDate.DateTime;
                    voteInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    voteInfo.ImageUrl = this.imageUrl.Value; ;
                    voteInfo.Summary = this.tbSummary.Text;

                    voteInfo.ContentImageUrl = this.contentImageUrl.Value;
                    voteInfo.ContentDescription = this.tbContentDescription.Text;
                    voteInfo.ContentIsImageOption = TranslateUtils.ToBool(this.ddlContentIsImageOption.SelectedValue);
                    voteInfo.ContentIsCheckBox = TranslateUtils.ToBool(this.ddlContentIsCheckBox.SelectedValue);
                    voteInfo.ContentResultVisible = EVoteResultVisibleUtils.GetValue(EVoteResultVisible.After);

                    voteInfo.EndTitle = this.tbEndTitle.Text;
                    voteInfo.EndImageUrl = this.endImageUrl.Value;
                    voteInfo.EndSummary = this.tbEndSummary.Text;

                    try
                    {
                        if (this.voteID > 0)
                        {
                            DataProviderWX.VoteDAO.Update(voteInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改投票活动", string.Format("投票活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改投票活动成功！");
                        }
                        else
                        {
                            this.voteID = DataProviderWX.VoteDAO.Insert(voteInfo);

                            DataProviderWX.VoteItemDAO.UpdateVoteID(base.PublishmentSystemID, this.voteID);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加投票活动", string.Format("投票活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加投票活动成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_vote.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "投票活动设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
