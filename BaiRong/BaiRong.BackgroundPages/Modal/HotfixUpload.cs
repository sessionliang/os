using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using BaiRong.Core.Net;
using BaiRong.Model;

namespace BaiRong.BackgroundPages.Modal
{
	public class HotfixUpload : BackgroundBasePage
	{
        public HtmlInputFile hifUpload;

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityPF.GetOpenWindowString("导入升级包", "modal_hotfixUpload.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            if (base.IsForbidden) return;
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (this.hifUpload.PostedFile != null && "" != this.hifUpload.PostedFile.FileName)
            {
                string filePath = this.hifUpload.PostedFile.FileName;
                string sExt = PathUtils.GetExtension(filePath);
                if (!StringUtils.EqualsIgnoreCase(sExt, ".zip"))
                {
                    base.FailMessage("补丁文件为zip格式，请选择有效的文件上传");
                    return;
                }
                try
                {
                    string localFilePath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_FileName);
                    FileUtils.DeleteFileIfExists(localFilePath);

                    this.hifUpload.PostedFile.SaveAs(localFilePath);

                    string redirectUrl = Modal.ProgressBar.GetRedirectUrlStringOfHotfixWithoutDownload();
                    PageUtils.Redirect(redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "文件上传失败！");
                }
            }
        }
	}
}
