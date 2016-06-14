using BaiRong.Core;
using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using CodeScales.Http.Methods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteServer.CRM.Core
{
    public class MessageManager
    {
        public static bool SendSMS(string mobile, string message, out string errorMessage)
        {
            message += "【SiteYun】";
            errorMessage = string.Empty;
            string username = "api";
            string password = "key-eb49d9f8e8a055d0106a07b9d7d4771a";
            string url = "https://sms-api.luosimao.com/v1/send.json";

            byte[] byteArray = Encoding.UTF8.GetBytes("mobile=" + mobile + "&message=" + message);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(username + ":" + password));
            webRequest.Headers.Add("Authorization", auth);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader php = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string returnString = php.ReadToEnd();

            if (returnString == @"{""error"":0,""msg"":""ok""}")
            {
                return true;
            }
            else
            {
                errorMessage = returnString.Substring(returnString.IndexOf("msg") + 6);
                errorMessage = errorMessage.Replace(@"""}", string.Empty);
                errorMessage = DecodeEncodedNonAsciiCharacters(errorMessage);
                return false;
            }
        }

        /// <summary>
        /// 使用sendcloud的WebAPI， 详见a href= "http://sendcloud.sohu.com/api-doc/web-api-ref">
        ///  </summary>
        public static bool SendMail(string email, string html, string filePath, out string errorMessage)
        {
            errorMessage = string.Empty;
            string subject = "SiteYun云建站通知信息";

            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send.xml"));

            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;
            //不同于登录SendCloud站点的帐号，您需要登录后台创建发信子帐号，使用子帐号和密码才可以进行邮件的发送。
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "postmaster@siteyunservice.sendcloud.org"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", "F1UorAm4"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", "service@siteyun.cn"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", "SiteYun云建站"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", email));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", subject));

            html = string.Format(@"
<div style=""font:Verdana normal 14px;color:#000;"">    
    <table cellpadding=""0"" cellspacing=""0"" width=""546"" style=""font-family:'Microsoft YaHei';
        font-size: 12px;  border: 1px solid #ddd;border-bottom:0px solid #ddd;"">
        <tbody><tr>
          <td colspan=""2"" style=""padding-left:40px;padding-top:20px;"">
            <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';width:536px;"">
                <tbody><tr>
                      <td valign=""middle"">
                <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';"">
                    <tbody><tr>
                        <td style=""font-size:18px;font-family:'Microsoft YaHei';"">
                             <a href=""http://a.siteyun.cn"" target=""_blank""><img src=""http://static.siteserver.cn/utils/email/siteyun/logo.png"" border=""0""></a>
                        </td>
                    </tr>
                </tbody></table>
            </td>
            <td valign=""middle"" style="""" align=""right"">
                <table cellpadding=""0"" cellspacing=""0"">
                    <tbody><tr>
                    <td style=""text-align:right;font-family:Arial;"">                                                                       
                    </td>
                    </tr>
                </tbody></table>
            </td>
                </tr>
            </tbody></table>
          </td>
        </tr>        
        <tr>
            <td colspan=""2"" style=""padding:20px 40px 0px 40px;"">
                <table cellpadding=""0"" cellspacing=""0"" width=""540"">
                    <tbody><tr>
                        <td style=""border-bottom:1px solid #999;height:2px;"">
                        </td>
                    </tr>
                </tbody></table>
            </td>
        </tr>
         <tr><td style=""text-align: left; padding: 10px 40px 0px 45px;"" align=""center""><font style=""font-family:'Microsoft YaHei';font-size:16px;"">
{0}
          </font>
                </td>
         </tr>
          <tr>
                <td style=""padding:10px 40px 0px 45px;"" align=""center""><br></td></tr>
        <tr>
                    <td colspan=""2"" style=""height:90px; width:536px;background-color:#E7E7E7;border:1px solid #e4e4e4;"">
                        <table style=""padding:26px 26px 20px;"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
                            <tbody><tr>
                                <td valign=""top"">
                                    <table style=""height:36px;font-size:12px;color:#999999;font-family:'Microsoft YaHei';"" cellpadding=""0"" cellspacing=""0"">
                                        <tbody><tr>
                                            <td>
                                                如果您在使用中有任何的疑问和建议，欢迎您给我们 <a target=""_blank"" style=""text-decoration: none;color:#0066cc;font-family:'Microsoft YaHei';"" href=""http://a.siteyun.cn""> 反馈意见</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                联系电话：4008-770-550 QQ：4008770550
                                            </td>
                                        </tr>
                                    </tbody></table>
                                </td>
                                <td align=""right"" valign=""top"">
                                    <a target=""_blank"" href=""http://a.siteyun.cn"">
                                        <img style=""border:0"" src=""http://static.siteserver.cn/utils/email/siteyun/logo_email_footer.png"">
                                     </a>
                                </td>
                            </tr>
                        </tbody></table>
                    </td>
                </tr>                    
    </tbody></table>      
    <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';width:625px;"">
                    <tbody><tr>
            <td align=""left"" style=""font-size:12px;width:295px;color:#666;padding:10px 0px 20px 0px;"">
                
            </td>
            <td align=""right"" style=""font-size:12px;width:315px;color:#666;padding:10px 0px 20px 0px;"">
                   <font style=""display: inline-block;"">更改SiteYun的Email通知设置，请<a target=""_blank"" style=""text-decoration: none;color:#0066cc;font-family:'Microsoft YaHei';"" href=""http://a.siteyun.cn/home""> 点击这里！</a></font>
            </td>
        </tr>
            </tbody></table>
            <img src=""http://static.siteserver.cn/utils/email/siteyun/dot.gif"" style=""border:none;"">


</div>
", StringUtils.ReplaceNewlineToBR(html));

            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "html", html));

            if (!string.IsNullOrEmpty(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = PathUtils.GetFileName(filePath);
                UTF8FileBody fileBody = new UTF8FileBody("file1", fileName, fileInfo);
                multipartEntity.AddBody(fileBody);
            }

            CodeScales.Http.Methods.HttpResponse response = client.Execute(postMethod);

            if (response.ResponseCode == 200)
            {
                return true;
            }
            else
            {
                errorMessage = EntityUtils.ToString(response.Entity);
                return false;
            }
            //Console.WriteLine("Response Code: " + response.ResponseCode);
            //Console.WriteLine("Response Content: " + EntityUtils.ToString(response.Entity));
        }

//        public static bool SendMail2(string email, string html, string filePath, out string errorMessage)
//        {
//            errorMessage = string.Empty;
//            string subject = "SiteYun云建站通知信息";
//            html = string.Format(@"
//<div style=""font:Verdana normal 14px;color:#000;"">    
//    <table cellpadding=""0"" cellspacing=""0"" width=""546"" style=""font-family:'Microsoft YaHei';
//        font-size: 12px;  border: 1px solid #ddd;border-bottom:0px solid #ddd;"">
//        <tbody><tr>
//          <td colspan=""2"" style=""padding-left:40px;padding-top:20px;"">
//            <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';width:536px;"">
//                <tbody><tr>
//                      <td valign=""middle"">
//                <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';"">
//                    <tbody><tr>
//                        <td style=""font-size:18px;font-family:'Microsoft YaHei';"">
//                             <a href=""http://a.siteyun.cn"" target=""_blank""><img src=""http://static.siteserver.cn/utils/email/siteyun/logo.png"" border=""0""></a>
//                        </td>
//                    </tr>
//                </tbody></table>
//            </td>
//            <td valign=""middle"" style="""" align=""right"">
//                <table cellpadding=""0"" cellspacing=""0"">
//                    <tbody><tr>
//                    <td style=""text-align:right;font-family:Arial;"">                                                                       
//                    </td>
//                    </tr>
//                </tbody></table>
//            </td>
//                </tr>
//            </tbody></table>
//          </td>
//        </tr>        
//        <tr>
//            <td colspan=""2"" style=""padding:20px 40px 0px 40px;"">
//                <table cellpadding=""0"" cellspacing=""0"" width=""540"">
//                    <tbody><tr>
//                        <td style=""border-bottom:1px solid #999;height:2px;"">
//                        </td>
//                    </tr>
//                </tbody></table>
//            </td>
//        </tr>
//         <tr><td style=""text-align: left; padding: 10px 40px 0px 45px;"" align=""center""><font style=""font-family:'Microsoft YaHei';font-size:16px;"">
//{0}
//          </font>
//                </td>
//         </tr>
//          <tr>
//                <td style=""padding:10px 40px 0px 45px;"" align=""center""><br></td></tr>
//        <tr>
//                    <td colspan=""2"" style=""height:90px; width:536px;background-color:#E7E7E7;border:1px solid #e4e4e4;"">
//                        <table style=""padding:26px 26px 20px;"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
//                            <tbody><tr>
//                                <td valign=""top"">
//                                    <table style=""height:36px;font-size:12px;color:#999999;font-family:'Microsoft YaHei';"" cellpadding=""0"" cellspacing=""0"">
//                                        <tbody><tr>
//                                            <td>
//                                                如果您在使用中有任何的疑问和建议，欢迎您给我们 <a target=""_blank"" style=""text-decoration: none;color:#0066cc;font-family:'Microsoft YaHei';"" href=""http://a.siteyun.cn""> 反馈意见</a>
//                                            </td>
//                                        </tr>
//                                        <tr>
//                                            <td>
//                                                联系电话：4008-770-550 QQ：4008770550
//                                            </td>
//                                        </tr>
//                                    </tbody></table>
//                                </td>
//                                <td align=""right"" valign=""top"">
//                                    <a target=""_blank"" href=""http://a.siteyun.cn"">
//                                        <img style=""border:0"" src=""http://static.siteserver.cn/utils/email/siteyun/logo_email_footer.png"">
//                                     </a>
//                                </td>
//                            </tr>
//                        </tbody></table>
//                    </td>
//                </tr>                    
//    </tbody></table>      
//    <table cellpadding=""0"" cellspacing=""0"" style=""font-family:'Microsoft YaHei';width:625px;"">
//                    <tbody><tr>
//            <td align=""left"" style=""font-size:12px;width:295px;color:#666;padding:10px 0px 20px 0px;"">
//                
//            </td>
//            <td align=""right"" style=""font-size:12px;width:315px;color:#666;padding:10px 0px 20px 0px;"">
//                   <font style=""display: inline-block;"">更改SiteYun的Email通知设置，请<a target=""_blank"" style=""text-decoration: none;color:#0066cc;font-family:'Microsoft YaHei';"" href=""http://a.siteyun.cn/home""> 点击这里！</a></font>
//            </td>
//        </tr>
//            </tbody></table>
//            <img src=""http://static.siteserver.cn/utils/email/siteyun/dot.gif"" style=""border:none;"">
//
//
//</div>
//", StringUtils.ReplaceNewlineToBR(html));

//            RestClient client = new RestClient();
//            client.BaseUrl = "https://api.mailgun.net/v2";
//            client.Authenticator = new HttpBasicAuthenticator("api", "key-11c00-brqcpu1gzwkppg847pnanevj-0");
//            RestRequest request = new RestRequest();
//            request.AddParameter("domain", "siteyun.com", ParameterType.UrlSegment);
//            request.Resource = "{domain}/messages";
//            request.AddParameter("from", "SiteYun 云建站 <service@siteyun.com>");
//            request.AddParameter("to", email);
//            request.AddParameter("subject", subject);
//            request.AddParameter("html", html);
//            request.Method = Method.POST;
//            client.ExecuteAsync(request, null);

//            return true;
//            //if (response.ResponseCode == 200)
//            //{
//            //    return true;
//            //}
//            //else
//            //{
//            //    errorMessage = EntityUtils.ToString(response.Entity);
//            //    return false;
//            //}
//            //Console.WriteLine("Response Code: " + response.ResponseCode);
//            //Console.WriteLine("Response Content: " + EntityUtils.ToString(response.Entity));
//        }

        public class UTF8FileBody : Body
        {
            private string m_name;
            private string m_fileName;
            private byte[] m_content;
            private string m_mimeType;

            public UTF8FileBody(string name, string fileName, FileInfo fileInfo, string mimeType)
                : this(name, fileName, fileInfo)
            {
                this.m_mimeType = mimeType;
            }

            public UTF8FileBody(string name, string fileName, FileInfo fileInfo)
            {
                this.m_name = name;
                this.m_fileName = fileName;
                this.m_content = null;

                if (fileInfo == null)
                {
                    this.m_content = new byte[0];
                }
                else
                {
                    using (BinaryReader reader = new BinaryReader(fileInfo.OpenRead()))
                    {
                        this.m_content = reader.ReadBytes((int)reader.BaseStream.Length);
                    }
                }
            }

            public byte[] GetContent(string boundry)
            {
                var bytes = new List<byte>();

                string paramBoundry = "--" + boundry + "\r\n";
                string stringParam = "Content-Disposition: form-data; name=\"" + m_name + "\"; filename=\"" + m_fileName + "\"\r\n";
                string paramEnd = null;
                if (m_mimeType != null)
                    paramEnd = "Content-Type: " + m_mimeType + "\r\n\r\n";
                else
                    paramEnd = "Content-Type: application/octet-stream\r\n\r\n";

                string foo = paramBoundry + stringParam + paramEnd;
                bytes.AddRange(Encoding.UTF8.GetBytes(paramBoundry + stringParam + paramEnd));
                bytes.AddRange(m_content);
                bytes.AddRange(Encoding.UTF8.GetBytes("\r\n"));
                return bytes.ToArray();
            }
        }

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }
}
