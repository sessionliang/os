using BaiRong.Core;
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
    public class EvaluationController : ApiController
    {
        [HttpGet]
        [ActionName("GetEvaluationParameter")]
        public IHttpActionResult GetEvaluationParameter()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    int channelID = RequestUtils.GetIntQueryString("channelID");
                    int ID = RequestUtils.GetIntQueryString("ID");

                    int totalNum = DataProvider.EvaluationContentDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, ID);

                    List<EvaluationContentInfo> infoList = DataProvider.EvaluationContentDAO.GetInfoListChecked(RequestUtils.PublishmentSystemID, channelID, ID);
                    List<object> list = new List<object>();
                    int i = infoList.Count;
                    foreach (EvaluationContentInfo info in infoList)
                    {
                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));
                        int floor = i--;
                        string description = TranslateUtils.ParseCommentContent(info.Description);
                        string compositeScore = info.CompositeScore;

                        list.Add(new { ID = info.ECID, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl, Floor = floor, Description = description, CompositeScore = compositeScore });
                    }

                    bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                    User user = SiteServer.API.Model.User.GetInstance();

                    var parameter = new { IsSuccess = true, TotalNum = totalNum, IsAnonymous = isAnonymous, User = user, Evaluations = list };

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
        [ActionName("SubmitEvaluation")]
        public IHttpActionResult SubmitEvaluation()
        {
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("ID");
            string description = RequestUtils.GetQueryStringNoSqlAndXss("description");
            string compositeScore = RequestUtils.GetQueryStringNoSqlAndXss("compositeScore");

            if (UserManager.Current.UserID == 0)
                return Ok(new { IsSuccess = false, ErrorMessage = "请先登录" });

            //!RequestUtils.IsAnonymous && 评论允许匿名评论
            if (RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                    if (nodeInfo != null)
                    {
                        EvaluationContentInfo info = new EvaluationContentInfo();
                        info.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        info.NodeID = channelID;
                        info.ContentID = contentID;
                        info.AddDate = DateTime.Now;
                        info.IPAddress = PageUtils.GetIPAddress();
                        info.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                        info.IsChecked = true;
                        info.Description = description;
                        info.CompositeScore = compositeScore;

                        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(info.PublishmentSystemID, channelID);

                        InputTypeParser.AddValuesToAttributes(ETableStyle.EvaluationContent, EvaluationContentInfo.TableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.QueryString, info.Attributes, EvaluationContentAttribute.HiddenAttributes);
                         

                        int id = DataProvider.EvaluationContentDAO.Insert(info);

                        #region 添加操作记录

                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, contentID);
                        string url = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        ConsoleLogInfo clinfo = new ConsoleLogInfo();
                        clinfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        clinfo.NodeID = channelID;
                        clinfo.ContentID = contentID;
                        clinfo.TableName = EvaluationContentInfo.TableName;
                        clinfo.SourceID = id;
                        clinfo.TargetDesc = "评价";
                        clinfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                        clinfo.RedirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        clinfo.AddDate = info.AddDate;
                        clinfo.ActionType = ETableStyle.EvaluationContent.ToString();

                        DataProvider.ConsoleLogDAO.Insert(clinfo);
                        #endregion

                        string message = "评价提交成功。";

                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));
                        int floor = 1;
                        description = TranslateUtils.ParseCommentContent(info.Description);
                        compositeScore = info.CompositeScore;

                        var evaluation = new { ID = id, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl, Floor = floor, Description = description, CompositeScore = compositeScore };

                        int totalNum = DataProvider.EvaluationContentDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, contentID);

                        var parameter = new { IsSuccess = true, SuccessMessage = message, TotalNum = totalNum };

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
        /// 获取用户评价记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserEvaluationRecord")]
        public IHttpActionResult GetUserEvaluationRecord()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户  
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                string actionType = ETableStyleUtils.GetValue(ETableStyle.EvaluationContent);

                ArrayList list = DataProvider.ConsoleLogDAO.GetInfoList(actionType, userName, startdate, enddate, pageIndex, prePageNum);
                int total = DataProvider.ConsoleLogDAO.GetCount(actionType, userName);
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
            int contentID = RequestUtils.GetIntQueryString("ID");
            try
            {
                int publishmentSystemID = RequestUtils.PublishmentSystemID;
                ETableStyle tableStyle = ETableStyle.EvaluationContent;

                string userName = UserManager.Current.UserName;
                //判断当前登录人员是否已经参与评价
                bool isSubmit = DataProvider.EvaluationContentDAO.IsExists(publishmentSystemID, nodeID, contentID, userName);

                bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                User user = SiteServer.API.Model.User.GetInstance();

                ArrayList list = ContentUtils.GetFunctionFileds(publishmentSystemID, nodeID, contentID, tableStyle, EvaluationContentInfo.TableName);

                var parameter = new { IsSuccess = true, TrialapplyFiles = list, IsAnonymous = isAnonymous, User = user, IsSubmit = isSubmit };
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }


    }
}
