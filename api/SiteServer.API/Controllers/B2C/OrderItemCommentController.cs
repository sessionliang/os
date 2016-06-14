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
    public class OrderCommentController : ApiController
    {

        /// <summary>
        /// 获取所有的商品咨询（分页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrderCommentList")]
        public IHttpActionResult GetOrderCommentList()
        {
            #region 分页数据
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string type = RequestUtils.GetQueryString("type");
            int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
            int contentID = RequestUtils.GetIntQueryString("contentID");
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int total = 0;
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "Good")
                {
                    total = DataProviderB2C.OrderItemCommentDAO.GetCount(string.Format(" Star = 5 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID));
                }
                else if (type == "Middle")
                {
                    total = DataProviderB2C.OrderItemCommentDAO.GetCount(string.Format(" Star >= 2 AND Star <=4 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID));
                }
                else if (type == "Bad")
                {
                    total = DataProviderB2C.OrderItemCommentDAO.GetCount(string.Format(" Star = 1 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID));
                }
            }
            else
                total = DataProviderB2C.OrderItemCommentDAO.GetCount(string.Format(" PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID));
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            List<OrderItemCommentInfo> orderCommentList = new List<OrderItemCommentInfo>();
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "Good")
                {
                    orderCommentList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(string.Format(" Star = 5 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID), pageIndex, prePageNum);
                }
                else if (type == "Middle")
                {
                    orderCommentList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(string.Format(" Star >= 2 AND Star <=4 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID), pageIndex, prePageNum);
                }
                else if (type == "Bad")
                {
                    orderCommentList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(string.Format(" Star = 1 AND PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID), pageIndex, prePageNum);
                }
            }
            else
                orderCommentList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(string.Format(" PublishmentSystemID = {0} AND ChannelID = {1} AND ContentID = {2} ", publishmentSystemID, channelID, contentID), pageIndex, prePageNum);
            List<OrderItemCommentParameter> orderCommentParameterList = new List<OrderItemCommentParameter>();
            foreach (OrderItemCommentInfo orderCommentInfo in orderCommentList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                PublishmentSystemParameter publishmentSystemParameter = new PublishmentSystemParameter() { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName, PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl };

                OrderItemCommentParameter orderCommentParameterInfo = new OrderItemCommentParameter() { ID = orderCommentInfo.ID, AddDate = orderCommentInfo.AddDate, AddUser = orderCommentInfo.AddUser, Comment = orderCommentInfo.Comment, GoodCount = orderCommentInfo.GoodCount, IsAnonymous = orderCommentInfo.IsAnonymous, OrderItemID = orderCommentInfo.OrderItemID, OrderUrl = orderCommentInfo.OrderUrl, Star = orderCommentInfo.Star, Tags = orderCommentInfo.Tags };

                orderCommentParameterInfo.OrderUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, orderCommentInfo.OrderUrl));

                List<string> imageUrlList = TranslateUtils.StringCollectionToStringList(orderCommentInfo.ImageUrl);
                string imageUrlStr = string.Empty;
                foreach (string imageUrl in imageUrlList)
                {
                    imageUrlStr += PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(RequestUtils.PublishmentSystemInfo, imageUrl)) + ",";
                }
                imageUrlStr = imageUrlStr.Trim(new char[] { ',' });
                orderCommentParameterInfo.ImageUrl = imageUrlStr;

                orderCommentParameterList.Add(orderCommentParameterInfo);
            }
            var orderInfoListParms = new { PageJson = pageJson, OrderCommentList = orderCommentParameterList };
            return Ok(orderInfoListParms);
        }


        /// <summary>
        /// 获取所有的商品咨询（分页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrderCommentInfo")]
        public IHttpActionResult GetOrderCommentInfo()
        {
            try
            {
                int orderItemCommentID = RequestUtils.GetIntQueryString("orderItemCommentID");
                OrderItemCommentInfo orderItemCommentInfo = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfo(orderItemCommentID);

                List<string> imageUrlList = TranslateUtils.StringCollectionToStringList(orderItemCommentInfo.ImageUrl);
                string imageUrlStr = string.Empty;
                foreach (string imageUrl in imageUrlList)
                {
                    imageUrlStr += PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(RequestUtils.PublishmentSystemInfo, imageUrl)) + ",";
                }
                imageUrlStr = imageUrlStr.Trim(new char[] { ',' });
                orderItemCommentInfo.ImageUrl = imageUrlStr;
                var orderInfoListParms = new { isSuccess = true, orderItemCommentInfo = orderItemCommentInfo };

                return Ok(orderInfoListParms);

            }
            catch (Exception ex)
            {
                return Ok(new { isSuccess = false, errorMessage = ex.Message });
            }
        }
    }
}
