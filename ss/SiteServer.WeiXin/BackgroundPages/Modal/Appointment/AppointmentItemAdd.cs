using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;

using System.Text;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AppointmentItemAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public TextBox tbTitle;
        public CheckBox cbIsDescription;
        public TextBox tbDescriptionTitle;
        public TextBox tbDescription;
        public Literal ltlTopImageUrl;
        public HtmlInputHidden topImageUrl;

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
        //public Button btnMapAddress;
        public Literal ltlMap;
       
        public CheckBox cbIsTel;
        public TextBox tbTelTitle;
        public TextBox tbTel;

        private int appointmentID;
        private int appointmentItemID;
         
        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int appointmentID, int appointmentItemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("appointmentID", appointmentID.ToString());
            arguments.Add("appointmentItemID", appointmentItemID.ToString());
            return PageUtilityWX.GetOpenWindowString("添加预约项目", "modal_appointmentItemAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int appointmentID, int appointmentItemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("appointmentID", appointmentID.ToString());
            arguments.Add("appointmentItemID", appointmentItemID.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑预约项目", "modal_appointmentItemAdd.aspx", arguments);
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

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
                this.ltlTopImageUrl.Text = string.Format(@"<img id=""preview_topImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", AppointmentManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                 
                if (this.appointmentItemID > 0)
                {
                    AppointmentItemInfo appointmentItemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(this.appointmentItemID);
                    if (appointmentItemInfo != null)
                    {
                        this.tbTitle.Text = appointmentItemInfo.Title;
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
                            this.ltlTopImageUrl.Text = string.Format(@"<img id=""preview_topImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, appointmentItemInfo.TopImageUrl));
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
             }

           // this.btnAddImageUrl.Attributes.Add("onclick", Modal.AppointmentItemPhotoUpload.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.imageUrlCollection.Value));
         }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                bool isAdd = false;
                AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo();
                appointmentItemInfo.PublishmentSystemID = base.PublishmentSystemID;
                if (this.appointmentItemID > 0)
                {
                    appointmentItemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(this.appointmentItemID);
                }

                appointmentItemInfo.AppointmentID = this.appointmentID;
                appointmentItemInfo.Title = this.tbTitle.Text;
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

                if (this.appointmentItemID > 0)
                {
                    DataProviderWX.AppointmentItemDAO.Update(appointmentItemInfo);
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改预约项目", string.Format("预约项目:{0}", this.tbTitle.Text));
                }
                else
                {
                    isAdd = true;
                    this.appointmentItemID = DataProviderWX.AppointmentItemDAO.Insert(appointmentItemInfo);
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "新增预约项目", string.Format("预约项目:{0}", this.tbTitle.Text));
                }

                string scripts = string.Format("window.parent.addItem('{0}', '{1}','{2}','{3}','{4}','{5}','{6}');", this.tbTitle.Text, this.tbMapAddress.Text, this.tbTel.Text, base.PublishmentSystemID, this.appointmentID, this.appointmentItemID,isAdd.ToString());
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
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
