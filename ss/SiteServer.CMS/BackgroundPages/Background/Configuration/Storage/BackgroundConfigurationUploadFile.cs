using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationUploadFile : BackgroundBasePage
	{
		public TextBox tbFileUploadDirectoryName;
		public RadioButtonList rblFileUploadDateFormatString;
		public RadioButtonList rblIsFileUploadChangeFileName;
        public TextBox tbFileUploadTypeCollection;
        public DropDownList ddlFileUploadTypeUnit;
        public TextBox tbFileUploadTypeMaxSize;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "附件上传设置", AppManager.CMS.Permission.WebSite.Configration);

                this.tbFileUploadDirectoryName.Text = base.PublishmentSystemInfo.Additional.FileUploadDirectoryName;

				this.rblFileUploadDateFormatString.Items.Add(new ListItem("按年存入不同目录(不推荐)", EDateFormatTypeUtils.GetValue(EDateFormatType.Year)));
				this.rblFileUploadDateFormatString.Items.Add(new ListItem("按年/月存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)));
				this.rblFileUploadDateFormatString.Items.Add(new ListItem("按年/月/日存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Day)));
                ControlUtils.SelectListItemsIgnoreCase(this.rblFileUploadDateFormatString, base.PublishmentSystemInfo.Additional.FileUploadDateFormatString);

				EBooleanUtils.AddListItems(this.rblIsFileUploadChangeFileName, "自动修改文件名", "保持文件名不变");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsFileUploadChangeFileName, base.PublishmentSystemInfo.Additional.IsFileUploadChangeFileName.ToString());

                this.tbFileUploadTypeCollection.Text = base.PublishmentSystemInfo.Additional.FileUploadTypeCollection.Replace("|", ",");
                int mbSize = GetMBSize(base.PublishmentSystemInfo.Additional.FileUploadTypeMaxSize);
				if (mbSize == 0)
				{
                    this.ddlFileUploadTypeUnit.SelectedIndex = 0;
                    this.tbFileUploadTypeMaxSize.Text = base.PublishmentSystemInfo.Additional.FileUploadTypeMaxSize.ToString();
				}
				else
				{
                    this.ddlFileUploadTypeUnit.SelectedIndex = 1;
                    this.tbFileUploadTypeMaxSize.Text = mbSize.ToString();
				}

                if (FileConfigManager.Instance.IsSaas)
                {
                    this.tbFileUploadTypeCollection.Enabled = false;
                    this.ddlFileUploadTypeUnit.Enabled = false;
                    this.tbFileUploadTypeMaxSize.Enabled = false;
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
                base.PublishmentSystemInfo.Additional.FileUploadDirectoryName = this.tbFileUploadDirectoryName.Text;

                base.PublishmentSystemInfo.Additional.FileUploadDateFormatString = EDateFormatTypeUtils.GetValue(EDateFormatTypeUtils.GetEnumType(this.rblFileUploadDateFormatString.SelectedValue));
                base.PublishmentSystemInfo.Additional.IsFileUploadChangeFileName = TranslateUtils.ToBool(this.rblIsFileUploadChangeFileName.SelectedValue);

                base.PublishmentSystemInfo.Additional.FileUploadTypeCollection = this.tbFileUploadTypeCollection.Text.Replace(",", "|");
                int kbSize = int.Parse(this.tbFileUploadTypeMaxSize.Text);
                base.PublishmentSystemInfo.Additional.FileUploadTypeMaxSize = (this.ddlFileUploadTypeUnit.SelectedIndex == 0) ? kbSize : 1024 * kbSize;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改附件上传设置");

                    base.SuccessMessage("上传附件设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "上传附件设置修改失败！");
				}
			}
		}

	}
}
