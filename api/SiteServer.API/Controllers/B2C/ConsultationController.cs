using BaiRong.Core;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.B2C
{
    public class ConsultationController : ApiController
    {
        /// <summary>
        /// 获取该用户所有的商品咨询（分页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAllConsultationList")]
        public IHttpActionResult GetAllConsultationList()
        {
            #region 分页数据
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string keywords = RequestUtils.GetQueryString("keywords");
            int total = DataProviderB2C.ConsultationDAO.GetCountByUser(RequestUtils.CurrentUserName, keywords);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            List<ConsultationInfo> consultationList = DataProviderB2C.ConsultationDAO.GetConsultationInfoList(RequestUtils.CurrentUserName, keywords, pageIndex, prePageNum);
            List<ConsultationParameter> consultationParameterList = new List<ConsultationParameter>();
            foreach (ConsultationInfo consultationInfo in consultationList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(consultationInfo.PublishmentSystemID);
                PublishmentSystemParameter publishmentSystemParameter = new PublishmentSystemParameter() { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName, PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl };
                ConsultationParameter consultationParameterInfo = new ConsultationParameter() { AddDate = consultationInfo.AddDate, AddUser = consultationInfo.AddUser, Answer = consultationInfo.Answer, ContentID = consultationInfo.ContentID, GoodsID = consultationInfo.ChannelID, PublishmentSystemInfo = publishmentSystemParameter, Question = consultationInfo.Question, ReplyDate = consultationInfo.ReplyDate, Title = consultationInfo.Title, Type = consultationInfo.Type, IsReply = consultationInfo.IsReply };

                consultationParameterInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, consultationInfo.ChannelID), consultationInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                consultationParameterInfo.ThumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, consultationInfo.ThumbUrl));
                consultationParameterList.Add(consultationParameterInfo);
            }
            var orderInfoListParms = new { PageJson = pageJson, ConsultationList = consultationParameterList };
            return Ok(orderInfoListParms);
        }

        /// <summary>
        /// 获取所有已经恢复的商品咨询（分页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetConsultationList")]
        public IHttpActionResult GetConsultationList()
        {
            #region 分页数据
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string keywords = RequestUtils.GetQueryString("keywords");
            string type = RequestUtils.GetQueryString("type");
            int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
            int contentID = RequestUtils.GetIntQueryString("contentID");
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int total = 0;
            if (!string.IsNullOrEmpty(type))
                total = DataProviderB2C.ConsultationDAO.GetCount(string.Format(" (Title like '%{0}%' OR Question like '%{0}%' OR Answer like '%{0}%') AND Type = '{1}' AND PublishmentSystemID = {2} AND ChannelID = {3} AND ContentID = {4} AND IsReply = '{5}' ", PageUtils.FilterSql(keywords), EConsultationTypeUtils.GetValue(EConsultationTypeUtils.GetEnumType(type)), publishmentSystemID, channelID, contentID, true.ToString()));
            else
                total = DataProviderB2C.ConsultationDAO.GetCount(string.Format(" (Title like '%{0}%' OR Question like '%{0}%' OR Answer like '%{0}%') AND PublishmentSystemID = {1} AND ChannelID = {2} AND ContentID = {3} AND IsReply = '{4}' ", PageUtils.FilterSql(keywords), publishmentSystemID, channelID, contentID, true.ToString()));
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            List<ConsultationInfo> consultationList = new List<ConsultationInfo>();
            if (!string.IsNullOrEmpty(type))
                consultationList = DataProviderB2C.ConsultationDAO.GetConsultationInfoList(string.Format(" (Title like '%{0}%' OR Question like '%{0}%' OR Answer like '%{0}%') AND Type = '{1}' AND PublishmentSystemID = {2} AND ChannelID = {3} AND ContentID = {4}  AND IsReply = '{5}'  ", PageUtils.FilterSql(keywords), EConsultationTypeUtils.GetValue(EConsultationTypeUtils.GetEnumType(type)), publishmentSystemID, channelID, contentID, true.ToString()), pageIndex, prePageNum);
            else
                consultationList = DataProviderB2C.ConsultationDAO.GetConsultationInfoList(string.Format(" (Title like '%{0}%' OR Question like '%{0}%' OR Answer like '%{0}%')  AND PublishmentSystemID = {1} AND ChannelID = {2} AND ContentID = {3}  AND IsReply = '{4}'  ", PageUtils.FilterSql(keywords), publishmentSystemID, channelID, contentID, true.ToString()), pageIndex, prePageNum);
            List<ConsultationParameter> consultationParameterList = new List<ConsultationParameter>();
            foreach (ConsultationInfo consultationInfo in consultationList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(consultationInfo.PublishmentSystemID);
                PublishmentSystemParameter publishmentSystemParameter = new PublishmentSystemParameter() { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName, PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl };
                ConsultationParameter consultationParameterInfo = new ConsultationParameter() { AddDate = consultationInfo.AddDate, AddUser = consultationInfo.AddUser, Answer = consultationInfo.Answer, ContentID = consultationInfo.ContentID, GoodsID = consultationInfo.ChannelID, PublishmentSystemInfo = publishmentSystemParameter, Question = consultationInfo.Question, ReplyDate = consultationInfo.ReplyDate, Title = consultationInfo.Title, Type = consultationInfo.Type, IsReply = consultationInfo.IsReply };

                consultationParameterInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, consultationInfo.ChannelID), consultationInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                consultationParameterInfo.ThumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, consultationInfo.ThumbUrl));
                consultationParameterList.Add(consultationParameterInfo);
            }
            var orderInfoListParms = new { PageJson = pageJson, ConsultationList = consultationParameterList };
            return Ok(orderInfoListParms);
        }

        /// <summary>
        /// 提交商品咨询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SaveConsultation")]
        public IHttpActionResult SaveConsultation()
        {
            try
            {
                int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
                int contentID = RequestUtils.GetIntQueryString("contentID");
                int channelID = RequestUtils.GetIntQueryString("channelID");
                string type = RequestUtils.GetQueryString("type");
                string question = RequestUtils.GetQueryString("question");
                ConsultationInfo consultation = new ConsultationInfo();
                consultation.AddDate = DateTime.Now;
                consultation.AddUser = RequestUtils.CurrentUserName;
                consultation.ContentID = contentID;
                consultation.ChannelID = channelID;
                consultation.PublishmentSystemID = publishmentSystemID;
                consultation.Question = question;
                consultation.IsReply = false;
                consultation.Type = type;
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                GoodsContentInfo goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, channelID, contentID);

                consultation.ThumbUrl = goodsContentInfo.ThumbUrl;
                consultation.Title = goodsContentInfo.Title;

                DataProviderB2C.ConsultationDAO.Insert(consultation);
                return Ok(new { isSuccess = true });
            }
            catch (Exception)
            {
                return Ok(new { isSuccess = false });
            }

        }
    }
}
