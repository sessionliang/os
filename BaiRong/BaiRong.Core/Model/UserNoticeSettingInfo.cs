using BaiRong.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BaiRong.Model
{
    public class UserNoticeSettingInfo
    {

        private int id;
        private string userNoticeType;
        private string userName;
        private bool byEmail;
        private bool byPhone;
        private bool byMessage;
        private string emailTitle;
        private string emailTemplate;
        private string phoneTemplate;
        private string messageTitle;
        private string messageTemplate;
        private bool isRequired;
        private bool isSignal;
        private string[] allow;


        public UserNoticeSettingInfo()
        {

        }

        public UserNoticeSettingInfo(int id, string userNoticeType, string userName, bool byEmail, bool byPhone, bool byMessage, string emailTitle, string emailTemplate, string phoneTemplate, string messageTitle, string messageTemplate, bool isRequired, bool isSignal, string[] allow)
        {
            this.id = id;
            this.userNoticeType = userNoticeType;
            this.userName = userName;
            this.byEmail = byEmail;
            this.byPhone = byPhone;
            this.byMessage = byMessage;
            this.emailTitle = emailTitle;
            this.emailTemplate = emailTemplate;
            this.phoneTemplate = phoneTemplate;
            this.messageTitle = messageTitle;
            this.messageTemplate = messageTemplate;
            this.isRequired = isRequired;
            this.isSignal = isSignal;
            this.allow = allow;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 用户提醒类型
        /// </summary>
        public string UserNoticeType
        {
            get { return userNoticeType; }
            set { userNoticeType = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 邮件方式
        /// </summary>
        public bool ByEmail
        {
            get { return byEmail; }
            set { byEmail = value; }
        }

        /// <summary>
        /// 短信方式
        /// </summary>
        public bool ByPhone
        {
            get { return byPhone; }
            set { byPhone = value; }
        }

        /// <summary>
        /// 站内信方式
        /// </summary>
        public bool ByMessage
        {
            get { return byMessage; }
            set { byMessage = value; }
        }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string EmailTitle
        {
            get { return emailTitle; }
            set { emailTitle = value; }
        }

        /// <summary>
        /// 邮件模板
        /// </summary>
        public string EmailTemplate
        {
            get { return emailTemplate; }
            set { emailTemplate = value; }
        }

        /// <summary>
        /// 短信模板
        /// </summary>
        public string PhoneTemplate
        {
            get { return phoneTemplate; }
            set { phoneTemplate = value; }
        }

        /// <summary>
        /// 站内信标题
        /// </summary>
        public string MessageTitle
        {
            get { return messageTitle; }
            set { messageTitle = value; }
        }

        /// <summary>
        /// 站内信模板
        /// </summary>
        public string MessageTemplate
        {
            get { return messageTemplate; }
            set { messageTemplate = value; }
        }

        /// <summary>
        /// 是否必选项
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        /// <summary>
        /// true-单选，false-多选
        /// </summary>
        public bool IsSignal
        {
            get { return isSignal; }
            set { isSignal = value; }
        }

        public string[] Allow
        {
            get { return allow; }
            set { allow = value; }
        }

    }
}
