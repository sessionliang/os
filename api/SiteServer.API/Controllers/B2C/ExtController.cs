using BaiRong.Core;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace SiteServer.API.Controllers.B2C
{
    public class ExtController : ApiController
    {
        [HttpGet]
        [ActionName("GetGuesses")]
        public IHttpActionResult GetGuesses()
        {
            List<FollowInfo> list = new List<FollowInfo>();
            List<HistoryInfo> list1 = new List<HistoryInfo>();

            var guesses = new ArrayList();

            list = DataProviderB2C.FollowDAO.GetUserFollowsByPage(RequestUtils.CurrentUserName, 1, 20);

            foreach (var followInfo in list)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(followInfo.PublishmentSystemID);
                if (publishmentSystemInfo == null)
                    continue;

                followInfo.ContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, followInfo.ChannelID, followInfo.ContentID);
                if (followInfo.ContentInfo == null)
                    continue;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(followInfo.ContentInfo.PublishmentSystemID, followInfo.ContentInfo.NodeID);
                if (nodeInfo == null)
                    continue;
                guesses.Add(new
                {
                    navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, followInfo.ContentInfo, publishmentSystemInfo.Additional.VisualType),
                    imageurl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, followInfo.ContentInfo.ImageUrl)),
                    title = followInfo.ContentInfo.Title,
                    price = followInfo.ContentInfo.PriceSale.ToString()
                });
            }

            if (guesses.Count < 20)
            {
                list1 = DataProviderB2C.HistoryDAO.GetUserHistorysByPage(RequestUtils.CurrentUserName, 1, 20 - guesses.Count);
                foreach (var historyInfo in list1)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(historyInfo.PublishmentSystemID);
                    if (publishmentSystemInfo == null)
                        continue;
                    historyInfo.ContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, historyInfo.ChannelID, historyInfo.ContentID);
                    if (historyInfo.ContentInfo == null)
                        continue;
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(historyInfo.ContentInfo.PublishmentSystemID, historyInfo.ContentInfo.NodeID);
                    if (nodeInfo == null)
                        continue;
                    var isCUNZAI = from a in list
                                   where a.ContentID == historyInfo.ContentID && a.ChannelID == historyInfo.ChannelID
                                   select a;
                    if (isCUNZAI.Count() > 0)
                    {
                        continue;
                    }
                    guesses.Add(new
                    {
                        navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, historyInfo.ContentInfo, publishmentSystemInfo.Additional.VisualType),
                        imageurl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, historyInfo.ContentInfo.ImageUrl)),
                        title = historyInfo.ContentInfo.Title,
                        price = historyInfo.ContentInfo.PriceSale.ToString()
                    });
                }
            }
            if (guesses.Count < 20)
            {

                var list2 = StlDataUtility.GetSqlContentsDataSource(BaiRongDataProvider.ConnectionString, "select * from model_B2C_Goods where IsChecked='True' and NodeID>0", 1, 20 - guesses.Count, "AddDate DESC");


                foreach (System.Data.Common.DbDataRecord item in list2)
                {
                    var contentInfo = new GoodsContentInfo(item);
                    if (contentInfo == null)
                        continue;
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(contentInfo.PublishmentSystemID, contentInfo.NodeID);
                    if (nodeInfo == null)
                        continue;
                    var currPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID);
                    if (currPublishmentSystemInfo == null)
                        continue;

                    var isCUNZAI = from a in list
                                   where a.ContentID == contentInfo.ID && a.ChannelID == contentInfo.NodeID
                                   select a;
                    if (isCUNZAI.Count() > 0)
                    {
                        continue;
                    }

                    var isCUNZAI1 = from a in list1
                                    where a.ContentID == contentInfo.ID && a.ChannelID == contentInfo.NodeID
                                    select a;
                    if (isCUNZAI1.Count() > 0)
                    {
                        continue;
                    }

                    guesses.Add(new
                    {
                        navigationUrl = PageUtility.GetContentUrl(currPublishmentSystemInfo, contentInfo, currPublishmentSystemInfo.Additional.VisualType),
                        imageurl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(currPublishmentSystemInfo, contentInfo.ImageUrl)),
                        title = StringUtils.CutString(contentInfo.Title, 0, 10) + "...",
                        price = contentInfo.PriceSale.ToString()
                    });
                }
            }
            guesses = SJSCArrayList(guesses, 9);




            var guessesParameter = new
            {
                IsSuccess = true,
                Guesses = guesses
            };
            return Ok(guessesParameter);
        }


        private ArrayList SJSCArrayList(ArrayList list, int count)
        {
            if (list.Count > count)
            {
                Random ra = new Random();
                int r = ra.Next(list.Count - 1);
                list.RemoveAt(r);
                return SJSCArrayList(list, count);
            }
            return list;
        }
    }
}
