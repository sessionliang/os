using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
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
    public class TrialController : ApiController
    {
        #region 试用申请
        [HttpGet]
        [ActionName("GetTrialApplyParameter")]
        public IHttpActionResult GetTrialApplyParameter()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    int channelID = RequestUtils.GetIntQueryString("channelID");
                    int ID = RequestUtils.GetIntQueryString("contentID");

                    int totalNum = DataProvider.TrialApplyDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, ID);

                    List<TrialApplyInfo> infoList = DataProvider.TrialApplyDAO.GetInfoListChecked(RequestUtils.PublishmentSystemID, channelID, ID);
                    List<object> list = new List<object>();

                    foreach (TrialApplyInfo info in infoList)
                    {
                        string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));


                        list.Add(new { ID = info.TAID, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl });
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
        [ActionName("SubmitApply")]
        public IHttpActionResult SubmitApply()
        {
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("contentID");
            string name = RequestUtils.GetQueryStringNoSqlAndXss("name");
            string phone = RequestUtils.GetQueryStringNoSqlAndXss("phone");

            if (UserManager.Current.UserID == 0)
                return Ok(new { IsSuccess = false, ErrorMessage = "请先登录" });

            int publishmentSystemID = RequestUtils.PublishmentSystemID;
            string userName = UserManager.Current.UserName;

            //判断当前登录人员是否已经参与 
            bool isCompare = DataProvider.TrialApplyDAO.IsExists(publishmentSystemID, channelID, contentID, userName);
            if (isCompare)
                return Ok(new { IsSuccess = false, ErrorMessage = "您已经提交过试用，谢谢你的参与！" });

            if (RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                    if (nodeInfo != null)
                    {
                        TrialApplyInfo info = new TrialApplyInfo();
                        info.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        info.NodeID = channelID;
                        info.ContentID = contentID;
                        info.AddDate = DateTime.Now;
                        info.IPAddress = PageUtils.GetIPAddress();
                        info.UserName = UserManager.Current.UserName;
                        info.IsChecked = false;
                        info.ApplyStatus = EFunctionStatusType.NotViewed.ToString();
                        info.Name = name;
                        info.Phone = phone;

                        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(info.PublishmentSystemID, channelID);

                        InputTypeParser.AddValuesToAttributes(ETableStyle.TrialApplyContent, TrialApplyInfo.TableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.QueryString, info.Attributes, TrialApplyAttribute.HiddenAttributes);


                        int id = DataProvider.TrialApplyDAO.Insert(info);

                        #region 添加操作记录

                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, contentID);
                        string url = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        ConsoleLogInfo clinfo = new ConsoleLogInfo();
                        clinfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        clinfo.NodeID = channelID;
                        clinfo.ContentID = contentID;
                        clinfo.TableName = TrialApplyInfo.TableName;
                        clinfo.SourceID = id;
                        clinfo.TargetDesc = "试用申请";
                        clinfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                        clinfo.RedirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                        clinfo.AddDate = info.AddDate;
                        clinfo.ActionType = ETableStyle.TrialApplyContent.ToString();
                        DataProvider.ConsoleLogDAO.Insert(clinfo);

                        #endregion

                        string message = "试用申请提交成功。";

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
        [ActionName("GetUserTrialApplyRecord")]
        public IHttpActionResult GetUserTrialApplyRecord()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户  
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                string actionType = ETableStyleUtils.GetValue(ETableStyle.TrialApplyContent);

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

                    TrialApplyInfo applyInfo = DataProvider.TrialApplyDAO.GetInfo(info.SourceID);

                    bool isSubmitReport = false;
                    isSubmitReport = DataProvider.TrialReportDAO.GetReportCountByApplyID(info.PublishmentSystemID, info.NodeID, info.ContentID, info.SourceID) > 0;

                    string redirectUrl = info.RedirectUrl;
                    if (applyInfo != null)
                    {
                        var loginfo = new { ID = info.CLID, AddDate = addDate, DateTime = dateTime, UserName = userName, ContentTitle = title, RedirectUrl = redirectUrl, isReport = applyInfo.IsReport, isSubmitReport = isSubmitReport, sourceID = info.SourceID };
                        loglist.Add(loginfo);
                    }
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
            string tableStyleStr = RequestUtils.GetRequestString("tableStyle");
            try
            {

                int totalNum = DataProvider.TrialApplyDAO.GetCount(RequestUtils.PublishmentSystemID, nodeID, contentID);

                int publishmentSystemID = RequestUtils.PublishmentSystemID;
                ETableStyle tableStyle = ETableStyleUtils.GetEnumType(tableStyleStr);
                ArrayList ftsList = DataProvider.FunctionTableStylesDAO.GetInfoList(publishmentSystemID, nodeID, contentID, ETableStyle.TrialApplyContent.ToString(), "files");

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, nodeID);

                ArrayList tableStyleList = BaiRongDataProvider.TableStyleDAO.GetFunctionTableStyle(TrialApplyInfo.TableName, nodeID, publishmentSystemID, contentID, ETableStyle.TrialApplyContent.ToString());

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, TrialApplyInfo.TableName, relatedIdentities);


                ArrayList tslist = new ArrayList();
                if (ftsList.Count > 0)
                {
                    foreach (TableStyleInfo info in tableStyleList)
                    {
                        if (ftsList.Contains(info.TableStyleID))
                        {
                            tslist.Add(info);
                        }
                    }
                }
                else
                {
                    foreach (TableStyleInfo info in tableStyleInfoArrayList)
                    {
                        if (info.IsVisible)
                        {
                            tslist.Add(info);
                        }
                    }
                }
                ArrayList list = new ArrayList();
                foreach (TableStyleInfo info in tslist)
                {

                    var tsinfo = new
                    {
                        TableStyleID = info.TableStyleID,
                        AttributeName = info.AttributeName,
                        DefaultValue = info.DefaultValue,
                        DisplayName = info.DisplayName,
                        ExtendValues = info.ExtendValues,
                        HelpText = info.HelpText,
                        InputType = info.InputType.ToString(),
                        IsHorizontal = info.IsHorizontal,
                        IsSingleLine = info.IsSingleLine,
                        IsVisible = info.IsVisible,
                        IsVisibleInList = info.IsVisibleInList,
                        TableName = info.TableName,
                        Taxis = info.Taxis,
                        Additional = info.Additional,
                        StyleItems = info.StyleItems
                    };
                    list.Add(tsinfo);
                }

                bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                User user = SiteServer.API.Model.User.GetInstance();


                var parameter = new { IsSuccess = true, TotalNum = totalNum, TrialapplyFiles = list, IsAnonymous = isAnonymous, User = user };
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        #endregion

        #region 试用报告

        /// <summary>
        /// 获取用户试用申请记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserTrialReportRecord")]
        public IHttpActionResult GetUserTrialReportRecord()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户  
                string startdate = RequestUtils.GetQueryStringNoSqlAndXss("startdate");
                string enddate = RequestUtils.GetQueryStringNoSqlAndXss("enddate");
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));

                string actionType = ETableStyleUtils.GetValue(ETableStyle.TrialReportContent);

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
        /// 获取试用报告表单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTrialReportFormInfoArray")]
        public IHttpActionResult GetTrialReportFormInfoArray()
        {
            try
            {
                int trialApplyID = RequestUtils.GetIntQueryString("trialApplyID");
                TrialApplyInfo trialApplyInfo = DataProvider.TrialApplyDAO.GetInfo(trialApplyID);
                if (trialApplyInfo == null)
                {
                    var errorParm = new { IsSuccess = false, errorMessage = "申请试用不存在！" };
                    return Ok(errorParm);
                }
                int nodeID = trialApplyInfo.NodeID;
                int contentID = trialApplyInfo.ContentID;
                int publishmentSystemID = trialApplyInfo.PublishmentSystemID;
                string tableStyleStr = ETableStyleUtils.GetValue(ETableStyle.TrialReportContent);

                ETableStyle tableStyle = ETableStyleUtils.GetEnumType(tableStyleStr);
                ArrayList ftsList = DataProvider.FunctionTableStylesDAO.GetInfoList(publishmentSystemID, nodeID, contentID, ETableStyle.TrialReportContent.ToString(), "files");

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, nodeID);

                ArrayList tableStyleList = BaiRongDataProvider.TableStyleDAO.GetFunctionTableStyle(TrialReportInfo.TableName, nodeID, publishmentSystemID, contentID, ETableStyle.TrialReportContent.ToString());

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, TrialReportInfo.TableName, relatedIdentities);


                ArrayList tslist = new ArrayList();
                if (ftsList.Count > 0)
                {
                    foreach (TableStyleInfo info in tableStyleList)
                    {
                        if (ftsList.Contains(info.TableStyleID))
                        {
                            tslist.Add(info);
                        }
                    }
                }
                else
                {
                    foreach (TableStyleInfo info in tableStyleInfoArrayList)
                    {
                        if (info.IsVisible)
                        {
                            tslist.Add(info);
                        }
                    }
                }
                ArrayList list = new ArrayList();
                foreach (TableStyleInfo info in tslist)
                {

                    var tsinfo = new
                    {
                        TableStyleID = info.TableStyleID,
                        AttributeName = info.AttributeName,
                        DefaultValue = info.DefaultValue,
                        DisplayName = info.DisplayName,
                        ExtendValues = info.ExtendValues,
                        HelpText = info.HelpText,
                        InputType = info.InputType.ToString(),
                        IsHorizontal = info.IsHorizontal,
                        IsSingleLine = info.IsSingleLine,
                        IsVisible = info.IsVisible,
                        IsVisibleInList = info.IsVisibleInList,
                        TableName = info.TableName,
                        Taxis = info.Taxis,
                        Additional = info.Additional,
                        StyleItems = info.StyleItems
                    };
                    list.Add(tsinfo);
                }

                bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                User user = SiteServer.API.Model.User.GetInstance();


                var parameter = new { IsSuccess = true, styleInfoArray = tslist, IsAnonymous = isAnonymous, User = user };
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        [HttpPost]
        [ActionName("AddTrialReport")]
        public IHttpActionResult AddTrialReport()
        {
            if (UserManager.Current.UserID == 0)
                return Ok(new { IsSuccess = false, ErrorMessage = "请先登录" });
            int trialApplyID = RequestUtils.GetIntQueryString("trialApplyID");
            TrialApplyInfo trialApplyInfo = DataProvider.TrialApplyDAO.GetInfo(trialApplyID);
            if (trialApplyInfo == null)
            {
                var errorParm = new { IsSuccess = false, errorMessage = "申请试用不存在！" };
                return Ok(errorParm);
            }
            int nodeID = trialApplyInfo.NodeID;
            int contentID = trialApplyInfo.ContentID;
            int publishmentSystemID = trialApplyInfo.PublishmentSystemID;
            bool isSubmitReport = false;

            isSubmitReport = DataProvider.TrialReportDAO.GetReportCountByApplyID(publishmentSystemID, nodeID, contentID, trialApplyID) > 0;
            if (isSubmitReport)
                return Ok(new { IsSuccess = false, ErrorMessage = "已经提交过试用报告，不要重复提交" });

            if (!trialApplyInfo.IsReport)
                return Ok(new { IsSuccess = false, ErrorMessage = "该使用不需要提交试用报告" });


            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            try
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);



                if (nodeInfo != null)
                {
                    TrialReportInfo info = new TrialReportInfo();
                    info.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    info.NodeID = nodeID;
                    info.ContentID = contentID;
                    info.AddDate = DateTime.Now;
                    info.IPAddress = PageUtils.GetIPAddress();
                    info.UserName = UserManager.Current.UserName;
                    info.TAID = trialApplyID;

                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(info.PublishmentSystemID, nodeID);

                    InputTypeParser.AddValuesToAttributes(ETableStyle.TrialReportContent, TrialReportInfo.TableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, info.Attributes, TrialReportAttribute.HiddenAttributes);


                    int id = DataProvider.TrialReportDAO.Insert(info);

                    #region 添加操作记录

                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, publishmentSystemInfo.AuxiliaryTableForContent, contentID);
                    string url = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                    ConsoleLogInfo clinfo = new ConsoleLogInfo();
                    clinfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    clinfo.NodeID = nodeID;
                    clinfo.ContentID = contentID;
                    clinfo.TableName = TrialReportInfo.TableName;
                    clinfo.SourceID = id;
                    clinfo.TargetDesc = "试用报告";
                    clinfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                    clinfo.RedirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType);
                    clinfo.AddDate = info.AddDate;
                    clinfo.ActionType = ETableStyle.TrialReportContent.ToString();
                    DataProvider.ConsoleLogDAO.Insert(clinfo);

                    #endregion

                    string message = "试用报告提交成功。";

                    string addDate = DateUtils.GetRelatedDateTimeString(info.AddDate, "前");
                    string dateTime = DateUtils.GetDateAndTimeString(info.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                    string userName = string.IsNullOrEmpty(info.UserName) ? "匿名用户" : info.UserName;
                    string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, info.UserName, EAvatarSize.Small));

                    var TrialReport = new { ID = id, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl };

                    var parameter = new { IsSuccess = true, SuccessMessage = message, TrialReport = TrialReport };

                    return Ok(parameter);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
            }

            return Ok(new { IsSuccess = false });
        }
        #endregion
    }
}
