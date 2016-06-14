using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class Import : BackgroundBasePage
	{
		public HtmlInputFile myFile;
		public RadioButtonList IsOverride;

        private string type;
        private int itemID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.type = base.GetQueryString("Type");
            this.itemID = base.GetIntQueryString("ItemID");

            if (!IsPostBack)
			{
				//base.ShowMessage(string.Format(@"您可以从官方网站下载采集规则，然后导入。<br />下载地址：<a href='{0}' target='_blank'>{0}</a>", PageUtility.PAGE_SHARE_CAIJI));
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (StringUtils.EqualsIgnoreCase(this.type, PageUtility.ModalSTL.Import.TYPE_GATHERRULE))
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    if (EFileSystemTypeUtils.GetEnumType(Path.GetExtension(filePath)) != EFileSystemType.Xml)
                    {
                        base.FailMessage("必须上传XML文件");
                        return;
                    }

                    try
                    {
                        string localFilePath = PathUtils.GetTemporaryFilesPath(Path.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportGatherRule(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));

                        StringUtility.AddLog(base.PublishmentSystemID, "导入采集规则");

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "导入采集规则失败！");
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(this.type, PageUtility.ModalSTL.Import.TYPE_INPUT))
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    if (EFileSystemTypeUtils.GetEnumType(Path.GetExtension(filePath)) != EFileSystemType.Zip)
                    {
                        base.FailMessage("必须上传ZIP文件");
                        return;
                    }

                    try
                    {
                        string localFilePath = PathUtils.GetTemporaryFilesPath(Path.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        //importObject.ImportInputByZipFile(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));
                        importObject.ImportInputByZipFile(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue), this.itemID);

                        StringUtility.AddLog(base.PublishmentSystemID, "导入提交表单");

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "导入提交表单失败！");
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(this.type, PageUtility.ModalSTL.Import.TYPE_RELATED_FIELD))
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    if (EFileSystemTypeUtils.GetEnumType(Path.GetExtension(filePath)) != EFileSystemType.Zip)
                    {
                        base.FailMessage("必须上传ZIP文件");
                        return;
                    }

                    try
                    {
                        string localFilePath = PathUtils.GetTemporaryFilesPath(Path.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportRelatedFieldByZipFile(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));

                        StringUtility.AddLog(base.PublishmentSystemID, "导入联动字段");

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "导入联动字段失败！");
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(this.type, PageUtility.ModalSTL.Import.TYPE_TAGSTYLE))
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    if (EFileSystemTypeUtils.GetEnumType(Path.GetExtension(filePath)) != EFileSystemType.Xml)
                    {
                        base.FailMessage("必须上传XML文件");
                        return;
                    }

                    try
                    {
                        string localFilePath = PathUtils.GetTemporaryFilesPath(Path.GetFileName(filePath));

                        myFile.PostedFile.SaveAs(localFilePath);

                        ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                        importObject.ImportTagStyle(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));

                        StringUtility.AddLog(base.PublishmentSystemID, "导入模板标签样式");

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "导入模板标签样式失败！");
                    }
                }
            }
		}
	}
}
