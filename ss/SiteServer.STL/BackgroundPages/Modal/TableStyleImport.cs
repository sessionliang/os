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
	public class TableStyleImport : BackgroundBasePage
	{
		public HtmlInputFile myFile;

        private string tableName;
        private ETableStyle tableStyle;
        private int relatedIdentity;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableName = base.GetQueryString("TableName");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.relatedIdentity = int.Parse(base.GetQueryString("RelatedIdentity"));
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
				string filePath = myFile.PostedFile.FileName;
                if (!EFileSystemTypeUtils.IsCompressionFile(PathUtils.GetExtension(filePath)))
				{
                    base.FailMessage("必须上传压缩文件");
					return;
				}

				try
				{
                    string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

					myFile.PostedFile.SaveAs(localFilePath);

					ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                    importObject.ImportTableStyleByZipFile(this.tableStyle, this.tableName, this.relatedIdentity, localFilePath);

                    StringUtility.AddLog(base.PublishmentSystemID, "导入表单显示样式", string.Format("类型:{0}", ETableStyleUtils.GetText(this.tableStyle)));

					JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
					base.FailMessage(ex, "导入表样式失败！");
				}
			}
		}
	}
}
