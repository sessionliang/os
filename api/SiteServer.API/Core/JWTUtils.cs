using BaiRong.Core;
using BaiRong.Core.JWT;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SiteServer.API.Core
{
    public class JWTUtils
    {
        public const string JWT_HEADER = "X-S2-Access-Token";

        public static bool IsValid(ApiController controller)
        {
            JWTAdministrator administrator = JWTUtils.GetJWTAdministrator(controller);
            return administrator == null ? false : true;
        }

        public static JWTAdministrator GetJWTAdministrator(ApiController controller)
        {
            IEnumerable<string> headerValues;
            var token = string.Empty;
            var keyFound = controller.Request.Headers.TryGetValues(JWTUtils.JWT_HEADER, out headerValues);
            if (keyFound)
            {
                token = headerValues.FirstOrDefault();
                var secretKey = FileConfigManager.Instance.APISecretKey;

                try
                {
                    JWTAdministrator jwtAdministrator = JsonWebToken.DecodeToObject(token, secretKey) as JWTAdministrator;

                    if (jwtAdministrator != null && !string.IsNullOrEmpty(jwtAdministrator.userName))
                    {
                        return jwtAdministrator;
                    }
                }
                catch { }
            }

            return null;
        }
    }
}