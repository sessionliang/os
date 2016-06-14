using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class SpecItemAdd : BackgroundBasePage
	{
        protected TextBox tbTitle;
        protected PlaceHolder phIcon;
        protected TextBox IconUrl;
        protected HtmlInputFile IconUrlUploader;

        private SpecItemInfo itemInfo;
        private int channelID;
        private int specID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int channelID, int specID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("channelID", channelID.ToString());
            arguments.Add("SpecID", specID.ToString());
            return PageUtilityB2C.GetOpenWindowString("添加规格值", "modal_specItemAdd.aspx", arguments, 560, 340);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int channelID, int specID, int itemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("channelID", channelID.ToString());
            arguments.Add("SpecID", specID.ToString());
            arguments.Add("ItemID", itemID.ToString());
            return PageUtilityB2C.GetOpenWindowString("修改规格值", "modal_specItemAdd.aspx", arguments, 560, 340);
        }

        public string GetPreviewImageSrc()
        {
            if (this.itemInfo != null && !string.IsNullOrEmpty(this.itemInfo.IconUrl))
            {
                return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, this.itemInfo.IconUrl);
            }
            return PageUtils.GetIconUrl("empty.gif");
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = base.GetIntQueryString("channelID");
            this.specID = base.GetIntQueryString("SpecID");

            SpecInfo specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, this.specID);

			if (!IsPostBack)
			{
                this.phIcon.Visible = specInfo.IsIcon;

                if (base.GetQueryString("ItemID") != null)
                {
                    int itemID = base.GetIntQueryString("ItemID");
                    this.itemInfo = SpecItemManager.GetSpecItemInfo(base.PublishmentSystemID, itemID);
                    if (this.itemInfo != null)
                    {
                        this.tbTitle.Text = this.itemInfo.Title;
                        this.IconUrl.Text = this.itemInfo.IconUrl;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
            string iconUrl = string.Empty;

            if (this.IconUrlUploader.PostedFile != null && "" != this.IconUrlUploader.PostedFile.FileName)
            {
                string filePath = this.IconUrlUploader.PostedFile.FileName;
                try
                {
                    string fileExtName = System.IO.Path.GetExtension(filePath).ToLower();
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);

                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        this.IconUrlUploader.PostedFile.SaveAs(localFilePath);
                        iconUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("文件上传失败:{0}", ex.Message));
                }
            }

            if (string.IsNullOrEmpty(iconUrl))
            {
                iconUrl = this.IconUrl.Text;
            }
				
			if (base.GetQueryString("ItemID") != null)
			{
				try
				{
                    int itemID = base.GetIntQueryString("ItemID");
                    this.itemInfo = SpecItemManager.GetSpecItemInfo(base.PublishmentSystemID, itemID);
                    if (this.itemInfo != null)
                    {
                        this.itemInfo.Title = this.tbTitle.Text;
                        this.itemInfo.IconUrl = iconUrl;
                    }
                    DataProviderB2C.SpecItemDAO.Update(base.PublishmentSystemID, this.itemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改规格值", string.Format("规格值:{0}", this.itemInfo.Title));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "规格值修改失败！");
				}
			}
			else
			{
                try
                {
                    this.itemInfo = new SpecItemInfo(0, base.PublishmentSystemID, this.specID, this.tbTitle.Text, iconUrl, false, 0);

                    DataProviderB2C.SpecItemDAO.Insert(base.PublishmentSystemID, this.itemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "添加规格值", string.Format("规格值:{0}", this.itemInfo.Title));

                    isChanged = true;
                }
                catch(Exception ex)
                {
                    base.FailMessage(ex, "规格值添加失败！");
                }
			}

			if (isChanged)
			{
                string urlSpecItem = BackgroundSpecItem.GetRedirectUrl(base.PublishmentSystemID, this.channelID, this.specID);
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, urlSpecItem);
			}
		}
	}
}
