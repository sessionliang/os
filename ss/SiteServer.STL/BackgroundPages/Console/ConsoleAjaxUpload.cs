using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;

using System.Web;
using BaiRong.Core.Drawing;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleAjaxUpload : BackgroundBasePage
    {
        public static string GetIndependentTemplateImageUrlUploadUrl(string directoryName)
        {
            return PageUtils.GetSTLUrl(string.Format("console_ajaxUpload.aspx?directoryName={0}&isUploadIndependentTemplateImageUrl=True", directoryName));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            NameValueCollection jsonAttributes = new NameValueCollection();

            if (base.GetBoolQueryString("isUploadIndependentTemplateImageUrl"))
            {
                string directoryName = base.GetQueryString("directoryName");
                string message = string.Empty;
                string url = string.Empty;
                string virtualUrl = string.Empty;

                bool success = this.UploadIndependentTemplateImageUrl(directoryName, out message, out url, out virtualUrl);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("virtualUrl", virtualUrl);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            base.Response.Write(jsonString);
            base.Response.End();
        }

        public bool UploadIndependentTemplateImageUrl(string directoryName, out string message, out string url, out string virtualUrl)
        {
            message = url = virtualUrl = string.Empty;

            if (base.Request.Files != null && base.Request.Files["Upload"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Upload"];

                try
                {
                    string fileExtName = PathUtils.GetExtension(postedFile.FileName).ToLower();
                    string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
                    string fileName = StringUtils.GetShortGUID().ToLower() + fileExtName;
                    string filePath = PathUtility.GetIndependentTemplateMetadataPath(directoryPath, fileName);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        postedFile.SaveAs(filePath);

                        string independentTemplateUrl = PageUtility.GetIndependentTemplatesUrl(directoryName);
                        url = PageUtility.GetIndependentTemplateMetadataUrl(independentTemplateUrl, fileName);
                        virtualUrl = fileName;

                        return true;
                    }
                    else
                    {
                        message = "您必须上传图片文件！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }
    }
}
