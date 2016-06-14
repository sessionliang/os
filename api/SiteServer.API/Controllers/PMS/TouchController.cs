using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
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
    public class TouchController : ApiController
    {
        public IEnumerable<TouchInfo> GetAllTouch()
        {
            int leadID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["leadID"]);
            int orderID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderID"]);

            List<TouchInfo> touchInfoList = DataProvider.TouchDAO.GetTouchInfoList(leadID, orderID);

            foreach (TouchInfo touchInfo in touchInfoList)
            {
                touchInfo.AddUserName = AdminManager.GetDisplayName(touchInfo.AddUserName, false);
            }

            return touchInfoList;
        }

        [HttpPut]
        public HttpResponseMessage PutUpdateTouch(TouchInfo item)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                item.AddUserName = AdminManager.Current.UserName;
                item.AddDate = DateTime.Now;

                DataProvider.TouchDAO.Update(item);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostAddTouch(TouchInfo item)
        {
            if (ModelState.IsValid)
            {
                item.AddUserName = AdminManager.Current.UserName;
                item.AddDate = DateTime.Now;
                item.LeadID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["leadID"]);
                item.OrderID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderID"]);

                int invoiceID = DataProvider.TouchDAO.Insert(item);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);

                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = item.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                DataProvider.TouchDAO.Delete(id);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
