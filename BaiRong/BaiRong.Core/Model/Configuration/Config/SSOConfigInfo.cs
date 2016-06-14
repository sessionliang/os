using System;
using System.Collections;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Xml;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core.Configuration
{
    public class SSOConfigInfo
    {
        private EIntegrationType integrationType;
        private bool isUser;
        private string userPrefix;
        private string isvKey;
        private string loginUrl; 
        private string callbackUrl;

        public SSOConfigInfo() { }

        public EIntegrationType IntegrationType
        {
            get { return this.integrationType; }
            set { this.integrationType = value; }
        }

        public bool IsUser
        {
            get { return this.isUser; }
            set { this.isUser = value; }
        }

        public string UserPrefix
        {
            get { return this.userPrefix; }
            set { this.userPrefix = value; }
        }

        public string ISVKey
        {
            get { return this.isvKey; }
            set { this.isvKey = value; }
        }

        public string LoginUrl
        {
            get { return this.loginUrl; }
            set { this.loginUrl = value; }
        }

        public string CallbackUrl
        {
            get { return this.callbackUrl; }
            set { this.callbackUrl = value; }
        }
    }
}
