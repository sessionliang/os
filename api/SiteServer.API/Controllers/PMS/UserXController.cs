using BaiRong.Core;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using SiteServer.Project.WebAPI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.PMS
{
    public class UserXController : ApiController
    {
        //public IEnumerable<Cart> GetAllCarts()
        //{
        //    List<Cart> carts = new List<Cart>();

        //    int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["publishmentSystemID"]);
        //    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
        //    if (publishmentSystemInfo != null)
        //    {
        //        ArrayList cartInfoArrayList = DataProvider.CartDAO.GetCartInfoArrayList(publishmentSystemID, PageUtils.SessionID, BaiRongDataProvider.UserDAO.CurrentUserName);
        //        foreach (CartInfo cartInfo in cartInfoArrayList)
        //        {
        //            string tableName = NodeManager.GetTableName(publishmentSystemInfo, cartInfo.ChannelID);
        //            GoodsContentInfo contentInfo = DataProvider.GoodsContentDAO.GetContentInfo(tableName, cartInfo.ContentID);
        //            if (contentInfo != null)
        //            {
        //                decimal priceDiscount = 0;
        //                Cart cart = new Cart { CartID = cartInfo.CartID, SN = contentInfo.SN, NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo), Title = contentInfo.Title, Price = PriceManager.GetPrice(publishmentSystemInfo, contentInfo, out priceDiscount), PriceDiscount = priceDiscount, PurchaseNum = cartInfo.PurchaseNum, ThumbUrl = contentInfo.ThumbUrl, Summary = contentInfo.Summary };

        //                carts.Add(cart);
        //            }
        //        }
        //    }

        //    return carts;
        //}

        public IHttpActionResult GetUser()
        {
            UserInfo userInfo = UserManager.Current;
            var user = new User { UserName = userInfo.UserName, DisplayName = userInfo.DisplayName };
            //var user = new User { UserName = "xinxing", DisplayName = "ÐÂÐÇ" };
            return Ok(user);
        }

        //public class CartIDWithPurchaseNum
        //{
        //    public int CartID { get; set; }
        //    public int PurchaseNum { get; set; }
        //} 

        //[HttpPut]
        //public HttpResponseMessage PutCart(CartIDWithPurchaseNum[] cartIDWithPurchaseNumArray)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    if (cartIDWithPurchaseNumArray == null || cartIDWithPurchaseNumArray.Length == 0)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    try
        //    {
        //        foreach (CartIDWithPurchaseNum cartIDWithPurchaseNum in cartIDWithPurchaseNumArray)
        //        {
        //            DataProvider.CartDAO.Update(cartIDWithPurchaseNum.CartID, cartIDWithPurchaseNum.PurchaseNum);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        //public HttpResponseMessage PostCart(Cart item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CartInfo cartInfo = new CartInfo(0, item.PublishmentSystemID, BaiRongDataProvider.UserDAO.CurrentUserName, PageUtils.SessionID, item.ChannelID, item.ContentID, item.GoodsID, item.PurchaseNum, DateTime.Now);
        //        DataProvider.CartDAO.InsertOrUpdate(cartInfo);

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);
        //        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = item.CartID }));
        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}

        //public HttpResponseMessage DeleteCart(int id)
        //{
        //    try
        //    {
        //        DataProvider.CartDAO.Delete(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}
    }
}
