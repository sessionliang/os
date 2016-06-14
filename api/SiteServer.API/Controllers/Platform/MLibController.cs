using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Drawing;
using BaiRong.Model;
using NetDimension.Weibo;
using Newtonsoft.Json;
using SiteServer.API.Core;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;

namespace SiteServer.API.Controllers
{
    /// <summary>
    /// Ͷ������
    /// </summary> 
    public class MLibController : ApiController
    {
        private string MLibDraftContentTableName = "bairong_MLibDraftContent";

        #region �ݸ�

        /// <summary>
        /// ��ȡ�û��ݸ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMLibDraftContents")]
        public IHttpActionResult GetUserMLibDraftContents()
        {
            try
            {
                string userName = UserManager.Current.UserName;//�û� 
                string title = RequestUtils.GetQueryStringNoSqlAndXss("title");
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                ArrayList list = DataProvider.MLibDraftContentDAO.GetUserMLibDraftContentList(userName, title, startdate, enddate, pageIndex, prePageNum);
                int total = list.Count;
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                var draftListParm = new { IsSuccess = true, DraftList = list, PageJson = pageJson };
                return Ok(draftListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// ��ȡ�û�ĳ���ݸ���Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMLibDraftContent")]
        public IHttpActionResult GetUserMLibDraftContent()
        {
            try
            {
                string userName = UserManager.Current.UserName;//�û�  

                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = RequestUtils.GetIntRequestString("nodeID");
                int contentID = RequestUtils.GetIntRequestString("contentID");

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeInfo.NodeID);

                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID); 

                //��ȡ��Ŀ�ֶ� 
                MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                string files = ContentUtils.MLibDraftContentAttributeNames(tableName);
                if (minfo != null && !string.IsNullOrEmpty(minfo.Field))
                    files = minfo.Field;

                ArrayList styleInfoArrayListOld = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);

                ArrayList myStyleInfoArrayList = new ArrayList();
                if (string.IsNullOrEmpty(files))
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayListOld)
                    {
                        if (styleInfo.IsVisible)
                            myStyleInfoArrayList.Add(styleInfo);
                    }
                }
                else
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayListOld)
                    {
                        ArrayList filesList = TranslateUtils.StringCollectionToArrayList(files);
                        foreach (string flesName in filesList)
                        {
                            if (styleInfo.IsVisible && flesName == styleInfo.AttributeName)
                                myStyleInfoArrayList.Add(styleInfo);
                        }
                    }
                } 
                var draftParm = new { IsSuccess = true, DraftItem = myStyleInfoArrayList };
                return Ok(draftParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }



        /// <summary>
        /// ��Ӳݸ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CreateMLibDraftContent")]
        public IHttpActionResult CreateMLibDraftContent()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = RequestUtils.GetIntRequestString("nodeID");

                if (publishmentSystemID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ��վ��" };
                    return Ok(parameter);
                }

                if (nodeID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ����Ŀ" };
                    return Ok(parameter);
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                contentInfo.PublishmentSystemID = publishmentSystemID;
                contentInfo.NodeID = nodeID;
                if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "ϵͳ���ϣ�����ʧ��" };
                    return Ok(parameter);
                }

                InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                string errorMessage = string.Empty;
                bool isSuccess = this.Upload(publishmentSystemInfo, contentInfo, tableStyle, tableName, relatedIdentities, out errorMessage);
                if (!isSuccess)
                {
                    parameter = new Parameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };
                    return Ok(parameter);
                }
                contentInfo.AddUserName = UserManager.Current.UserName;
                contentInfo.AddDate = DateTime.Now;
                contentInfo.LastEditUserName = contentInfo.AddUserName;
                contentInfo.LastEditDate = DateTime.Now;

                contentInfo.CheckedLevel = LevelManager.LevelInt.CaoGao;
                contentInfo.IsChecked = false;

                contentInfo.SourceID = nodeID;
                contentInfo.ReferenceID = 0;
                contentInfo.MemberName = UserManager.Current.UserName;
                int contentID = DataProvider.ContentDAO.Insert(MLibDraftContentTableName, publishmentSystemInfo, contentInfo);
                contentInfo.ID = contentID;
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(parameter);
            }

        }

        /// <summary>
        /// �޸Ĳݸ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateMLibDraftContent")]
        public IHttpActionResult UpdateMLibDraftContent()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                int contentID = RequestUtils.GetIntRequestString("contentID");
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = RequestUtils.GetIntRequestString("nodeID");

                if (publishmentSystemID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ��վ��" };
                    return Ok(parameter);
                }

                if (nodeID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ����Ŀ" };
                    return Ok(parameter);
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                contentInfo.PublishmentSystemID = publishmentSystemID;
                contentInfo.NodeID = nodeID;
                if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "ϵͳ���ϣ�����ʧ��" };
                    return Ok(parameter);
                }

                InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                string errorMessage = string.Empty;
                bool isSuccess = this.Upload(publishmentSystemInfo, contentInfo, tableStyle, tableName, relatedIdentities, out errorMessage);
                if (!isSuccess)
                {
                    parameter = new Parameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };
                    return Ok(parameter);
                }
                contentInfo.ID = contentID;
                contentInfo.AddUserName = UserManager.Current.UserName;
                contentInfo.AddDate = DateTime.Now;
                contentInfo.LastEditUserName = contentInfo.AddUserName;
                contentInfo.LastEditDate = DateTime.Now;

                contentInfo.CheckedLevel = LevelManager.LevelInt.CaoGao;
                contentInfo.IsChecked = false;
                contentInfo.SourceID = nodeID;
                contentInfo.ReferenceID = 0;
                contentInfo.MemberName = UserManager.Current.UserName;
                DataProvider.ContentDAO.Update(MLibDraftContentTableName, publishmentSystemInfo, contentInfo);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        /// <summary>
        /// ɾ���ݸ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DeleteMLibDraftContent")]
        public IHttpActionResult DeleteMLibDraftContent()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                bool delete = TranslateUtils.ToBool(RequestUtils.GetRequestString("deleteStr"));
                if (delete)
                {
                    ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(RequestUtils.GetRequestString("IDsCollection"));
                    if (idsArrayList.Count > 0)
                        //ɾ���ݸ�
                        DataProvider.MLibDraftContentDAO.Delete(idsArrayList);
                }
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        #endregion

        #region �ϴ�/������ع���


        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="contentInfo"></param>
        /// <param name="tableStyle"></param>
        /// <param name="tableName"></param>
        /// <param name="relatedIdentities"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        private bool Upload(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, ETableStyle tableStyle, string tableName, ArrayList relatedIdentities, out string ErrorMessage)
        {
            bool IsSuccess = true;
            ErrorMessage = string.Empty;

            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                if (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.Video || styleInfo.InputType == EInputType.File)
                {
                    string attributeName = InputTypeParser.GetAttributeNameToUploadForTouGao(styleInfo.AttributeName);
                    if (HttpContext.Current.Request.Files[attributeName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Files[attributeName].FileName))
                    {
                        HttpPostedFile postedFile = HttpContext.Current.Request.Files[attributeName];
                        string filePath = postedFile.FileName;
                        string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                        string localDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                        string localFileName = PathUtility.GetUploadFileName(publishmentSystemInfo, filePath);
                        string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                        if (styleInfo.InputType == EInputType.Image)
                        {
                            if (!PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�ϴ�ͼƬ��ʽ����ȷ��";
                                return IsSuccess;
                            }
                            if (!PathUtility.IsImageSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�ϴ�ʧ�ܣ��ϴ�ͼƬ�����涨�ļ���С��";
                                return IsSuccess;
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(publishmentSystemInfo, localFilePath);
                            string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            imageUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, imageUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, imageUrl);
                        }
                        else if (styleInfo.InputType == EInputType.Video)
                        {
                            if (!PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�ϴ���Ƶ��ʽ����ȷ��";
                                return IsSuccess;
                            }
                            if (!PathUtility.IsVideoSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�ϴ�ʧ�ܣ��ϴ���Ƶ�����涨�ļ���С��";
                                return IsSuccess;
                            }
                            postedFile.SaveAs(localFilePath);
                            string videoUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            videoUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, videoUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, videoUrl);
                        }
                        else if (styleInfo.InputType == EInputType.File)
                        {
                            if (!PathUtility.IsFileExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�˸�ʽ�������ϴ�����ѡ����Ч���ļ���";
                                return IsSuccess;
                            }
                            if (!PathUtility.IsFileSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                IsSuccess = false;
                                ErrorMessage = "�ϴ�ʧ�ܣ��ϴ��ļ������涨�ļ���С��";
                                return IsSuccess;
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(publishmentSystemInfo, localFilePath);
                            string fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            fileUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, fileUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, fileUrl);
                        }
                    }
                }
            }
            return IsSuccess;
        }

        #endregion

        #region Ͷ�巶Χ

        /// <summary>
        /// ��ȡ�û���Ͷ���վ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMLibPublishmentSystem")]
        public IHttpActionResult GetUserMLibPublishmentSystem()
        {
            try
            {
                ArrayList list = PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser);

                var listParm = new { IsSuccess = true, PublishmentSystemList = list };
                return Ok(listParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }


        /// <summary>
        /// ��ȡ�û���Ͷ�����Ŀ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMLibNodeInfo")]
        public IHttpActionResult GetUserMLibNodeInfo()
        {
            try
            {
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                ArrayList list = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, pinfo.PublishmentSystemID);

                var listParm = new { IsSuccess = true, NodeList = list };
                return Ok(listParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }



        /// <summary>
        /// ��ȡͶ���ֶ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMLibFileds")]
        public IHttpActionResult GetMLibFileds()
        {
            var parameter = new Parameter { IsSuccess = true };
            try
            {
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = RequestUtils.GetIntRequestString("nodeID");
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                if (nodeInfo == null)
                {
                    if (nodeID < 0)
                    {
                        parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ��վ����Ŀ." };
                        return Ok(parameter);
                    }
                }
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);


                //��ȡ��Ŀ�ֶ� 
                MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                string files = ContentUtils.MLibDraftContentAttributeNames(tableName);
                if (minfo != null && !string.IsNullOrEmpty(minfo.Field))
                    files = minfo.Field;

                string fileds = ContentUtils.GetMLibFileds(new NameValueCollection(), publishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, false, false, files);

                string syncScript = string.Empty;

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.IsVisible && styleInfo.InputType == EInputType.TextEditor)
                    {
                        ETextEditorType editorType = ETextEditorType.UEditor;
                        if (string.IsNullOrEmpty(styleInfo.Additional.EditorTypeString))
                        {
                            editorType = publishmentSystemInfo.Additional.TextEditorType;
                        }
                        else
                        {
                            editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
                        }

                        if (editorType == ETextEditorType.UEditor)
                        {
                            syncScript += string.Format(@"UE.getEditor('{0}').sync();", styleInfo.AttributeName);
                        }
                    }
                }

                string onclickString = syncScript + InputParserUtils.GetValidateSubmitOnClickScript("myForm", true, "autoCheckKeywords()");

                var listParm = new { IsSuccess = true, NodeList = fileds, OnclickString = onclickString };
                return Ok(listParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }


        /// <summary>
        /// ��ǰ��¼�˺��Ƿ�����Ͷ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CantMLib")]
        public IHttpActionResult CantMLib()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                bool canMlib = UserManager.CurrentCanDoByValidityDate;

                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ǰʱ���ѳ������Ͷ����Ч�ڣ�����ʹ��Ͷ�壬������Ҫ����ϵ����Ա." };
                }
                //�Ƿ���Ͷ����δʹ�� 
                canMlib = UserManager.CurrentCanDoByMLibNum;
                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "����Ͷ�������Ѵﵽ����." };
                }
                //��������߱�����������Ͷ�� 
                canMlib = UserManager.CurrentCanDoByAdmin;
                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "����Ͷ�������ѱ��޸ģ��п����Ǹ�������ߵĹ���Ա�˺ű�����������ϵ����Ա." };
                }
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        #endregion

        #region Ͷ����Ϣ

        /// <summary>
        /// ��ȡ�û�Ͷ����Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMLibContent")]
        public IHttpActionResult GetUserMLibContent()
        {
            try
            {
                string userName = UserManager.Current.UserName;//�û� 
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                int nodeID = RequestUtils.GetIntRequestString("nodeID");
                string title = RequestUtils.GetQueryStringNoSqlAndXss("title");
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                ArrayList nodeList = new ArrayList();
                if (nodeID == 0)
                {
                    ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, publishmentSystemInfo.PublishmentSystemID);
                    foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
                    {
                        nodeList.Add(nodeInfo.NodeID);
                    }
                }

                StringBuilder whereString = new StringBuilder();
                whereString.AppendFormat(" WHERE MemberName = '{0}'", UserManager.Current.UserName);
                if (!string.IsNullOrEmpty(title))
                {
                    whereString.AppendFormat(" AND Title like '%{0}%'", title);
                }
                if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                {
                    whereString.Append(" AND (AddDate>='" + startdate + "' AND AddDate<='" + enddate + "') ");
                }
                whereString.AppendFormat(" AND PublishmentSystemID = {0}", publishmentSystemID.ToString());
                if (nodeID == 0)
                {
                    nodeID = publishmentSystemID;

                    string nodeStr = string.Empty;
                    foreach (string id in nodeList)
                    {
                        nodeStr = " NodeID = {" + id + "} AND";
                    }
                    if (string.IsNullOrEmpty(nodeStr))
                        nodeStr = nodeStr.Substring(0, nodeStr.Length - 4);
                    whereString.AppendFormat(" AND ({0})", nodeStr);

                }
                else
                {
                    whereString.AppendFormat(" AND NodeID = {0}", nodeID.ToString());
                }

                string nodeTableName = NodeManager.GetTableName(publishmentSystemInfo, EAuxiliaryTableType.BackgroundContent);

                string sql = whereString.ToString();

                ArrayList list = BaiRongDataProvider.ContentDAO.GetUserContent(nodeTableName, whereString.ToString(), pageIndex, prePageNum);
                int total = list.Count;
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                var listParm = new { IsSuccess = true, ContentList = list, UserMLibCount = UserManager.Current.MLibNum, PageJson = pageJson };
                return Ok(listParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }



        /// <summary>
        /// Ͷ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CreateMLibContent")]
        public IHttpActionResult CreateMLibContent()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                bool canMlib = UserManager.CurrentCanDoByValidityDate;

                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ǰʱ���ѳ������Ͷ����Ч�ڣ�����ʹ��Ͷ�壬������Ҫ����ϵ����Ա." };
                    return Ok(parameter);
                }


                //�Ƿ���Ͷ����δʹ�� 
                canMlib = UserManager.CurrentCanDoByMLibNum;
                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "����Ͷ�������Ѵﵽ����." };
                    return Ok(parameter);
                }
                //��������߱�����������Ͷ�� 
                canMlib = UserManager.CurrentCanDoByAdmin;
                if (!canMlib)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "����Ͷ�������ѱ��޸ģ��п����Ǹ�������ߵĹ���Ա�˺ű�����������ϵ����Ա." };
                    return Ok(parameter);
                }

                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = RequestUtils.GetIntRequestString("nodeID");

                if (publishmentSystemID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ��վ��" };
                    return Ok(parameter);
                }

                if (nodeID == 0)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "��ѡ����Ŀ" };
                    return Ok(parameter);
                }


                //������ 
                string userName = UserManager.CurrentNewGroupMLibAddUser;


                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                contentInfo.PublishmentSystemID = publishmentSystemID;
                contentInfo.NodeID = nodeID;
                if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = "ϵͳ���ϣ�����ʧ��" };
                    return Ok(parameter);
                }

                InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                string errorMessage = string.Empty;
                bool isSuccess = this.Upload(publishmentSystemInfo, contentInfo, tableStyle, tableName, relatedIdentities, out errorMessage);
                if (!isSuccess)
                {
                    parameter = new Parameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };
                    return Ok(parameter);
                }

                contentInfo.AddUserName = userName;
                contentInfo.AddDate = DateTime.Now;
                contentInfo.LastEditUserName = contentInfo.AddUserName;
                contentInfo.LastEditDate = DateTime.Now;
                contentInfo.CheckedLevel = 0;
                contentInfo.SourceID = nodeID;
                contentInfo.ReferenceID = 0;
                contentInfo.MemberName = UserManager.Current.UserName;

                int checkedLevel = 0;
                bool isChecked = ContentUtils.ContentIsChecked(contentInfo.AddUserName, publishmentSystemInfo, nodeInfo.NodeID, out checkedLevel);

                contentInfo.IsChecked = isChecked;
                contentInfo.CheckedLevel = checkedLevel;

                int id = DataProvider.ContentDAO.Insert(tableName, publishmentSystemInfo, contentInfo);
                contentInfo.ID = id;


                //�ж��ǲ��������Ȩ��
                int checkedLevelOfUser = 0;
                bool isCheckedOfUser = CheckManager.GetUserCheckLevel(publishmentSystemInfo, contentInfo.NodeID, out checkedLevelOfUser);
                if (LevelManager.IsCheckable(publishmentSystemInfo, contentInfo.NodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isCheckedOfUser, checkedLevelOfUser))
                {
                    //�����˼�¼
                    BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, new ArrayList() { id }, 0, true, contentInfo.AddUserName, contentInfo.IsChecked, contentInfo.CheckedLevel, "");
                }

                //�޸��û����е�Ͷ������
                UserInfo userInfo = UserManager.Current;
                userInfo.MLibNum = userInfo.MLibNum + 1;
                BaiRongDataProvider.UserDAO.Update(userInfo);

                //����ҳ��
                if (isChecked)
                {
                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(publishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, nodeInfo.NodeID, id, 0);
                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);

                }
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(parameter);
            }

        }


        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ExportUserMLibContent")]
        public IHttpActionResult ExportUserMLibContent()
        {
            try
            {
                int publishmentSystemID = RequestUtils.GetIntRequestString("publishmentSystemID");
                int nodeID = RequestUtils.GetIntRequestString("nodeID");

                ArrayList mLibScopeInfoArrayList = new ArrayList();
                mLibScopeInfoArrayList = PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser);

                string httpHost = HttpContext.Current.Request.Url.Host;
                string docFileName = UserManager.Current.UserName + "�ĸ��.xls";
                ContentUtils.CreateExcel(mLibScopeInfoArrayList, httpHost, docFileName);

                var successParm = new { IsSuccess = true };
                return Ok(successParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        #endregion

    }
}
