using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;



using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class AdMaterialAdd : BackgroundBasePage
    {
        public TextBox AdMaterialName;
        public DropDownList AdMaterialType;
        public RadioButtonList IsEnabled;

        public PlaceHolder phCode;
        public TextBox Code;

        public PlaceHolder phText;
        public TextBox TextWord;
        public TextBox TextLink;
        public TextBox TextColor;
        public TextBox TextFontSize;

        public PlaceHolder phImage;
        public TextBox ImageUrl;
        public Button ImageUrlSelect;
        public Button ImageUrlUpload;
        public TextBox ImageLink;
        public TextBox ImageWidth;
        public TextBox ImageHeight;
        public TextBox ImageAlt;

        public PlaceHolder phFlash;
        public TextBox FlashUrl;
        public Button FlashUrlSelect;
        public Button FlashUrlUpload;
        public TextBox FlashWidth;
        public TextBox FlashHeight;
        public DropDownList Weight;
        public PlaceHolder phWeight;

        private bool isEdit = false;
        private int advID;
        private int theAdMaterialID;

        public static string GetOpenWindowStringToAdd(int adMaterialID, int publishmentSystemID, int advID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("adMaterialID", adMaterialID.ToString());
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("advID", advID.ToString());
            return PageUtility.GetOpenWindowString("添加广告物料", "modal_adMaterialAdd.aspx", arguments, 900, 520);
        }

        public static string GetOpenWindowStringToEdit(int adMaterialID, int publishmentSystemID, int advID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("adMaterialID", adMaterialID.ToString());
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("advID", advID.ToString());
            return PageUtility.GetOpenWindowString("修改广告物料", "modal_adMaterialAdd.aspx", arguments, 900, 520);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.advID = TranslateUtils.ToInt(base.GetQueryString("AdvID"));

            if (base.GetQueryString("AdMaterialID") != null)
            {
                this.theAdMaterialID = TranslateUtils.ToInt(base.GetQueryString("AdMaterialID"));
                if (this.theAdMaterialID > 0)
                {
                    this.isEdit = true;
                }
            }

            if (!Page.IsPostBack)
            {

                EAdvTypeUtils.AddListItems(AdMaterialType);
                ControlUtils.SelectListItems(AdMaterialType, EAdvTypeUtils.GetValue(EAdvType.HtmlCode));

                EBooleanUtils.AddListItems(this.IsEnabled);
                ControlUtils.SelectListItems(this.IsEnabled, true.ToString());

                EAdvWeightUtils.AddListItems(this.Weight);
                ControlUtils.SelectListItems(this.Weight, EAdvWeightUtils.GetValue(EAdvWeight.Level1));

                AdvInfo advInfo = DataProvider.AdvDAO.GetAdvInfo(this.advID, base.PublishmentSystemID);
                if (advInfo != null)
                {
                    if (advInfo.RotateType == EAdvRotateType.HandWeight)
                    {
                        this.phWeight.Visible = true;
                    }
                    else
                    {
                        this.phWeight.Visible = false;
                    }
                }
                this.ImageUrl.Attributes.Add("onchange", JsManager.GetShowImageScript("preview_ImageUrl", base.PublishmentSystemInfo.PublishmentSystemUrl));

                string showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.ImageUrl.ClientID);
                this.ImageUrlSelect.Attributes.Add("onclick", showPopWinString);

                //false -- 不添加水印
                showPopWinString = Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.ImageUrl.ClientID, false);
                this.ImageUrlUpload.Attributes.Add("onclick", showPopWinString);

                this.FlashUrl.Attributes.Add("onchange", JsManager.GetShowImageScript("preview_FlashUrl", base.PublishmentSystemInfo.PublishmentSystemUrl));

                showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.FlashUrl.ClientID);
                this.FlashUrlSelect.Attributes.Add("onclick", showPopWinString);

                //false -- 不添加水印
                showPopWinString = Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.FlashUrl.ClientID, false);
                this.FlashUrlUpload.Attributes.Add("onclick", showPopWinString);

                if (this.isEdit)
                {
                    AdMaterialInfo adMaterialInfo = DataProvider.AdMaterialDAO.GetAdMaterialInfo(this.theAdMaterialID, base.PublishmentSystemID);
                    this.AdMaterialName.Text = adMaterialInfo.AdMaterialName;
                    this.AdMaterialType.SelectedValue = EAdvTypeUtils.GetValue(adMaterialInfo.AdMaterialType);
                    this.IsEnabled.SelectedValue = adMaterialInfo.IsEnabled.ToString();

                    this.Code.Text = adMaterialInfo.Code;
                    this.TextWord.Text = adMaterialInfo.TextWord;
                    this.TextLink.Text = adMaterialInfo.TextLink;
                    this.TextColor.Text = adMaterialInfo.TextColor;
                    this.TextFontSize.Text = adMaterialInfo.TextFontSize.ToString();
                    this.Weight.SelectedValue = adMaterialInfo.Weight.ToString();
                    if (adMaterialInfo.AdMaterialType == EAdvType.Image)
                    {
                        this.ImageUrl.Text = adMaterialInfo.ImageUrl;
                        this.ImageLink.Text = adMaterialInfo.ImageLink;
                        this.ImageWidth.Text = adMaterialInfo.ImageWidth.ToString();
                        this.ImageHeight.Text = adMaterialInfo.ImageHeight.ToString();
                        this.ImageAlt.Text = adMaterialInfo.ImageAlt;
                    }
                    else if (adMaterialInfo.AdMaterialType == EAdvType.Flash)
                    {
                        this.FlashUrl.Text = adMaterialInfo.ImageUrl;
                        this.FlashWidth.Text = adMaterialInfo.ImageWidth.ToString();
                        this.FlashHeight.Text = adMaterialInfo.ImageHeight.ToString();
                    }
                }

                this.ReFresh(null, EventArgs.Empty);
            }

            base.SuccessMessage(string.Empty);
        }

        public string GetPreviewImageSrc(string adType)
        {
            EAdvType type = EAdvTypeUtils.GetEnumType(adType);
            string imageUrl = this.ImageUrl.Text;
            if (type == EAdvType.Flash)
            {
                imageUrl = this.FlashUrl.Text;
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string extension = PathUtils.GetExtension(imageUrl);
                if (EFileSystemTypeUtils.IsImage(extension))
                {
                    return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl);
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

            this.phCode.Visible = this.phText.Visible = this.phImage.Visible = this.phFlash.Visible = false;

            EAdvType adType = EAdvTypeUtils.GetEnumType(this.AdMaterialType.SelectedValue);
            if (adType == EAdvType.HtmlCode)
            {
                this.phCode.Visible = true;
            }
            else if (adType == EAdvType.JsCode)
            {
                this.phCode.Visible = true;
            }
            else if (adType == EAdvType.Text)
            {
                this.phText.Visible = true;
            }
            else if (adType == EAdvType.Image)
            {
                this.phImage.Visible = true;
            }
            else if (adType == EAdvType.Flash)
            {
                this.phFlash.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.isEdit == false)
                {
                    if (DataProvider.AdMaterialDAO.IsExists(this.AdMaterialName.Text, base.PublishmentSystemID))
                    {
                        base.FailMessage(string.Format("名称为“{0}”的广告物料已存在，请更改广告物料名称！", this.AdMaterialName.Text));
                        return;
                    }
                }

                try
                {
                    if (this.isEdit)
                    {
                        AdMaterialInfo adMaterialInfo = DataProvider.AdMaterialDAO.GetAdMaterialInfo(this.theAdMaterialID, base.PublishmentSystemID);
                        adMaterialInfo.AdMaterialName = this.AdMaterialName.Text;
                        adMaterialInfo.AdMaterialType = EAdvTypeUtils.GetEnumType(this.AdMaterialType.SelectedValue);
                        adMaterialInfo.IsEnabled = TranslateUtils.ToBool(this.IsEnabled.SelectedValue);

                        adMaterialInfo.Code = this.Code.Text;
                        adMaterialInfo.TextWord = this.TextWord.Text;
                        adMaterialInfo.TextLink = this.TextLink.Text;
                        adMaterialInfo.TextColor = this.TextColor.Text;
                        adMaterialInfo.TextFontSize = TranslateUtils.ToInt(this.TextFontSize.Text);
                        adMaterialInfo.Weight = TranslateUtils.ToInt(this.Weight.SelectedValue);
                        if (adMaterialInfo.AdMaterialType == EAdvType.Image)
                        {
                            adMaterialInfo.ImageUrl = this.ImageUrl.Text;
                            adMaterialInfo.ImageLink = this.ImageLink.Text;
                            adMaterialInfo.ImageWidth = TranslateUtils.ToInt(this.ImageWidth.Text);
                            adMaterialInfo.ImageHeight = TranslateUtils.ToInt(this.ImageHeight.Text);
                            adMaterialInfo.ImageAlt = this.ImageAlt.Text;
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Flash)
                        {
                            adMaterialInfo.ImageUrl = this.FlashUrl.Text;
                            adMaterialInfo.ImageWidth = TranslateUtils.ToInt(this.FlashWidth.Text);
                            adMaterialInfo.ImageHeight = TranslateUtils.ToInt(this.FlashHeight.Text);
                        }

                        DataProvider.AdMaterialDAO.Update(adMaterialInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "修改广告物料", string.Format("广告物料名称：{0}", adMaterialInfo.AdMaterialName));

                        base.SuccessMessage("修改广告物料成功！");
                    }
                    else
                    {
                        AdMaterialInfo adMaterialInfo = new AdMaterialInfo(0, base.PublishmentSystemID, this.advID, this.AdMaterialName.Text, EAdvTypeUtils.GetEnumType(this.AdMaterialType.SelectedValue), this.Code.Text, this.TextWord.Text, this.TextLink.Text, this.TextColor.Text, TranslateUtils.ToInt(this.TextFontSize.Text), this.ImageUrl.Text, this.ImageLink.Text, TranslateUtils.ToInt(this.ImageWidth.Text), TranslateUtils.ToInt(this.ImageHeight.Text), this.ImageAlt.Text, TranslateUtils.ToInt(this.Weight.SelectedValue), TranslateUtils.ToBool(this.IsEnabled.SelectedValue));
                        if (adMaterialInfo.AdMaterialType == EAdvType.Flash)
                        {
                            adMaterialInfo.ImageUrl = this.FlashUrl.Text;
                            adMaterialInfo.ImageWidth = TranslateUtils.ToInt(this.FlashWidth.Text);
                            adMaterialInfo.ImageHeight = TranslateUtils.ToInt(this.FlashHeight.Text);
                        }

                        DataProvider.AdMaterialDAO.Insert(adMaterialInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "新增广告物料", string.Format("广告物料名称：{0}", adMaterialInfo.AdMaterialName));

                        base.SuccessMessage("新增广告物料成功！");
                    }
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_adMaterial.aspx?PublishmentSystemID={0}&AdvID={1}", base.PublishmentSystemID, this.advID)));

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("操作失败：{0}", ex.Message));
                }
            }
        }

    }
}
