using BaiRong.Core;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Net;
using Senparc.Weixin.MP.Entities;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteServer.WeiXin.Core
{
    public class WifiManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/wifi/img/banner.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        private static string GetWifiUrl(WifiInfo wifiInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl("http://vvvv.wechatwifi.com/verify.wifi2"));
        }

        public static string GetWifiUrl(WifiInfo wifiInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            UInt32 opTime = (UInt32)(DateTime.Now - startTime).TotalSeconds;

            AccountInfo accountInfo = WeiXinManager.GetAccountInfo(wifiInfo.PublishmentSystemID);

            string targetID = accountInfo.WeChatID;

            string plainStr = "source_id=" + wxOpenID + "&target_id=" + targetID + "&type=1&op_time=" + opTime;

            string encrypt = DESEncrypt(plainStr.Trim());

            attributes.Add("a", encrypt);
            attributes.Add("b", "0pqqYvq7j0xqqmq5");
            attributes.Add("c", "1");
            return PageUtils.AddQueryString(GetWifiUrl(wifiInfo), attributes);
        }
    

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<WifiInfo> wifiInfoList = DataProviderWX.WifiDAO.GetWifiInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (WifiInfo wifiInfo in wifiInfoList)
            {
                Article article = null;

                if (wifiInfo != null)
                {
                    string imageUrl = WifiManager.GetImageUrl(publishmentSystemInfo, wifiInfo.ImageUrl);
                    string pageUrl = WifiManager.GetWifiUrl(wifiInfo, wxOpenID);

                    article = new Article()
                    {
                        Title = wifiInfo.Title,
                        Description = wifiInfo.Summary,
                        PicUrl = imageUrl,
                        Url = pageUrl
                    };
                }

                if (article != null)
                {
                    articleList.Add(article);
                }
            }

            return articleList;
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get { return "0pqqYvq7j0xqqmq5"; } //0pqqYvq7j0xqqmq5
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string IV
        {
            get { return "8105547186756005"; } //8105547186756005
        }

        /// <summary>   
        /// AES加密   
        /// </summary>   
        /// <param name="data">要加密的字符串</param>   
        /// <returns>加密后的字符串</returns>   
        public static string DESEncrypt(string data)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                byte[] bData = UTF8Encoding.UTF8.GetBytes(data);
                aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
                aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform iCryptoTransform = aes.CreateEncryptor();
                byte[] bResult = iCryptoTransform.TransformFinalBlock(bData, 0, bData.Length);

                return ByteToHex(bResult);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>   
        /// AES解密   
        /// </summary>   
        /// <param name="data">要解密的字符串</param>   
        /// <returns>解密后的字符串</returns>   
        public static string DESDecrypt(string data)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                byte[] bData = HexToByte(data);
                aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
                aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform iCryptoTransform = aes.CreateDecryptor();
                byte[] bResult = iCryptoTransform.TransformFinalBlock(bData, 0, bData.Length);
                return Encoding.UTF8.GetString(bResult);
            }
            catch
            {
                throw;
            }
        }

        private static byte[] HexToByte(string data)
        {
            data = data.Replace(" ", "");

            byte[] comBuffer = new byte[data.Length / 2];

            for (int i = 0; i < data.Length; i += 2)
                comBuffer[i / 2] = (byte)Convert.ToByte(data.Substring(i, 2), 16);

            return comBuffer;
        }

        private static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            foreach (byte data in comByte)
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

            return builder.ToString().ToUpper().Replace(" ", "");
        }
    }
}
