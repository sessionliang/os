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
                LogUtils.AddErrorLog(ex, "΢�Ű󶨴���");
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("failed:id=" + id)
            };
        }

        /// <summary>
        /// �û�������Ϣ��΢��ƽ̨�Զ�Postһ������������ȴ���ӦXML��
        /// PS���˷���Ϊ�򻯷�����Ч����OldPostһ�¡�
        /// v0.8֮��İ汾���Խ��Senparc.Weixin.MP.MvcExtension��չ����ʹ��WeixinResult����MiniPost������
        /// </summary>
        [HttpPost]
        [ActionName("url")]
        public HttpResponseMessage PostUrl(int id)
        {
            try
            {
                //LogUtils.AddLog(string.Empty, "΢��Post��ʼ", string.Format("publishmentSystemID:{0}", id));

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
                            Content = new StringContent("��������")
                        };
                    }
                }

                var messageHandler = new GexiaMessageHandler(accountInfo, HttpContext.Current.Request.InputStream, 10);

                messageHandler.Execute();//ִ��΢�Ŵ������

                if (messageHandler.ResponseDocument != null)
                {
                    //LogUtils.AddLog(string.Empty, "΢��Post�ɹ�", string.Format("Response:{0}", messageHandler.ResponseDocument.ToString()));

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
                LogUtils.AddErrorLog(ex, "΢��Post����Ӧ��ID��" + id);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("failed:id=" + id)
            };
        }


        #region ����ٶ�ֱ�������ҳ��

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
