using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

//http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
namespace SiteServer.API.Controllers
{
    [RoutePrefix("api/v1/contents")]
    public class ContentsController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public HttpResponseMessage Get(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "this api is not allow" });
        }

        [Route("Post")]
        [HttpPost]
        // POST: api/Default
        public HttpResponseMessage Post([FromBody]string value)
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                if (!string.IsNullOrEmpty(value))
                {
                    nvc = TranslateUtils.ToNameValueCollection(value);
                }

                //create content model
                BackgroundContentInfo contentInfo = new BackgroundContentInfo();
                contentInfo.SetExtendedAttribute(nvc);

                string tableName = string.Empty;

                int publishmentSystemID = contentInfo.PublishmentSystemID;
                int nodeID = contentInfo.NodeID;
                PublishmentSystemInfo publishmentSystemInfo = null;

                #region 保存内容
                if (publishmentSystemID > 0 && nodeID > 0)
                {
                    //depend on publishmentSystemID and nodeID
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (publishmentSystemInfo != null)
                    {
                        #region 处理内容中的文件
                        contentInfo.Content = StringUtility.TextEditorContentEncode(contentInfo.Content, publishmentSystemInfo, true);
                        #endregion

                        tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    }
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    //publishSystem is not exists or node is not exists
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "publishmentSystemID and nodeID less than 0 OR publishmentSystemInfo and nodeInfo is not exists" });
                }

                //set isChecked
                contentInfo.IsChecked = true;
                contentInfo.AddDate = DateTime.Now;
                contentInfo.AddUserName = "api";
                int contentID = BaiRongDataProvider.ContentDAO.Insert(tableName, contentInfo);
                #endregion

                #region 保存上传的文件
                //if upload file is exists
                HttpFileCollection fileCollection = HttpContext.Current.Request.Files;
                foreach (HttpPostedFile file in fileCollection)
                {
                    //file name
                    string fileName = PathUtils.GetFileName(file.FileName);
                    //file extension
                    string fileExtension = PathUtils.GetExtension(fileName).ToLower();
                    //localDirectoryPath
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtension);
                    //localFilePath
                    string localFilePath = PathUtils.Combine(localDirectoryPath, fileName);

                    bool isAllowType = false;
                    bool isAllowLength = false;
                    int type = 0;
                    if (PathUtility.IsUploadExtenstionAllowed(EUploadType.File, publishmentSystemInfo, fileExtension))
                    {
                        isAllowType = true;
                        type = 1;
                    }
                    else if (PathUtility.IsUploadExtenstionAllowed(EUploadType.Image, publishmentSystemInfo, fileExtension))
                    {
                        isAllowType = true;
                        type = 2;
                    }
                    else if (PathUtility.IsUploadExtenstionAllowed(EUploadType.Video, publishmentSystemInfo, fileExtension))
                    {
                        isAllowType = true;
                        type = 3;
                    }

                    if (type == 1 && PathUtility.IsFileSizeAllowed(publishmentSystemInfo, file.ContentLength))
                    {
                        isAllowLength = true;
                    }
                    else if (type == 2 && PathUtility.IsImageSizeAllowed(publishmentSystemInfo, file.ContentLength))
                    {
                        isAllowLength = true;
                    }
                    else if (type == 3 && PathUtility.IsVideoSizeAllowed(publishmentSystemInfo, file.ContentLength))
                    {
                        isAllowLength = true;
                    }

                    if (!isAllowType || !isAllowLength)
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = "upload file type is not allowed or greater than limited length" });

                    file.SaveAs(localFilePath);
                }
                #endregion

                return this.Request.CreateResponse(HttpStatusCode.OK, new { id = contentID });
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = ex.Message });
            }
        }

        // PUT: api/Default/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "this api is not allowed" });
        }

        // DELETE: api/Default/5
        public HttpResponseMessage Delete(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "this api is not allowed" });
        }
    }
}
