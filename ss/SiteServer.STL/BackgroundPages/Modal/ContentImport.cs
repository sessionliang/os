using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using SiteServer.CMS.Model;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class ContentImport : BackgroundBasePage
	{
        public RadioButtonList ImportType;
		public HtmlInputFile myFile;
        public RadioButtonList IsOverride;
        public TextBox ImportStart;
        public TextBox ImportCount;
        public RadioButtonList ContentLevel;

        private int nodeID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
			if (!IsPostBack)
			{
                int checkedLevel = 0;
                bool isChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, base.PublishmentSystemID, out checkedLevel);
                LevelManager.LoadContentLevelToEdit(this.ContentLevel, base.PublishmentSystemInfo, this.nodeID, null, isChecked, checkedLevel);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
                bool isChecked = false;
                int checkedLevel = TranslateUtils.ToIntWithNagetive(this.ContentLevel.SelectedValue);
                if (checkedLevel >= base.PublishmentSystemInfo.CheckContentLevel)
                {
                    isChecked = true;
                }

				try
				{
                    if (StringUtils.EqualsIgnoreCase(this.ImportType.SelectedValue, PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentZip))
                    {
                        string filePath = myFile.PostedFile.FileName;
                        if (!EFileSystemTypeUtils.Equals(EFileSystemType.Zip, PathUtils.GetExtension(filePath)))
                        {
                            base.FailMessage("必须上传后缀为“.zip”的压缩文件");
                            return;
                        }

                        string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                        importObject.ImportContentsByZipFile(nodeInfo, localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text), isChecked, checkedLevel);
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.ImportType.SelectedValue, PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentAccess))
                    {
                        string filePath = myFile.PostedFile.FileName;
                        if (!StringUtils.EqualsIgnoreCase(PathUtils.GetExtension(filePath), ".mdb"))
                        {
                            base.FailMessage("必须上传后缀为“.mdb”的Access文件");
                            return;
                        }

                        string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportContentsByAccessFile(this.nodeID, localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text), isChecked, checkedLevel);
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.ImportType.SelectedValue, PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentExcel))
                    {
                        string filePath = myFile.PostedFile.FileName;
                        if (!StringUtils.EqualsIgnoreCase(PathUtils.GetExtension(filePath), ".xls"))
                        {
                            base.FailMessage("必须上传后缀为“.xls”的Excel文件");
                            return;
                        }

                        string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportContentsByExcelFile(this.nodeID, localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text), isChecked, checkedLevel);
                    }
                    else if (StringUtils.EqualsIgnoreCase(this.ImportType.SelectedValue, PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentTxtZip))
                    {
                        string filePath = myFile.PostedFile.FileName;
                        if (!EFileSystemTypeUtils.Equals(EFileSystemType.Zip, PathUtils.GetExtension(filePath)))
                        {
                            base.FailMessage("必须上传后缀为“.zip”的压缩文件");
                            return;
                        }

                        string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportContentsByTxtZipFile(this.nodeID, localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text), isChecked, checkedLevel);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "导入内容", string.Empty);

					JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
					base.FailMessage(ex, "导入内容失败！");
				}
			}
		}
	}
}
