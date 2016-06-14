using BaiRong.Core;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
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


namespace SiteServer.API.Controllers.B2C
{
    public class B2CController : ApiController
    {
        [HttpGet]
        [ActionName("GetB2CParameter")]
        public IHttpActionResult GetB2CParameter()
        {
            List<Cart> carts = new List<Cart>();
            AmountInfo amount = new AmountInfo();
            UserSettingInfo setting = new UserSettingInfo();

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                string userName = RequestUtils.CurrentUserName;
                string sessionID = PageUtils.SessionID;

                List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(publishmentSystemInfo.PublishmentSystemID, sessionID, userName);
                List<CartInfo> cartInfoListAvaliable = new List<CartInfo>();
                foreach (CartInfo cartInfo in cartInfoList)
                {
                    GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
                    GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(cartInfo.GoodsID, contentInfo);
                    if (contentInfo == null)
                        continue;
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID);
                    if (nodeInfo == null)
                        continue;
                    if (contentInfo != null && goodsInfo != null)
                    {
                        decimal price = PriceManager.GetPrice(publishmentSystemInfo, cartInfo, false);
                        string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                        spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                        if (contentInfo != null)
                        {
                            Cart cart = new Cart { CartID = cartInfo.CartID, SN = contentInfo.SN, NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, publishmentSystemInfo.Additional.VisualType), Title = contentInfo.Title, Spec = spec, Price = price, PurchaseNum = cartInfo.PurchaseNum, ImageUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.ImageUrl), ThumbUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.ThumbUrl), Summary = contentInfo.Summary };

