using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using BaiRong.Core;
using KeywordInfo = SiteServer.WeiXin.Model.KeywordInfo;
using SiteServer.CMS.Model;
using System.Collections;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class GexiaMessageHandler : MessageHandler<MessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */

        private int publishmentSystemID;
        private AccountInfo accountInfo;

        public GexiaMessageHandler(AccountInfo accountInfo, Stream inputStream, int maxRecordCount = 0)
            : base(inputStream, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;

            this.publishmentSystemID = accountInfo.PublishmentSystemID;
            this.accountInfo = accountInfo;
        }

        private IResponseMessageBase GetResponseMessage(string keyword)
        {
            KeywordInfo keywordInfo = null;

            int keywordID = DataProviderWX.KeywordMatchDAO.GetKeywordIDByMPController(this.publishmentSystemID, keyword);
            if (keywordID == 0 && StringUtils.StartsWith(keyword, "GX_"))
            {
                EKeywordType keywordType = EKeywordTypeUtils.GetEnumType(keyword.Substring(3));
                keywordInfo = new KeywordInfo(0, this.publishmentSystemID, keyword, false, keywordType, EMatchType.Exact, string.Empty, DateTime.Now, 0);
            }

            if (keywordID == 0 && keywordInfo == null && this.accountInfo.IsDefaultReply && !string.IsNullOrEmpty(this.accountInfo.DefaultReplyKeyword))
            {
                keywordID = DataProviderWX.KeywordMatchDAO.GetKeywordIDByMPController(this.publishmentSystemID, this.accountInfo.DefaultReplyKeyword);
            }

            if (keywordInfo == null && keywordID > 0)
            {
                keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);
            }

            return this.GetResponseMessage(keywordInfo, keyword);
        }

        private IResponseMessageBase GetResponseMessage(KeywordInfo keywordInfo, string keyword)
        {
            try
            {
                if (keywordInfo != null && !keywordInfo.IsDisabled)
                {
                    if (keywordInfo.KeywordType == EKeywordType.Text)
                    {
                        DataProviderWX.CountDAO.AddCount(this.publishmentSystemID, ECountType.RequestText);

                        var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = keywordInfo.Reply;
                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.News)
                    {
                        DataProviderWX.CountDAO.AddCount(this.publishmentSystemID, ECountType.RequestNews);

                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();

                        SiteServer.CMS.Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

                        foreach (KeywordResourceInfo resourceInfo in DataProviderWX.KeywordResourceDAO.GetResourceInfoList(keywordInfo.KeywordID))
                        {
                            string imageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, resourceInfo.ImageUrl));

                            string pageUrl = string.Empty;
                            if (resourceInfo.ResourceType == EResourceType.Site)
                            {
                                if (resourceInfo.ChannelID > 0 && resourceInfo.ContentID > 0)
                                {
                                    pageUrl = PageUtilityWX.GetContentUrl(publishmentSystemInfo, resourceInfo.ChannelID, resourceInfo.ContentID);
                                }
                                else if (resourceInfo.ChannelID > 0)
                                {
                                    pageUrl = PageUtilityWX.GetChannelUrl(publishmentSystemInfo, resourceInfo.ChannelID);
                                }
                                else
                                {
                                    pageUrl = PageUtilityWX.GetChannelUrl(publishmentSystemInfo, publishmentSystemInfo.PublishmentSystemID);
                                }
                            }
                            else if (resourceInfo.ResourceType == EResourceType.Content)
                            {
                                pageUrl = PageUtilityWX.GetWeiXinFileUrl(publishmentSystemInfo, resourceInfo.KeywordID, resourceInfo.ResourceID);
                            }
                            else if (resourceInfo.ResourceType == EResourceType.Url)
                            {
                                pageUrl = resourceInfo.NavigationUrl;
                            }

                            responseMessage.Articles.Add(new Article()
                            {
                                Title = resourceInfo.Title,
                                Description = MPUtils.GetSummary(resourceInfo.Summary, resourceInfo.Content),
                                PicUrl = imageUrl,
                                Url = pageUrl
                            });
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Coupon)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = CouponManager.Trigger(keywordInfo, keyword, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Vote)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = VoteManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Message)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = MessageManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Appointment)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = AppointmentManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Conference)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = ConferenceManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Map)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = MapManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.View360)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = View360Manager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Album)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = AlbumManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Scratch)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = LotteryManager.Trigger(keywordInfo, ELotteryType.Scratch, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.BigWheel)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = LotteryManager.Trigger(keywordInfo, ELotteryType.BigWheel, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.GoldEgg)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = LotteryManager.Trigger(keywordInfo, ELotteryType.GoldEgg, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Flap)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = LotteryManager.Trigger(keywordInfo, ELotteryType.Flap, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.YaoYao)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = LotteryManager.Trigger(keywordInfo, ELotteryType.YaoYao, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Search)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = SearchManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Store)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = StoreManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Wifi)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = WifiManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Collect)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = CollectManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
                        if (articleList.Count > 0)
                        {
                            foreach (Article article in articleList)
                            {
                                responseMessage.Articles.Add(article);
                            }
                        }

                        return responseMessage;
                    }
                    else if (keywordInfo.KeywordType == EKeywordType.Card)
                    {
                        var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                        List<Article> articleList = CardManager.Trigger(keywordInfo, base.RequestMessage.FromUserName);
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
            catch (Exception ex)
            {
                LogUtils.AddLog(string.Empty, "Gexia Error", ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //var rm = base.CreateResponseMessage<ResponseMessageText>();
            //rm.Content = "诗水蛇山神庙";
            //return rm;

            string keyword = requestMessage.Content;
            return this.GetResponseMessage(keyword);
        }

        // <summary>
        // 处理位置请求
        // </summary>
        // <param name="requestMessage"></param>
        // <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(this.publishmentSystemID, requestMessage as RequestMessageLocation, base.WeixinOpenId);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageNews>();
        //    responseMessage.Articles.Add(new Article()
        //    {
        //        Title = "您刚才发送了图片信息",
        //        Description = "您发送的图片将会显示在边上",
        //        PicUrl = requestMessage.PicUrl,
        //        Url = "http://weixin.senparc.com"
        //    });
        //    responseMessage.Articles.Add(new Article()
        //    {
        //        Title = "第二条",
        //        Description = "第二条带连接的内容",
        //        PicUrl = requestMessage.PicUrl,
        //        Url = "http://weixin.senparc.com"
        //    });
        //    return responseMessage;
        //}

        //<summary>
        //处理语音请求
        //</summary>
        //<param name="requestMessage"></param>
        //<returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            responseMessage.Music.Title = "这里是一条音乐消息";
            responseMessage.Music.Description = "来自Jeffrey Su的美妙歌声~~";
            return responseMessage;
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
        //    return responseMessage;
        //}

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        //        {
        //            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //            responseMessage.Content = string.Format(@"您发送了一条连接信息：
        //Title：{0}
        //Description:{1}
        //Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
        //            return responseMessage;
        //        }

        //菜单点击
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            string keyword = requestMessage.EventKey;
            return this.GetResponseMessage(keyword);
        }

        //关注时触发
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            DataProviderWX.CountDAO.AddCount(this.publishmentSystemID, ECountType.UserSubscribe);

            int keywordID = 0;
            string keyword = string.Empty;
            if (this.accountInfo.IsWelcome && !string.IsNullOrEmpty(this.accountInfo.WelcomeKeyword))
            {
                keyword = this.accountInfo.WelcomeKeyword;
                keywordID = DataProviderWX.KeywordMatchDAO.GetKeywordIDByMPController(this.publishmentSystemID, keyword);
            }

            KeywordInfo keywordInfo = null;
            if (keywordID > 0)
            {
                keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);
            }

            return this.GetResponseMessage(keywordInfo, keyword);
        }

        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            DataProviderWX.CountDAO.AddCount(this.publishmentSystemID, ECountType.UserUnsubscribe);

            return base.OnEvent_UnsubscribeRequest(requestMessage);
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */

            if (this.accountInfo.IsDefaultReply && !string.IsNullOrEmpty(this.accountInfo.DefaultReplyKeyword))
            {
                string keyword = this.accountInfo.DefaultReplyKeyword;
                return this.GetResponseMessage(keyword);
            }

            return null;

            //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            //return responseMessage;
        }
    }
}
