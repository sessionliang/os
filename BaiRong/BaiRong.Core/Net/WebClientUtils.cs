using System;
using System.Collections;
using System.Text;
using System.Net;

using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Core.Net
{
	public class WebClientUtils
	{
		// 获取指定网页的HTML代码
		public static string GetRemoteFileSource(string Url, ECharset charset, string cookieString)
		{
			try
			{
                Uri uri = new Uri(PageUtils.AddProtocolToUrl(Url.Trim()));
				HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(uri);
				if (!string.IsNullOrEmpty(cookieString))
				{
					hwReq.Headers.Add("Cookie", cookieString); 
				}
				HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();
				hwReq.Method = "Get";
				//hwReq.ContentType = "text/html";
				hwReq.KeepAlive = false;
                
				System.IO.StreamReader reader = new System.IO.StreamReader(hwRes.GetResponseStream(), ECharsetUtils.GetEncoding(charset));
				return reader.ReadToEnd();
			}
			catch
			{
				throw new Exception(string.Format("页面地址“{0}”无法访问！", Url));
			}
		}

        public static string GetRemoteFileSource(string url, ECharset charset)
		{
			return GetRemoteFileSource(url, charset, string.Empty);
		}

        public static HttpStatusCode GetRemoteUrlStatusCode(string url)
        {
            Uri uri = new Uri(PageUtils.AddProtocolToUrl(url.Trim()));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "HEAD";                           //设置请求方式为请求头，这样就不需要把整个网页下载下来 
            req.KeepAlive = false;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            return res.StatusCode;
        }

		public static bool SaveRemoteFileToLocal(string remoteUrl, string localFileName)
		{
			try
			{
				WebClient myWebClient = new WebClient();
				myWebClient.DownloadFile(remoteUrl, localFileName);
			}
			catch
			{
				throw new Exception(string.Format("页面地址“{0}”无法访问！", remoteUrl));
			}
			return true;
		}

        public static bool Post(string url, NameValueCollection arguments, out string retval)
        {
            retval = string.Empty;
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (string key in arguments.Keys)
                {
                    builder.AppendFormat("{0}={1}&", key, arguments[key]);
                }
                if (builder.Length > 0) builder.Length -= 1;
                byte[] postData = Encoding.UTF8.GetBytes(builder.ToString());
                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header
                byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                retval = Encoding.UTF8.GetString(responseData);//解码

                return true;
            }
            catch(Exception ex)
            {
                retval = ex.Message;
            }
            return false;
        }

        public static bool Post(string url, string requestData, out string retval)
        {
            retval = string.Empty;
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(requestData);
                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header
                byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                retval = Encoding.UTF8.GetString(responseData);//解码

                return true;
            }
            catch (Exception ex)
            {
                retval = ex.Message;
            }
            return false;
        }

        public static bool Get(string url, string requestData, out string retval)
        {
            retval = string.Empty;
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(requestData);
                WebClient webClient = new WebClient();
                byte[] responseData = webClient.UploadData(url, "GET", postData);//得到返回字符流  
                retval = Encoding.UTF8.GetString(responseData);//解码

                return true;
            }
            catch (Exception ex)
            {
                retval = ex.Message;
            }
            return false;
        }
	}
}
