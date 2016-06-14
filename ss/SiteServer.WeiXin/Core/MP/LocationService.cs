using System.Collections.Generic;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.GoogleMap;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Context;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.Core
{
    public class LocationService
    {
        public ResponseMessageNews GetResponseMessage(int publishmentSystemID, RequestMessageLocation requestMessage, string wxOpenID)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);

            List<Article> articleList = StoreManager.TriggerStoreItem(publishmentSystemID, requestMessage.Location_X.ToString(), requestMessage.Location_Y.ToString(), wxOpenID);

            var markersList = new List<Markers>();
            markersList.Add(new Markers()
            {
                X = requestMessage.Location_X,
                Y = requestMessage.Location_Y,
                Color = "red",
                Label = "S",
                Size = MarkerSize.Default,
            });
            var mapSize = "480x600";
            var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
                                                            markersList, mapSize);
            responseMessage.Articles.Add(new Article()
            {
                Description = string.Format("根据您的地理位置获取的附近门店。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}",
                              requestMessage.Location_X, requestMessage.Location_Y,
                              requestMessage.Scale, requestMessage.Label),
                PicUrl = articleList[0].PicUrl,
                Title = articleList[0].Title,
                Url = articleList[0].Url
            });

            if (articleList.Count > 0)
            {
                foreach (Article article in articleList)
                {

                    responseMessage.Articles.Add(article);
                }
            }

            return responseMessage;
        }
    }
}