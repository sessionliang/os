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
    /// 内容比较反馈
    /// </summary>
    public class CompareController : ApiController
    {
        #region 内容比较反馈
        [HttpGet]
        [ActionName("GetCompareParameter")]
        public IHttpActionResult GetCompareParameter()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    int channelID = RequestUtils.GetIntQueryString("channelID");
                    int ID = RequestUtils.GetIntQueryString("contentID");

                    int totalNum = DataProvider.SurveyQuestionnaireDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, ID);

                    ArrayList infoList = DataProvider.CompareContentDAO.GetInfoList(RequestUtils.PublishmentSystemID, channelID, ID);
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
        [ActionName("SubmitCompare")]
        public IHttpActionResult SubmitCompare()
        {
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("contentID");

            if (UserManager.Current.UserID == 0)
                return Ok(new { IsSuccess = false, ErrorMessage = "请先登录" });

            int publishmentSystemID = RequestUtils.PublishmentSystemID;
            string userName = UserManager.Current.UserName;
            //判断当前登录人员是否已经参与 
            bool isCompare = DataProvider.CompareContentDAO.IsExists(publishmentSystemID, channelID, contentID, userName);
           // if (isCompare)
               //return Ok(new { IsSuccess = false, ErrorMessage = "您已经提交过反馈，谢谢你的参与！" });

            if (RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                    if (nodeInfo != null)
                    {
                        CompareContentInfo info = new CompareContentInfo();
                        info.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        info.NodeID = channelID;
                        info.ContentID = contentID;
                        info.AddDate = DateTime.Now;
                        info.IPAddress = PageUtils.GetIPAddress();
                        info.UserName = UserManager.Current.UserName;
                        info.CompareStatus = false;

                        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(info.PublishmentSystemID, channelID);

                        InputTypeParser.AddValuesToAttributes(ETableStyle.CompareContent, CompareContentInfo.TableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.QueryString, info.Attributes, CompareContentAttribute.HiddenAttributes);


                        int id = DataProvider.CompareContentDAO.Insert(info);

                        #region 添加操作记录

                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, contentID);
                        string url = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        ConsoleLogInfo clinfo = new ConsoleLogInfo();
                        clinfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        clinfo.NodeID = channelID;
                        clinfo.ContentID = contentID;
                        clinfo.TableName = CompareContentInfo.TableName;
                        clinfo.SourceID = id;
                        clinfo.TargetDesc = "比较反馈";
                        clinfo.UserName = UserManager.Current.UserName;
                        clinfo.RedirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        clinfo.AddDate = info.AddDate;
                        clinfo.ActionType = ETableStyle.CompareContent.ToString();
                        DataProvider.ConsoleLogDAO.Insert(clinfo);

                        #endregion

                        string message = "提交成功。";

                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));

                        var compare = new { ID = id, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl };

                        var parameter = new { IsSuccess = true, SuccessMessage = message, Compare = compare };

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
        /// 获取用户比较反馈记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserCompareRecord")]
        public IHttpActionResult GetUserCompareRecord()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户  
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                string actionType = ETableStyleUtils.GetValue(ETableStyle.CompareContent);

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
                ETableStyle tableStyle = ETableStyle.CompareContent;

                string userName = UserManager.Current.UserName;
                //判断当前登录人员是否已经参与比较反馈
                bool isCompare = DataProvider.CompareContentDAO.IsExists(publishmentSystemID, nodeID, contentID, userName);

                bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                User user = SiteServer.API.Model.User.GetInstance();

                ArrayList list = ContentUtils.GetFunctionFileds(publishmentSystemID, nodeID, contentID, tableStyle, CompareContentInfo.TableName);

                var parameter = new { IsSuccess = true, TrialapplyFiles = list, IsAnonymous = isAnonymous, User = user, IsCompare = isCompare };
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
