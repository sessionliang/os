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
using BaiRong.Core.AuxiliaryTable;

using BaiRong.Core.Cryptography;
using SiteServer.Project.Model;
using SiteServer.Project.Core;

namespace SiteServer.Project.BackgroundPages.Services
{
    public class Action : Page
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {
            if ((base.Page.Request.Form != null && base.Page.Request.Form.Count > 0) || (base.Request.Files != null && base.Request.Files.Count > 0))
            {
                int projectID = TranslateUtils.ToInt(base.Request.QueryString["projectID"]);
                string type = base.Request.QueryString["type"];

                if (StringUtils.EqualsIgnoreCase(type, "Apply"))
                {
                    this.ApplyAdd(projectID);
                }
            }
        }

        private void ApplyAdd(int projectID)
        {
            try
            {
                ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(projectID, base.Request.Form);

                int applyID = DataProvider.ApplyDAO.Insert(applyInfo);

                string toDepartmentName = string.Empty;
                if (applyInfo.DepartmentID > 0)
                {
                    toDepartmentName = "至" + DepartmentManager.GetDepartmentName(applyInfo.DepartmentID);
                }
                ApplyManager.LogNew(applyID, toDepartmentName);

                this.ltlScript.Text = Action.GetCallbackScript(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = Action.GetCallbackScript(false, ex.Message);
            }
        }

        public static string GetCallbackScript(bool isSuccess, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("failureMessage", failureMessage);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            return string.Format("<script>window.parent.stlApplyCallback('{0}');</script>", jsonString);
        }

        //private string UploadFile(HttpPostedFile myFile, bool isImage)
        //{
        //    string fileUrl = string.Empty;

        //    string filePath = myFile.FileName;
        //    try
        //    {
        //        string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo);
        //        string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);

        //        string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

        //        string fileExtName = PathUtils.GetExtension(filePath);

        //        if (isImage)
        //        {
        //            if (PathUtils.IsFileExtenstionAllowed(base.PublishmentSystemInfo.Additional.UploadImageTypeCollection, fileExtName))
        //            {
        //                if (myFile.ContentLength > base.PublishmentSystemInfo.Additional.UploadImageTypeMaxSize * 1024)
        //                {
        //                    return string.Empty;
        //                }
        //                myFile.SaveAs(localFilePath);

        //                FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

        //                fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
        //                fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
        //            }
        //        }
        //        else
        //        {
        //            if (!PathUtils.IsFileExtenstionAllowed(base.PublishmentSystemInfo.Additional.UploadFileTypeCollectionForbidden, fileExtName))
        //            {
        //                if (myFile.ContentLength > base.PublishmentSystemInfo.Additional.UploadFileTypeMaxSize * 1024)
        //                {
        //                    return string.Empty;
        //                }
        //                myFile.SaveAs(localFilePath);

        //                fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
        //                fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
        //            }
        //        }
        //    }
        //    catch { }

        //    return fileUrl;
        //}
    }
}
