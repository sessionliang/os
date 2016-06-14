using BaiRong.Core;
using BaiRong.Core.JWT;
using BaiRong.Model;
using SiteServer.API.Core;
using SiteServer.CMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SiteServer.API.Controllers
{
    [RoutePrefix("api/v1/administrators")]
    public class AdministratorsController : ApiController
    {
        [Route("login")]
        [HttpGet]
        public HttpResponseMessage Login()
        {
            string userName = RequestUtils.GetQueryString("userName");
            string password = RequestUtils.GetQueryString("password");
            bool rememberMe = RequestUtils.GetBoolQueryString("rememberMe");

            string token = null;
            string errorMessage = null;
            if (AdminManager.Authticate(userName, password, out errorMessage))
            {
                AdministratorInfo  adminInfo = AdminManager.GetAdminInfo(userName);
                token = JsonWebToken.Encode(new JWTAdministrator(adminInfo), FileConfigManager.Instance.APISecretKey, JwtHashAlgorithm.HS256);
                BaiRongDataProvider.AdministratorDAO.RedirectFromLoginPage(userName, rememberMe);

                var success = new { UserName = userName, Token = token };
                return this.Request.CreateResponse(HttpStatusCode.OK, success);
            }
            else
            {
                var error = new { Message = errorMessage };
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, error);
            }
        }

        [Route("{userName}")]
        [HttpGet]
        public HttpResponseMessage GetAdministrator(string userName)
        {
            if (JWTUtils.IsValid(this))
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new { UserName = userName });
            }

            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = "API 访问未被授权" });
        }
    }
}
