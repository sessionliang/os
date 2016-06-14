using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.STL.StlTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    [RoutePrefix("api/services/cms")]
    public class CmsController : ApiController
    {
        private int PublishmentSystemID;
        private PublishmentSystemInfo PublishmentSystemInfo;
        private string ScriptString;

        [HttpPost, HttpGet]
        [Route("action")]
        public HttpResponseMessage Action()
        {
            bool isHtml = false;
            PublishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetQueryString("PublishmentSystemID"));
            PublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(PublishmentSystemID);
            bool isCrossDomain = PageUtility.IsCrossDomain(PublishmentSystemInfo);
            bool isCorsCross = PageUtility.IsCorsCrossDomain(PublishmentSystemInfo);
            string type = RequestUtils.GetQueryString("type");
            if (PublishmentSystemInfo != null && !string.IsNullOrEmpty(type))
            {
                if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Input))
                {
                    int inputID = TranslateUtils.ToInt(RequestUtils.GetQueryString("inputID"));
                    this.InputAdd(isCrossDomain, isCorsCross, inputID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_WebsiteMessage))
                {
                    int websiteMessageID = TranslateUtils.ToInt(RequestUtils.GetQueryString("websiteMessageID"));
                    this.WebsiteMessageAdd(isCrossDomain, isCorsCross, websiteMessageID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Content))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetQueryString("channelID"));
                    this.ContentInputAdd(isCrossDomain, isCorsCross, styleID, channelID);
                }
                //else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Comment))
                //{
                //    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                //    int channelID = TranslateUtils.ToInt(RequestUtils.GetQueryString("channelID"));
                //    int contentID = TranslateUtils.ToInt(RequestUtils.GetQueryString("contentID"));
                //    this.CommentInputAdd(isCrossDomain, isCorsCross, styleID, channelID, contentID);
                //}
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Login))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    string userFrom = RequestUtils.GetPostString("UserFrom");
                    string userName = RequestUtils.GetPostString("UserName");
                    string password = RequestUtils.GetPostString("Password");

                    this.LoginInputAdd(isCrossDomain, isCorsCross, styleID, userFrom, userName, password);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Register))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.RegisterInputAdd(isCrossDomain, isCorsCross, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Resume))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.ResumeAdd(isCrossDomain, isCorsCross, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovPublicApply))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.GovPublicApplyAdd(isCrossDomain, isCorsCross, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovPublicQuery))
                {
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.GovPublicQueryAdd(isCrossDomain, isCorsCross, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovInteractApply))
                {
                    int nodeID = TranslateUtils.ToInt(RequestUtils.GetQueryString("nodeID"));
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.GovInteractApplyAdd(isCrossDomain, isCorsCross, nodeID, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovInteractQuery))
                {
                    int nodeID = TranslateUtils.ToInt(RequestUtils.GetQueryString("nodeID"));
                    int styleID = TranslateUtils.ToInt(RequestUtils.GetQueryString("styleID"));
                    this.GovInteractQueryAdd(isCrossDomain, isCorsCross, nodeID, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Vote))
                {
                    int nodeID = TranslateUtils.ToInt(RequestUtils.GetQueryString("nodeID"));
                    int contentID = TranslateUtils.ToInt(RequestUtils.GetQueryString("contentID"));
                    this.VoteAdd(isCrossDomain, isCorsCross, nodeID, contentID);
                }
                #region  by 20151127 sofuny   信息订阅
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_SubscribeQuery))
                {
                    object output = this.SubscribeQuery(isCrossDomain, isCorsCross);

                    HttpResponseMessage responseOutput = Request.CreateResponse(HttpStatusCode.OK, output, new MediaTypeHeaderValue("application/json"));
                    return responseOutput;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_SubscribeApply))
                {
                    object output = this.SaveSubscribeUser(isCrossDomain, isCorsCross);

                    HttpResponseMessage responseOutput = Request.CreateResponse(HttpStatusCode.OK, output, new MediaTypeHeaderValue("application/json"));
                    return responseOutput;
                }
                #endregion
                #region  by 20151127 sofuny   评价管理
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_EvaluationApply))
                {
                    object output = this.SaveSubscribeUser(isCrossDomain, isCorsCross);

                    HttpResponseMessage responseOutput = Request.CreateResponse(HttpStatusCode.OK, output, new MediaTypeHeaderValue("application/json"));
                    return responseOutput;
                }


                #endregion

                #region add by sessionliang 20160301 广告
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Ad))
                {
                    string adAreaName = PageUtils.FilterSqlAndXss(RequestUtils.QueryString["adAreaName"]);
                    ETemplateType templateType = ETemplateTypeUtils.GetEnumType(RequestUtils.QueryString["templateType"]);
                    string uniqueID = PageUtils.FilterSqlAndXss(RequestUtils.QueryString["uniqueID"]);
                    this.AdHtml(adAreaName, templateType, uniqueID);

                    //表示输出html
                    isHtml = true;
                }
                #endregion
            }

            if (PublishmentSystemInfo.Additional.IsSonSiteAlone)
            {
                //替换全部
                ScriptString = ScriptString.Replace(string.Format("\"{0}\"", PublishmentSystemInfo.PublishmentSystemUrl), "\"/\"");
                ScriptString = ScriptString.Replace(string.Format("'{0}'", PublishmentSystemInfo.PublishmentSystemUrl), "'/'");
                ScriptString = ScriptString.Replace(PublishmentSystemInfo.PublishmentSystemUrl, string.Empty);
                //文件服务器
                if (PublishmentSystemInfo.Additional.EditorUploadFilePre != null && PublishmentSystemInfo.Additional.EditorUploadFilePre.Length > 0)
                {
                    ScriptString = ScriptString.Replace("/upload/images", string.Format("{0}/upload/images", PublishmentSystemInfo.PublishmentSystemUrl));
                    ScriptString = ScriptString.Replace("/upload/files", string.Format("{0}/upload/files", PublishmentSystemInfo.PublishmentSystemUrl));
                    ScriptString = ScriptString.Replace("/upload/videos", string.Format("{0}/upload/videos", PublishmentSystemInfo.PublishmentSystemUrl));
                }
            }

            HttpResponseMessage response = null;

            if (!isHtml)//输出json，前台js处理
            {
                //上传文件之后，需要重新设置一下Content-Type
                response = Request.CreateResponse(HttpStatusCode.OK, new { ScriptString = ScriptString }, new MediaTypeHeaderValue("application/json"));
                return response;
            }
            else//输出html，直接写在页面中
            {
                response = new HttpResponseMessage();
                response.Content = new StringContent(ScriptString);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }

        }


        private void InputAdd(bool isCrossDomain, bool isCorsCross, int inputID)
        {
            InputInfo inputInfo = null;
            if (inputID > 0)
            {
                inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
            }
            if (inputInfo != null)
            {
                bool isValidateCode = inputInfo.Additional.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, PublishmentSystemID, inputInfo.InputID);

                string ipAddress = PageUtils.GetIPAddress();
                string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

                InputContentInfo contentInfo = new InputContentInfo(0, inputInfo.InputID, 0, inputInfo.IsChecked, BaiRongDataProvider.UserDAO.CurrentUserName, ipAddress, location, DateTime.Now, string.Empty);

                try
                {
                    if (isValidateCode)
                    {
                        if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, inputInfo.InputID, isCrossDomain || isCorsCross);

                            if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!inputInfo.Additional.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception("请先登录系统!");
                    }


                    InputTypeParser.AddValuesToAttributes(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, PublishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes);

                    if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in HttpContext.Current.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = HttpContext.Current.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    #region 是否校验重复数据
                    if (inputInfo.Additional.IsUnique)
                    {
                        if (!string.IsNullOrEmpty(inputInfo.Additional.UniquePro) && DataProvider.InputContentDAO.IsExistsPro(inputInfo.InputID, inputInfo.Additional.UniquePro, contentInfo.GetExtendedAttribute(inputInfo.Additional.UniquePro)))
                            throw new Exception("请不要重复提交!");
                    }
                    #endregion

                    DataProvider.InputContentDAO.Insert(contentInfo);

                    MessageManager.SendMailByInput(PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    MessageManager.SendSMSByInput(PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;
                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
                    {
                        if (string.IsNullOrEmpty(inputInfo.Additional.MessageSuccess))
                        {
                            message = "表单提交成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "表单提交成功。";
                            }
                        }
                        else
                        {
                            message = inputInfo.Additional.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
                    }


                    ScriptString = InputTemplate.GetInputCallbackScript(PublishmentSystemInfo, inputID, true, message);

                    //if (contentInfo.IsChecked == EBoolean.True)
                    //{
                    //    FileSystemObject FSO = new FileSystemObject(PublishmentSystemID);
                    //    FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    //}
                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
                    {
                        if (string.IsNullOrEmpty(inputInfo.Additional.MessageFailure))
                        {
                            message = "表单提交失败，" + ex.Message;
                        }
                        else
                        {
                            message = inputInfo.Additional.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
                    }

                    ScriptString = InputTemplate.GetInputCallbackScript(PublishmentSystemInfo, inputID, false, message);
                }
            }
        }

        private void WebsiteMessageAdd(bool isCrossDomain, bool isCorsCross, int websiteMessageID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;
            if (websiteMessageID > 0)
            {
                websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
            }
            if (websiteMessageInfo != null)
            {
                bool isValidateCode = websiteMessageInfo.Additional.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);

                string ipAddress = PageUtils.GetIPAddress();
                string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

                WebsiteMessageContentInfo contentInfo = new WebsiteMessageContentInfo(0, websiteMessageInfo.WebsiteMessageID, 0, websiteMessageInfo.IsChecked, BaiRongDataProvider.UserDAO.CurrentUserName, ipAddress, location, DateTime.Now, string.Empty);

                try
                {
                    if (isValidateCode)
                    {
                        if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, websiteMessageInfo.WebsiteMessageID, isCrossDomain || isCorsCross);

                            if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!websiteMessageInfo.Additional.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception(string.Format("请先登录系统!<a href='{0}' target='_blank'>点击登录</a>", PageUtility.ParseNavigationUrl(PublishmentSystemID, websiteMessageInfo.Additional.LoginUrl)));
                    }


                    InputTypeParser.AddValuesToAttributes(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, PublishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes);

                    //分类
                    contentInfo.ClassifyID = TranslateUtils.ToInt(RequestUtils.GetPostString("WebsiteMessageClassifyID"));
                    if (contentInfo.ClassifyID == 0)
                    {
                        contentInfo.ClassifyID = DataProvider.WebsiteMessageClassifyDAO.GetDefaultClassifyID();
                    }

                    if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in HttpContext.Current.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = HttpContext.Current.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    DataProvider.WebsiteMessageContentDAO.Insert(contentInfo);

                    //MessageManager.SendMailByWebsiteMessage(PublishmentSystemInfo, websiteMessageInfo, relatedIdentities, contentInfo);

                    //MessageManager.SendSMSByWebsiteMessage(PublishmentSystemInfo, websiteMessageInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;
                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
                    {
                        if (string.IsNullOrEmpty(websiteMessageInfo.Additional.MessageSuccess))
                        {
                            message = "表单提交成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "表单提交成功。";
                            }
                        }
                        else
                        {
                            message = websiteMessageInfo.Additional.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
                    }


                    ScriptString = WebsiteMessageTemplate.GetWebsiteMessageCallbackScript(PublishmentSystemInfo, websiteMessageID, true, message);

                    //if (contentInfo.IsChecked == EBoolean.True)
                    //{
                    //    FileSystemObject FSO = new FileSystemObject(PublishmentSystemID);
                    //    FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    //}
                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
                    {
                        if (string.IsNullOrEmpty(websiteMessageInfo.Additional.MessageFailure))
                        {
                            message = "表单提交失败，" + ex.Message;
                        }
                        else
                        {
                            message = websiteMessageInfo.Additional.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
                    }

                    ScriptString = WebsiteMessageTemplate.GetWebsiteMessageCallbackScript(PublishmentSystemInfo, websiteMessageID, false, message);
                }
            }
        }

        private void ContentInputAdd(bool isCrossDomain, bool isCorsCross, int styleID, int channelID)
        {
            if (RequestUtils.GetPostString(ContentAttribute.NodeID) != null)
            {
                channelID = TranslateUtils.ToInt(RequestUtils.GetPostString(ContentAttribute.NodeID), channelID);
            }
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(tagStyleInfo.SettingsXML);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemID, channelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(PublishmentSystemInfo, nodeInfo);
            if (inputInfo != null && nodeInfo != null)
            {
                bool isValidateCode = inputInfo.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(PublishmentSystemID, nodeInfo.NodeID);

                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                contentInfo.NodeID = nodeInfo.NodeID;
                contentInfo.PublishmentSystemID = PublishmentSystemID;
                if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                }
                else
                {
                    contentInfo.AddUserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                }

                try
                {
                    if (isValidateCode)
                    {
                        if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, styleID, isCrossDomain || isCorsCross);

                            if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!inputInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception("请先登录系统!");
                    }

                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, PublishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes);

                    if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in HttpContext.Current.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = HttpContext.Current.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.IsChecked = inputInfo.IsChecked;
                    contentInfo.CheckedLevel = 0;

                    DataProvider.ContentDAO.Insert(tableName, PublishmentSystemInfo, contentInfo);

                    MessageManager.SendMailByContentInput(PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    MessageManager.SendSMSByContentInput(PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;

                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
                    {
                        if (string.IsNullOrEmpty(inputInfo.MessageSuccess))
                        {
                            message = "内容添加成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "内容添加成功。";
                            }
                        }
                        else
                        {
                            message = inputInfo.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
                    }

                    ScriptString = ContentInputTemplate.GetInputCallbackScript(PublishmentSystemInfo, styleID, true, message);

                    if (contentInfo.IsChecked)
                    {
                        FileSystemObject FSO = new FileSystemObject(PublishmentSystemID);
                        //FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Empty;

                    if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
                    {
                        if (string.IsNullOrEmpty(inputInfo.MessageFailure))
                        {
                            message = "内容添加失败，" + ex.Message;
                        }
                        else
                        {
                            message = inputInfo.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
                    }

                    ScriptString = ContentInputTemplate.GetInputCallbackScript(PublishmentSystemInfo, styleID, false, message);
                }
            }
        }

        //private void CommentInputAdd(bool isCrossDomain, bool isCorsCross, int styleID, int channelID, int contentID)
        //{
        //    TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
        //    if (tagStyleInfo == null)
        //    {
        //        tagStyleInfo = new TagStyleInfo();
        //    }
        //    TagStyleCommentInputInfo inputInfo = new TagStyleCommentInputInfo(tagStyleInfo.SettingsXML);
        //    NodeInfo nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemID, channelID);
        //    if (inputInfo != null && nodeInfo != null)
        //    {
        //        bool isValidateCode = inputInfo.IsValidateCode;
        //        if (isValidateCode)
        //        {
        //            isValidateCode = FileConfigManager.Instance.IsValidateCode;
        //        }

        //        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(PublishmentSystemID, nodeInfo.NodeID);

        //        CommentInfo commentInfo = new CommentInfo();

        //        try
        //        {
        //            if (isValidateCode)
        //            {
        //                if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
        //                {
        //                    VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, styleID, isCrossDomain);

        //                    if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
        //                    {
        //                        throw new Exception("验证码不正确!");
        //                    }
        //                }
        //            }

        //            if (!inputInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
        //            {
        //                throw new Exception("请先登录系统!");
        //            }

        //            InputTypeParser.AddValuesToAttributes(ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, PublishmentSystemInfo, relatedIdentities, Request.Form, commentInfo.Attributes);

        //            if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
        //            {
        //                foreach (string attributeName in HttpContext.Current.Request.Files.AllKeys)
        //                {
        //                    HttpPostedFile myFile = HttpContext.Current.Request.Files(attributeName);
        //                    if (myFile != null && "" != myFile.FileName)
        //                    {
        //                        string fileUrl = this.UploadFile(myFile);
        //                        commentInfo.SetExtendedAttribute(attributeName, fileUrl);
        //                    }
        //                }
        //            }

        //            commentInfo.PublishmentSystemID = PublishmentSystemID;
        //            commentInfo.NodeID = channelID;
        //            commentInfo.ContentID = contentID;
        //            commentInfo.AddDate = DateTime.Now;
        //            commentInfo.IPAddress = PageUtils.GetIPAddress();
        //            commentInfo.Location = BaiRongDataProvider.IP2CityDAO.GetCity(commentInfo.IPAddress);
        //            commentInfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
        //            commentInfo.IsChecked = inputInfo.IsChecked;

        //            DataProvider.CommentContentDAO.Insert(PublishmentSystemID, commentInfo);

        //            MessageManager.SendMail(PublishmentSystemInfo, inputInfo, ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, commentInfo.NodeID, commentInfo);

        //            MessageManager.SendSMS(PublishmentSystemInfo, inputInfo, ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, commentInfo.NodeID, commentInfo);

        //            string message = string.Empty;

        //            if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
        //            {
        //                message = "评论添加成功，正在审核。";
        //                if (commentInfo.IsChecked)
        //                {
        //                    message = "评论添加成功。";
        //                }
        //            }
        //            else
        //            {
        //                message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
        //            }

        //            ScriptString = CommentInputTemplate.GetCallbackScript(PublishmentSystemInfo, true, message);

        //            if (commentInfo.IsChecked && contentID > 0)
        //            {
        //                FileSystemObject FSO = new FileSystemObject(PublishmentSystemID);
        //                FSO.CreateImmediately(EChangedType.Edit, ETemplateType.ContentTemplate, channelID, contentID, 0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = string.Empty;

        //            if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
        //            {
        //                message = ex.Message;
        //            }
        //            else
        //            {
        //                message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
        //            }

        //            ScriptString = CommentInputTemplate.GetCallbackScript(PublishmentSystemInfo, false, message);
        //        }
        //    }
        //}

        private void LoginInputAdd(bool isCrossDomain, bool isCorsCross, int styleID, string userFrom, string userName, string password)
        {
            //TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            //if (tagStyleInfo == null)
            //{
            //    tagStyleInfo = new TagStyleInfo();
            //}
            //TagStyleLoginInfo loginInfo = new TagStyleLoginInfo(tagStyleInfo.SettingsXML);
            //if (loginInfo != null)
            //{
            //    bool isValidateCode = loginInfo.IsValidateCode;
            //    if (isValidateCode)
            //    {
            //        isValidateCode = FileConfigManager.Instance.IsValidateCode;
            //    }

            //    try
            //    {
            //        if (isValidateCode)
            //        {
            //            if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
            //            {
            //                VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, styleID, isCrossDomain||isCorsCross);

            //                if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
            //                {
            //                    throw new Exception("登录失败，验证码不正确!");
            //                }
            //            }
            //        }

            //        string errorMessage = string.Empty;
            //        if (BaiRongDataProvider.UserDAO.Validate(userName, password, out errorMessage))
            //        {
            //            BaiRongDataProvider.UserDAO.Login(userFrom, userName, true);
            //            ScriptString = LoginTemplate.GetCallbackScript(PublishmentSystemInfo, styleID, true, string.Empty, userName);
            //        }
            //        else
            //        {
            //            throw new Exception("登录失败，" + errorMessage);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ScriptString = LoginTemplate.GetCallbackScript(PublishmentSystemInfo, styleID, false, ex.Message, string.Empty);
            //    }
            //}
        }

        private void RegisterInputAdd(bool isCrossDomain, bool isCorsCross, int styleID)
        {
            //TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            //if (tagStyleInfo == null)
            //{
            //    tagStyleInfo = new TagStyleInfo();
            //}
            //TagStyleRegisterInfo registerInfo = new TagStyleRegisterInfo(tagStyleInfo.SettingsXML);

            //bool isValidateCode = registerInfo.IsValidateCode;
            //if (isValidateCode)
            //{
            //    isValidateCode = FileConfigManager.Instance.IsValidateCode;
            //}

            //try
            //{
            //    if (isValidateCode)
            //    {
            //        if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
            //        {
            //            VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, styleID, isCrossDomain||isCorsCross);

            //            if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
            //            {
            //                throw new Exception("验证码不正确!");
            //            }
            //        }
            //    }

            //    string userName = RequestUtils.GetPostString(UserAttribute.UserName);
            //    string displayName = RequestUtils.GetPostString(UserAttribute.DisplayName);
            //    string password = RequestUtils.GetPostString(UserAttribute.Password);
            //    string comfirmPassword = RequestUtils.GetPostString("ComfirmPassword");
            //    string email = RequestUtils.GetPostString(UserAttribute.Email);

            //    if (BaiRongDataProvider.UserDAO.IsExists(userName))
            //    {
            //        throw new Exception("用户名已被注册，请更换用户名!");
            //    }

            //    if (!BaiRongDataProvider.UserDAO.IsUserNameCompliant(userName))
            //    {
            //        throw new Exception("用户名包含不规则字符，请更换用户名!");
            //    }

            //    if (!UserConfigManager.Additional.IsEmailDuplicated && BaiRongDataProvider.UserDAO.IsEmailExists(email))
            //    {
            //        throw new Exception("电子邮件地址已被注册，请更换邮箱!");
            //    }

            //    if (password != comfirmPassword)
            //    {
            //        throw new Exception("确认密码与密码不一致!");
            //    }

            //    UserInfo userInfo = new UserInfo();
            //    ProductUserInfo productUserInfo = ProductUserInfo.Create(userName, userInfo);

            //    userInfo.UserName = userName;
            //    userInfo.DisplayName = displayName;
            //    userInfo.Password = password;
            //    if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
            //    {
            //        userInfo.IsChecked = true;
            //    }
            //    else
            //    {
            //        userInfo.IsChecked = false;
            //    }
            //    userInfo.IsLockedOut = false;
            //    userInfo.Email = email;

            //    int typeID = registerInfo.TypeID;
            //    if (typeID > 0)
            //    {
            //        if (!UserTypeManager.IsExists(typeID))
            //        {
            //            typeID = 0;
            //        }
            //    }
            //    if (typeID == 0)
            //    {
            //        typeID = UserTypeManager.GetDefaultTypeID();
            //    }

            //    UserTypeInfo typeInfo = UserTypeManager.GetTypeInfo(typeID);

            //    if (typeInfo != null)
            //    {
            //        userInfo.TypeID = typeInfo.TypeID;

            //        ArrayList relatedIdentities = UserManager.GetRelatedIdentities(userInfo.TypeID);
            //        TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TableName, relatedIdentities, HttpContext.Current.Request.Form, userInfo.Attributes);

            //        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfoByCredits(0);

            //        if (groupInfo != null)
            //        {
            //            productUserInfo.GroupID = groupInfo.GroupID;
            //            productUserInfo.Credits = groupInfo.CreditsFrom;
            //        }
            //    }

            //    string errorMessage = string.Empty;
            //    string message = string.Empty;
            //    if (BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage))
            //    {
            //        ProductUserInfo puInfo = DataProvider.ProductUserDAO.GetProductUserInfo(productUserInfo.UserName);
            //        if (puInfo != null)
            //        {
            //            DataProvider.ProductUserDAO.Delete(productUserInfo.UserName);
            //        }
            //        productUserInfo.Credits = registerInfo.Credits;
            //        productUserInfo.GroupID = registerInfo.GroupID;
            //        DataProvider.ProductUserDAO.Insert(productUserInfo);
            //        if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
            //        {
            //            string checkCode = EncryptUtils.Md5(userName);
            //            string mailBody = UserConfigManager.Additional.RegisterVerifyMailContent;
            //            mailBody = mailBody.Replace("(UserName)", displayName);
            //            mailBody = mailBody.Replace("(SiteUrl)", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
            //            mailBody = mailBody.Replace("(VerifyUrl)", PageUtils.AddProtocolToUrl(PageUtils.GetUserCenterUrl(string.Format("register.aspx?checkCode={0}&userName={1}", checkCode, userName))));

            //            UserMailManager.SendMail(userInfo.Email, "邮件注册确认", mailBody, out errorMessage);
            //        }

            //        if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
            //        {

            //            if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
            //            {
            //                message = UserConfigManager.Additional.RegisterWelcome;
            //            }
            //            else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
            //            {
            //                string emailUrl = "http://mail." + userInfo.Email.Substring(userInfo.Email.IndexOf('@') + 1);
            //                message = string.Format(@"注册激活邮件已发送到您的邮箱<a href=""{0}"" target=""_blank"">{1}</a>，点击进入邮箱激活。", emailUrl, userInfo.Email);
            //            }
            //            else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Manually)
            //            {
            //                message = "注册已提交，请等待审核，谢谢。";
            //            }
            //        }
            //        else
            //        {
            //            message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
            //        }

            //        //替换{User.UserName}等
            //        message = StlEntityParser.ReplaceStlUserEntities(message, userInfo);
            //    }
            //    else
            //    {
            //        throw new Exception(errorMessage);
            //    }

            //    ScriptString = RegisterTemplate.GetInputCallbackScript(PublishmentSystemInfo, styleID, true, message);
            //}
            //catch (Exception ex)
            //{
            //    string message = string.Empty;
            //    if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
            //    {
            //        message = "注册失败，" + ex.Message;
            //    }
            //    else
            //    {
            //        message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
            //    }

            //    ScriptString = RegisterTemplate.GetInputCallbackScript(PublishmentSystemInfo, styleID, false, message);
            //}
        }

        private void ResumeAdd(bool isCrossDomain, bool isCorsCross, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(tagStyleInfo.SettingsXML);

            try
            {
                if (!resumeInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    throw new Exception("请先登录系统!");
                }

                ResumeContentInfo contentInfo = DataProvider.ResumeContentDAO.GetContentInfo(PublishmentSystemID, styleID, HttpContext.Current.Request.Form);

                DataProvider.ResumeContentDAO.Insert(contentInfo);

                MessageManager.SendMailByResumeInput(PublishmentSystemInfo, resumeInfo, contentInfo);

                MessageManager.SendSMSByResumeInput(PublishmentSystemInfo, resumeInfo, contentInfo);

                string message = string.Empty;

                if (string.IsNullOrEmpty(RequestUtils.GetPostString("successTemplateString")))
                {
                    if (string.IsNullOrEmpty(resumeInfo.MessageSuccess))
                    {
                        message = "简历添加成功。";
                    }
                    else
                    {
                        message = resumeInfo.MessageSuccess;
                    }
                }
                else
                {
                    message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("successTemplateString"));
                }

                ScriptString = ResumeTemplate.GetCallbackScript(PublishmentSystemInfo, true, message);
            }
            catch (Exception ex)
            {
                string message = string.Empty;

                if (string.IsNullOrEmpty(RequestUtils.GetPostString("failureTemplateString")))
                {
                    if (string.IsNullOrEmpty(resumeInfo.MessageFailure))
                    {
                        message = "简历添加失败，" + ex.Message;
                    }
                    else
                    {
                        message = resumeInfo.MessageFailure;
                    }
                }
                else
                {
                    message = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetPostString("failureTemplateString"));
                }

                ScriptString = ResumeTemplate.GetCallbackScript(PublishmentSystemInfo, false, message);
            }
        }

        private void GovPublicApplyAdd(bool isCrossDomain, bool isCorsCross, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleGovPublicApplyInfo tagStyleGovPublicApplyInfo = new TagStyleGovPublicApplyInfo(tagStyleInfo.SettingsXML);

            try
            {
                GovPublicApplyInfo applyInfo = DataProvider.GovPublicApplyDAO.GetApplyInfo(PublishmentSystemID, styleID, HttpContext.Current.Request.Form);

                int applyID = DataProvider.GovPublicApplyDAO.Insert(applyInfo);

                string fromName = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicName);
                if (applyInfo.IsOrganization)
                {
                    fromName = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgName);
                }
                string toDepartmentName = string.Empty;
                if (applyInfo.DepartmentID > 0)
                {
                    toDepartmentName = "至" + applyInfo.DepartmentName;
                }
                GovPublicApplyManager.LogNew(PublishmentSystemID, applyID, fromName, toDepartmentName);

                MessageManager.SendMailByGovPublicApply(PublishmentSystemInfo, tagStyleGovPublicApplyInfo, applyInfo);

                MessageManager.SendSMSByGovPublicApply(PublishmentSystemInfo, tagStyleGovPublicApplyInfo, applyInfo);

                ScriptString = GovPublicApplyTemplate.GetCallbackScript(PublishmentSystemInfo, true, applyInfo.QueryCode, string.Empty);
            }
            catch (Exception ex)
            {
                ScriptString = GovPublicApplyTemplate.GetCallbackScript(PublishmentSystemInfo, false, string.Empty, ex.Message);
            }
        }

        private void GovPublicQueryAdd(bool isCrossDomain, bool isCorsCross, int styleID)
        {
            try
            {
                bool isOrganization = TranslateUtils.ToBool(RequestUtils.GetPostString(GovPublicApplyAttribute.IsOrganization));
                string queryName = RequestUtils.GetPostString("queryName");
                string queryCode = RequestUtils.GetPostString(GovPublicApplyAttribute.QueryCode);
                GovPublicApplyInfo applyInfo = DataProvider.GovPublicApplyDAO.GetApplyInfo(PublishmentSystemID, isOrganization, queryName, queryCode);
                if (applyInfo != null)
                {
                    ScriptString = GovPublicQueryTemplate.GetCallbackScript(PublishmentSystemInfo, true, applyInfo, string.Empty);
                }
                else
                {
                    ScriptString = GovPublicQueryTemplate.GetCallbackScript(PublishmentSystemInfo, false, null, "系统找不到对应的申请，请确认您的输入值是否正确");
                }
            }
            catch (Exception ex)
            {
                ScriptString = GovPublicQueryTemplate.GetCallbackScript(PublishmentSystemInfo, false, null, ex.Message);
            }
        }

        private void GovInteractApplyAdd(bool isCrossDomain, bool isCorsCross, int nodeID, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleGovInteractApplyInfo tagStyleGovInteractApplyInfo = new TagStyleGovInteractApplyInfo(tagStyleInfo.SettingsXML);

            try
            {
                bool isValidateCode = tagStyleGovInteractApplyInfo.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                if (isValidateCode)
                {
                    if (RequestUtils.GetPostString(ValidateCodeManager.AttributeName) != null)
                    {
                        VCManager vcManager = ValidateCodeManager.GetInstance(PublishmentSystemID, styleID, isCrossDomain || isCorsCross);

                        if (!vcManager.IsCodeValid(RequestUtils.GetPostString(ValidateCodeManager.AttributeName)))
                        {
                            throw new Exception("验证码不正确!");
                        }
                    }
                }

                if (!tagStyleGovInteractApplyInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    throw new Exception("请先登录系统!");
                }

                GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(PublishmentSystemInfo, nodeID, HttpContext.Current.Request.Form);

                if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                {
                    foreach (string attributeName in HttpContext.Current.Request.Files.AllKeys)
                    {
                        HttpPostedFile myFile = HttpContext.Current.Request.Files[attributeName];
                        if (myFile != null && "" != myFile.FileName)
                        {
                            string fileUrl = string.Empty;
                            fileUrl = this.UploadFile(myFile);
                            contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                        }
                    }
                }

                int contentID = DataProvider.ContentDAO.Insert(PublishmentSystemInfo.AuxiliaryTableForGovInteract, PublishmentSystemInfo, contentInfo);

                string realName = contentInfo.RealName;
                string toDepartmentName = string.Empty;
                if (contentInfo.DepartmentID > 0)
                {
                    toDepartmentName = "至" + contentInfo.DepartmentName;
                }
                GovInteractApplyManager.LogNew(PublishmentSystemID, nodeID, contentID, realName, toDepartmentName);

                MessageManager.SendMail(PublishmentSystemInfo, tagStyleGovInteractApplyInfo, ETableStyle.GovInteractContent, PublishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID, contentInfo);

                MessageManager.SendSMS(PublishmentSystemInfo, tagStyleGovInteractApplyInfo, ETableStyle.GovInteractContent, PublishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID, contentInfo);

                ScriptString = GovInteractApplyTemplate.GetCallbackScript(PublishmentSystemInfo, nodeID, true, contentInfo.QueryCode, string.Empty);
            }
            catch (Exception ex)
            {
                ScriptString = GovInteractApplyTemplate.GetCallbackScript(PublishmentSystemInfo, nodeID, false, string.Empty, ex.Message);
            }
        }

        private void GovInteractQueryAdd(bool isCrossDomain, bool isCorsCross, int nodeID, int styleID)
        {
            try
            {
                string queryCode = RequestUtils.GetPostString(GovInteractContentAttribute.QueryCode);
                GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(PublishmentSystemInfo, nodeID, queryCode);
                if (contentInfo != null)
                {
                    ScriptString = GovInteractQueryTemplate.GetCallbackScript(PublishmentSystemInfo, true, contentInfo, string.Empty);
                }
                else
                {
                    ScriptString = GovInteractQueryTemplate.GetCallbackScript(PublishmentSystemInfo, false, null, "您输入的查询号不正确");
                }
            }
            catch (Exception ex)
            {
                ScriptString = GovInteractQueryTemplate.GetCallbackScript(PublishmentSystemInfo, false, null, ex.Message);
            }
        }

        private void VoteAdd(bool isCrossDomain, bool isCorsCross, int nodeID, int contentID)
        {
            try
            {
                VoteContentInfo contentInfo = DataProvider.VoteContentDAO.GetContentInfo(PublishmentSystemInfo, contentID);
                if ((contentInfo.EndDate - DateTime.Now).Seconds <= 0)
                {
                    throw new Exception("对不起，投票已经结束");
                }
                string cookieName = DataProvider.VoteOperationDAO.GetCookieName(this.PublishmentSystemID, nodeID, contentID);
                if (CookieUtils.IsExists(cookieName))
                {
                    throw new Exception("对不起，不能重复投票");
                }

                ArrayList optionIDArrayList = TranslateUtils.StringCollectionToIntArrayList(RequestUtils.GetPostString("voteOption_" + contentID));
                foreach (int optionID in optionIDArrayList)
                {
                    DataProvider.VoteOptionDAO.AddVoteNum(optionID);
                }
                DataProvider.VoteOperationDAO.Insert(new VoteOperationInfo(0, this.PublishmentSystemID, nodeID, contentID, PageUtils.GetIPAddress(), UserManager.Current.UserName, DateTime.Now));

                ScriptString = VoteTemplate.GetCallbackScript(PublishmentSystemInfo, nodeID, contentID, true, string.Empty);
                CookieUtils.SetCookie(cookieName, true.ToString(), DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                ScriptString = VoteTemplate.GetCallbackScript(PublishmentSystemInfo, nodeID, contentID, false, ex.Message);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            string filePath = myFile.FileName;
            try
            {
                string fileExtName = PathUtils.GetExtension(filePath);
                string localDirectoryPath = PathUtility.GetUploadDirectoryPath(PublishmentSystemInfo, fileExtName);
                string localFileName = PathUtility.GetUploadFileName(PublishmentSystemInfo, filePath);

                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                if (!PathUtility.IsFileExtenstionAllowed(PublishmentSystemInfo, fileExtName))
                {
                    return string.Empty;
                }
                if (!PathUtility.IsFileSizeAllowed(PublishmentSystemInfo, myFile.ContentLength))
                {
                    return string.Empty;
                }

                myFile.SaveAs(localFilePath);
                FileUtility.AddWaterMark(PublishmentSystemInfo, localFilePath);

                fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(PublishmentSystemInfo, localFilePath);
                fileUrl = PageUtility.GetVirtualUrl(PublishmentSystemInfo, fileUrl);
            }
            catch { }

            return fileUrl;
        }

        #region add by sessionliang 20160301 输出广告html
        /// <summary>
        /// 输出广告html
        /// </summary>
        /// <param name="adAreaName"></param>
        /// <param name="templateType"></param>
        /// <param name="uniqueID"></param>
        private void AdHtml(string adAreaName, ETemplateType templateType, string uniqueID)
        {
            if (!string.IsNullOrEmpty(adAreaName) && !string.IsNullOrEmpty(templateType.ToString()))
            {
                AdvInfo advInfo = null;
                if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.ChannelTemplate || templateType == ETemplateType.ContentTemplate)
                {
                    int channelID = TranslateUtils.ToInt(RequestUtils.QueryString["channelID"]);

                    advInfo = AdvManager.GetAdvInfoByAdAreaName(templateType, adAreaName, PublishmentSystemID, channelID, 0);
                }
                else if (templateType == ETemplateType.FileTemplate)
                {
                    int fileTemplateID = TranslateUtils.ToInt(RequestUtils.QueryString["fileTemplateID"]);

                    advInfo = AdvManager.GetAdvInfoByAdAreaName(templateType, adAreaName, PublishmentSystemID, 0, fileTemplateID);
                }
                if (advInfo != null)
                {
                    ArrayList adMaterialInfoList = new ArrayList();
                    AdMaterialInfo adMaterialInfo = AdvManager.GetShowAdMaterialInfo(PublishmentSystemID, advInfo, out adMaterialInfoList);
                    if (advInfo.RotateType == EAdvRotateType.Equality || advInfo.RotateType == EAdvRotateType.HandWeight)
                    {
                        if (adMaterialInfo.AdMaterialType == EAdvType.HtmlCode)
                        {
                            ScriptString = string.Format(@"<!--
document.write('{0}');
-->
", StringUtils.ToJsString(adMaterialInfo.Code));
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Text)
                        {
                            string style = string.Empty;
                            if (!string.IsNullOrEmpty(adMaterialInfo.TextColor))
                            {
                                style += string.Format("color:{0};", adMaterialInfo.TextColor);
                            }
                            if (adMaterialInfo.TextFontSize > 0)
                            {
                                style += string.Format("font-size:{0}px;", adMaterialInfo.TextFontSize);
                            }
                            ScriptString = string.Format(@"<!--
document.write('<a href=""{0}"" target=""_blank"" style=""{1}"">{2}</a>\r\n');
-->
", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(PublishmentSystemInfo, adMaterialInfo.TextLink)), style, StringUtils.ToJsString(adMaterialInfo.TextWord));
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Image)
                        {
                            string attribute = string.Empty;
                            if (adMaterialInfo.ImageWidth > 0)
                            {
                                attribute += string.Format(@" width=""{0}""", adMaterialInfo.ImageWidth);
                            }
                            if (adMaterialInfo.ImageHeight > 0)
                            {
                                attribute += string.Format(@" height=""{0}""", adMaterialInfo.ImageHeight);
                            }
                            if (!string.IsNullOrEmpty(adMaterialInfo.ImageAlt))
                            {
                                attribute += string.Format(@" title=""{0}""", adMaterialInfo.ImageAlt);
                            }
                            ScriptString = string.Format(@"<!--
document.write('<a href=""{0}"" target=""_blank""><img src=""{1}"" {2} border=""0"" /></a>\r\n');
-->
", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(PublishmentSystemInfo, adMaterialInfo.ImageLink)), PageUtility.ParseNavigationUrl(PublishmentSystemInfo, adMaterialInfo.ImageUrl), attribute);
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Flash)
                        {
                            ScriptString = string.Format(@"<!--
document.write('<div id=""flashcontent_{0}""></div>');
var so_{0} = new SWFObject(""{1}"", ""flash_{0}"", ""{2}"", ""{3}"", ""7"", """");

so_{0}.addParam(""quality"", ""high"");
so_{0}.addParam(""wmode"", ""transparent"");

so_{0}.write(""flashcontent_{0}"");
-->
", uniqueID, PageUtility.ParseNavigationUrl(PublishmentSystemInfo, adMaterialInfo.ImageUrl), adMaterialInfo.ImageWidth.ToString(), adMaterialInfo.ImageHeight.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region by  20151127 sofuny 培生

        private object SubscribeQuery(bool isCrossDomain, bool isCorsCross)
        {
            try
            {
                ArrayList alist = DataProvider.SubscribeDAO.GetInfoList(PublishmentSystemInfo.PublishmentSystemID, string.Format(" and ParentID != 0  and Enabled='{0}' ", EBoolean.True));

                List<object> list = new List<object>();
                foreach (SubscribeInfo info in alist)
                {
                    list.Add(new { ItemID = info.ItemID, ItemName = info.ItemName });
                }

                var parameter = new { IsSuccess = true, Subscribe = list, CurrentEmail = UserManager.Current.Email, CurrentMobile = UserManager.Current.Mobile };

                return parameter;
            }
            catch (Exception ex)
            {
                return new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public object SaveSubscribeUser(bool isCrossDomain, bool isCorsCross)
        {
            try
            {
                //if (!RequestUtils.IsAnonymous)
                //{
                //    return new { IsSuccess = false, ErrorMessage = "请登录!" };
                //}
                string email = RequestUtils.GetRequestStringNoSqlAndXss("Email");
                string mobile = RequestUtils.GetRequestStringNoSqlAndXss("Mobile") == "undefined" ? "" : RequestUtils.GetRequestStringNoSqlAndXss("Mobile");
                string subscribeID = RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID");
                if (RequestUtils.GetRequestStringNoSqlAndXss("Email") == null || RequestUtils.GetRequestStringNoSqlAndXss("Email") == "undefined")
                {
                    return new { IsSuccess = false, ErrorMessage = "请输入正确的邮箱!" };
                }
                if (RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID") == null || RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID") == "undefined")
                {
                    return new { IsSuccess = false, ErrorMessage = "请选择订阅内容!" };
                }
                UserInfo info = RequestUtils.Current;
                int userID = 0;
                if (info != null)
                {
                    userID = info.UserID;
                }
                //else
                //    return new { IsSuccess = false, ErrorMessage = "请登录!" }; 


                if (DataProvider.SubscribeUserDAO.IsExists(email))
                {
                    SubscribeUserInfo subscribeUserInfo = DataProvider.SubscribeUserDAO.GetContentInfo(PublishmentSystemInfo.PublishmentSystemID, email);
                    string olduname = subscribeUserInfo.SubscribeName;
                    subscribeUserInfo.Mobile = mobile;
                    subscribeUserInfo.UserID = userID;
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = subscribeID;
                    DataProvider.SubscribeUserDAO.Update(subscribeUserInfo, olduname);
                    string message = "您的邮箱订阅成功！";
                    var parameter = new { IsSuccess = true, Message = message };

                    return parameter;
                }
                else
                {
                    SubscribeUserInfo subscribeUserInfo = new SubscribeUserInfo();
                    subscribeUserInfo.Email = email;
                    subscribeUserInfo.Mobile = mobile;
                    subscribeUserInfo.UserID = userID;
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = subscribeID;
                    subscribeUserInfo.PublishmentSystemID = PublishmentSystemInfo.PublishmentSystemID;
                    DataProvider.SubscribeUserDAO.Insert(subscribeUserInfo);
                    DataProvider.SubscribeDAO.UpdateSubscribeNum(PublishmentSystemInfo.PublishmentSystemID, subscribeID, 1);
                    sendText(PublishmentSystemInfo, subscribeUserInfo);

                    //统计顶级的数量
                    SubscribeInfo sinfo = DataProvider.SubscribeDAO.GetDefaultInfo(PublishmentSystemInfo.PublishmentSystemID);
                    DataProvider.SubscribeDAO.UpdateContentNum(PublishmentSystemInfo.PublishmentSystemID, sinfo.ItemID, DataProvider.SubscribeUserDAO.GetCount(PublishmentSystemInfo.PublishmentSystemID, string.Empty));
                    string message = "您的邮箱订阅成功！";
                    var parameter = new { IsSuccess = true, Message = message };

                    return parameter;

                }
            }
            catch (Exception ex)
            {
                return new { IsSuccess = false, ErrorMessage = "您的邮箱订阅失败：" + ex.Message };
            }
        }


        public void sendText(PublishmentSystemInfo publishmentSystemInfo, SubscribeUserInfo info)
        {

            string strBody = "【" + publishmentSystemInfo.PublishmentSystemName + "】" + "您的信息订阅成功";
            bool isSuccess = true;
            //发送订阅成功邮件
            try
            {
                ISmtpMail smtpMail = MailUtils.GetInstance();
                string[] usernames = info.Email.Split(new char[] { ',' });
                smtpMail.AddRecipient(usernames);

                smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                smtpMail.IsHtml = true;
                smtpMail.Subject = "【" + publishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";


                smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + strBody + "</pre>";
                smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                //开始发送
                string errorMessage = string.Empty;
                isSuccess = smtpMail.Send(out errorMessage);
                if (isSuccess)
                {
                    StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "订阅信息成功发送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                }
            }
            catch (Exception ex)
            {
            }

            //记录邮件发送状态
            SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
            {
                Email = info.Email,
                PushType = ESubscribePushType.ManualPush,
                SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemInfo.PublishmentSystemID, info.SubscribeName),
                SubscriptionTemplate = "订阅信息成功发送邮件",
                PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                UserName = BaiRongDataProvider.AdministratorDAO.UserName
            };
            DataProvider.SubscribePushRecordDAO.Insert(srinfo);


            //发送手机提醒
            if (!string.IsNullOrEmpty(info.Mobile))
            {
                ArrayList mobileArrayList = new ArrayList();
                mobileArrayList.Add(info.Mobile);

                if (mobileArrayList.Count > 0)
                {
                    try
                    {
                        string errorMessage = string.Empty;
                        isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);

                        if (isSuccess)
                        {
                            StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "订阅信息发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), strBody));

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //记录邮件发送状态
                srinfo = new SubscribePushRecordInfo()
                {
                    Mobile = info.Mobile,
                    PushType = ESubscribePushType.ManualPush,
                    SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemInfo.PublishmentSystemID, info.SubscribeName),
                    SubscriptionTemplate = "订阅信息成功发送手机提醒",
                    PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                    PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                    UserName = BaiRongDataProvider.AdministratorDAO.UserName
                };
                DataProvider.SubscribePushRecordDAO.Insert(srinfo);
            }
        }

        #endregion
    }
}
