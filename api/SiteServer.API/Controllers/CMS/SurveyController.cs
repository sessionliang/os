using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.API.Core;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    /// <summary>
    /// 调查问卷
    /// </summary>
    public class SurveyController : ApiController
    {
        #region 调查问卷
        [HttpGet]
        [ActionName("GetSurveyParameter")]
        public IHttpActionResult GetSurveyParameter()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    int channelID = RequestUtils.GetIntQueryString("channelID");
                    int ID = RequestUtils.GetIntQueryString("contentID");

                    int totalNum = DataProvider.SurveyQuestionnaireDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, ID);

                    ArrayList infoList = DataProvider.SurveyQuestionnaireDAO.GetInfoList(RequestUtils.PublishmentSystemID, channelID, ID);
                    List<object> list = new List<object>();

                    foreach (SurveyQuestionnaireInfo info in infoList)
                    {
                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));


                        list.Add(new { ID = info.SQID, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl });
                    }

                    bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                    User user = SiteServer.API.Model.User.GetInstance();

                    var parameter = new { IsSuccess = true, TotalNum = totalNum, IsAnonymous = isAnonymous, User = user, TrialApplys = list };

                    return Ok(parameter);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
            }

            return Ok(new { IsSuccess = false });
        }

        [HttpGet]
        [ActionName("SubmitSurveyQuestionnaire")]
        public IHttpActionResult SubmitSurveyQuestionnaire()
        {
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("contentID");

            if (UserManager.Current.UserID == 0)
                return Ok(new { IsSuccess = false, ErrorMessage = "请先登录" });

            int publishmentSystemID = RequestUtils.PublishmentSystemID;
            string userName = UserManager.Current.UserName;
            //判断当前登录人员是否已经参与 
            bool isCompare = DataProvider.SurveyQuestionnaireDAO.IsExists(publishmentSystemID, channelID, contentID, userName);
            if (isCompare)
                return Ok(new { IsSuccess = false, ErrorMessage = "您已经提交过反馈，谢谢你的参与！" });
            if (RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                    if (nodeInfo != null)
                    {
                        SurveyQuestionnaireInfo info = new SurveyQuestionnaireInfo();
                        info.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        info.NodeID = channelID;
                        info.ContentID = contentID;
                        info.AddDate = DateTime.Now;
                        info.IPAddress = PageUtils.GetIPAddress();
                        info.UserName = UserManager.Current.UserName;
                        info.SurveyStatus = false;

                        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(info.PublishmentSystemID, channelID);

                        InputTypeParser.AddValuesToAttributes(ETableStyle.SurveyContent, SurveyQuestionnaireInfo.TableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.QueryString, info.Attributes, SurveyQuestionnaireAttribute.HiddenAttributes);


                        int id = DataProvider.SurveyQuestionnaireDAO.Insert(info);

                        #region 添加操作记录

                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, contentID);
                        string url = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        ConsoleLogInfo clinfo = new ConsoleLogInfo();
                        clinfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        clinfo.NodeID = channelID;
                        clinfo.ContentID = contentID;
                        clinfo.TableName = SurveyQuestionnaireInfo.TableName;
                        clinfo.SourceID = id;
                        clinfo.TargetDesc = "调查问卷";
                        clinfo.UserName = UserManager.Current.UserName;
                        clinfo.RedirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        clinfo.AddDate = info.AddDate;
                        clinfo.ActionType = ETableStyle.SurveyContent.ToString();
                        DataProvider.ConsoleLogDAO.Insert(clinfo);

                        #endregion

                        string message = "调查问卷提交成功。";

                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));

                        var TrialApply = new { ID = id, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl };

                        var parameter = new { IsSuccess = true, SuccessMessage = message, TrialApply = TrialApply };

                        return Ok(parameter);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
                }
            }

            return Ok(new { IsSuccess = false });
        }



        private readonly Hashtable tableNameHashtable = new Hashtable();
        private readonly Hashtable tableStyleHashtable = new Hashtable();
        /// <summary>
        /// 获取用户试用申请记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserSurveyQuestionnaireRecord")]
        public IHttpActionResult GetUserSurveyQuestionnaireRecord()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户  
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                string actionType = ETableStyleUtils.GetValue(ETableStyle.SurveyContent);

                ArrayList list = DataProvider.ConsoleLogDAO.GetInfoList(actionType, userName, startdate, enddate, pageIndex, prePageNum);
                int total = list.Count;
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                ArrayList loglist = new ArrayList();
                foreach (ConsoleLogInfo info in list)
                {
                    int publishmentSystemID = info.PublishmentSystemID;
                    int nodeID = info.NodeID;
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    string tableName = tableNameHashtable[publishmentSystemID] as string;
                    if (tableName == null)
                    {
                        tableName = publishmentSystemInfo.AuxiliaryTableForContent;
                        tableNameHashtable[publishmentSystemID] = tableName;
                    }
                    ETableStyle tableStyle = ETableStyleUtils.GetEnumType(tableStyleHashtable[nodeID] as string);
                    if (ETableStyleUtils.Equals(tableStyle, ETableStyle.BackgroundContent))
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        tableNameHashtable[nodeID] = tableStyle;
                    }
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, info.ContentID);
                    string title = contentInfo.Title;

                    string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                    string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);

                    string redirectUrl = info.RedirectUrl;
                    var loginfo = new { ID = info.CLID, AddDate = addDate, DateTime = dateTime, UserName = userName, ContentTitle = title, RedirectUrl = redirectUrl };
                    loglist.Add(loginfo);
                }

                var draftListParm = new { IsSuccess = true, InfoList = loglist, PageJson = pageJson };
                return Ok(draftListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }




        /// <summary>
        /// 获取功能设置的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFunctionFiles")]
        public IHttpActionResult GetFunctionFiles()
        {
            int nodeID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("contentID");
            try
            {
                int publishmentSystemID = RequestUtils.PublishmentSystemID;
                ETableStyle tableStyle = ETableStyle.SurveyContent;

                string userName = UserManager.Current.UserName;
                //判断当前登录人员是否已经参与调查问卷
                bool isSurvey = DataProvider.SurveyQuestionnaireDAO.IsExists(publishmentSystemID, nodeID, contentID, userName);

                bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                User user = SiteServer.API.Model.User.GetInstance();

                ArrayList list = ContentUtils.GetFunctionFileds(publishmentSystemID, nodeID, contentID, tableStyle, SurveyQuestionnaireInfo.TableName);

                var parameter = new { IsSuccess = true, TrialapplyFiles = list, IsAnonymous = isAnonymous, User = user, IsSurvey = isSurvey };
                return Ok(parameter);
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
