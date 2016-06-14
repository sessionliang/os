using BaiRong.Core;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
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
    public class RequestController : ApiController
    {
        [HttpGet]
        [ActionName("GetRequestParameter")]
        public IHttpActionResult GetRequestParameter()
        {
            RequestInfo requestInfo = DataProviderB2C.RequestDAO.GetLastRequestInfo(RequestUtils.CurrentUserName);
            RequestParameter parameter = new RequestParameter { RequestTypeList = B2CConfigurationManager.GetPublishmentSystemAdditional(RequestUtils.PublishmentSystemID).RequestTypeCollection };
            if (requestInfo != null)
            {
                parameter.Website = requestInfo.Website;
                parameter.Email = requestInfo.Email;
                parameter.Mobile = requestInfo.Mobile;
                parameter.QQ = requestInfo.QQ;
            }
            return Ok(parameter);
        }

        [HttpPost]
        [ActionName("SubmitRequest")]
        public HttpResponseMessage SubmitRequest()
        {
            if (ModelState.IsValid && !RequestUtils.IsAnonymous)
            {
                RequestInfo requestInfoToAdd = new RequestInfo(HttpContext.Current.Request.Form);
                requestInfoToAdd.SN = StringUtils.GetShortGUID(true);
                requestInfoToAdd.UserName = RequestUtils.CurrentUserName;
                requestInfoToAdd.AddDate = DateTime.Now;

                int requestID = DataProviderB2C.RequestDAO.Insert(requestInfoToAdd);

                RequestAnswerInfo answerInfoToAdd = new RequestAnswerInfo(HttpContext.Current.Request.Form);
                answerInfoToAdd.RequestID = requestID;
                answerInfoToAdd.AddDate = DateTime.Now;

                DataProviderB2C.RequestAnswerDAO.Insert(answerInfoToAdd);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = requestID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpGet]
        [ActionName("GetRequestList")]
        public IHttpActionResult GetRequestList()
        {
            List<RequestInfo> list = DataProviderB2C.RequestDAO.GetRequestInfoList(RequestUtils.CurrentUserName);
            foreach (RequestInfo requestInfo in list)
            {
                requestInfo.Status = ERequestStatusUtils.GetText(requestInfo.Status);
            }
            return Ok(list);
        }

        [HttpGet]
        [ActionName("GetRequest")]
        public IHttpActionResult GetRequest(int id)
        {
            RequestInfo requestInfo = DataProviderB2C.RequestDAO.GetRequestInfo(id);
            requestInfo.Status = ERequestStatusUtils.GetText(requestInfo.Status);

            List<RequestAnswerInfo> answerInfoList = DataProviderB2C.RequestAnswerDAO.GetAnswerInfoList(id);

            RequestResponse requestResponse = new RequestResponse { Request = requestInfo, AnswerList = answerInfoList };
            return Ok(requestResponse);
        }

        [HttpPost]
        [ActionName("SubmitAnswer")]
        public IHttpActionResult SubmitRequest(int id)
        {
            RequestAnswerInfo answerInfo = new RequestAnswerInfo(HttpContext.Current.Request.Form);
            answerInfo.RequestID = id;
            answerInfo.AddDate = DateTime.Now;
            answerInfo.IsAnswer = false;

            DataProviderB2C.RequestAnswerDAO.Insert(answerInfo);

            RequestInfo requestInfo = DataProviderB2C.RequestDAO.GetRequestInfo(id);
            requestInfo.Status = ERequestStatusUtils.GetText(requestInfo.Status);

            List<RequestAnswerInfo> answerInfoList = DataProviderB2C.RequestAnswerDAO.GetAnswerInfoList(id);

            RequestResponse requestResponse = new RequestResponse { Request = requestInfo, AnswerList = answerInfoList };
            return Ok(requestResponse);
        }

        [HttpPost]
        [ActionName("SubmitRequestEstimate")]
        public HttpResponseMessage SubmitRequestEstimate(int id)
        {
            if (ModelState.IsValid && !RequestUtils.IsAnonymous)
            {
                ERequestEstimate estimateValue = ERequestEstimateUtils.GetEnumType(HttpContext.Current.Request.Form[RequestAttribute.EstimateValue]);
                string estimateComment = HttpContext.Current.Request.Form[RequestAttribute.EstimateComment];
                DataProviderB2C.RequestDAO.Estimate(id, estimateValue, estimateComment);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
