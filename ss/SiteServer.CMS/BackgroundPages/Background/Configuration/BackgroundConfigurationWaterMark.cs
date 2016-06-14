using System;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


using System.Web;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundConfigurationWaterMark : BackgroundBasePage
    {
        public RadioButtonList IsWaterMark;
        public Literal WaterMarkPosition;
        public Control WaterMarkPositionRow;
        public DropDownList WaterMarkTransparency;
        public Control WaterMarkTransparencyRow;
        public TextBox WaterMarkMinWidth;
        public TextBox WaterMarkMinHeight;
        public Control WaterMarkMinRow;
        public RadioButtonList IsImageWaterMark;
        public Control IsImageWaterMarkRow;
        public TextBox WaterMarkFormatString;
        public Control WaterMarkFormatStringRow;
        public DropDownList WaterMarkFontName;
        public Control WaterMarkFontNameRow;
        public TextBox WaterMarkFontSize;
        public Control WaterMarkFontSizeRow;
        public TextBox WaterMarkImagePath;
        public Control WaterMarkImagePathRow;
        public Button ImageUrlSelect;
        public Button ImageUrlUpload;

        //水印图片质量
        public DropDownList WaterMarkQty;
        public Control WaterMarkQtyRow;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "图片水印设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.IsWaterMark);
                ControlUtils.SelectListItemsIgnoreCase(this.IsWaterMark, base.PublishmentSystemInfo.Additional.IsWaterMark.ToString());

                this.LoadWaterMarkPosition(base.PublishmentSystemInfo.Additional.WaterMarkPosition);

                for (int i = 1; i <= 10; i++)
                {
                    this.WaterMarkTransparency.Items.Add(new ListItem(i + "0%", i.ToString()));
                }
                ControlUtils.SelectListItemsIgnoreCase(this.WaterMarkTransparency, base.PublishmentSystemInfo.Additional.WaterMarkTransparency.ToString());

                //水印图片质量
                for (int i = 1; i <= 10; i++)
                {
                    this.WaterMarkQty.Items.Add(new ListItem(i + "0%", i.ToString()));
                }
                ControlUtils.SelectListItemsIgnoreCase(this.WaterMarkQty, base.PublishmentSystemInfo.Additional.Qty.ToString());

                this.WaterMarkMinWidth.Text = base.PublishmentSystemInfo.Additional.WaterMarkMinWidth.ToString();
                this.WaterMarkMinHeight.Text = base.PublishmentSystemInfo.Additional.WaterMarkMinHeight.ToString();

                EBooleanUtils.AddListItems(this.IsImageWaterMark, "图片型", "文字型");
                ControlUtils.SelectListItemsIgnoreCase(this.IsImageWaterMark, base.PublishmentSystemInfo.Additional.IsImageWaterMark.ToString());

                this.WaterMarkFormatString.Text = base.PublishmentSystemInfo.Additional.WaterMarkFormatString;

                this.LoadSystemFont();
                ControlUtils.SelectListItemsIgnoreCase(this.WaterMarkFontName, base.PublishmentSystemInfo.Additional.WaterMarkFontName);

                this.WaterMarkFontSize.Text = base.PublishmentSystemInfo.Additional.WaterMarkFontSize.ToString();

                this.WaterMarkImagePath.Text = base.PublishmentSystemInfo.Additional.WaterMarkImagePath;

                this.IsWaterMark_SelectedIndexChanged(null, null);
                this.WaterMarkImagePath.Attributes.Add("onchange", JsManager.GetShowImageScript("preview_WaterMarkImagePath", base.PublishmentSystemInfo.PublishmentSystemUrl));

                string showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.WaterMarkImagePath.ClientID);
                this.ImageUrlSelect.Attributes.Add("onclick", showPopWinString);

                showPopWinString = Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.WaterMarkImagePath.ClientID);
                this.ImageUrlUpload.Attributes.Add("onclick", showPopWinString);
            }
        }

        private void LoadWaterMarkPosition(int selectPosition)
        {
            this.WaterMarkPosition.Text = "<table width=\"300\" height=\"243\" border=\"0\" background=\"../pic/flower.jpg\">";
            for (int i = 1; i < 10; i++)
            {
                if ((i % 3) == 1)
                {
                    this.WaterMarkPosition.Text = this.WaterMarkPosition.Text + "<tr>";
                }
                if (selectPosition == i)
                {
                    object obj1 = this.WaterMarkPosition.Text;
                    this.WaterMarkPosition.Text = string.Concat(new object[] { obj1, "<td width=\"33%\" style=\"font-size:18px;\" align=\"center\"><input type=\"radio\" id=\"WaterMarkPosition\" name=\"WaterMarkPosition\" value=\"", i, "\" checked>#", i, "</td>" });
                }
                else
                {
                    object obj2 = this.WaterMarkPosition.Text;
                    this.WaterMarkPosition.Text = string.Concat(new object[] { obj2, "<td width=\"33%\" style=\"font-size:18px;\" align=\"center\"><input type=\"radio\" id=\"WaterMarkPosition\" name=\"WaterMarkPosition\" value=\"", i, "\" >#", i, "</td>" });
                }
                if ((i % 3) == 0)
                {
                    this.WaterMarkPosition.Text = this.WaterMarkPosition.Text + "</tr>";
                }
            }
            this.WaterMarkPosition.Text = this.WaterMarkPosition.Text + "</table>";
        }

        private void LoadSystemFont()
        {
            FontFamily[] familyArray = new InstalledFontCollection().Families;
            for (int i = 0; i < familyArray.Length; i++)
            {
                FontFamily family = familyArray[i];
                this.WaterMarkFontName.Items.Add(new ListItem(family.Name, family.Name));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.Additional.IsWaterMark = TranslateUtils.ToBool(this.IsWaterMark.SelectedValue);
                base.PublishmentSystemInfo.Additional.WaterMarkPosition = TranslateUtils.ToInt(base.Request.Form["WaterMarkPosition"]);
                base.PublishmentSystemInfo.Additional.WaterMarkTransparency = TranslateUtils.ToInt(this.WaterMarkTransparency.SelectedValue);
                base.PublishmentSystemInfo.Additional.WaterMarkMinWidth = TranslateUtils.ToInt(this.WaterMarkMinWidth.Text);
                base.PublishmentSystemInfo.Additional.WaterMarkMinHeight = TranslateUtils.ToInt(this.WaterMarkMinHeight.Text);
                base.PublishmentSystemInfo.Additional.IsImageWaterMark = TranslateUtils.ToBool(this.IsImageWaterMark.SelectedValue);
                base.PublishmentSystemInfo.Additional.WaterMarkFormatString = this.WaterMarkFormatString.Text;
                base.PublishmentSystemInfo.Additional.WaterMarkFontName = this.WaterMarkFontName.SelectedValue;
                base.PublishmentSystemInfo.Additional.WaterMarkFontSize = TranslateUtils.ToInt(this.WaterMarkFontSize.Text);
                base.PublishmentSystemInfo.Additional.WaterMarkImagePath = this.WaterMarkImagePath.Text;

                //水印图片质量
                base.PublishmentSystemInfo.Additional.Qty = TranslateUtils.ToInt(this.WaterMarkQty.SelectedValue);

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改图片水印设置");
                    base.SuccessMessage("图片水印设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "图片水印设置修改失败！");
                }
            }
        }

        public void IsWaterMark_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EBooleanUtils.Equals(this.IsWaterMark.SelectedValue, EBoolean.True))
            {
                this.WaterMarkPositionRow.Visible = this.WaterMarkTransparencyRow.Visible = this.WaterMarkMinRow.Visible = this.IsImageWaterMarkRow.Visible = this.WaterMarkQtyRow.Visible = true;
                if (EBooleanUtils.Equals(this.IsImageWaterMark.SelectedValue, EBoolean.True))
                {
                    this.WaterMarkFormatStringRow.Visible = this.WaterMarkFontNameRow.Visible = this.WaterMarkFontSizeRow.Visible = false;
                    this.WaterMarkImagePathRow.Visible = true;
                }
                else
                {
                    this.WaterMarkFormatStringRow.Visible = this.WaterMarkFontNameRow.Visible = this.WaterMarkFontSizeRow.Visible = true;
                    this.WaterMarkImagePathRow.Visible = false;
                }
            }
            else
            {
                this.WaterMarkPositionRow.Visible = this.WaterMarkTransparencyRow.Visible = this.WaterMarkMinRow.Visible = this.IsImageWaterMarkRow.Visible = this.WaterMarkFormatStringRow.Visible = this.WaterMarkFontNameRow.Visible = this.WaterMarkFontSizeRow.Visible = this.WaterMarkImagePathRow.Visible = this.WaterMarkQtyRow.Visible = false;
            }
        }
    }
}
