using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages.Modal
{
	public class StorageAdd : BackgroundBasePage
	{
        public TextBox tbStorageName;
        public TextBox tbStorageUrl;
        public DropDownList ddlStorageType;

        public PlaceHolder phFtp;
        public TextBox tbFtpServer;
        public TextBox tbFtpPort;
        public TextBox tbFtpUserName;
        public TextBox tbFtpPassword;
        public RadioButtonList rblIsPassiveMode;

        public PlaceHolder phLocal;
        public TextBox tbLocalDirectoryPath;

        public TextBox tbDescription;

        public static string GetOpenWindowStringToAdd()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityPF.GetOpenWindowString("添加空间", "modal_storageAdd.aspx", arguments, 450, 550);
        }

        public static string GetOpenWindowStringToEdit(int storageID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("StorageID", storageID.ToString());
            return PageUtilityPF.GetOpenWindowString("修改空间", "modal_storageAdd.aspx", arguments, 450, 550);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                EStorageTypeUtils.AddListItems(this.ddlStorageType);
                EBooleanUtils.AddListItems(this.rblIsPassiveMode, "被动", "主动");

                if (!string.IsNullOrEmpty(base.GetQueryString("StorageID")))
                {
                    int storageID = TranslateUtils.ToInt(base.GetQueryString("StorageID"));
                    StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
                    if (storageInfo != null)
                    {
                        this.tbStorageName.Text = storageInfo.StorageName;
                        this.tbStorageUrl.Text = storageInfo.StorageUrl;
                        ControlUtils.SelectListItems(this.ddlStorageType, EStorageTypeUtils.GetValue(storageInfo.StorageType));
                        this.tbDescription.Text = storageInfo.Description;

                        if (storageInfo.StorageType == EStorageType.Ftp)
                        {
                            FTPStorageInfo ftpStorageInfo = BaiRongDataProvider.FTPStorageDAO.GetFTPStorageInfo(storageID);
                            if (ftpStorageInfo != null)
                            {
                                this.tbFtpServer.Text = ftpStorageInfo.Server;
                                this.tbFtpPort.Text = ftpStorageInfo.Port.ToString();
                                this.tbFtpUserName.Text = ftpStorageInfo.UserName;
                                this.tbFtpPassword.Text = ftpStorageInfo.Password;
                                this.tbFtpPassword.Attributes.Add("value", ftpStorageInfo.Password);
                                ControlUtils.SelectListItems(this.rblIsPassiveMode, ftpStorageInfo.IsPassiveMode.ToString());
                            }
                        }
                        else if (storageInfo.StorageType == EStorageType.Local)
                        {
                            LocalStorageInfo localStorageInfo = BaiRongDataProvider.LocalStorageDAO.GetLocalStorageInfo(storageID);
                            if (localStorageInfo != null)
                            {
                                this.tbLocalDirectoryPath.Text = localStorageInfo.DirectoryPath;
                            }
                        }
                    }
                }

                this.ddlStorageType_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void ddlStorageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EStorageType storageType = EStorageTypeUtils.GetEnumType(this.ddlStorageType.SelectedValue);
            this.phFtp.Visible = this.phLocal.Visible = false;
            if (storageType == EStorageType.Ftp)
            {
                this.phFtp.Visible = true;
            }
            else if (storageType == EStorageType.Local)
            {
                this.phLocal.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            if (!string.IsNullOrEmpty(base.GetQueryString("StorageID")))
            {
                try
                {
                    int storageID = TranslateUtils.ToInt(base.GetQueryString("StorageID"));
                    StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);

                    if (storageInfo.StorageName != this.tbStorageName.Text)
                    {
                        if (BaiRongDataProvider.StorageDAO.IsExists(this.tbStorageName.Text))
                        {
                            base.FailMessage("空间修改失败，空间名称已存在！");
                            return;
                        }
                    }
                    storageInfo.StorageName = this.tbStorageName.Text;
                    storageInfo.StorageUrl = this.tbStorageUrl.Text;
                    storageInfo.StorageType = EStorageTypeUtils.GetEnumType(this.ddlStorageType.SelectedValue);
                    storageInfo.IsEnabled = true;
                    storageInfo.Description = this.tbDescription.Text;
                    storageInfo.StorageType = EStorageTypeUtils.GetEnumType(this.ddlStorageType.SelectedValue);

                    if (storageInfo.StorageType == EStorageType.Ftp)
                    {
                        FTPStorageInfo ftpStorageInfo = BaiRongDataProvider.FTPStorageDAO.GetFTPStorageInfo(storageID);
                        if (ftpStorageInfo == null)
                        {
                            ftpStorageInfo = new FTPStorageInfo();
                        }
                        ftpStorageInfo.Server = this.tbFtpServer.Text;
                        ftpStorageInfo.Port = TranslateUtils.ToInt(this.tbFtpPort.Text, 21);
                        ftpStorageInfo.UserName = this.tbFtpUserName.Text;
                        ftpStorageInfo.Password = this.tbFtpPassword.Text;
                        ftpStorageInfo.IsPassiveMode = TranslateUtils.ToBool(this.rblIsPassiveMode.SelectedValue);

                        if (ftpStorageInfo.StorageID == 0)
                        {
                            ftpStorageInfo.StorageID = storageID;
                            BaiRongDataProvider.FTPStorageDAO.Insert(ftpStorageInfo);
                        }
                        else
                        {
                            BaiRongDataProvider.FTPStorageDAO.Update(ftpStorageInfo);
                        }
                    }
                    else if (storageInfo.StorageType == EStorageType.Local)
                    {
                        LocalStorageInfo localStorageInfo = BaiRongDataProvider.LocalStorageDAO.GetLocalStorageInfo(storageID);
                        if (localStorageInfo == null)
                        {
                            localStorageInfo = new LocalStorageInfo();
                        }
                        localStorageInfo.DirectoryPath = this.tbLocalDirectoryPath.Text;

                        if (localStorageInfo.StorageID == 0)
                        {
                            localStorageInfo.StorageID = storageID;
                            BaiRongDataProvider.LocalStorageDAO.Insert(localStorageInfo);
                        }
                        else
                        {
                            BaiRongDataProvider.LocalStorageDAO.Update(localStorageInfo);
                        }
                    }

                    BaiRongDataProvider.StorageDAO.Update(storageInfo);

                    base.SuccessMessage("空间修改成功！");
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "空间修改失败！");
                }
            }
            else
            {
                if (BaiRongDataProvider.StorageDAO.IsExists(this.tbStorageName.Text))
                {
                    base.FailMessage("空间添加失败，空间名称已存在！");
                }
                else
                {
                    try
                    {
                        StorageInfo storageInfo = new StorageInfo();

                        storageInfo.StorageName = this.tbStorageName.Text;
                        storageInfo.StorageUrl = this.tbStorageUrl.Text;
                        storageInfo.StorageType = EStorageTypeUtils.GetEnumType(this.ddlStorageType.SelectedValue);
                        storageInfo.IsEnabled = true;
                        storageInfo.Description = this.tbDescription.Text;
                        storageInfo.StorageType = EStorageTypeUtils.GetEnumType(this.ddlStorageType.SelectedValue);
                        int storageID = BaiRongDataProvider.StorageDAO.Insert(storageInfo);

                        if (storageInfo.StorageType == EStorageType.Ftp)
                        {
                            FTPStorageInfo ftpStorageInfo = new FTPStorageInfo();
                            ftpStorageInfo.StorageID = storageID;
                            ftpStorageInfo.Server = this.tbFtpServer.Text;
                            ftpStorageInfo.Port = TranslateUtils.ToInt(this.tbFtpPort.Text, 21);
                            ftpStorageInfo.UserName = this.tbFtpUserName.Text;
                            ftpStorageInfo.Password = this.tbFtpPassword.Text;
                            ftpStorageInfo.IsPassiveMode = TranslateUtils.ToBool(this.rblIsPassiveMode.SelectedValue);
                            BaiRongDataProvider.FTPStorageDAO.Insert(ftpStorageInfo);
                        }
                        else if (storageInfo.StorageType == EStorageType.Local)
                        {
                            LocalStorageInfo localStorageInfo = new LocalStorageInfo();
                            localStorageInfo.StorageID = storageID;
                            localStorageInfo.DirectoryPath = this.tbLocalDirectoryPath.Text;
                            BaiRongDataProvider.LocalStorageDAO.Insert(localStorageInfo);
                        }

                        base.SuccessMessage("空间添加成功！");
                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "空间添加失败！");
                    }
                }
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
