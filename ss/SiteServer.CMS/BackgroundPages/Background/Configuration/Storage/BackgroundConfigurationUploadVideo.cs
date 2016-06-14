using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationUploadVideo : BackgroundBasePage
	{
		public TextBox tbVideoUploadDirectoryName;
		public RadioButtonList rblVideoUploadDateFormatString;
		public RadioButtonList rblIsVideoUploadChangeFileName;
        public TextBox tbVideoUploadTypeCollection;
        public DropDownList ddlVideoUploadTypeUnit;
        public TextBox tbVideoUploadTypeMaxSize;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "视频上传设置", AppManager.CMS.Permission.WebSite.Configration);

                this.tbVideoUploadDirectoryName.Text = base.PublishmentSystemInfo.Additional.VideoUploadDirectoryName;

				this.rblVideoUploadDateFormatString.Items.Add(new ListItem("按年存入不同目录(不推荐)", EDateFormatTypeUtils.GetValue(EDateFormatType.Year)));
				this.rblVideoUploadDateFormatString.Items.Add(new ListItem("按年/月存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)));
				this.rblVideoUploadDateFormatString.Items.Add(new ListItem("按年/月/日存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Day)));
                ControlUtils.SelectListItemsIgnoreCase(this.rblVideoUploadDateFormatString, base.PublishmentSystemInfo.Additional.VideoUploadDateFormatString);

				EBooleanUtils.AddListItems(this.rblIsVideoUploadChangeFileName, "自动修改文件名", "保持文件名不变");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsVideoUploadChangeFileName, base.PublishmentSystemInfo.Additional.IsVideoUploadChangeFileName.ToString());

                this.tbVideoUploadTypeCollection.Text = base.PublishmentSystemInfo.Additional.VideoUploadTypeCollection.Replace("|", ",");
                int mbSize = GetMBSize(base.PublishmentSystemInfo.Additional.VideoUploadTypeMaxSize);
				if (mbSize == 0)
				{
                    this.ddlVideoUploadTypeUnit.SelectedIndex = 0;
                    this.tbVideoUploadTypeMaxSize.Text = base.PublishmentSystemInfo.Additional.VideoUploadTypeMaxSize.ToString();
				}
				else
				{
                    this.ddlVideoUploadTypeUnit.SelectedIndex = 1;
                    this.tbVideoUploadTypeMaxSize.Text = mbSize.ToString();
				}

                if (FileConfigManager.Instance.IsSaas)
                {
                    this.tbVideoUploadTypeCollection.Enabled = false;
                    this.ddlVideoUploadTypeUnit.Enabled = false;
                    this.tbVideoUploadTypeMaxSize.Enabled = false;
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
                base.PublishmentSystemInfo.Additional.VideoUploadDirectoryName = this.tbVideoUploadDirectoryName.Text;

                base.PublishmentSystemInfo.Additional.VideoUploadDateFormatString = EDateFormatTypeUtils.GetValue(EDateFormatTypeUtils.GetEnumType(this.rblVideoUploadDateFormatString.SelectedValue));
                base.PublishmentSystemInfo.Additional.IsVideoUploadChangeFileName = TranslateUtils.ToBool(this.rblIsVideoUploadChangeFileName.SelectedValue);

                base.PublishmentSystemInfo.Additional.VideoUploadTypeCollection = this.tbVideoUploadTypeCollection.Text.Replace(",", "|");
                int kbSize = int.Parse(this.tbVideoUploadTypeMaxSize.Text);
                base.PublishmentSystemInfo.Additional.VideoUploadTypeMaxSize = (this.ddlVideoUploadTypeUnit.SelectedIndex == 0) ? kbSize : 1024 * kbSize;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改视频上传设置");

                    base.SuccessMessage("上传视频设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "上传视频设置修改失败！");
				}
			}
		}

	}
}
