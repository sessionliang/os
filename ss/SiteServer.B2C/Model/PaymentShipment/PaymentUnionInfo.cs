using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public class PaymentUnionInfo : ExtendedAttributes
    {
        public PaymentUnionInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MerID
        {
            get { return base.GetString("MerID", string.Empty); }
            set { base.SetExtendedAttribute("MerID", value); }
        }

        /// <summary>
        /// 商户私钥证书
        /// </summary>
        public string SignCertPath
        {
            get { return base.GetString("SignCertPath", string.Empty); }
            set { base.SetExtendedAttribute("SignCertPath", value); }
        }

        /// <summary>
        /// 商户私钥证书密码
        /// </summary>
        public string SignCertPwd
        {
            get { return base.GetString("SignCertPwd", string.Empty); }
            set { base.SetExtendedAttribute("SignCertPwd", value); }
        }

        /// <summary>
        /// 银联公钥证书
        /// </summary>
        public string EncryptCert
        {
            get { return base.GetString("EncryptCert", string.Empty); }
            set { base.SetExtendedAttribute("EncryptCert", value); }
        }

        /// <summary>
        /// 是否为测试
        /// </summary>
        public bool IsTest
        {
            get { return base.GetBool("IsTest", false); }
            set { base.SetExtendedAttribute("IsTest", value.ToString()); }
        }

    }
}
