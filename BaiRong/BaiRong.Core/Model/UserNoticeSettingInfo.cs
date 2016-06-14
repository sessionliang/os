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
        /// �û���������
        /// </summary>
        public string UserNoticeType
        {
            get { return userNoticeType; }
            set { userNoticeType = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// �ʼ���ʽ
        /// </summary>
        public bool ByEmail
        {
            get { return byEmail; }
            set { byEmail = value; }
        }

        /// <summary>
        /// ���ŷ�ʽ
        /// </summary>
        public bool ByPhone
        {
            get { return byPhone; }
            set { byPhone = value; }
        }

        /// <summary>
        /// վ���ŷ�ʽ
        /// </summary>
        public bool ByMessage
        {
            get { return byMessage; }
            set { byMessage = value; }
        }

        /// <summary>
        /// �ʼ�����
        /// </summary>
        public string EmailTitle
        {
            get { return emailTitle; }
            set { emailTitle = value; }
        }

        /// <summary>
        /// �ʼ�ģ��
        /// </summary>
        public string EmailTemplate
        {
            get { return emailTemplate; }
            set { emailTemplate = value; }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public string PhoneTemplate
        {
            get { return phoneTemplate; }
            set { phoneTemplate = value; }
        }

        /// <summary>
        /// վ���ű���
        /// </summary>
        public string MessageTitle
        {
            get { return messageTitle; }
            set { messageTitle = value; }
        }

        /// <summary>
        /// վ����ģ��
        /// </summary>
        public string MessageTemplate
        {
            get { return messageTemplate; }
            set { messageTemplate = value; }
        }

        /// <summary>
        /// �Ƿ��ѡ��
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        /// <summary>
        /// true-��ѡ��false-��ѡ
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
