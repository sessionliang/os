using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationUploadImage : BackgroundBasePage
	{
		public TextBox tbImageUploadDirectoryName;
		public RadioButtonList rblImageUploadDateFormatString;
		public RadioButtonList rblIsImageUploadChangeFileName;
        public TextBox tbImageUploadTypeCollection;
        public DropDownList ddlImageUploadTypeUnit;
        public TextBox tbImageUploadTypeMaxSize;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "图片上传设置", AppManager.CMS.Permission.WebSite.Configration);

                this.tbImageUploadDirectoryName.Text = base.PublishmentSystemInfo.Additional.ImageUploadDirectoryName;

				this.rblImageUploadDateFormatString.Items.Add(new ListItem("按年存入不同目录(不推荐)", EDateFormatTypeUtils.GetValue(EDateFormatType.Year)));
				this.rblImageUploadDateFormatString.Items.Add(new ListItem("按年/月存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)));
				this.rblImageUploadDateFormatString.Items.Add(new ListItem("按年/月/日存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Day)));
                ControlUtils.SelectListItemsIgnoreCase(this.rblImageUploadDateFormatString, base.PublishmentSystemInfo.Additional.ImageUploadDateFormatString);

				EBooleanUtils.AddListItems(this.rblIsImageUploadChangeFileName, "自动修改文件名", "保持文件名不变");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsImageUploadChangeFileName, base.PublishmentSystemInfo.Additional.IsImageUploadChangeFileName.ToString());

                this.tbImageUploadTypeCollection.Text = base.PublishmentSystemInfo.Additional.ImageUploadTypeCollection.Replace("|", ",");
                int mbSize = GetMBSize(base.PublishmentSystemInfo.Additional.ImageUploadTypeMaxSize);
				if (mbSize == 0)
				{
                    this.ddlImageUploadTypeUnit.SelectedIndex = 0;
                    this.tbImageUploadTypeMaxSize.Text = base.PublishmentSystemInfo.Additional.ImageUploadTypeMaxSize.ToString();
				}
				else
				{
                    this.ddlImageUploadTypeUnit.SelectedIndex = 1;
                    this.tbImageUploadTypeMaxSize.Text = mbSize.ToString();
				}

                if (FileConfigManager.Instance.IsSaas)
                {
                    this.tbImageUploadTypeCollection.Enabled = false;
                    this.ddlImageUploadTypeUnit.Enabled = false;
                    this.tbImageUploadTypeMaxSize.Enabled = false;
                }
			}
		}


		private static int GetMBSize(int kbSize)
		{
			int retval = 0;
			if (kbSize >= 1024 && ((kbSize % 1024) == 0))
			{
				retval = kbSize / 1024;
			}
			return retval;
		}


		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.ImageUploadDirectoryName = this.tbImageUploadDirectoryName.Text;

                base.PublishmentSystemInfo.Additional.ImageUploadDateFormatString = EDateFormatTypeUtils.GetValue(EDateFormatTypeUtils.GetEnumType(this.rblImageUploadDateFormatString.SelectedValue));
                base.PublishmentSystemInfo.Additional.IsImageUploadChangeFileName = TranslateUtils.ToBool(this.rblIsImageUploadChangeFileName.SelectedValue);

                base.PublishmentSystemInfo.Additional.ImageUploadTypeCollection = this.tbImageUploadTypeCollection.Text.Replace(",", "|");
                int kbSize = int.Parse(this.tbImageUploadTypeMaxSize.Text);
                base.PublishmentSystemInfo.Additional.ImageUploadTypeMaxSize = (this.ddlImageUploadTypeUnit.SelectedIndex == 0) ? kbSize : 1024 * kbSize;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改图片上传设置");

                    base.SuccessMessage("上传图片设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "上传图片设置修改失败！");
				}
			}
		}

	}
}
