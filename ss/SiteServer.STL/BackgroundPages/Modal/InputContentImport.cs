using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using System.Data.OleDb;
using System.Data;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class InputContentImport : BackgroundBasePage
	{
        public RadioButtonList ImportType;
		public HtmlInputFile myFile;
        public TextBox ImportStart;
        public TextBox ImportCount;
        public RadioButtonList IsChecked;

        private InputInfo inputInfo;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int inputID = base.GetIntQueryString("InputID");
            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.IsChecked, "已审核", "未审核");
                ControlUtils.SelectListItems(this.IsChecked, true.ToString());
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
				try
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
                    importObject.ImportInputContentsByExcelFile(this.inputInfo, localFilePath, TranslateUtils.ToInt(this.ImportStart.Text), TranslateUtils.ToInt(this.ImportCount.Text), TranslateUtils.ToBool(this.IsChecked.SelectedValue));

                    StringUtility.AddLog(base.PublishmentSystemID, "导入提交表单内容", string.Format("提交表单：{0}", this.inputInfo.InputName));

					JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "导入提交表单失败！");
				}
			}
		}
	}
}
