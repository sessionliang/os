using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Drawing;
using BaiRong.Model;
using NetDimension.Weibo;
using Newtonsoft.Json;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;

namespace SiteServer.API.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [ActionName("GetUser")]
        public IHttpActionResult GetUser()
        {
            User user = SiteServer.API.Model.User.GetInstance();

            var userParameter = new UserParameter { User = user, IsAnonymous = RequestUtils.IsAnonymous };

            return Ok(userParameter);
        }

        [HttpGet]
        [ActionName("Logout")]
        public IHttpActionResult Logout()
        {
            BaiRongDataProvider.UserDAO.Logout();
            Parameter parameter = new Parameter { IsSuccess = true };
            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("Login")]
        public IHttpActionResult Login()
        {
            string loginName = RequestUtils.GetQueryString("loginName");
            string password = RequestUtils.GetQueryString("password");
            bool isPersistent = RequestUtils.GetBoolQueryString("isPersistent");

            bool useBase64 = RequestUtils.GetBoolQueryString("useBase64");

            //base64解码
            if (useBase64)
            {
                try
                {
                    byte[] data = Base64Decoder.Decoder.GetDecoded(password);
                    password = Encoding.GetEncoding("utf-8").GetString(data);
                }
                catch
                {
                }
            }



            string userName = string.Empty;
            string errorMessage = string.Empty;

            string groupSN = "";
            bool isSuccess = BaiRongDataProvider.UserDAO.ValidateByLoginName(groupSN, loginName, password, out userName, out errorMessage);
            if (isSuccess)
            {
                BaiRongDataProvider.UserDAO.Login(groupSN, userName, isPersistent);

                if (UserConfigManager.Additional.IsRecordIP)
                {
                    //记录登录记录
                    UserLogInfo logInfo = new UserLogInfo(0, userName, PageUtils.GetIPAddress(), DateTime.Now, EUserActionTypeUtils.GetValue(EUserActionType.Login), EUserActionTypeUtils.GetText(EUserActionType.Login));
                    BaiRongDataProvider.UserLogDAO.Insert(logInfo);
                    //登录记录
                    UserInfo userInfo = UserManager.GetUserInfo(string.Empty, userName);
                    userInfo.LoginNum++;
                    BaiRongDataProvider.UserDAO.Update(userInfo);
                }

            }
            else
            {
                errorMessage = "登录失败，" + errorMessage;
            }

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };
            return Ok(retval);
        }

        #region Old Method Register,Without ValidCode
        [HttpGet]
        [ActionName("Register")]
        public IHttpActionResult Register()
        {
            string loginName = RequestUtils.GetQueryString("loginName");
            string password = RequestUtils.GetQueryString("password");
            string email = RequestUtils.GetQueryString("email");
            string mobile = RequestUtils.GetQueryString("mobile");
            string waitUrl = RequestUtils.GetQueryString("returnUrl");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            bool isRedirectToLogin = false;
            string groupSN = "";
            string homeUrl = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            homeUrl = publishmentSystemInfo != null ? publishmentSystemInfo.PublishmentSystemUrl : string.Empty;

            bool isSuccess = false;

            isSuccess = UserManager.RegisterByUserController(groupSN, loginName, password, email, mobile, homeUrl, out isRedirectToLogin, out successMessage, out errorMessage);
            if (isSuccess)
            {
                UserInfo user = BaiRongDataProvider.UserDAO.GetUserInfo(groupSN, loginName);
                if (user.IsChecked)
                {
                    BaiRongDataProvider.UserDAO.Login(groupSN, loginName, false);
                }
                else
                {
                    isSuccess = true;
                    successMessage = "用户已经注册成功!邮箱验证或者手机验证或者管理员审核之后，进行登录";
                }

            }

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, IsRedirectToLogin = isRedirectToLogin, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }
        #endregion


        #region New Method Register, With ValidCode
        [HttpGet]
        [ActionName("RegisterWithValidCode")]
        public IHttpActionResult RegisterNew()
        {
            string loginName = RequestUtils.GetQueryString("loginName");
            string password = RequestUtils.GetQueryString("password");
            string email = RequestUtils.GetQueryString("email");
            string mobile = RequestUtils.GetQueryString("mobile");
            string validCode = RequestUtils.GetQueryString("validCode");
            string waitUrl = RequestUtils.GetQueryString("returnUrl");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            bool isRedirectToLogin = false;
            string groupSN = "";
            string homeUrl = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            homeUrl = publishmentSystemInfo != null ? publishmentSystemInfo.PublishmentSystemUrl : string.Empty;
            //homeUrl = string.Format("index={0}&wait={1}", homeUrl, waitUrl);

            bool isSuccess = false;

            isSuccess = UserManager.RegisterByUserApi(groupSN, loginName, password, email, mobile, validCode, homeUrl, waitUrl, out isRedirectToLogin, out successMessage, out errorMessage);
            if (isSuccess)
            {
                UserInfo user = BaiRongDataProvider.UserDAO.GetUserInfo(groupSN, loginName);
                if (user.IsChecked)
                {
                    BaiRongDataProvider.UserDAO.Login(groupSN, loginName, false);
                }
                else
                {
                    isSuccess = true;
                    successMessage = "用户已经注册成功!验证通过之后，即可登录";
                }

            }

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, IsRedirectToLogin = isRedirectToLogin, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }
        #endregion


        [HttpGet]
        [ActionName("UpdateUserInfo")]
        public IHttpActionResult UpdateUserInfo()
        {
            string userName = RequestUtils.GetQueryString("userName");
            string email = RequestUtils.GetQueryString("email");
            string mobile = RequestUtils.GetQueryString("mobile");
            string signature = RequestUtils.GetQueryString("signature");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string groupSN = "";
            bool isSuccess = UserManager.UpdateUserInfo(groupSN, userName, email, mobile, signature, out successMessage, out errorMessage);

            if (isSuccess)
                UserManager.RemoveCache(false, string.Empty, userName);
            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }


        [HttpGet]
        [ActionName("UpdateBasicUserInfo")]
        public IHttpActionResult UpdateBasicUserInfo()
        {
            string userName = RequestUtils.GetQueryString("userName");
            //string email = RequestUtils.GetQueryString("email");
            //string mobile = RequestUtils.GetQueryString("mobile");
            string signature = RequestUtils.GetQueryString("signature");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string groupSN = "";
            bool isSuccess = UserManager.UpdateUserBasicInfo(groupSN, userName, signature, out successMessage, out errorMessage);

            if (isSuccess)
                UserManager.RemoveCache(false, string.Empty, userName);

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }

        [HttpGet]
        [ActionName("UpdateDetailUserInfo")]
        public IHttpActionResult UpdateDetailUserInfo()
        {
            string bloodType = RequestUtils.GetQueryString("bloodType");
            int height = RequestUtils.GetIntQueryString("height");
            string maritalStatus = RequestUtils.GetQueryString("maritalStatus");
            string education = RequestUtils.GetQueryString("education");
            string provinceValue = RequestUtils.GetQueryString("provinceValue");
            string address = RequestUtils.GetQueryString("address");
            string QQ = RequestUtils.GetQueryString("QQ");
            string WeiBo = RequestUtils.GetQueryString("WeiBo");
            string WeiXin = RequestUtils.GetQueryString("WeiXin");
            string gender = RequestUtils.GetQueryString("gender");
            string organization = RequestUtils.GetQueryString("organization");
            string department = RequestUtils.GetQueryString("department");
            string position = RequestUtils.GetQueryString("position");
            string interects = RequestUtils.GetQueryString("interects");
            string graduation = RequestUtils.GetQueryString("graduation");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string groupSN = "";
            bool isSuccess = UserManager.UpdateUserInfo(groupSN, bloodType, height, maritalStatus, education, graduation, provinceValue, address, QQ, WeiBo, WeiXin, gender, organization, department, position, interects, out successMessage, out errorMessage);

            if (isSuccess)
                UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }

        [HttpPost]
        [ActionName("UpdateAutoDetailUserInfo")]
        public IHttpActionResult UpdateAutoDetailUserInfo()
        {
            try
            {
                string formStr = RequestUtils.GetPostStringNoXss("form");
                NameValueCollection kv = TranslateUtils.ToNameValueCollection(formStr);

                string successMessage = string.Empty;
                string errorMessage = string.Empty;

                TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, null, kv, UserManager.Current.Attributes);
                BaiRongDataProvider.UserDAO.Update(UserManager.Current);
                UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);

                LoginParameter retval = new LoginParameter { IsSuccess = true, SuccessMessage = successMessage, ErrorMessage = errorMessage };
                return Ok(retval);
            }
            catch (Exception ex)
            {
                LoginParameter retval = new LoginParameter { IsSuccess = true, SuccessMessage = string.Empty, ErrorMessage = ex.Message };
                return Ok(retval);
            }

        }

        [HttpGet]
        [ActionName("ForgetPassword")]
        public IHttpActionResult ForgetPassword()
        {
            string loginName = RequestUtils.GetQueryString("loginName");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            string groupSN = string.Empty;
            if (RequestUtils.PublishmentSystemInfo != null)
                groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
            bool isSuccess = UserManager.ForgetPassword(groupSN, loginName, out successMessage, out errorMessage);

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }

        [HttpGet]
        [ActionName("ChangePassword")]
        public IHttpActionResult ChangePassword()
        {
            string currentPassword = RequestUtils.GetQueryStringNoSqlAndXss("currentPassword");
            string newPassword = RequestUtils.GetQueryStringNoSqlAndXss("newPassword");

            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            bool isSuccess = false;

            if (RequestUtils.IsAnonymous)
            {
                errorMessage = "对不起，登录已超时，请重新登录系统";
            }
            else if (BaiRongDataProvider.UserDAO.Validate("", RequestUtils.CurrentUserName, currentPassword, out errorMessage))
            {
                int userID = BaiRongDataProvider.UserDAO.GetUserID("", RequestUtils.CurrentUserName);
                isSuccess = BaiRongDataProvider.UserDAO.ChangePassword(userID, newPassword);

                if (isSuccess)
                    UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);
            }

            if (!isSuccess && string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "对不起，您输入的密码有误，请重新输入";
            }

            LoginParameter retval = new LoginParameter { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
            return Ok(retval);
        }

        #region Old Method ThirdLogin, With PublishmentSystem

        [HttpGet]
        [ActionName("GetThirdLoginTypeParameter")]
        public IHttpActionResult GetThirdLoginTypeParameter()
        {
            List<BaiRongThirdLoginInfo> thirdLogins = new List<BaiRongThirdLoginInfo>();
            BaiRongThirdLoginInfo thirdLogin = null;
            //PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            //if (publishmentSystemInfo != null)
            //{
            List<BaiRongThirdLoginInfo> thirdLoginInfoList = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfoList();
            foreach (BaiRongThirdLoginInfo thirdLoginInfo in thirdLoginInfoList)
            {
                if (thirdLoginInfo.IsEnabled)
                {
                    thirdLogins.Add(thirdLoginInfo);
                    if (thirdLoginInfo != null)
                    {
                        thirdLogin = thirdLoginInfo;
                    }
                }
            }
            if (thirdLogin == null && thirdLogins.Count > 0)
            {
                thirdLogin = thirdLogins[0];
            }

            //当前登录用户已经绑定的第三方登录
            List<int> bindedThirdLogins = new List<int>();
            if (!RequestUtils.IsAnonymous)
            {
                foreach (BaiRongThirdLoginInfo thirdLoginInfo in thirdLoginInfoList)
                {
                    if (thirdLoginInfo.IsEnabled)
                    {
                        string thirdLoginID = BaiRongDataProvider.UserBindingDAO.GetUserBindByUserAndType(UserManager.Current.UserID, thirdLoginInfo.ThirdLoginType.ToString());
                        if (!string.IsNullOrEmpty(thirdLoginID))
                            bindedThirdLogins.Add((int)thirdLoginInfo.ThirdLoginType);
                    }
                }
            }

            var thirdLoginParameter = new ThirdLogin { IsSucess = true, ThirdLoginList = thirdLogins, BindedThirdLoginList = bindedThirdLogins };
            return Ok(thirdLoginParameter);
            //}
            //else
            //{
            //    var thirdLoginParameter = new ThirdLogin { IsSucess = false, ThirdLoginList = null };
            //    return Ok(thirdLoginParameter);
            //}
        }

        // 第三方登陆 begin wujiaqiang

        public BaiRongThirdLoginInfo GetThirdLoginInfo(ESiteserverThirdLoginType siteserverThirdLoginType, int publishmentSystemID)
        {

            List<BaiRongThirdLoginInfo> thirdLoginInfoList = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfoList();
            foreach (BaiRongThirdLoginInfo thirdLoginInfo in thirdLoginInfoList)
            {
                if (thirdLoginInfo.IsEnabled && thirdLoginInfo.ThirdLoginType == siteserverThirdLoginType)
                {
                    return thirdLoginInfo;
                }
            }

            return null;
        }

        #region 第三方登录 腾讯QQ和新浪weibo

        [HttpGet]
        [ActionName("SdkLogin")]
        public IHttpActionResult SdkLogin()
        {
            int loginType = RequestUtils.GetIntQueryString("sdkType");

            string returnUrl = RequestUtils.GetQueryString("returnUrl");

            string login_url = "";
            string indexPageUrl = "";
            string publishmentSystemName = string.Empty;
            string groupSN = string.Empty;
            int publishmentSystemID = 0;
            if (RequestUtils.PublishmentSystemInfo != null)
            {
                indexPageUrl = PageUtility.GetIndexPageUrl(RequestUtils.PublishmentSystemInfo, EVisualType.Static);
                publishmentSystemName = RequestUtils.PublishmentSystemInfo.PublishmentSystemName;
                publishmentSystemID = RequestUtils.PublishmentSystemInfo.PublishmentSystemID;
                groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
            }


            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.PublishmentSystemID", publishmentSystemID.ToString());
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.IndexPageUrl", returnUrl);
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.GroupSN", groupSN);
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.PublishmentSystemName", publishmentSystemName);


            if (loginType == 1)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.QQ, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl;
                string state = Guid.NewGuid().ToString().Replace("-", "");
                login_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + clientID + "&state=" + state + "&redirect_uri=" + redirectUri + "&scope=get_user_info,get_info,get_other_info";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.state" + clientID, state);
            }

            if (loginType == 2)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.Weibo, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl;
                string responseType = "code";
                login_url = "https://api.weibo.com/oauth2/authorize?client_id=" + clientID + "&response_type=" + responseType + "&redirect_uri=" + redirectUri + "";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
            }

            if (loginType == 3)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.WeixinPC, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl;
                string scope = "snsapi_login";
                login_url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + clientID + "&redirect_uri=" + redirectUri + "&response_type=code&scope=" + scope + "&state=STATE#wechat_redirect";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
            }

            return Ok(login_url);
        }

        [HttpGet]
        [ActionName("AuthLogin")]
        public IHttpActionResult AuthLogin()
        {
            bool isSuccess = false;
            string indexPageUrl = DbCacheManager.Get("SiteServer.API.Controllers.UserController.IndexPageUrl");
            string responseGroupSN = DbCacheManager.Get("SiteServer.API.Controllers.UserController.GroupSN");
            string responsePublishmentSystemName = DbCacheManager.Get("SiteServer.API.Controllers.UserController.PublishmentSystemName");
            int responsePublishmentSystemID = TranslateUtils.ToInt(DbCacheManager.Get("SiteServer.API.Controllers.UserController.PublishmentSystemID"));
            int loginType = TranslateUtils.ToInt(DbCacheManager.Get("SiteServer.API.Controllers.UserController.loginType"));
            ThirdLoginParameter thirdLoginParameter = new ThirdLoginParameter();
            if (loginType == 1)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.QQ, responsePublishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientid = thirdLoginAuthInfo.AppKey;
                string clientsecret = thirdLoginAuthInfo.AppSercet;
                string redirecturi = thirdLoginAuthInfo.CallBackUrl;

                string send_url = "";//临时变量，发送URL,接受返回结果 
                string rezult = ""; //用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
                string state = "";
                string code = "";//临时Authorization Code，官方提示10分钟过期
                string access_token = "";//通过Authorization Code返回结果获取到的Access Token
                string expires_in = ""; //expires_in是该Access Token的有效期，单位为秒
                string new_client_id = "";//通过Access Token返回来的client_id 
                string openid = ""; //通过Access Token返回来的openid，QQ用户唯一值，可以与网站用户数据关联

                state = RequestUtils.GetQueryString("state");

                bool isExistsState = DbCacheManager.IsExists("SiteServer.API.Controllers.UserController.state" + clientid);

                if (!isExistsState)
                {
                    HttpContext.Current.Response.Write("state未初始化");
                    HttpContext.Current.Response.End();
                }

                //如果返回state与之前发出的判断正确
                if (state == DbCacheManager.Get("SiteServer.API.Controllers.UserController.state" + clientid))
                {
                    code = RequestUtils.GetQueryString("code");
                    //==============通过Authorization Code和基本资料获取Access Token=================
                    send_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + clientid + "&client_secret=" + clientsecret + "&code=" + code + "&state=" + state + "&redirect_uri=" + redirecturi;

                    //发送并接受返回值
                    rezult = HttpGet(send_url);
                    //如果失败
                    if (rezult.Contains("error"))
                    {
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        //======================通过Access Token来获取用户的OpenID==============
                        string[] parm = rezult.Split('&');
                        access_token = parm[0].Split('=')[1];//取得 access_token
                        expires_in = parm[1].Split('=')[1];//取得 过期时间
                        send_url = "https://graph.qq.com/oauth2.0/me?access_token=" + access_token;  //拼接url
                        rezult = HttpGet(send_url);//发送并接受返回值
                        //如果失败
                        if (rezult.Contains("error"))
                        {
                            HttpContext.Current.Response.End();
                        }

                        //取得文字出现
                        int str_start = rezult.IndexOf('(') + 1;
                        int str_last = rezult.LastIndexOf(')') - 1;

                        //取得JSON字符串
                        rezult = rezult.Substring(str_start, (str_last - str_start));

                        //反序列化JSON
                        Dictionary<string, string> _dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);


                        //取值
                        _dic.TryGetValue("client_id", out new_client_id);
                        _dic.TryGetValue("openid", out openid);

                        //储存获取数据用到的信息
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.access_token" + clientid, access_token);
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.clientid" + clientid, clientid);
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.openid" + clientid, openid);

                        send_url = "https://graph.qq.com/user/get_user_info?access_token=" + access_token + "&oauth_consumer_key=" + clientid + "&openid=" + openid;

                        //发送并接受返回值
                        rezult = HttpGet(send_url);

                        //反序列化JSON
                        Dictionary<string, string> _dic2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);

                        string ret = "", msg = "", nickname = "", face = "", face1 = "", sex = "", vip_level = "", qzone_level = "";

                        //取值
                        _dic2.TryGetValue("ret", out ret);
                        _dic2.TryGetValue("msg", out msg);

                        //如果失败
                        if (ret != "0")
                        {
                            HttpContext.Current.Response.End();
                        }

                        _dic2.TryGetValue("nickname", out nickname);
                        _dic2.TryGetValue("figureurl_qq_1", out face);
                        _dic2.TryGetValue("figureurl_qq_2", out face1);
                        _dic2.TryGetValue("gender", out sex);
                        _dic2.TryGetValue("vip", out vip_level);
                        _dic2.TryGetValue("level", out qzone_level);

                        //对nickname的特殊字符做处理
                        nickname = StringUtils.ReplaceInvalidChar(nickname);

                        string groupSN = responseGroupSN;
                        string successMessage = string.Empty;
                        string errorMessage = string.Empty;
                        string userName = string.Empty;
                        bool isRedirectToLogin = false;

                        int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(openid);

                        string oldUserName = UserManager.GetUserInfo(groupSN, nickname).UserName;
                        if (oldUserName != "")
                        {
                            nickname = nickname + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 4);
                        }

                        bool IsSuccess = true;
                        string SiteName = responsePublishmentSystemName;
                        string ThirdLoginType = ESiteserverThirdLoginType.QQ.ToString();
                        string IndexPageUrl = indexPageUrl;
                        string ThirdLoginNickName = nickname;
                        string ThirdLoginPassword = "123456";
                        string ThirdLoginUserHeadUrl = face;
                        string SuccessMessage = "第三方授权成功!";
                        string ErrorMessage = "第三方授权失败!";

                        thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };

                        if (userBindingCount > 0)
                        {
                            UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);
                            if (info != null)
                            {
                                thirdLoginParameter.ThirdLoginNickName = UserManager.GetUserName(userBindingCount);
                                thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(info.UserID);
                                return Ok(thirdLoginParameter);
                            }
                            else
                            {
                                BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userBindingCount, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.QQ));
                            }
                        }
                        //UserInfo current = UserManager.Current;
                        //if (current == null || current.UserID == 0)
                        //{
                        string topEmial = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                        isSuccess = UserManager.RegisterByUserController(groupSN, nickname, "123456", "" + topEmial + "@qq.com", "", string.Empty, out isRedirectToLogin, out successMessage, out errorMessage);
                        if (!isSuccess)
                        {
                            thirdLoginParameter.IsSuccess = false;
                            thirdLoginParameter.ErrorMessage = errorMessage;
                        }
                        int userID = UserManager.GetUserInfo(groupSN, nickname).UserID;
                        UserInfo userInfo = new UserInfo();
                        userInfo.UserID = userID;
                        userInfo.AvatarLarge = face1;
                        userInfo.AvatarMiddle = face1;
                        userInfo.AvatarSmall = face;
                        userInfo.IsChecked = true;
                        BaiRongDataProvider.UserDAO.Update(userInfo);
                        UserManager.RemoveCache(false, string.Empty, userName);
                        //}
                        //else
                        //{
                        //    userID = current.UserID;
                        //}

                        if (userID > 0)
                        {
                            BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(userID, ESiteserverThirdLoginType.QQ.ToString(), openid);
                        }
                        return Ok(thirdLoginParameter);
                    }
                }
            }

            if (loginType == 2)
            {

                BaiRongThirdLoginInfo newThirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.Weibo, responsePublishmentSystemID);
                ThirdLoginAuthInfo newThirdLoginAuthInfo = new ThirdLoginAuthInfo(newThirdLoginInfo.SettingsXML);
                string clientid = newThirdLoginAuthInfo.AppKey;
                string clientsecret = newThirdLoginAuthInfo.AppSercet;
                string redirecturi = newThirdLoginAuthInfo.CallBackUrl;
                string code = RequestUtils.GetQueryString("code");

                NetDimension.Weibo.OAuth oauth = new NetDimension.Weibo.OAuth(clientid, clientsecret, redirecturi);

                AccessToken accessToken = oauth.GetAccessTokenByAuthorizationCode(code); //请注意这里返回的是AccessToken对象，不是string
                string accessTokenString = oauth.AccessToken;
                oauth = new NetDimension.Weibo.OAuth(clientid, clientsecret, accessTokenString, "");//用Token实例化OAuth无需再次进入验证流程
                TokenResult result = oauth.VerifierAccessToken();	//测试保存的AccessToken的有效性
                Client Sina = new Client(oauth);
                string ApiUserID = Sina.API.Entity.Account.GetUID();


                System.Web.HttpContext.Current.Response.ContentType = "application/json";

                WeiboParameter[] webpara = new WeiboParameter[] {
                                        new WeiboParameter("source",clientid),
                                        new WeiboParameter("access_token", accessTokenString),
                                        new WeiboParameter("uid",ApiUserID)
                };

                string returnResult = Sina.GetCommand("https://api.weibo.com/2/users/show.json", webpara);


                string nickname = GetJsonAtt("screen_name", returnResult);
                string headimgurl = GetJsonAtt("profile_image_url", returnResult);

                //对nickname的特殊字符做处理
                nickname = StringUtils.ReplaceInvalidChar(nickname);


                string groupSN = responseGroupSN;
                string successMessage = string.Empty;
                string errorMessage = string.Empty;
                string userName = string.Empty;
                bool isRedirectToLogin = false;
                int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(ApiUserID);

                string oldUserName = UserManager.GetUserInfo(groupSN, nickname).UserName;
                if (oldUserName != "")
                {
                    nickname = nickname + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 4);
                }

                bool IsSuccess = true;
                string SiteName = responsePublishmentSystemName;
                string ThirdLoginType = ESiteserverThirdLoginType.Weibo.ToString();
                string IndexPageUrl = indexPageUrl;
                string ThirdLoginNickName = nickname;
                string ThirdLoginPassword = "123456";
                string ThirdLoginUserHeadUrl = headimgurl;
                string SuccessMessage = "第三方授权成功!";
                string ErrorMessage = "第三方授权失败!";

                thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };

                if (userBindingCount > 0)
                {
                    UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);
                    if (info != null)
                    {
                        thirdLoginParameter.ThirdLoginNickName = UserManager.GetUserName(userBindingCount);
                        thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(info.UserID);
                        return Ok(thirdLoginParameter);
                    }
                    else
                    {
                        BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userBindingCount, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.Weibo));
                    }
                }

                string topEmial = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                isSuccess = UserManager.RegisterByUserController(groupSN, nickname, "123456", "" + topEmial + "@sina.com", "", string.Empty, out isRedirectToLogin, out successMessage, out errorMessage);
                if (!isSuccess)
                {
                    thirdLoginParameter.IsSuccess = false;
                    thirdLoginParameter.ErrorMessage = errorMessage;
                }
                int userID = UserManager.GetUserInfo(groupSN, nickname).UserID;
                UserInfo userInfo = new UserInfo();
                userInfo.UserID = userID;
                userInfo.AvatarLarge = headimgurl;
                userInfo.AvatarMiddle = headimgurl;
                userInfo.AvatarSmall = headimgurl;
                userInfo.IsChecked = true;
                BaiRongDataProvider.UserDAO.Update(userInfo);
                UserManager.RemoveCache(false, string.Empty, userName);

                if (userID > 0)
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(userID, ESiteserverThirdLoginType.Weibo.ToString(), ApiUserID);
                }
                return Ok(thirdLoginParameter);
            }

            if (loginType == 3)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.WeixinPC, responsePublishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientid = thirdLoginAuthInfo.AppKey;
                string clientsecret = thirdLoginAuthInfo.AppSercet;
                string redirecturi = thirdLoginAuthInfo.CallBackUrl;

                string code = RequestUtils.GetQueryString("code");

                //==============通过Authorization Code和基本资料获取Access Token=================
                string send_url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + clientid + "&secret=" + clientsecret + "&code=" + code + "&grant_type=authorization_code";

                //发送并接受返回值
                string rezult = HttpGet(send_url);

                string access_token = GetJsonAtt("access_token", rezult);
                string openid = GetJsonAtt("openid", rezult);

                string getUserInfo_url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid;

                string userinfoJson = HttpGet(getUserInfo_url);


                string nickname = GetJsonAtt("nickname", userinfoJson);
                string headimgurl = GetJsonAtt("headimgurl", userinfoJson);


                //对nickname的特殊字符做处理
                nickname = StringUtils.ReplaceInvalidChar(nickname);

                string groupSN = responseGroupSN;
                string successMessage = string.Empty;
                string errorMessage = string.Empty;
                string userName = string.Empty;
                bool isRedirectToLogin = false;
                int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(openid);

                string oldUserName = UserManager.GetUserInfo(groupSN, nickname).UserName;
                if (oldUserName != "")
                {
                    nickname = nickname + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 4);
                }

                bool IsSuccess = true;
                string SiteName = responsePublishmentSystemName;
                string ThirdLoginType = ESiteserverThirdLoginType.WeixinPC.ToString();
                string IndexPageUrl = indexPageUrl;
                string ThirdLoginNickName = nickname;
                string ThirdLoginPassword = "123456";
                string ThirdLoginUserHeadUrl = headimgurl;
                string SuccessMessage = "第三方授权成功!";
                string ErrorMessage = "第三方授权失败!";

                thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };

                if (userBindingCount > 0)
                {
                    UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);
                    if (info != null)
                    {
                        thirdLoginParameter.ThirdLoginNickName = UserManager.GetUserName(userBindingCount);
                        thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(info.UserID);
                        return Ok(thirdLoginParameter);
                    }
                    else
                    {
                        BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userBindingCount, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.WeixinPC));
                    }
                }


                string topEmial = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                isSuccess = UserManager.RegisterByUserController(groupSN, nickname, "123456", "" + topEmial + "@weixin.com", "", string.Empty, out isRedirectToLogin, out successMessage, out errorMessage);
                if (!isSuccess)
                {
                    thirdLoginParameter.IsSuccess = false;
                    thirdLoginParameter.ErrorMessage = errorMessage;
                }
                int userID = UserManager.GetUserInfo(groupSN, nickname).UserID;
                UserInfo userInfo = new UserInfo();
                userInfo.UserID = userID;
                userInfo.IsChecked = true;
                userInfo.AvatarLarge = headimgurl;
                userInfo.AvatarMiddle = headimgurl;
                userInfo.AvatarSmall = headimgurl;
                BaiRongDataProvider.UserDAO.Update(userInfo);
                UserManager.RemoveCache(false, string.Empty, userName);

                if (userID > 0)
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(userID, ESiteserverThirdLoginType.WeixinPC.ToString(), openid);
                }


                return Ok(thirdLoginParameter);

            }

            return Ok(thirdLoginParameter);
        }


        [HttpGet]
        [ActionName("SdkBind")]
        public IHttpActionResult SdkBind()
        {
            int loginType = RequestUtils.GetIntQueryString("sdkType");

            string returnUrl = RequestUtils.GetQueryString("returnUrl");

            string login_url = "";
            string indexPageUrl = "";
            string publishmentSystemName = string.Empty;
            string groupSN = string.Empty;
            int publishmentSystemID = 0;
            if (RequestUtils.PublishmentSystemInfo != null)
            {
                indexPageUrl = PageUtility.GetIndexPageUrl(RequestUtils.PublishmentSystemInfo, EVisualType.Static);
                publishmentSystemName = RequestUtils.PublishmentSystemInfo.PublishmentSystemName;
                publishmentSystemID = RequestUtils.PublishmentSystemInfo.PublishmentSystemID;
                groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
            }


            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.PublishmentSystemID", publishmentSystemID.ToString());
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.IndexPageUrl", returnUrl);
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.GroupSN", groupSN);
            DbCacheManager.Insert("SiteServer.API.Controllers.UserController.PublishmentSystemName", publishmentSystemName);


            if (loginType == 1)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.QQ, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl + "?type=innerBind";
                string state = Guid.NewGuid().ToString().Replace("-", "");
                login_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + clientID + "&state=" + state + "&redirect_uri=" + redirectUri + "&scope=get_user_info,get_info,get_other_info";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.state" + clientID, state);
            }

            if (loginType == 2)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.Weibo, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl + "?type=innerBind";
                string responseType = "code";
                login_url = "https://api.weibo.com/oauth2/authorize?client_id=" + clientID + "&response_type=" + responseType + "&redirect_uri=" + redirectUri + "";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
            }

            if (loginType == 3)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.WeixinPC, publishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientID = thirdLoginAuthInfo.AppKey;
                string redirectUri = thirdLoginAuthInfo.CallBackUrl + "?type=innerBind";
                string scope = "snsapi_login";
                login_url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + clientID + "&redirect_uri=" + redirectUri + "&response_type=code&scope=" + scope + "&state=STATE#wechat_redirect";

                DbCacheManager.Insert("SiteServer.API.Controllers.UserController.loginType", loginType.ToString());
            }

            return Ok(login_url);
        }

        [HttpGet]
        [ActionName("SdkUnBind")]
        public IHttpActionResult SdkUnBind()
        {
            try
            {
                string unBindThirdType = RequestUtils.GetQueryStringNoSqlAndXss("sdkType");
                if (string.IsNullOrEmpty(unBindThirdType))
                    return Ok(new { IsSuccess = false, ErrorMessage = "解绑失败！未指定解绑类型" });
                if (RequestUtils.IsAnonymous)
                    return Ok(new { IsSuccess = false, ErrorMessage = "解绑失败！请重新登录之后，在进行解绑" });
                int userID = UserManager.Current.UserID;
                if (unBindThirdType == "1")
                    BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userID, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.QQ));
                else if (unBindThirdType == "2")
                    BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userID, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.Weibo));
                else if (unBindThirdType == "3")
                    BaiRongDataProvider.UserBindingDAO.DeleteByUserID(userID, ESiteserverThirdLoginTypeUtils.GetValue(ESiteserverThirdLoginType.WeixinPC));
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = "解绑失败！" + ex.Message });
            }

        }

        [HttpGet]
        [ActionName("AuthBind")]
        public IHttpActionResult AuthBind()
        {
            string indexPageUrl = DbCacheManager.Get("SiteServer.API.Controllers.UserController.IndexPageUrl");
            string responseGroupSN = DbCacheManager.Get("SiteServer.API.Controllers.UserController.GroupSN");
            string responsePublishmentSystemName = DbCacheManager.Get("SiteServer.API.Controllers.UserController.PublishmentSystemName");
            int responsePublishmentSystemID = TranslateUtils.ToInt(DbCacheManager.Get("SiteServer.API.Controllers.UserController.PublishmentSystemID"));
            int loginType = TranslateUtils.ToInt(DbCacheManager.Get("SiteServer.API.Controllers.UserController.loginType"));
            ThirdLoginParameter thirdLoginParameter = new ThirdLoginParameter();
            if (loginType == 1)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.QQ, responsePublishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientid = thirdLoginAuthInfo.AppKey;
                string clientsecret = thirdLoginAuthInfo.AppSercet;
                string redirecturi = thirdLoginAuthInfo.CallBackUrl + "?type=innerBind";

                string send_url = "";//临时变量，发送URL,接受返回结果 
                string rezult = ""; //用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
                string state = "";
                string code = "";//临时Authorization Code，官方提示10分钟过期
                string access_token = "";//通过Authorization Code返回结果获取到的Access Token
                string expires_in = ""; //expires_in是该Access Token的有效期，单位为秒
                string new_client_id = "";//通过Access Token返回来的client_id 
                string openid = ""; //通过Access Token返回来的openid，QQ用户唯一值，可以与网站用户数据关联

                state = RequestUtils.GetQueryString("state");

                bool isExistsState = DbCacheManager.IsExists("SiteServer.API.Controllers.UserController.state" + clientid);

                if (!isExistsState)
                {
                    HttpContext.Current.Response.Write("state未初始化");
                    HttpContext.Current.Response.End();
                }

                //如果返回state与之前发出的判断正确
                if (state == DbCacheManager.Get("SiteServer.API.Controllers.UserController.state" + clientid))
                {
                    code = RequestUtils.GetQueryString("code");
                    //==============通过Authorization Code和基本资料获取Access Token=================
                    send_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + clientid + "&client_secret=" + clientsecret + "&code=" + code + "&state=" + state + "&redirect_uri=" + redirecturi;

                    //发送并接受返回值
                    rezult = HttpGet(send_url);
                    //如果失败
                    if (rezult.Contains("error"))
                    {
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        //======================通过Access Token来获取用户的OpenID==============
                        string[] parm = rezult.Split('&');
                        access_token = parm[0].Split('=')[1];//取得 access_token
                        expires_in = parm[1].Split('=')[1];//取得 过期时间
                        send_url = "https://graph.qq.com/oauth2.0/me?access_token=" + access_token;  //拼接url
                        rezult = HttpGet(send_url);//发送并接受返回值
                        //如果失败
                        if (rezult.Contains("error"))
                        {
                            HttpContext.Current.Response.End();
                        }

                        //取得文字出现
                        int str_start = rezult.IndexOf('(') + 1;
                        int str_last = rezult.LastIndexOf(')') - 1;

                        //取得JSON字符串
                        rezult = rezult.Substring(str_start, (str_last - str_start));

                        //反序列化JSON
                        Dictionary<string, string> _dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);


                        //取值
                        _dic.TryGetValue("client_id", out new_client_id);
                        _dic.TryGetValue("openid", out openid);

                        //储存获取数据用到的信息
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.access_token" + clientid, access_token);
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.clientid" + clientid, clientid);
                        DbCacheManager.Insert("SiteServer.API.Controllers.UserController.openid" + clientid, openid);

                        send_url = "https://graph.qq.com/user/get_user_info?access_token=" + access_token + "&oauth_consumer_key=" + clientid + "&openid=" + openid;

                        //发送并接受返回值
                        rezult = HttpGet(send_url);

                        //反序列化JSON
                        Dictionary<string, string> _dic2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);

                        string ret = "", msg = "", nickname = "", face = "", face1 = "", sex = "", vip_level = "", qzone_level = "";

                        //取值
                        _dic2.TryGetValue("ret", out ret);
                        _dic2.TryGetValue("msg", out msg);

                        //如果失败
                        if (ret != "0")
                        {
                            HttpContext.Current.Response.End();
                        }

                        _dic2.TryGetValue("nickname", out nickname);
                        _dic2.TryGetValue("figureurl_qq_1", out face);
                        _dic2.TryGetValue("figureurl_qq_2", out face1);
                        _dic2.TryGetValue("gender", out sex);
                        _dic2.TryGetValue("vip", out vip_level);
                        _dic2.TryGetValue("level", out qzone_level);


                        //对nickname的特殊字符做处理
                        nickname = StringUtils.ReplaceInvalidChar(nickname);

                        string groupSN = responseGroupSN;
                        string successMessage = string.Empty;
                        string errorMessage = string.Empty;
                        string userName = string.Empty;
                        bool isRedirectToLogin = false;

                        int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(openid);//用户ID

                        UserInfo current = UserManager.Current;
                        bool IsSuccess = true;
                        string SiteName = responsePublishmentSystemName;
                        string ThirdLoginType = ESiteserverThirdLoginType.QQ.ToString();
                        string IndexPageUrl = indexPageUrl;
                        string ThirdLoginNickName = current.UserName;
                        string ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                        string ThirdLoginUserHeadUrl = face;
                        string SuccessMessage = "第三方授权成功!";
                        string ErrorMessage = "第三方授权失败!";

                        thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };

                        if (userBindingCount > 0)
                        {
                            UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);

                            if (info == null)
                            {
                                //删除
                                BaiRongDataProvider.UserBindingDAO.DeleteByThirdUserID(openid);

                                BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.QQ.ToString(), openid);
                                thirdLoginParameter.ThirdLoginNickName = current.UserName;
                                thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                                return Ok(thirdLoginParameter);
                            }
                            else if (info.UserName != current.UserName)
                            {
                                //已经绑定过账户，不能进行二次绑定
                                ErrorMessage = "该QQ已经绑定过用户，请换一个QQ进行绑定";
                                IsSuccess = false;
                                thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };
                                return Ok(thirdLoginParameter);
                            }
                        }
                        else
                        {
                            BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.QQ.ToString(), openid);
                            thirdLoginParameter.ThirdLoginNickName = current.UserName;
                            thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                            return Ok(thirdLoginParameter);
                        }
                        return Ok(thirdLoginParameter);
                    }
                }
            }

            if (loginType == 2)
            {

                BaiRongThirdLoginInfo newThirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.Weibo, responsePublishmentSystemID);
                ThirdLoginAuthInfo newThirdLoginAuthInfo = new ThirdLoginAuthInfo(newThirdLoginInfo.SettingsXML);
                string clientid = newThirdLoginAuthInfo.AppKey;
                string clientsecret = newThirdLoginAuthInfo.AppSercet;
                string redirecturi = newThirdLoginAuthInfo.CallBackUrl + "?type=innerBind";
                string code = RequestUtils.GetQueryString("code");

                NetDimension.Weibo.OAuth oauth = new NetDimension.Weibo.OAuth(clientid, clientsecret, redirecturi);

                AccessToken accessToken = oauth.GetAccessTokenByAuthorizationCode(code); //请注意这里返回的是AccessToken对象，不是string
                string accessTokenString = oauth.AccessToken;
                oauth = new NetDimension.Weibo.OAuth(clientid, clientsecret, accessTokenString, "");//用Token实例化OAuth无需再次进入验证流程
                TokenResult result = oauth.VerifierAccessToken();	//测试保存的AccessToken的有效性
                Client Sina = new Client(oauth);
                string ApiUserID = Sina.API.Entity.Account.GetUID();


                System.Web.HttpContext.Current.Response.ContentType = "application/json";

                WeiboParameter[] webpara = new WeiboParameter[] {
                                        new WeiboParameter("source",clientid),
                                        new WeiboParameter("access_token", accessTokenString),
                                        new WeiboParameter("uid",ApiUserID)
                };

                string returnResult = Sina.GetCommand("https://api.weibo.com/2/users/show.json", webpara);


                string nickname = GetJsonAtt("screen_name", returnResult);
                string headimgurl = GetJsonAtt("profile_image_url", returnResult);


                //对nickname的特殊字符做处理
                nickname = StringUtils.ReplaceInvalidChar(nickname);

                string groupSN = responseGroupSN;
                string successMessage = string.Empty;
                string errorMessage = string.Empty;
                string userName = string.Empty;
                bool isRedirectToLogin = false;
                int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(ApiUserID);

                UserInfo current = UserManager.Current;
                bool IsSuccess = true;
                string SiteName = responsePublishmentSystemName;
                string ThirdLoginType = ESiteserverThirdLoginType.Weibo.ToString();
                string IndexPageUrl = indexPageUrl;
                string ThirdLoginNickName = current.UserName;
                string ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                string ThirdLoginUserHeadUrl = headimgurl;
                string SuccessMessage = "第三方授权成功!";
                string ErrorMessage = "第三方授权失败!";

                thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };


                if (userBindingCount > 0)
                {
                    UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);

                    if (info == null)
                    {
                        //删除
                        BaiRongDataProvider.UserBindingDAO.DeleteByThirdUserID(ApiUserID);

                        BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.Weibo.ToString(), ApiUserID);
                        thirdLoginParameter.ThirdLoginNickName = current.UserName;
                        thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                        return Ok(thirdLoginParameter);
                    }
                    else if (info.UserName != current.UserName)
                    {
                        //已经绑定过账户，不能进行二次绑定
                        ErrorMessage = "该微博已经绑定过用户，请换一个微博进行绑定";
                        IsSuccess = false;
                        thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };
                        return Ok(thirdLoginParameter);
                    }
                }
                else
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.Weibo.ToString(), ApiUserID);
                    thirdLoginParameter.ThirdLoginNickName = current.UserName;
                    thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                    return Ok(thirdLoginParameter);
                }

                return Ok(thirdLoginParameter);
            }

            if (loginType == 3)
            {
                BaiRongThirdLoginInfo thirdLoginInfo = GetThirdLoginInfo(ESiteserverThirdLoginType.WeixinPC, responsePublishmentSystemID);
                ThirdLoginAuthInfo thirdLoginAuthInfo = new ThirdLoginAuthInfo(thirdLoginInfo.SettingsXML);

                string clientid = thirdLoginAuthInfo.AppKey;
                string clientsecret = thirdLoginAuthInfo.AppSercet;
                string redirecturi = thirdLoginAuthInfo.CallBackUrl + "?type=innerBind";

                string code = RequestUtils.GetQueryString("code");

                //==============通过Authorization Code和基本资料获取Access Token=================
                string send_url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + clientid + "&secret=" + clientsecret + "&code=" + code + "&grant_type=authorization_code";

                //发送并接受返回值
                string rezult = HttpGet(send_url);

                string access_token = GetJsonAtt("access_token", rezult);
                string openid = GetJsonAtt("openid", rezult);

                string getUserInfo_url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid;

                string userinfoJson = HttpGet(getUserInfo_url);


                string nickname = GetJsonAtt("nickname", userinfoJson);
                string headimgurl = GetJsonAtt("headimgurl", userinfoJson);


                //对nickname的特殊字符做处理
                nickname = StringUtils.ReplaceInvalidChar(nickname);

                string groupSN = responseGroupSN;
                string successMessage = string.Empty;
                string errorMessage = string.Empty;
                string userName = string.Empty;
                bool isRedirectToLogin = false;
                int userBindingCount = BaiRongDataProvider.BaiRongThirdLoginDAO.GetUserBindingCount(openid);

                UserInfo current = UserManager.Current;
                bool IsSuccess = true;
                string SiteName = responsePublishmentSystemName;
                string ThirdLoginType = ESiteserverThirdLoginType.WeixinPC.ToString();
                string IndexPageUrl = indexPageUrl;
                string ThirdLoginNickName = current.UserName;
                string ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                string ThirdLoginUserHeadUrl = headimgurl;
                string SuccessMessage = "第三方授权成功!";
                string ErrorMessage = "第三方授权失败!";

                thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };


                if (userBindingCount > 0)
                {
                    UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfo(userBindingCount);

                    if (info == null)
                    {
                        //删除
                        BaiRongDataProvider.UserBindingDAO.DeleteByThirdUserID(openid);

                        BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.WeixinPC.ToString(), openid);
                        thirdLoginParameter.ThirdLoginNickName = current.UserName;
                        thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                        return Ok(thirdLoginParameter);
                    }
                    else if (info.UserName != current.UserName)
                    {
                        //已经绑定过账户，不能进行二次绑定
                        ErrorMessage = "该微博已经绑定过用户，请换一个微博进行绑定";
                        IsSuccess = false;
                        thirdLoginParameter = new ThirdLoginParameter { IsSuccess = IsSuccess, SiteName = SiteName, ThirdLoginType = ThirdLoginType, IndexPageUrl = IndexPageUrl, ThirdLoginNickName = ThirdLoginNickName, ThirdLoginPassword = ThirdLoginPassword, ThirdLoginUserHeadUrl = ThirdLoginUserHeadUrl, PublishmentSystemID = responsePublishmentSystemID, SuccessMessage = SuccessMessage, ErrorMessage = ErrorMessage };
                        return Ok(thirdLoginParameter);
                    }
                }
                else
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.InsertUserBinding(current.UserID, ESiteserverThirdLoginType.WeixinPC.ToString(), openid);
                    thirdLoginParameter.ThirdLoginNickName = current.UserName;
                    thirdLoginParameter.ThirdLoginPassword = BaiRongDataProvider.UserDAO.GetPassword(current.UserID);
                    return Ok(thirdLoginParameter);
                }

                return Ok(thirdLoginParameter);

            }

            return Ok(thirdLoginParameter);
        }

        #endregion

        // 第三方登陆 end wujiaqiang

        #endregion

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 获取json对象值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="json">属性值</param>
        /// <returns></returns>
        public string GetJsonAtt(string name, string jsonContent)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonContent);
            object value;
            if (json.TryGetValue(name, out value))
            {
                return Convert.ToString(value);
            }
            return "";
        }


        /// <summary>
        /// 根据信息类型获取用户信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMessage")]
        public IHttpActionResult GetUserMessage()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户
                string type = RequestUtils.GetQueryStringNoSqlAndXss("type");//信息类型
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = 0;
                if (type == EUserMessageType.SystemAnnouncement.ToString())
                    total = BaiRongDataProvider.UserMessageDAO.GetCount(" MessageType = '" + type + "'");
                else
                    total = BaiRongDataProvider.UserMessageDAO.GetCount(" MessageType = '" + type + "'" + " AND MessageTo = '" + userName + "'");
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<UserMessageInfo> list = BaiRongDataProvider.UserMessageDAO.GetReciveMessageInfoList(userName, EUserMessageTypeUtils.GetEnumType(type), pageIndex, prePageNum);

                if (EUserMessageTypeUtils.Equals(EUserMessageType.SystemAnnouncement, type))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Title = FormatMessageTitle(list[i].Title, list[i].AddDate, list[i].IsViewed);
                        if (string.IsNullOrEmpty(list[i].MessageFrom))
                        {
                            list[i].MessageFrom = "系统";
                        }
                    }
                }

                var userMessageListParm = new { IsSuccess = true, userMessageList = list, PageJson = pageJson };
                return Ok(userMessageListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        public string FormatMessageTitle(string title, DateTime addDate, bool isViewed)
        {
            //IsViewed=true 并且 发布日期在n天内
            if (!isViewed && (DateTime.Now - addDate).TotalSeconds < 3600 * 24 * UserConfigManager.Additional.NewOfDays && (DateTime.Now - addDate).TotalSeconds > 0)
            {
                //n天内，消息为最新
                title = title + " [最新]";
            }
            return title;
        }

        /// <summary>
        /// 获取用户信息详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMessageDetail")]
        public IHttpActionResult GetUserMessageDetail()
        {
            try
            {
                int messageID = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("messageID"));//信息ID

                UserMessageInfo info = BaiRongDataProvider.UserMessageDAO.GetMessageInfo(messageID);
                if (info.MessageType != EUserMessageType.SystemAnnouncement)
                    //标记位已读信息
                    info.IsViewed = true;
                BaiRongDataProvider.UserMessageDAO.Update(info);
                if (string.IsNullOrEmpty(info.MessageFrom))
                {
                    info.MessageFrom = "系统";
                }
                var userMessageParm = new { IsSuccess = true, info = info };
                return Ok(userMessageParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        /// <summary>
        /// 获取用户发送信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserMessageSend")]
        public IHttpActionResult GetUserMessageSend()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户

                List<UserMessageInfo> list = BaiRongDataProvider.UserMessageDAO.GetSendMessageInfoList(userName);

                var userMessageListParm = new { IsSuccess = true, userMessageList = list };
                return Ok(userMessageListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        /// <summary>
        /// 获取用户站内信列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetSiteMessage")]
        public IHttpActionResult GetSiteMessage()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = 0;
                if (!string.IsNullOrEmpty(userName))
                    total = BaiRongDataProvider.UserMessageDAO.GetCount(" MessageTo = '" + userName + "' AND MessageType = 'Private'");

                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<UserMessageInfo> list = BaiRongDataProvider.UserMessageDAO.GetReciveMessageInfoList(userName, EUserMessageType.Private, pageIndex, prePageNum);

                var userMessageListParm = new { IsSuccess = true, userMessageList = list, PageJson = pageJson };
                return Ok(userMessageListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        /// <summary>
        /// 删除用户发送的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DeleteUserMessage")]
        public IHttpActionResult DeleteUserMessage()
        {
            try
            {
                int messageID = RequestUtils.GetIntQueryString("messageID");
                if (messageID == 0)
                    return Ok(new { IsSuccess = true });
                if (RequestUtils.IsAnonymous)
                    return Ok(new { IsSuccess = false, ErrorMessage = "删除失败！请重新登录之后，在进行删除" });
                BaiRongDataProvider.UserMessageDAO.Delete(messageID);
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = "删除失败！" + ex.Message });
            }
        }

        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SendMessage")]
        public IHttpActionResult SendMessage()
        {
            try
            {
                string userName = RequestUtils.GetQueryStringNoSqlAndXss("userName");
                string msg = RequestUtils.GetQueryStringNoSqlAndXss("msg");
                string title = RequestUtils.GetQueryStringNoSqlAndXss("title");
                int parent = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("parentID"), 0);
                if (RequestUtils.IsAnonymous)
                    return Ok(new { IsSuccess = false, ErrorMessage = "发送失败！请重新登录之后，在进行删除" });
                if (string.IsNullOrEmpty(userName))
                    return Ok(new { IsSuccess = false, ErrorMessage = "发送失败！请填写接受人" });
                if (string.IsNullOrEmpty(title))
                    return Ok(new { IsSuccess = false, ErrorMessage = "发送失败！请填写标题" });
                if (string.IsNullOrEmpty(msg))
                    return Ok(new { IsSuccess = false, ErrorMessage = "发送失败！请填写内容" });

                UserMessageInfo userMessageInfo = new UserMessageInfo();
                userMessageInfo.AddDate = DateTime.Now;
                userMessageInfo.LastAddDate = DateTime.Now;
                userMessageInfo.Content = msg;
                userMessageInfo.IsViewed = false;
                userMessageInfo.MessageFrom = UserManager.Current.UserName;
                userMessageInfo.MessageTo = userName;
                userMessageInfo.Title = title;
                userMessageInfo.ParentID = parent;
                userMessageInfo.MessageType = EUserMessageType.Private;
                BaiRongDataProvider.UserMessageDAO.Insert(userMessageInfo);
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = "发送失败！" + ex.Message });
            }
        }

        /// <summary>
        /// 是否有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("HasNewMessage")]
        public IHttpActionResult HasNewMessage()
        {
            try
            {
                int total = 0;
                total = BaiRongDataProvider.UserMessageDAO.GetCount("IsViewed = 'False' AND MessageType = '" + EUserMessageType.SystemAnnouncement + "' OR MessageType = '" + EUserMessageType.Private);
                if (total > 0)
                    return Ok(new { IsSuccess = false, hasMsg = true, count = total });
                else
                    return Ok(new { IsSuccess = false, hasMsg = false, count = 0 });

            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
            }
        }


        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserAvatar")]
        public IHttpActionResult GetUserAvatar()
        {
            try
            {
                User user = SiteServer.API.Model.User.GetInstance();
                if (user != null)
                {
                    var userMessageListParm = new { IsSuccess = true, AvatarLarge = user.AvatarLarge, AvatarMiddle = user.AvatarMiddle, AvtarSmall = user.AvatarSmall, IsAnonymous = RequestUtils.IsAnonymous };
                    return Ok(userMessageListParm);
                }
                else
                {
                    var userMessageListParm = new { IsSuccess = false, IsAnonymous = RequestUtils.IsAnonymous };
                    return Ok(userMessageListParm);
                }

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

        /// <summary>
        /// 更新用户图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateUserAvatar")]
        public IHttpActionResult UpdateUserAvatar()
        {
            try
            {
                UserInfo user = UserManager.Current;
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string fileName = PathUtils.GetFileName(file.FileName);
                    string fileExtend = PathUtils.GetExtension(fileName).Trim('.');
                    EImageType imageType = EImageTypeUtils.GetEnumType(fileExtend);
                    if (!EImageTypeUtils.Equals(imageType, EImageType.Unknown))
                    {
                        string temporaryImagePath = APIPageUtils.ParseUrl(PathUtils.GetTemporaryFilesPath(string.Format("image.{0}", EImageTypeUtils.GetValue(imageType))));
                        DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(temporaryImagePath));
                        file.SaveAs(temporaryImagePath);

                        string largeName = string.Format("large_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));
                        string middleName = string.Format("large_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));
                        string smallName = string.Format("large_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));


                        //大图PageUtils.GetUserAvatarUrl
                        string imagePath = APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserFilesUrl(user.UserName, largeName)));
                        if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 180, 180, false))
                        {
                            FileUtils.DeleteFileIfExists(APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserAvatarUrl(user.GroupSN, user.UserName, EAvatarSize.Large))));
                            user.AvatarLarge = largeName;
                        }

                        //中图
                        imagePath = APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserFilesUrl(user.UserName, middleName)));
                        if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 120, 120, false))
                        {
                            FileUtils.DeleteFileIfExists(APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserAvatarUrl(user.GroupSN, user.UserName, EAvatarSize.Middle))));
                            user.AvatarMiddle = middleName;
                        }

                        //小图
                        imagePath = APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserFilesUrl(user.UserName, smallName)));
                        if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 48, 48, false))
                        {
                            FileUtils.DeleteFileIfExists(APIPageUtils.ParseUrl(HttpContext.Current.Request.MapPath(PageUtils.GetUserAvatarUrl(user.GroupSN, user.UserName, EAvatarSize.Small))));
                            user.AvatarSmall = smallName;
                        }

                        BaiRongDataProvider.UserDAO.Update(user);

                        UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);
                    }
                }
                var pathedAvatarLarge = PageUtils.GetUserFilesUrl(user.UserName, user.AvatarLarge);
                var pathedAvatarMiddle = PageUtils.GetUserFilesUrl(user.UserName, user.AvatarMiddle);
                var pathedAvatarSmall = PageUtils.GetUserFilesUrl(user.UserName, user.AvatarSmall);

                var userMessageListParm = new { IsSuccess = true, AvatarLarge = pathedAvatarLarge, AvatarMiddle = pathedAvatarMiddle, AvatarSmall = pathedAvatarSmall, IsAnonymous = RequestUtils.IsAnonymous };
                return Ok(userMessageListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 获取可用的密保问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetSecurityQuestionList")]
        public IHttpActionResult GetSecurityQuestionList()
        {

            try
            {
                UserInfo user = UserManager.Current;
                if (user == null || string.IsNullOrEmpty(user.UserName))
                    user = UserManager.GetUserInfo(RequestUtils.GetQueryString("groupSN") == null ? string.Empty : RequestUtils.GetQueryString("groupSN"), RequestUtils.GetQueryString("userName"));

                string enStr = user.SCQU;
                NameValueCollection nvc = new NameValueCollection();
                nvc = TranslateUtils.ParseJsonStringToNameValueCollection(enStr);
                List<UserSecurityQuestionInfo> list = BaiRongDataProvider.UserSecurityQuestionDAO.GetSecurityQuestionInfoList();
                if (nvc.Keys.Count == 1)
                {
                    return Ok(new { IsSuccess = true, SecurityQuestionList = list, Que1 = nvc.Keys[0] });
                }
                else if (nvc.Keys.Count == 2)
                {
                    return Ok(new { IsSuccess = true, SecurityQuestionList = list, Que1 = nvc.Keys[0], Que2 = nvc.Keys[1] });
                }
                else if (nvc.Keys.Count == 3)
                {
                    return Ok(new { IsSuccess = true, SecurityQuestionList = list, Que1 = nvc.Keys[0], Que2 = nvc.Keys[1], Que3 = nvc.Keys[2] });
                }
                else
                {
                    return Ok(new { IsSuccess = true, SecurityQuestionList = list });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 校验用户密保问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ValidateSecurityQuestion")]
        public IHttpActionResult ValidateSecurityQuestion()
        {
            try
            {
                bool isValidate = false;
                UserInfo user = UserManager.Current;
                string que = RequestUtils.GetQueryStringNoSqlAndXss("que");
                string anw = RequestUtils.GetQueryStringNoSqlAndXss("anw");

                string enStr = user.SCQU;
                NameValueCollection nvc = new NameValueCollection();
                nvc = TranslateUtils.ParseJsonStringToNameValueCollection(enStr);
                foreach (string key in nvc.Keys)
                {
                    if (key == que && nvc[key] == anw)
                    {
                        isValidate = true;
                        break;
                    }
                }
                return Ok(new { IsSuccess = true, isValidate = isValidate });

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 修改用户密保问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("UpdateSecurityQuestion")]
        public IHttpActionResult UpdateSecurityQuestion()
        {
            try
            {
                UserInfo user = UserManager.Current;
                string userName = user.UserName;
                string que1 = RequestUtils.GetQueryStringNoSqlAndXss("que1");
                string que2 = RequestUtils.GetQueryStringNoSqlAndXss("que2");
                string que3 = RequestUtils.GetQueryStringNoSqlAndXss("que3");
                string anw1 = RequestUtils.GetQueryStringNoSqlAndXss("anw1");
                string anw2 = RequestUtils.GetQueryStringNoSqlAndXss("anw2");
                string anw3 = RequestUtils.GetQueryStringNoSqlAndXss("anw3");
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add(que1, anw1);
                nvc.Add(que2, anw2);
                nvc.Add(que3, anw3);
                string json = TranslateUtils.NameValueCollectionToJsonString(nvc);
                user.SCQU = json;
                user.IsSetSCQU = true;
                BaiRongDataProvider.UserDAO.Update(user);
                UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);
                return Ok(new { IsSuccess = true });

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 获取用户密保问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserSecurityQuestionAnwser")]
        public IHttpActionResult GetUserSecurityQuestionAnwser()
        {

            try
            {
                UserInfo user = UserManager.Current;
                string enStr = user.SCQU;
                NameValueCollection nvc = new NameValueCollection();
                nvc = TranslateUtils.ParseJsonStringToNameValueCollection(enStr);
                if (nvc.Keys.Count == 1)
                {
                    return Ok(new { IsSuccess = true, Que1 = nvc.Keys[0] });
                }
                else if (nvc.Keys.Count == 2)
                {
                    return Ok(new { IsSuccess = true, Que1 = nvc.Keys[0], Que2 = nvc.Keys[1] });
                }
                else if (nvc.Keys.Count == 3)
                {
                    return Ok(new { IsSuccess = true, Que1 = nvc.Keys[0], Que2 = nvc.Keys[1], Que3 = nvc.Keys[2] });
                }
                else
                {
                    return Ok(new { IsSuccess = true });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 获取用户登录记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserLoginLog")]
        public IHttpActionResult GetUserLoginLog()
        {
            try
            {
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = BaiRongDataProvider.UserLogDAO.GetCount(string.Format(" Action = '{0}' AND UserName = '{1}' ", EUserActionTypeUtils.GetValue(EUserActionType.Login), RequestUtils.CurrentUserName));
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<UserLogInfo> list = BaiRongDataProvider.UserLogDAO.GetUserLoginLogByPage(RequestUtils.CurrentUserName, pageIndex, prePageNum);

                var newList = from l in list
                              select new { l.Action, l.AddDate, l.ID, l.IPAddress, l.Summary, l.UserName, City = BaiRongDataProvider.IP2CityDAO.GetCity(l.IPAddress) };

                var userLoginInfoListParms = new { IsSuccess = true, UserLoginInfoList = newList, PageJson = pageJson };
                return Ok(userLoginInfoListParms);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 获取用户绑定信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserBindInfo")]
        public IHttpActionResult GetUserBindInfo()
        {
            try
            {
                UserInfo user = UserManager.Current;

                var userBindInfoParms = new { IsSuccess = true, IsBindEmai = user.IsBindEmail, IsBindPhone = user.IsBindPhone, IsSetSQCU = user.IsSetSCQU };
                return Ok(userBindInfoParms);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 用户绑定邮箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BindEmail")]
        public IHttpActionResult BindEmail()
        {
            try
            {
                UserInfo userInfo = UserManager.Current;
                string groupSN = "";
                string userName = RequestUtils.CurrentUserName;
                string email = "";
                string errorMessage = string.Empty;
                email = RequestUtils.GetPostStringNoSqlAndXss("email");

                if (!string.IsNullOrEmpty(email) && userInfo.Email != email)
                {
                    //验证邮箱已经绑定
                    if (BaiRongDataProvider.UserDAO.IsEmailExists(groupSN, email))
                    {
                        return Ok(new { IsSuccess = false, errorMessage = "该邮箱已经被注册，请更换另一个邮箱" });
                    }
                }

                UserNoticeSettingInfo settingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.BindEmail));
                if (UserNoticeSettingManager.SendMsg(settingInfo, email, out errorMessage))
                {
                    return Ok(new { IsSuccess = true });
                }
                else
                {
                    return Ok(new { IsSuccess = false, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }

        /// <summary>
        /// 验证绑定邮箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BindEmailValidate")]
        public IHttpActionResult BindEmailValidate()
        {
            try
            {
                string userName = RequestUtils.CurrentUserName;
                string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
                string email = RequestUtils.GetPostStringNoSqlAndXss("email");
                string errorMessage = string.Empty;
                string successMessage = string.Empty;

                VCManager vc = VCManager.GetInstanceOfValidateCode();
                if (vc.IsCodeValid(validateCode))
                {
                    //验证通过
                    UserInfo user = UserManager.Current;
                    user.IsBindEmail = true;
                    user.Email = email;
                    BaiRongDataProvider.UserDAO.Update(user);
                    UserManager.RemoveCache(false, user.GroupSN, userName);
                    successMessage = "绑定成功";
                    return Ok(new { IsSuccess = true, successMessage = successMessage, email = email });
                }
                else
                {
                    errorMessage = "验证码不正确";
                    //验证码不正确
                    return Ok(new { IsSuccess = false, errorMessage = errorMessage });
                }


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }


        /// <summary>
        /// 用户绑定手机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BindPhone")]
        public IHttpActionResult BindPhone()
        {
            try
            {
                UserInfo userInfo = UserManager.Current;
                string groupSN = "";
                string userName = RequestUtils.CurrentUserName;
                string phoneNum = "";
                string errorMessage = string.Empty;
                phoneNum = RequestUtils.GetPostStringNoSqlAndXss("phoneNum");

                if (!string.IsNullOrEmpty(phoneNum) && userInfo.Mobile != phoneNum)
                {
                    //手机号已经绑定
                    if (BaiRongDataProvider.UserDAO.IsMobileExists(groupSN, phoneNum))
                    {
                        return Ok(new { IsSuccess = false, errorMessage = "该手机号已经被注册，请更换另一个手机号" });
                    }
                }

                UserNoticeSettingInfo settingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.BindPhone));
                if (UserNoticeSettingManager.SendMsg(settingInfo, phoneNum, out errorMessage))
                {
                    return Ok(new { IsSuccess = true });
                }
                else
                {
                    return Ok(new { IsSuccess = false, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }

        /// <summary>
        /// 验证绑定手机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BindPhoneValidate")]
        public IHttpActionResult BindPhoneValidate()
        {
            try
            {
                string userName = RequestUtils.CurrentUserName;
                string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
                string phoneNum = RequestUtils.GetPostStringNoSqlAndXss("phoneNum");
                string errorMessage = string.Empty;
                string successMessage = string.Empty;

                VCManager vc = VCManager.GetInstanceOfValidateCode();
                if (vc.IsCodeValid(validateCode))
                {
                    //验证通过
                    UserInfo user = UserManager.Current;
                    user.IsBindPhone = true;
                    user.Mobile = phoneNum;
                    BaiRongDataProvider.UserDAO.Update(user);
                    UserManager.RemoveCache(false, user.GroupSN, userName);
                    successMessage = "绑定成功";
                    return Ok(new { IsSuccess = true, successMessage = successMessage, phone = phoneNum });
                }
                else
                {
                    errorMessage = "验证码不正确";
                    //验证码不正确
                    return Ok(new { IsSuccess = false, errorMessage = errorMessage });
                }


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }

        /// <summary>
        /// 解除绑定手机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveBindPhone")]
        public IHttpActionResult RemoveBindPhone()
        {
            try
            {
                string userName = RequestUtils.CurrentUserName;
                string errorMessage = string.Empty;
                string successMessage = string.Empty;

                //验证通过
                UserInfo user = UserManager.Current;
                user.IsBindPhone = false;
                user.Mobile = string.Empty;
                BaiRongDataProvider.UserDAO.Update(user);
                UserManager.RemoveCache(false, user.GroupSN, userName);
                successMessage = "解除绑定成功";
                return Ok(new { IsSuccess = true, successMessage = successMessage });

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }


        /// <summary>
        /// 通过邮箱找回密码--第一步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByEmailStep1")]
        public IHttpActionResult FindPasswordByEmailStep1()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);
                string key = string.Empty;
                if (!string.IsNullOrEmpty(validateCode))
                {
                    if (!VCManager.GetInstanceOfValidateCode().IsCodeValid(validateCode))
                    {
                        errorMessage = "验证码不正确";//NotBindEmail
                        return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                    }
                }
                if (info == null)
                {
                    errorMessage = "请填写正确的用户名";//NotBindEmail
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
                if (info.IsBindEmail)
                {
                    key = EncryptUtils.Md5(userName + "emailstep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                    successMessage = "BindEmail";
                    return Ok(new { IsSuccess = true, key = key, userName = userName, successMessage = successMessage });
                }
                else
                {
                    errorMessage = "邮箱没有绑定";//NotBindEmail
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 通过邮箱找回密码--第二步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByEmailStep2")]
        public IHttpActionResult FindPasswordByEmailStep2()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string email = RequestUtils.GetPostStringNoSqlAndXss("Email");
            string keyStep1 = RequestUtils.GetPostStringNoSqlAndXss("key");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            string key = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);

                if (string.IsNullOrEmpty(keyStep1))
                {
                    errorMessage = "ValidateKeyIsNULL";//ValidateKeyIsNULL
                }
                else if (keyStep1 != EncryptUtils.Md5(userName + "emailstep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                {
                    errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                }

                if (string.IsNullOrEmpty(email) || email != info.Email)
                {
                    errorMessage = "用户输入邮箱和绑定邮箱不一致";//InputEmailIsNotBindedEmail
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });


                if (info.IsBindEmail && !string.IsNullOrEmpty(info.Email))
                {
                    //发送邮件
                    UserNoticeSettingInfo noticeSettingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPassword));
                    if (noticeSettingInfo != null)
                    {
                        key = EncryptUtils.Md5(userName + info.Email + "emailstep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                        Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                        dicReplace.Add("userName", info.UserName);
                        bool flag = UserNoticeSettingManager.SendMsg(noticeSettingInfo, info.Email, dicReplace, out errorMessage);
                        successMessage = "SendEmail";
                        return Ok(new { IsSuccess = flag, key = key, userName = userName, successMessage = successMessage, errorMessage = errorMessage });
                    }
                    else
                    {
                        errorMessage = "InnerError";
                        return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                    }
                }
                else
                {
                    errorMessage = "NotBindEmail";
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 通过邮箱找回密码--第三步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByEmailStep3")]
        public IHttpActionResult FindPasswordByEmailStep3()
        {
            try
            {
                string groupSN = RequestUtils.GetPostStringNoSqlAndXss("groupSN");
                string userName = RequestUtils.GetPostStringNoSqlAndXss("userName");
                string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
                string email = RequestUtils.GetPostStringNoSqlAndXss("email");
                string keyStep2 = RequestUtils.GetPostStringNoSqlAndXss("key");

                string errorMessage = string.Empty;
                string successMessage = string.Empty;

                if (string.IsNullOrEmpty(keyStep2))
                {
                    errorMessage = "验证码为空";//ValidateKeyIsNULL
                }
                else if (keyStep2 != EncryptUtils.Md5(userName + email + "emailstep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                {
                    errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                }

                if (string.IsNullOrEmpty(userName))
                {
                    errorMessage = "用户名为空";//userNameIsNULL
                }

                if (string.IsNullOrEmpty(email))
                {
                    errorMessage = "邮箱为空";//emailIsNULL
                }


                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, UserName = userName, Email = email, ErrorMessage = errorMessage });

                VCManager vc = VCManager.GetInstanceOfValidateCode();
                if (vc.IsCodeValid(validateCode))
                {
                    string key = EncryptUtils.Md5(userName + email + "emailstep3" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                    //验证通过
                    return Ok(new { IsSuccess = true, UserName = userName, Email = email, ErrorMessage = errorMessage, Key = key });
                }
                else
                {
                    errorMessage = "验证码不正确";
                    //验证码不正确
                    return Ok(new { IsSuccess = false, ErrorMessage = errorMessage });
                }


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }


        /// <summary>
        /// 通过手机找回密码--第一步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByPhoneStep1")]
        public IHttpActionResult FindPasswordByPhoneStep1()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);
                string key = string.Empty;
                if (!string.IsNullOrEmpty(validateCode))
                {
                    if (!VCManager.GetInstanceOfValidateCode().IsCodeValid(validateCode))
                    {
                        errorMessage = "验证码不正确";//NotBindEmail
                        return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                    }
                }
                if (info == null)
                {
                    errorMessage = "请填写正确的用户名";//NotBindPhone
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
                if (info.IsBindPhone)
                {
                    key = EncryptUtils.Md5(userName + "Phonestep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                    successMessage = "BindPhone";
                    return Ok(new { IsSuccess = true, key = key, userName = userName, successMessage = successMessage });
                }
                else
                {
                    errorMessage = "手机号没有绑定";//NotBindPhone
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 通过邮箱找回密码--第二步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByPhoneStep2")]
        public IHttpActionResult FindPasswordByPhoneStep2()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string Phone = RequestUtils.GetPostStringNoSqlAndXss("Phone");
            string keyStep1 = RequestUtils.GetPostStringNoSqlAndXss("key");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            string key = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);

                if (string.IsNullOrEmpty(keyStep1))
                {
                    errorMessage = "ValidateKeyIsNULL";//ValidateKeyIsNULL
                }
                else if (keyStep1 != EncryptUtils.Md5(userName + "Phonestep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                {
                    errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                }

                if (string.IsNullOrEmpty(Phone) || Phone != info.Mobile)
                {
                    errorMessage = "用户输入手机号和绑定手机号不一致";//InputPhoneIsNotBindedPhone
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });


                if (info.IsBindPhone && !string.IsNullOrEmpty(info.Mobile))
                {
                    //发送邮件
                    UserNoticeSettingInfo noticeSettingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPassword));
                    if (noticeSettingInfo != null)
                    {
                        key = EncryptUtils.Md5(userName + info.Mobile + "Phonestep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                        VCManager vc = VCManager.GetInstanceOfValidateCode();
                        string cookieName = vc.GetCookieName();
                        string validateCode = VCManager.CreateValidateCode(false, 6);
                        CookieUtils.SetCookie(cookieName, validateCode, DateTime.Now.AddMinutes(5));//5分钟
                        Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                        dicReplace.Add("userName", info.UserName);
                        bool flag = UserNoticeSettingManager.SendMsg(noticeSettingInfo, info.Mobile, dicReplace, out errorMessage);
                        successMessage = "SendPhone";
                        return Ok(new { IsSuccess = flag, key = key, userName = userName, successMessage = successMessage, errorMessage = errorMessage });
                    }
                    else
                    {
                        errorMessage = "InnerError";
                        return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                    }
                }
                else
                {
                    errorMessage = "NotBindPhone";
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 通过邮箱找回密码--第三步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordByPhoneStep3")]
        public IHttpActionResult FindPasswordByPhoneStep3()
        {
            try
            {
                string groupSN = RequestUtils.GetPostStringNoSqlAndXss("groupSN");
                string userName = RequestUtils.GetPostStringNoSqlAndXss("userName");
                string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
                string Phone = RequestUtils.GetPostStringNoSqlAndXss("Phone");
                string keyStep2 = RequestUtils.GetPostStringNoSqlAndXss("key");

                string errorMessage = string.Empty;
                string successMessage = string.Empty;

                if (string.IsNullOrEmpty(keyStep2))
                {
                    errorMessage = "验证码为空";//ValidateKeyIsNULL
                }
                else if (keyStep2 != EncryptUtils.Md5(userName + Phone + "Phonestep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                {
                    errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                }

                if (string.IsNullOrEmpty(userName))
                {
                    errorMessage = "用户名为空";//userNameIsNULL
                }

                if (string.IsNullOrEmpty(Phone))
                {
                    errorMessage = "手机号为空";//PhoneIsNULL
                }


                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, UserName = userName, Phone = Phone, ErrorMessage = errorMessage });

                VCManager vc = VCManager.GetInstanceOfValidateCode();
                if (vc.IsCodeValid(validateCode))
                {
                    string key = EncryptUtils.Md5(userName + Phone + "Phonestep3" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                    //验证通过
                    return Ok(new { IsSuccess = true, UserName = userName, Phone = Phone, ErrorMessage = errorMessage, Key = key });
                }
                else
                {
                    errorMessage = "验证码不正确";
                    //验证码不正确
                    return Ok(new { IsSuccess = false, ErrorMessage = errorMessage });
                }


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }


        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPassword")]
        public IHttpActionResult FindPassword()
        {
            string newPassword = RequestUtils.GetPostStringNoSqlAndXss("NewPassword");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string email = RequestUtils.GetPostStringNoSqlAndXss("Email");

            string type = RequestUtils.GetPostStringNoSqlAndXss("Type");
            string keyStep = RequestUtils.GetPostStringNoSqlAndXss("Key");


            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            bool isSuccess = false;

            try
            {
                if (type == "phone")
                {

                    if (string.IsNullOrEmpty(newPassword))
                    {
                        errorMessage = "密码为空";//newPasswordIsNULL
                    }

                    if (string.IsNullOrEmpty(userName))
                    {
                        errorMessage = "用户名为空";//userNameIsNULL
                    }

                    if (string.IsNullOrEmpty(email))
                    {
                        errorMessage = "手机号为空";//emailIsNULL
                    }

                    if (string.IsNullOrEmpty(keyStep))
                    {
                        errorMessage = "验证码为空";//ValidateKeyIsNULL
                    }
                    else if (keyStep != EncryptUtils.Md5(userName + email + "Phonestep3" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                    {
                        errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                    }

                }
                else if (type == "email")
                {

                    if (string.IsNullOrEmpty(newPassword))
                    {
                        errorMessage = "密码为空";//newPasswordIsNULL
                    }

                    if (string.IsNullOrEmpty(userName))
                    {
                        errorMessage = "用户名为空";//userNameIsNULL
                    }

                    if (string.IsNullOrEmpty(email))
                    {
                        errorMessage = "邮箱为空";//emailIsNULL
                    }

                    if (string.IsNullOrEmpty(keyStep))
                    {
                        errorMessage = "验证码为空";//ValidateKeyIsNULL
                    }
                    else if (keyStep != EncryptUtils.Md5(userName + email + "emailstep3" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                    {
                        errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                    }

                }
                else if (type == "sqcu")
                {
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        errorMessage = "密码为空";//newPasswordIsNULL
                    }

                    if (string.IsNullOrEmpty(userName))
                    {
                        errorMessage = "用户名为空";//userNameIsNULL
                    }

                    if (string.IsNullOrEmpty(keyStep))
                    {
                        errorMessage = "验证码为空";//ValidateKeyIsNULL
                    }
                    else if (keyStep != EncryptUtils.Md5(userName + "scqustep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                    {
                        errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                    }
                }
                else
                {
                    errorMessage = "type is null";
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, UserName = userName, Email = email, ErrorMessage = errorMessage });

                UserInfo user = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);

                isSuccess = BaiRongDataProvider.UserDAO.ChangePassword(user.UserID, newPassword);

                if (isSuccess)
                {
                    UserManager.RemoveCache(false, string.Empty, UserManager.Current.UserName);
                    UserNoticeSettingInfo noticeInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPasswordAfter));
                    List<string> targetList = new List<string>();
                    targetList.Add(user.UserName);
                    targetList.Add(user.Email);
                    targetList.Add(user.Mobile);

                    Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                    dicReplace.Add("userName", user.UserName);
                    UserNoticeSettingManager.SendMsg(noticeInfo, targetList, dicReplace, out errorMessage);
                }

                var retval = new { IsSuccess = isSuccess, SuccessMessage = successMessage, ErrorMessage = errorMessage };
                return Ok(retval);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }


        }


        /// <summary>
        /// 通过密保找回密码--第一步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordBySCQUStep1")]
        public IHttpActionResult FindPasswordBySCQUStep1()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string validateCode = RequestUtils.GetPostStringNoSqlAndXss("validateCode");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);
                string key = string.Empty;
                if (!string.IsNullOrEmpty(validateCode))
                {
                    if (!VCManager.GetInstanceOfValidateCode().IsCodeValid(validateCode))
                    {
                        errorMessage = "验证码不正确";//NotBindEmail
                        return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                    }
                }
                if (info.IsSetSCQU)
                {
                    key = EncryptUtils.Md5(userName + "scqustep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);
                    successMessage = "SetSCQU";
                    return Ok(new { IsSuccess = true, key = key, userName = userName, successMessage = successMessage });
                }
                else
                {
                    errorMessage = "用户没有设置密码问题验证";//NotSetSCQU
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// 通过密保找回密码--第二步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FindPasswordBySCQUStep2")]
        public IHttpActionResult FindPasswordBySCQUStep2()
        {
            string groupSN = RequestUtils.GetPostStringNoSqlAndXss("GroupSN");
            string userName = RequestUtils.GetPostStringNoSqlAndXss("UserName");
            string keyStep1 = RequestUtils.GetPostStringNoSqlAndXss("KeyStep1");
            string que = RequestUtils.GetPostStringNoSqlAndXss("que");
            string anw = RequestUtils.GetPostStringNoSqlAndXss("anw");
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            string key = string.Empty;
            try
            {
                UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, userName);

                if (string.IsNullOrEmpty(keyStep1))
                {
                    errorMessage = "验证码为空";//ValidateKeyIsNULL
                }
                else if (keyStep1 != EncryptUtils.Md5(userName + "scqustep1" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour))
                {
                    errorMessage = "验证码已过期";//ValidateKeyIsOutDate
                }

                if (string.IsNullOrEmpty(que))
                {
                    errorMessage = "用户没有选择问题";//InputQuestionIsNULL
                }

                if (string.IsNullOrEmpty(anw))
                {
                    errorMessage = "用户输入答案为空";//InputAnwserIsNULL
                }

                string scqu = info.SCQU;
                NameValueCollection nvc = new NameValueCollection();
                nvc = TranslateUtils.ParseJsonStringToNameValueCollection(scqu);
                bool isValidate = false;
                foreach (string k in nvc.Keys)
                {
                    if (k == que && nvc[k] == anw)
                    {
                        isValidate = true;
                        break;
                    }
                }

                if (!isValidate)
                {
                    errorMessage = "用户输入答案错误";
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    return Ok(new { IsSuccess = false, key = key, userName = userName, errorMessage = errorMessage });
                else
                {
                    key = EncryptUtils.Md5(userName + "scqustep2" + DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour);

                    return Ok(new { IsSuccess = true, key = key, userName = userName, successMessage = successMessage, password = info.Password });
                }

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }



        /// <summary>
        /// 获取账号安全级别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AccountSafeLevel")]
        public IHttpActionResult AccountSafeLevel()
        {
            try
            {
                UserInfo user = UserManager.Current;
                string level = UserManager.CalculateAccountSafeLevel();
                string pwdComplex = UserManager.CalculatePasswordComplex();

                var accountSafeLevelPrams = new { IsSuccess = true, Level = level, IsBindEmai = user.IsBindEmail, IsBindPhone = user.IsBindPhone, IsSetSQCU = user.IsSetSCQU, PwdComplex = pwdComplex };
                return Ok(accountSafeLevelPrams);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        [HttpGet]
        [ActionName("IsPersistent")]
        public IHttpActionResult IsPersistent()
        {
            try
            {
                string ticketName = string.Empty;
                string groupSN = string.Empty;
                string userName = string.Empty;
                bool isPersistent = false;
                if (RequestUtils.PublishmentSystemInfo != null)
                    groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
                string cookieStr = CookieUtils.GetCookie(UserAuthConfig.AuthCookieName);
                if (!string.IsNullOrEmpty(cookieStr))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieStr);
                    if (ticket != null)
                    {
                        isPersistent = ticket.IsPersistent;
                        ticketName = ticket.Name;
                        groupSN = ticketName.Split('.')[0];
                        userName = ticketName.Split('.')[1];
                        BaiRongDataProvider.UserDAO.Login(groupSN, userName, isPersistent);

                        //记录登录记录
                        UserLogInfo logInfo = new UserLogInfo(0, userName, PageUtils.GetIPAddress(), DateTime.Now, EUserActionTypeUtils.GetValue(EUserActionType.Login), EUserActionTypeUtils.GetText(EUserActionType.Login));
                        BaiRongDataProvider.UserLogDAO.Insert(logInfo);

                        return Ok(new { IsSuccess = true, UserName = userName, IsPersistent = isPersistent, GroupSN = groupSN });
                    }
                    else
                    {
                        return Ok(new { IsSuccess = false, UserName = "", IsPersistent = isPersistent, GroupSN = "", errorMessage = "Auth is gone" });
                    }
                }
                else
                {
                    return Ok(new { IsSuccess = false, UserName = "", IsPersistent = isPersistent, GroupSN = "", errorMessage = "Auth is gone" });
                }

            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        [HttpGet]
        [ActionName("GetEnablePathList")]
        public IHttpActionResult GetEnablePathList()
        {
            try
            {
                List<string> enableList = new List<string>();
                UserNoticeSettingInfo noticeInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPassword));
                if (noticeInfo.ByEmail)
                    enableList.Add("ByEmail");
                if (noticeInfo.ByPhone)
                    enableList.Add("ByPhone");
                return Ok(new { IsSuccess = true, List = enableList });


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(errorParm);
            }
        }


        [HttpGet]
        [ActionName("GetEnablePathListForMessage")]
        public IHttpActionResult GetEnablePathListForMessage()
        {
            try
            {
                List<string> enableList = new List<string>();
                if (ConfigManager.Additional.MailIsEnabled)
                    enableList.Add("ByEmail");

                if (SMSServerManager.IsEnabled)
                    enableList.Add("ByPhone");
                return Ok(new { IsSuccess = true, List = enableList });


            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, ErrorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        [HttpGet]
        [ActionName("LoadUserProperty")]
        public IHttpActionResult LoadUserProperty()
        {
            try
            {

                string additionalAttributes = string.Empty;
                NameValueCollection pageScripts = new NameValueCollection();

                string groupSN = BaiRongDataProvider.UserDAO.TABLE_NAME;
                StringBuilder sbHtml = new StringBuilder();
                ArrayList styleInfoArrayList = TableStyleManager.GetUserTableStyleInfoArrayList(groupSN);
                if (styleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            string inputHtml = TableInputParser.Parse(styleInfo, styleInfo.AttributeName, UserManager.Current.Attributes, true, false, additionalAttributes, pageScripts, false);
                            string helpText = styleInfo.HelpText;
                            if (!string.IsNullOrEmpty(helpText))
                            {
                                helpText = string.Format("（{0}）", styleInfo.HelpText);
                            }
                            sbHtml.AppendFormat(this.GetFormatString(styleInfo.InputType), styleInfo.DisplayName, inputHtml, helpText);
                        }
                    }
                }

                foreach (string key in pageScripts.Keys)
                {
                    sbHtml.AppendFormat(pageScripts[key]);
                }

                return Ok(new { IsSuccess = true, isAnonymous = RequestUtils.IsAnonymous, UserPropertys = sbHtml.ToString() });
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, isAnonymous = RequestUtils.IsAnonymous, ErrorMessage = ex.Message });
            }
        }
        #region 用户字段辅助方法

        protected virtual string GetFormatString(EInputType inputType)
        {
            string formatString = string.Empty;
            if (inputType == EInputType.TextEditor)
            {
                formatString = this.FormatTextEditor;
            }
            else
            {
                formatString = this.FormatDefault;
            }
            return formatString;
        }

        Hashtable inputTypeWithFormatStringHashtable = new Hashtable();
        public string FormatTextEditor
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<li><span class='mcr_s1'>{0}：</span></li>
<li>{1} {2}</li>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.TextEditor)] = value;
            }
        }

        public string FormatDefault
        {
            get
            {
                string formatString = inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Text)] as string;
                if (string.IsNullOrEmpty(formatString))
                {
                    formatString = @"
<li><span class='mcr_s1'>{0}：</span>{1} {2}</li>
";
                }
                return formatString;
            }
            set
            {
                inputTypeWithFormatStringHashtable[EInputTypeUtils.GetValue(EInputType.Text)] = value;
            }
        }
        #endregion


        [HttpGet]
        [ActionName("GetUserInvoices")]
        public IHttpActionResult GetUserInvoices()
        {
            string userName = RequestUtils.Current.UserName;

            InvoiceInfo invoice = null;


            List<InvoiceInfo> invoices = DataProviderB2C.InvoiceDAO.GetInvoiceInfoList("", userName);

            if (invoices.Count > 0)
            {
                invoice = invoices[0];
            }
            var invoicesParameter = new { IsSuccess = true, Invoice = invoice, Invoices = invoices };
            return Ok(invoicesParameter);
        }

        [HttpGet]
        [ActionName("GetUserInvoicesOne")]
        public IHttpActionResult GetUserInvoicesOne()
        {
            string userName = RequestUtils.Current.UserName;
            int id = TranslateUtils.ToInt(RequestUtils.GetQueryString("id"));
            InvoiceInfo invoice = DataProviderB2C.InvoiceDAO.GetInvoiceInfo(id);
            var invoicesParameter = new { IsSuccess = true, Invoice = invoice };
            return Ok(invoicesParameter);
        }

        [HttpGet]
        [ActionName("GetConsigneeList")]
        public IHttpActionResult GetConsigneeList()
        {
            string userName = RequestUtils.Current.UserName;

            ConsigneeInfo consignee = null;


            List<ConsigneeInfo> consignees = DataProviderB2C.ConsigneeDAO.GetConsigneeInfoList("", userName);

            if (consignees.Count > 0)
            {
                consignee = consignees[0];
            }
            var invoicesParameter = new { IsSuccess = true, Consignee = consignee, Consignees = consignees };
            return Ok(invoicesParameter);
        }


        [HttpGet]
        [ActionName("GetConsigneeOne")]
        public IHttpActionResult GetConsigneeOne()
        {
            string userName = RequestUtils.Current.UserName;
            int id = TranslateUtils.ToInt(RequestUtils.GetQueryString("id"));
            ConsigneeInfo consignee = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(id);
            var invoicesParameter = new { IsSuccess = true, Consignee = consignee };
            return Ok(invoicesParameter);
        }

        [HttpGet]
        [ActionName("getUserShipmentOne")]
        public IHttpActionResult getUserShipmentOne()
        {
            string userName = RequestUtils.Current.UserName;
            int id = TranslateUtils.ToInt(RequestUtils.GetQueryString("id"));
            ShipmentInfo shipment = DataProviderB2C.ShipmentDAO.GetShipmentInfo(id);
            var invoicesParameter = new { IsSuccess = true, Shipment = shipment };
            return Ok(invoicesParameter);
        }

        [HttpGet]
        [ActionName("GetFollowList")]
        public IHttpActionResult GetFollowList()
        {

            try
            {
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = DataProviderB2C.FollowDAO.GetCount(RequestUtils.CurrentUserName);
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<FollowInfo> list = DataProviderB2C.FollowDAO.GetUserFollowsByPage(RequestUtils.CurrentUserName, pageIndex, prePageNum);

                List<FollowInfo> followList = new List<FollowInfo>();
                foreach (FollowInfo followInfo in list)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(followInfo.PublishmentSystemID);
                    followInfo.ContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, followInfo.ChannelID, followInfo.ContentID);
                    if (followInfo.ContentInfo != null)
                    {
                        followInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, followInfo.ContentInfo, publishmentSystemInfo.Additional.VisualType);
                        followInfo.ContentInfo.ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, followInfo.ContentInfo.ImageUrl));
                        followList.Add(followInfo);
                    }
                }


                var newList = from a in followList
                              select new
                              {
                                  ID = a.ID,
                                  GoodsPublishmentSystemID = a.ContentInfo.PublishmentSystemID,
                                  GoodsNodeID = a.ContentInfo.NodeID,
                                  GoodsID = a.ContentInfo.ID,
                                  ImageUrl = a.ContentInfo.ImageUrl,
                                  GoodsName = a.ContentInfo.Title,
                                  GoodsPrice = a.ContentInfo.PriceSale,
                                  GoodsCommentsCount = a.ContentInfo.Comments,
                                  GoodsPraiseRate = "100%",
                                  NavigationUrl = a.NavigationUrl,
                                  FirstGoodID = GetDefaultGoodsID(a.ContentInfo.PublishmentSystemID, a.ContentInfo.ID)
                              };
                var userFollowListParms = new { IsSuccess = true, FollowList = newList, PageJson = pageJson };
                return Ok(userFollowListParms);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }


        [HttpGet]
        [ActionName("RemoveUserFollow")]
        public IHttpActionResult RemoveUserFollow()
        {

            try
            {
                string ids = RequestUtils.GetQueryStringNoSqlAndXss("ids");
                if (!string.IsNullOrEmpty(ids))
                {
                    var idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (idArray.Length > 0)
                    {
                        var idArraylist = new ArrayList();

                        foreach (var item in idArray)
                        {
                            idArraylist.Add(item);
                        }
                        DataProviderB2C.FollowDAO.Delete(idArraylist);

                        return Ok(new { IsSuccess = true });
                    }
                }
                return Ok(new { IsSuccess = false, errorMessage = "未选择任何项" });
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }


        [HttpGet]
        [ActionName("GetUserCart")]
        public IHttpActionResult GetUserCart()
        {
            List<Cart> carts = new List<Cart>();
            string userName = RequestUtils.CurrentUserName;
            string sessionID = PageUtils.SessionID;
            PublishmentSystemInfo firstPublishmentSystemInfo = null;
            List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(sessionID, userName);
            List<CartInfo> cartInfoListAvaliable = new List<CartInfo>();
            foreach (CartInfo cartInfo in cartInfoList)
            {

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(cartInfo.PublishmentSystemID);
                if (firstPublishmentSystemInfo == null)
                    firstPublishmentSystemInfo = publishmentSystemInfo;
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
            if (firstPublishmentSystemInfo != null)
            {
                PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                {
                    PublishmentSystemID = firstPublishmentSystemInfo.PublishmentSystemID,
                    PublishmentSystemName = firstPublishmentSystemInfo.PublishmentSystemName,
                    PublishmentSystemUrl = firstPublishmentSystemInfo.PublishmentSystemUrl
                };

                var userCartListParms = new { IsSuccess = true, Carts = carts, PublishmentSystemInfo = publishmentSystemParamter };
                return Ok(userCartListParms);
            }
            else
            {
                ArrayList publishmentSystemIDArray = new ArrayList();
                publishmentSystemIDArray.AddRange(PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.B2C));
                publishmentSystemIDArray.AddRange(PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.WeixinB2C));
                int thePublishmentSystemID = 0;
                if (publishmentSystemIDArray.Count > 0)
                    thePublishmentSystemID = TranslateUtils.ToInt(publishmentSystemIDArray[0].ToString());
                PublishmentSystemInfo thePublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(thePublishmentSystemID);
                PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                {
                    PublishmentSystemID = thePublishmentSystemInfo.PublishmentSystemID,
                    PublishmentSystemName = thePublishmentSystemInfo.PublishmentSystemName,
                    PublishmentSystemUrl = thePublishmentSystemInfo.PublishmentSystemUrl
                };

                var userCartListParms = new { IsSuccess = true, Carts = carts, PublishmentSystemInfo = publishmentSystemParamter };
                return Ok(userCartListParms);
            }
        }

        //获取商品默认规格项
        private int GetDefaultGoodsID(int PublishmentSystemInfoID, int ContentID)
        {
            List<GoodsInfo> goodsInfoListInDB = DataProviderB2C.GoodsDAO.GetGoodsInfoList(PublishmentSystemInfoID, ContentID);
            GoodsInfo firstGoods = new GoodsInfo();
            if (goodsInfoListInDB.Count > 0)
                return goodsInfoListInDB[0].GoodsID;
            return 0;
        }


        [HttpGet]
        [ActionName("FollowAddToCart")]
        public IHttpActionResult FollowAddToCart()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                string ids = RequestUtils.GetQueryStringNoSqlAndXss("ids");
                if (!string.IsNullOrEmpty(ids))
                {
                    var idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (idArray.Length > 0)
                    {
                        foreach (var followId in idArray)
                        {
                            var follow = DataProviderB2C.FollowDAO.GetFollowInfo(TranslateUtils.ToInt(followId));

                            var followFirstGoodsId = GetDefaultGoodsID(follow.PublishmentSystemID, follow.ContentID);
                            if (followFirstGoodsId != 0)
                            {
                                CartInfo cartInfo = new CartInfo(0, follow.PublishmentSystemID, RequestUtils.CurrentUserName, PageUtils.SessionID, follow.ChannelID, follow.ContentID, followFirstGoodsId, 1, DateTime.Now);
                                DataProviderB2C.CartDAO.InsertOrUpdate(cartInfo);
                            }
                        }

                        parameter = new Parameter { IsSuccess = true };

                    }
                }
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }



        [HttpGet]
        [ActionName("GetHistoryList")]
        public IHttpActionResult GetHistoryList()
        {

            try
            {
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = DataProviderB2C.HistoryDAO.GetCount(RequestUtils.CurrentUserName);
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<HistoryInfo> list = DataProviderB2C.HistoryDAO.GetUserHistorysByPage(RequestUtils.CurrentUserName, pageIndex, prePageNum);

                List<HistoryInfo> historyList = new List<HistoryInfo>();
                foreach (HistoryInfo historyInfo in list)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(historyInfo.PublishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(historyInfo.PublishmentSystemID, historyInfo.ChannelID);
                    historyInfo.ContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, historyInfo.ChannelID, historyInfo.ContentID);
                    if (publishmentSystemInfo != null && nodeInfo != null && historyInfo.ContentInfo != null)
                    {
                        historyInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, historyInfo.ContentInfo, publishmentSystemInfo.Additional.VisualType);
                        historyInfo.ContentInfo.ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, historyInfo.ContentInfo.ImageUrl));
                        historyList.Add(historyInfo);
                    }
                }


                var newList = from a in historyList
                              select new
                              {
                                  ID = a.ID,
                                  GoodsPublishmentSystemID = a.ContentInfo.PublishmentSystemID,
                                  GoodsNodeID = a.ContentInfo.NodeID,
                                  GoodsID = a.ContentInfo.ID,
                                  ImageUrl = a.ContentInfo.ImageUrl,
                                  GoodsName = a.ContentInfo.Title,
                                  GoodsPrice = a.ContentInfo.PriceSale,
                                  GoodsCommentsCount = a.ContentInfo.Comments,
                                  GoodsPraiseRate = "100%",
                                  NavigationUrl = a.NavigationUrl,
                                  FirstGoodID = GetDefaultGoodsID(a.ContentInfo.PublishmentSystemID, a.ContentInfo.ID)
                              };
                var userHistoryListParms = new { IsSuccess = true, HistoryList = newList, PageJson = pageJson };
                return Ok(userHistoryListParms);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }



        [HttpGet]
        [ActionName("HistoryAddToCart")]
        public IHttpActionResult HistoryAddToCart()
        {
            var parameter = new Parameter { IsSuccess = true };

            try
            {
                string ids = RequestUtils.GetQueryStringNoSqlAndXss("ids");
                if (!string.IsNullOrEmpty(ids))
                {
                    var idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (idArray.Length > 0)
                    {
                        foreach (var historyId in idArray)
                        {
                            var history = DataProviderB2C.HistoryDAO.GetHistoryInfo(TranslateUtils.ToInt(historyId));

                            var historyFirstGoodsId = GetDefaultGoodsID(history.PublishmentSystemID, history.ContentID);
                            if (historyFirstGoodsId != 0)
                            {
                                CartInfo cartInfo = new CartInfo(0, history.PublishmentSystemID, RequestUtils.CurrentUserName, PageUtils.SessionID, history.ChannelID, history.ContentID, historyFirstGoodsId, 1, DateTime.Now);
                                DataProviderB2C.CartDAO.InsertOrUpdate(cartInfo);
                            }
                        }

                        parameter = new Parameter { IsSuccess = true };

                    }
                }
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }


        [HttpGet]
        [ActionName("RemoveUserHistory")]
        public IHttpActionResult RemoveUserHistory()
        {

            try
            {
                string ids = RequestUtils.GetQueryStringNoSqlAndXss("ids");
                if (!string.IsNullOrEmpty(ids))
                {
                    var idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (idArray.Length > 0)
                    {
                        var idArraylist = new ArrayList();

                        foreach (var item in idArray)
                        {
                            idArraylist.Add(item);
                        }
                        DataProviderB2C.HistoryDAO.Delete(idArraylist);

                        return Ok(new { IsSuccess = true });
                    }
                }
                return Ok(new { IsSuccess = false, errorMessage = "未选择任何项" });
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        [HttpGet]
        [ActionName("GetHomeUrl")]
        public IHttpActionResult GetHomeUrl()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
                if (publishmentSystemInfo != null)
                    return Ok(new { isSuccess = true, homeUrl = publishmentSystemInfo.PublishmentSystemDir });
                else
                    return Ok(new { isSuccess = false, errorMessage = "没有启用会员中心" });
            }
            catch (Exception ex)
            {
                return Ok(new { isSuccess = false, errorMessage = ex.Message });
            }
        }


        [HttpGet]
        [ActionName("GetUserAllMessage")]
        public IHttpActionResult GetUserAllMessage()
        {
            try
            {
                string userName = UserManager.Current.UserName;//用户
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
                int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
                int total = 0;
                total = BaiRongDataProvider.UserMessageDAO.GetCount(" MessageType = 'SystemAnnouncement' OR (MessageType = 'System'" + " AND MessageTo = '" + userName + "')");
                string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

                List<UserMessageInfo> list = BaiRongDataProvider.UserMessageDAO.GetReciveMessageInfoList(userName, EUserMessageType.SystemAnnouncement, pageIndex, prePageNum);

                var list1 = BaiRongDataProvider.UserMessageDAO.GetReciveMessageInfoList(userName, EUserMessageType.System, pageIndex, prePageNum);
                list.AddRange(list1);


                var userMessageListParm = new { IsSuccess = true, userMessageList = list, PageJson = pageJson };
                return Ok(userMessageListParm);
            }
            catch (Exception ex)
            {
                var errorParm = new { IsSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }

        }

    }
}