                            carts.Add(cart);
                            cartInfoListAvaliable.Add(cartInfo);
                        }
                    }
                }

                amount = PriceManager.GetAmountInfoByCarts(publishmentSystemInfo, cartInfoListAvaliable);

                setting = DataProviderB2C.UserSettingDAO.GetSettingInfo(userName, sessionID);
            }

            var b2cParameter = new B2CParameter { User = RequestUtils.Current, Setting = setting, IsAnonymous = RequestUtils.IsAnonymous, Carts = carts, Amount = amount };

            return Ok(b2cParameter);
        }

        [HttpGet]
        [ActionName("UpdateCart")]
        public IHttpActionResult UpdateCart()
        {
            int cartID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["cartID"]);
            int purchaseNum = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["purchaseNum"]);

            AmountInfo amount = new AmountInfo();

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                if (purchaseNum <= 0)
                {
                    purchaseNum = 1;
                }
                DataProviderB2C.CartDAO.Update(cartID, purchaseNum);

                List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(publishmentSystemInfo.PublishmentSystemID, PageUtils.SessionID, RequestUtils.CurrentUserName);

                amount = PriceManager.GetAmountInfoByCarts(publishmentSystemInfo, cartInfoList);
            }

            return Ok(amount);
        }

        [HttpGet]
        [ActionName("AddToCart")]
        public IHttpActionResult AddToCart()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                int publishmentSystemID = RequestUtils.PublishmentSystemID;
                int channelID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["channelID"]);
                int contentID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["contentID"]);
                int goodsID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["goodsID"]);
                int purchaseNum = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["purchaseNum"]);

                CartInfo cartInfo = new CartInfo(0, publishmentSystemID, RequestUtils.CurrentUserName, PageUtils.SessionID, channelID, contentID, goodsID, purchaseNum, DateTime.Now);
                DataProviderB2C.CartDAO.InsertOrUpdate(cartInfo);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("AddToFollow")]
        public IHttpActionResult AddToFollow()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                if (!RequestUtils.IsAnonymous)
                {
                    int publishmentSystemID = RequestUtils.PublishmentSystemID;
                    int channelID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["channelID"]);
                    int contentID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["contentID"]);

                    FollowInfo followInfo = new FollowInfo { PublishmentSystemID = publishmentSystemID, ChannelID = channelID, ContentID = contentID, UserName = RequestUtils.CurrentUserName, AddDate = DateTime.Now };
                    DataProviderB2C.FollowDAO.Insert(followInfo);
                }
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("DeleteCart")]
        public IHttpActionResult DeleteCart(int id)
        {
            AmountInfo amount = new AmountInfo();

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                DataProviderB2C.CartDAO.Delete(id);

                List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(publishmentSystemInfo.PublishmentSystemID, PageUtils.SessionID, RequestUtils.CurrentUserName);

                amount = PriceManager.GetAmountInfoByCarts(publishmentSystemInfo, cartInfoList);
            }

            return Ok(amount);
        }

        [HttpGet]
        [ActionName("UpdateSetting")]
        public IHttpActionResult UpdateSetting()
        {
            string province = HttpContext.Current.Request.QueryString["province"];

            var parameter = new Parameter { IsSuccess = true };

            try
            {
                string userName = UserManager.Current.UserName;
                UserSettingInfo settingInfo = new UserSettingInfo { UserName = userName, SessionID = PageUtils.SessionID, Province = province };
                DataProviderB2C.UserSettingDAO.Update(settingInfo);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("GetGuesses")]
        public IHttpActionResult GetGuesses()
        {
            var parameter = new
            {
                issuccess = true,
                guesses = ""
            };

            return Ok(parameter);
        }


        [HttpGet]
        [ActionName("AddToHistory")]
        public IHttpActionResult AddToHistory()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                if (!RequestUtils.IsAnonymous)
                {
                    int publishmentSystemID = RequestUtils.PublishmentSystemID;
                    int channelID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["channelID"]);
                    int contentID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["contentID"]);

                    HistoryInfo historyInfo = new HistoryInfo { PublishmentSystemID = publishmentSystemID, ChannelID = channelID, ContentID = contentID, UserName = RequestUtils.CurrentUserName, AddDate = DateTime.Now };
                    DataProviderB2C.HistoryDAO.Insert(historyInfo);
                }
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        /// <summary>
        /// 获取第一个B2C站点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFirstB2CPageInfo")]
        public IHttpActionResult GetFirstB2CPageInfo()
        {
            try
            {
                string userName = RequestUtils.CurrentUserName;
                string sessionID = PageUtils.SessionID;
                int publishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetQueryString("publishmentSystemID"));
                List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(sessionID, RequestUtils.CurrentUserName);

                if (publishmentSystemID == 0)
                {
                    foreach (CartInfo cartInfo in cartInfoList)
                    {
                        publishmentSystemID = cartInfo.PublishmentSystemID;
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        if (publishmentSystemInfo != null)
                        {
                            GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
                            GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(cartInfo.GoodsID, contentInfo);
                            if (contentInfo == null)
                                continue;
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID);
                            if (nodeInfo == null)
                                continue;
                            if (contentInfo != null && goodsInfo != null)
                            {
                                decimal price = PriceManager.GetPrice(publishmentSystemInfo, cartInfo, false);
                                string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                                spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                                if (contentInfo != null)
                                {
                                    return Ok(new
                                    {
                                        isSuccess = true,
                                        pageInfo = new
                                        {
                                            publishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                                            channelID = publishmentSystemInfo.PublishmentSystemID,
                                            contentID = 0,
                                            siteUrl = publishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/'),
                                            homeUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, publishmentSystemInfo.Additional.HomeUrl).TrimEnd('/'),
                                            currentUrl = "",
                                            rootUrl = PageUtils.AddProtocolToUrl(PageUtils.GetRootUrl(string.Empty)).TrimEnd('/'),
                                            apiUrl = PageUtility.IsCorsCrossDomain(publishmentSystemInfo) ? PageUtils.AddProtocolToUrl(publishmentSystemInfo.Additional.APIUrl) : PageUtils.AddProtocolToUrl(PageUtils.GetRootUrl(string.Empty)).TrimEnd('/')
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    return Ok(new
                    {
                        isSuccess = true,
                        pageInfo = new
                        {
                            publishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                            channelID = publishmentSystemInfo.PublishmentSystemID,
                            contentID = 0,
                            siteUrl = publishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/'),
                            homeUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, publishmentSystemInfo.Additional.HomeUrl).TrimEnd('/'),
                            currentUrl = "",
                            rootUrl = PageUtils.AddProtocolToUrl(PageUtils.GetRootUrl(string.Empty)).TrimEnd('/'),
                            apiUrl = PageUtility.IsCorsCrossDomain(publishmentSystemInfo) ? PageUtils.AddProtocolToUrl(publishmentSystemInfo.Additional.APIUrl) : PageUtils.AddProtocolToUrl(PageUtils.GetRootUrl(string.Empty)).TrimEnd('/')
                        }
                    });
                }

                return Ok(new { isSuccess = false, errorMessage = "出现错误！" });
            }
            catch (Exception ex)
            {
                var error = new { isSuccess = false, errorMessage = ex.Message };
                return Ok(error);
            }
        }
    }
}
