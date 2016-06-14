using System;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigWaterMark : BackgroundBasePage
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

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.User.LeftMenu.ID_UserCenterSetting, "Õº∆¨ÀÆ”°≈‰÷√", AppManager.User.Permission.Usercenter_Setting);

				EBooleanUtils.AddListItems(this.IsWaterMark);
                ControlUtils.SelectListItemsIgnoreCase(this.IsWaterMark, UserConfigManager.Additional.IsWaterMark.ToString());

                this.LoadWaterMarkPosition(UserConfigManager.Additional.WaterMarkPosition);

				for (int i = 1; i <= 10; i++)
				{
					this.WaterMarkTransparency.Items.Add(new ListItem(i + "0%", i.ToString()));
				}
                ControlUtils.SelectListItemsIgnoreCase(this.WaterMarkTransparency, UserConfigManager.Additional.WaterMarkTransparency.ToString());

                this.WaterMarkMinWidth.Text = UserConfigManager.Additional.WaterMarkMinWidth.ToString();
                this.WaterMarkMinHeight.Text = UserConfigManager.Additional.WaterMarkMinHeight.ToString();

				EBooleanUtils.AddListItems(this.IsImageWaterMark, "Õº∆¨–Õ", "Œƒ◊÷–Õ");
                ControlUtils.SelectListItemsIgnoreCase(this.IsImageWaterMark, UserConfigManager.Additional.IsImageWaterMark.ToString());

                this.WaterMarkFormatString.Text = UserConfigManager.Additional.WaterMarkFormatString;

				this.LoadSystemFont();
                ControlUtils.SelectListItemsIgnoreCase(this.WaterMarkFontName, UserConfigManager.Additional.WaterMarkFontName);

                this.WaterMarkFontSize.Text = UserConfigManager.Additional.WaterMarkFontSize.ToString();

                this.WaterMarkImagePath.Text = UserConfigManager.Additional.WaterMarkImagePath;

				this.IsWaterMark_SelectedIndexChanged(null, null);
			}
		}

		private void LoadWaterMarkPosition (int selectPosition)
		{
			this.WaterMarkPosition.Text = "<table width=\"300\" height=\"243\" border=\"0\" background=\"../pic/flower.jpg\">";
			for (int i = 1;i < 10; i++)
			{
				if ((i % 3) == 1)
				{
					this.WaterMarkPosition.Text = this.WaterMarkPosition.Text + "<tr>";
				}
				if (selectPosition == i)
				{
					object obj1 = this.WaterMarkPosition.Text;
					this.WaterMarkPosition.Text = string.Concat(new object[]{obj1, "<td width=\"33%\" style=\"font-size:18px;\" align=\"center\"><input type=\"radio\" id=\"WaterMarkPosition\" name=\"WaterMarkPosition\" value=\"", i, "\" checked>#", i, "</td>"});
				}
				else
				{
					object obj2 = this.WaterMarkPosition.Text;
					this.WaterMarkPosition.Text = string.Concat(new object[]{obj2, "<td width=\"33%\" style=\"font-size:18px;\" align=\"center\"><input type=\"radio\" id=\"WaterMarkPosition\" name=\"WaterMarkPosition\" value=\"", i, "\" >#", i, "</td>"});
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
			for (int i = 0;i < familyArray.Length; i++)
			{
				FontFamily family = familyArray[i];
				this.WaterMarkFontName.Items.Add(new ListItem(family.Name, family.Name));
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                UserConfigManager.Additional.IsWaterMark = TranslateUtils.ToBool(this.IsWaterMark.SelectedValue);
                UserConfigManager.Additional.WaterMarkPosition = TranslateUtils.ToInt(base.Request.Form["WaterMarkPosition"]);
                UserConfigManager.Additional.WaterMarkTransparency = TranslateUtils.ToInt(this.WaterMarkTransparency.SelectedValue);
                UserConfigManager.Additional.WaterMarkMinWidth = TranslateUtils.ToInt(this.WaterMarkMinWidth.Text);
                UserConfigManager.Additional.WaterMarkMinHeight = TranslateUtils.ToInt(this.WaterMarkMinHeight.Text);
                UserConfigManager.Additional.IsImageWaterMark = TranslateUtils.ToBool(this.IsImageWaterMark.SelectedValue);
                UserConfigManager.Additional.WaterMarkFormatString = this.WaterMarkFormatString.Text;
                UserConfigManager.Additional.WaterMarkFontName = this.WaterMarkFontName.SelectedValue;
                UserConfigManager.Additional.WaterMarkFontSize = TranslateUtils.ToInt(this.WaterMarkFontSize.Text);
                UserConfigManager.Additional.WaterMarkImagePath = this.WaterMarkImagePath.Text;
				
				try
				{
                    BaiRongDataProvider .UserConfigDAO.Update(UserConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "–ﬁ∏ƒÕº∆¨ÀÆ”°≈‰÷√");

                    base.SuccessMessage("Õº∆¨ÀÆ”°≈‰÷√–ﬁ∏ƒ≥…π¶£°");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "Õº∆¨ÀÆ”°≈‰÷√–ﬁ∏ƒ ß∞‹£°");
				}
			}
		}

		public void IsWaterMark_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (EBooleanUtils.Equals(this.IsWaterMark.SelectedValue, EBoolean.True))
			{
				this.WaterMarkPositionRow.Visible = this.WaterMarkTransparencyRow.Visible = this.WaterMarkMinRow.Visible = this.IsImageWaterMarkRow.Visible = true;
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
				this.WaterMarkPositionRow.Visible = this.WaterMarkTransparencyRow.Visible = this.WaterMarkMinRow.Visible = this.IsImageWaterMarkRow.Visible = this.WaterMarkFormatStringRow.Visible = this.WaterMarkFontNameRow.Visible = this.WaterMarkFontSizeRow.Visible = this.WaterMarkImagePathRow.Visible = false;
			}
		}
	}
}
