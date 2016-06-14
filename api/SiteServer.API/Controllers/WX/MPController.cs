using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Net;
using BaiRong.Model;
using Senparc.Weixin.MP;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.WX
{
    public class MPController : ApiController
    {
        [HttpGet]
        [ActionName("url")]
        public HttpResponseMessage GetUrl(int id)
        {
            try
            {
                string signature = HttpContext.Current.Request.QueryString["signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];
                string echostr = HttpContext.Current.Request.QueryString["echostr"];

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(id);

                if (accountInfo != null && !string.IsNullOrEmpty(signature))
                {
                    accountInfo.IsBinding = true;
                    DataProviderWX.AccountDAO.Update(accountInfo);
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(echostr)
                    };
                }
                else
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("failed:id=" + id)
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex, "微信绑定错误");
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("failed:id=" + id)
            };
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("url")]
        public HttpResponseMessage PostUrl(int id)
        {
            try
            {
                //LogUtils.AddLog(string.Empty, "微信Post开始", string.Format("publishmentSystemID:{0}", id));

                int publishmentSystemID = id;
                if (publishmentSystemID == 0)
                {
                    string userName = HttpContext.Current.Request.QueryString["openId"];
                    List<int> publishmentSystemIDList = BaiRongDataProvider.AdministratorDAO.GetPublishmentSystemIDList(userName);
                    if (publishmentSystemIDList != null && publishmentSystemIDList.Count > 0)
                    {
                        publishmentSystemID = publishmentSystemIDList[0];
                    }
                }

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(publishmentSystemID);

                bool isQCloud = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isQCloud"]);

                if (!isQCloud)
                {
                    string signature = HttpContext.Current.Request.QueryString["signature"];
                    string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                    string nonce = HttpContext.Current.Request.QueryString["nonce"];
                    string echostr = HttpContext.Current.Request.QueryString["echostr"];

                    if (!CheckSignature.Check(signature, timestamp, nonce, accountInfo.Token))
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("参数错误")
                        };
                    }
                }

                var messageHandler = new GexiaMessageHandler(accountInfo, HttpContext.Current.Request.InputStream, 10);

                messageHandler.Execute();//执行微信处理过程

                if (messageHandler.ResponseDocument != null)
                {
                    //LogUtils.AddLog(string.Empty, "微信Post成功", string.Format("Response:{0}", messageHandler.ResponseDocument.ToString()));

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(messageHandler.ResponseDocument.ToString())
                    };
                }
                else
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("")
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex, "微信Post错误，应用ID：" + id);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("failed:id=" + id)
            };
        }


        #region 处理百度直达号生成页面

        //[HttpGet]
        //public HttpResponseMessage resetPage()
        //{
        //    int count = 0;

        //    try
        //    {
        //        string retval = "";

        //        string publishmentSystemDir = @"";

        //        string[] publishmentSystemArry = publishmentSystemDir.Split(',');

        //        for (int i = 0; i < publishmentSystemArry.Length; i++)
        //        {
        //            string[] respage = publishmentSystemArry[i].Split('/');

        //            string getDataAppID = respage[0].Trim();
        //            string getUrl = respage[1].Trim();
        //            string getParame = respage[2].Trim();

        //            string postUrl = "http://" + getUrl + "/siteserver/stl/background_executeOnce.aspx?publishmentSystemDir=" + getParame + "&dataAppID=" + getDataAppID;
        //            HttpContext.Current.Response.Write(retval);

        //            WebClientUtils.Post(postUrl, new NameValueCollection(), out retval);
        //            count++;
        //            HttpContext.Current.Response.Write(retval);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Response.Write(ex.Message);
        //        HttpContext.Current.Response.Write(count);
        //    }

        //    HttpContext.Current.Response.Write(count);

        //    return new HttpResponseMessage();

        //}

        #endregion
    }
}
