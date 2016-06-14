using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BaiRong.Controls;

using SiteServer.BBS.Model;


using BaiRong.Core;
using SiteServer.BBS.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundAdAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public TextBox AdName;
        public DropDownList AdType;
        public RadioButtonList IsEnabled;
        public CheckBox IsDateLimited;
        public HtmlTableRow StartDateRow;
        public HtmlTableRow EndDateRow;
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;

        public PlaceHolder phCode;
        public TextBox Code;

        public PlaceHolder phText;
        public TextBox TextWord;
        public TextBox TextLink;
        public TextBox TextColor;
        public TextBox TextFontSize;

        public PlaceHolder phImage;
        public TextBox ImageUrl;
        public HtmlInputFile ImageUrlUploader;
        public TextBox ImageLink;
        public TextBox ImageWidth;
        public TextBox ImageHeight;
        public TextBox ImageAlt;

        public PlaceHolder phFlash;
        public TextBox FlashUrl;
        public HtmlInputFile FlashUrlUploader;
        public TextBox FlashWidth;
        public TextBox FlashHeight;

        private int adID;
        private EAdLocation adLocation;

        public static string GetRedirectUrl(int publishmentSystemID, EAdLocation adLocation, int adID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_adAdd.aspx?publishmentSystemID={0}&adLocation={1}&adID={2}", publishmentSystemID, EAdLocationUtils.GetValue(adLocation), adID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.adLocation = EAdLocationUtils.GetEnumType(base.GetQueryString("adLocation"));
            this.adID = base.GetIntQueryString("adID");

            if (!IsPostBack)
            {
                string pageTitle = this.adID > 0 ? "修改广告" : "添加广告";
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, pageTitle, AppManager.BBS.Permission.BBS_Template);

                this.ltlPageTitle.Text = pageTitle;

                this.StartDate.Text = DateTime.Now.ToString(DateUtils.FormatStringDateTime);
                this.EndDate.Text = DateTime.Now.AddMonths(1).ToString(DateUtils.FormatStringDateTime);

                EAdTypeUtils.AddListItems(AdType);
                ControlUtils.SelectListItems(AdType, EAdTypeUtils.GetValue(EAdType.HtmlCode));

                EBooleanUtils.AddListItems(this.IsEnabled);
                ControlUtils.SelectListItems(this.IsEnabled, true.ToString());

                this.ImageUrl.Attributes.Add("onchange", StringUtilityBBS.GetShowImageScript("preview_ImageUrl"));

                if (this.adID > 0)
                {
                    AdInfo adInfo = DataProvider.AdDAO.GetAdInfo(this.adID);
                    this.AdName.Text = adInfo.AdName;
                    this.AdType.SelectedValue = EAdTypeUtils.GetValue(adInfo.AdType);
                    this.IsEnabled.SelectedValue = adInfo.IsEnabled.ToString();
                    this.IsDateLimited.Checked = adInfo.IsDateLimited;
                    this.StartDate.Text = adInfo.StartDate.ToString(DateUtils.FormatStringDateTime);
                    this.EndDate.Text = adInfo.EndDate.ToString(DateUtils.FormatStringDateTime);

                    this.Code.Text = adInfo.Code;
                    this.TextWord.Text = adInfo.TextWord;
                    this.TextLink.Text = adInfo.TextLink;
                    this.TextColor.Text = adInfo.TextColor;
                    this.TextFontSize.Text = adInfo.TextFontSize.ToString();
                    if (adInfo.AdType == EAdType.Image)
                    {
                        this.ImageUrl.Text = adInfo.ImageUrl;
                        this.ImageLink.Text = adInfo.ImageLink;
                        this.ImageWidth.Text = adInfo.ImageWidth.ToString();
                        this.ImageHeight.Text = adInfo.ImageHeight.ToString();
                        this.ImageAlt.Text = adInfo.ImageAlt;
                    }
                    else if (adInfo.AdType == EAdType.Flash)
                    {
                        this.FlashUrl.Text = adInfo.ImageUrl;
                        this.FlashWidth.Text = adInfo.ImageWidth.ToString();
                        this.FlashHeight.Text = adInfo.ImageHeight.ToString();
                    }
                }

                this.ReFresh(null, EventArgs.Empty);
            }

            base.SuccessMessage(string.Empty);
        }

        public string GetPreviewImageSrc(string adType)
        {
            EAdType type = EAdTypeUtils.GetEnumType(adType);
            string imageUrl = this.ImageUrl.Text;
            if (type == EAdType.Flash)
            {
                imageUrl = this.FlashUrl.Text;
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string extension = PathUtils.GetExtension(imageUrl);
                if (EFileSystemTypeUtils.IsImage(extension))
                {
                    return PageUtils.ParseNavigationUrl(imageUrl);
                }
                else if (EFileSystemTypeUtils.IsFlash(extension))
                {
                    return PageUtils.GetIconUrl("flash.jpg");
                }
                else if (EFileSystemTypeUtils.IsPlayer(extension))
                {
                    return PageUtils.GetIconUrl("player.gif");
                }
            }
            return PageUtils.GetIconUrl("empty.gif");
        }

        public void ReFresh(object sender, EventArgs E)
        {
            if (this.IsDateLimited.Checked)
            {
                this.StartDateRow.Visible = true;
                this.EndDateRow.Visible = true;
            }
            else
            {
                this.StartDateRow.Visible = false;
                this.EndDateRow.Visible = false;
            }

            this.phCode.Visible = this.phText.Visible = this.phImage.Visible = this.phFlash.Visible = false;
            this.IsDateLimited.Enabled = true;

            EAdType adType = EAdTypeUtils.GetEnumType(this.AdType.SelectedValue);
            if (adType == EAdType.HtmlCode)
            {
                this.phCode.Visible = true;
            }
            else if (adType == EAdType.JsCode)
            {
                this.phCode.Visible = true;
                this.IsDateLimited.Enabled = false;
            }
            else if (adType == EAdType.Text)
            {
                this.phText.Visible = true;
            }
            else if (adType == EAdType.Image)
            {
                this.phImage.Visible = true;
            }
            else if (adType == EAdType.Flash)
            {
                this.phFlash.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                AdInfo adInfo = new AdInfo(this.adID, base.PublishmentSystemID, this.AdName.Text, EAdTypeUtils.GetEnumType(this.AdType.SelectedValue), this.adLocation, this.Code.Text, this.TextWord.Text, this.TextLink.Text, this.TextColor.Text, TranslateUtils.ToInt(this.TextFontSize.Text), this.ImageUrl.Text, this.ImageLink.Text, TranslateUtils.ToInt(this.ImageWidth.Text), TranslateUtils.ToInt(this.ImageHeight.Text), this.ImageAlt.Text, TranslateUtils.ToBool(this.IsEnabled.SelectedValue), this.IsDateLimited.Checked, TranslateUtils.ToDateTime(this.StartDate.Text), TranslateUtils.ToDateTime(this.EndDate.Text));

                if (adInfo.AdType == EAdType.Flash)
                {
                    adInfo.ImageUrl = this.FlashUrl.Text;
                    adInfo.ImageWidth = TranslateUtils.ToInt(this.FlashWidth.Text);
                    adInfo.ImageHeight = TranslateUtils.ToInt(this.FlashHeight.Text);
                }

                if (this.adID == 0)
                {
                    if (DataProvider.AdDAO.IsExists(base.PublishmentSystemID, this.AdName.Text))
                    {
                        base.FailMessage(string.Format("名称为“{0}”的广告已存在，请更改广告名称！", this.AdName.Text));
                        return;
                    }
                }

                if (adInfo.AdType == EAdType.Image || adInfo.AdType == EAdType.Flash)
                {
                    if (base.Request.Files != null && base.Request.Files.Count >= 1)
                    {
                        HttpPostedFile myFile = base.Request.Files[0];

                        if (myFile != null && "" != myFile.FileName)
                        {
                            string filePath = myFile.FileName;
                            try
                            {
                                string directoryPath = PathUtility.GetPublishmentSystemPath(base.PublishmentSystemID, "upload");
                                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                string fileName = PathUtils.GetFileName(filePath);

                                string localFilePath = PathUtils.Combine(directoryPath, fileName);

                                string fileExtName = PathUtils.GetExtension(filePath);

                                if (!PathUtils.IsFileExtenstionAllowed("gif|jpg|jpeg|bmp|png|swf", fileExtName))
                                {
                                    base.FailMessage("此文件格式不正确，请更换文件上传！");
                                    return;
                                }

                                myFile.SaveAs(localFilePath);

                                adInfo.ImageUrl = PageUtilityBBS.GetBBSUrl(base.PublishmentSystemID, "upload/" + fileName);
                            }
                            catch (Exception ex)
                            {
                                base.FailMessage(ex, string.Format("文件上传失败:{0}", ex.Message));
                                return;
                            }
                        }
                    }
                }

                try
                {
                    if (this.adID > 0)
                    {
                        DataProvider.AdDAO.Update(base.PublishmentSystemID, adInfo);

                        base.SuccessMessage("修改广告成功！");
                    }
                    else
                    {

                        DataProvider.AdDAO.Insert(base.PublishmentSystemID, adInfo);

                        base.SuccessMessage("新增广告成功！");
                    }

                    base.AddWaitAndRedirectScript(BackgroundAd.GetRedirectUrl(base.PublishmentSystemID, this.adLocation));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("操作失败：{0}", ex.Message));
                }
            }
        }
    }
}

