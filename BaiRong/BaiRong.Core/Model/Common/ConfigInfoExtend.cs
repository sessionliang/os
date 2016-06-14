using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Model
{
    public class ConfigInfoExtend : ExtendedAttributes
    {
        public ConfigInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }

        public ERestrictionType RestrictionType
        {
            get { return ERestrictionTypeUtils.GetEnumType(base.GetString("RestrictionType", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionType", ERestrictionTypeUtils.GetValue(value)); }
        }

        public bool IsLog
        {
            get { return base.GetBool("IsLog", true); }
            set { base.SetExtendedAttribute("IsLog", value.ToString()); }
        }

        public bool IsLogTask
        {
            get { return base.GetBool("IsLogTask", true); }
            set { base.SetExtendedAttribute("IsLogTask", value.ToString()); }
        }

        public string Cipherkey
        {
            get { return base.GetString("Cipherkey", string.Empty); }
            set { base.SetExtendedAttribute("Cipherkey", value); }
        }

        public string SMSAccount
        {
            get { return base.GetString("SMSAccount", string.Empty); }
            set { base.SetExtendedAttribute("SMSAccount", value); }
        }

        public string SMSPassword
        {
            get { return base.GetString("SMSPassword", string.Empty); }
            set { base.SetExtendedAttribute("SMSPassword", value); }
        }

        public string SMSMD5String
        {
            get { return base.GetString("SMSMD5String", string.Empty); }
            set { base.SetExtendedAttribute("SMSMD5String", value); }
        }

        public string SMSServerType
        {
            get { return base.GetString("SMSServerType", string.Empty); }
            set { base.SetExtendedAttribute("SMSServerType", value); }
        }

        /// <summary>
        /// �Ƿ����Զ�����ҳ�湦��
        /// �����������������ֶ�����ҳ��
        /// </summary>
        public bool IsUseAjaxCreatePage
        {
            get { return base.GetBool("IsUseAjaxCreatePage", true); }
            set { base.SetExtendedAttribute("IsUseAjaxCreatePage", value.ToString()); }
        }

        /// <summary>
        /// �Ƿ����ù���xss����
        /// ������ã���������ӵ�ʱ�򣬻�������е��ֶΣ�����file,image,video��
        /// ��������ã��򲻽��й��ˣ���ʱ��xss����Σ��
        /// </summary>
        public bool IsFilterXss
        {
            get { return base.GetBool("IsFilterXss", false); }
            set { base.SetExtendedAttribute("IsFilterXss", value.ToString()); }
        }


        /// <summary>
        /// �Ƿ�����SiteServer�����������
        /// ������ã�������������ɲ�����ʱ�򣬻ύ��������������ŶӴ���
        /// ��������ã��������ɽ���IIS����
        /// </summary>
        public bool IsSiteServerServiceCreate
        {
            get { return base.GetBool("IsSiteServerServiceCreate", false); }
            set { base.SetExtendedAttribute("IsSiteServerServiceCreate", value.ToString()); }
        }

        /// <summary>
        /// �Ƿ�ֻ�鿴�Լ���ӵ�����
        /// ����ǣ���ô����Աֻ�ܲ鿴�Լ���ӵ�����
        /// ������ǣ���ô����Ա���Բ鿴��������Ա��������ݣ�Ĭ��false
        /// ע�⣺���������룬վ�����Ա����˹���Ա����������Ч
        /// add by sessionliang at 20151217
        /// </summary>
        public bool IsViewContentOnlySelf
        {
            get { return base.GetBool("IsViewContentOnlySelf", false); }
            set { base.SetExtendedAttribute("IsViewContentOnlySelf", value.ToString()); }
        }

        #region SiteYun

        public int SiteYun_OrderID
        {
            get { return base.GetInt("SiteYun_OrderID", 0); }
            set { base.SetExtendedAttribute("SiteYun_OrderID", value.ToString()); }
        }

        public string SiteYun_RedirectUrl
        {
            get { return base.GetString("SiteYun_RedirectUrl", string.Empty); }
            set { base.SetExtendedAttribute("SiteYun_RedirectUrl", value); }
        }

        #endregion


        #region Email Settings
        public string MailDomain
        {
            get { return base.GetString("MailDomain", "smtp.exmail.qq.com"); }
            set { base.SetExtendedAttribute("MailDomain", value); }
        }

        public int MailDomainPort
        {
            get { return base.GetInt("MailDomainPort", 25); }
            set { base.SetExtendedAttribute("MailDomainPort", value.ToString()); }
        }

        public string MailFromName
        {
            get { return base.GetString("MailFromName", string.Empty); }
            set { base.SetExtendedAttribute("MailFromName", value); }
        }

        public string MailServerUserName
        {
            get { return base.GetString("MailServerUserName", string.Empty); }
            set { base.SetExtendedAttribute("MailServerUserName", value); }
        }

        public string MailServerPassword
        {
            get { return base.GetString("MailServerPassword", string.Empty); }
            set { base.SetExtendedAttribute("MailServerPassword", value); }
        }

        public bool MailIsEnabled
        {
            get { return base.GetBool("MailIsEnabled", false); }
            set { base.SetExtendedAttribute("MailIsEnabled", value.ToString()); }
        }

        public bool EnableSsl
        {
            get { return base.GetBool("EnableSsl", false); }
            set { base.SetExtendedAttribute("EnableSsl", value.ToString()); }

        }
        #endregion

        #region Log Settings
        /// <summary>
        /// �Ƿ���ʱ����ֵ
        /// </summary>
        public bool IsTimeThreshold
        {
            get { return base.GetBool("IsTimeThreshold", false); }
            set { base.SetExtendedAttribute("IsTimeThreshold", value.ToString()); }
        }

        public int TimeThreshold
        {
            get { return base.GetInt("TimeThreshold", 60); }
            set { base.SetExtendedAttribute("TimeThreshold", value.ToString()); }
        }

        /// <summary>
        /// �Ƿ���������ֵ
        /// </summary>
        public bool IsCounterThreshold
        {
            get { return base.GetBool("IsCounterThreshold", false); }
            set { base.SetExtendedAttribute("IsCounterThreshold", value.ToString()); }
        }

        public int CounterThreshold
        {
            get { return base.GetInt("CounterThreshold", 3); }
            set { base.SetExtendedAttribute("CounterThreshold", value.ToString()); }
        }

        #endregion

        #region ��֧����

        /// <summary>
        /// ��֧�����İٶȵ�ͼAK
        /// add by sofuny at 20160104
        /// </summary>
        public string OrganizationBaiduAK
        {
            get { return base.GetString("OrganizationBaiduAK", string.Empty); }
            set { base.SetExtendedAttribute("OrganizationBaiduAK", value); }
        }


        /// <summary>
        /// ��֧�����Ƿ����ÿ���
        /// add by sofuny at 20160104
        /// </summary>
        public bool OrganizationIsCrossDomain
        {
            get { return base.GetBool("OrganizationIsCrossDomain", false); }
            set { base.SetExtendedAttribute("OrganizationIsCrossDomain", value.ToString()); }
        }
        #endregion


        #region ����� add by sofuny at 20160108

        /// <summary>
        /// �Ƿ������û�����Ͷ��
        /// �����û�Ͷ�壬�û����ĲŻ���ʾͶ��˵�
        /// </summary>
        public bool IsUseMLib
        {
            get { return base.GetBool("IsUseMLib", false); }
            set { base.SetExtendedAttribute("IsUseMLib", value.ToString()); }
        }

        /// <summary>
        /// �Ƿ������û��鵥������
        /// ���л�Ա�ĸ������ͬһ����Ա��ݷ���Ͷ�ݵĸ���������ѡ��[�����û��鵥������]��������û�����������
        /// Ĭ��Ϊ��
        /// </summary>
        public bool IsUnifiedMLibAddUser
        {
            get { return base.GetBool("IsUnifiedMLibAddUser", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibAddUser", value.ToString()); }
        }


        /// <summary>
        /// ���ͳһ�����߹���Ա�˺�
        /// </summary>
        public string UnifiedMLibAddUser
        {
            get { return base.GetString("UnifiedMLibAddUser", AdminManager.Current.UserName); }
            set { base.SetExtendedAttribute("UnifiedMLibAddUser", value); }
        }


        /// <summary>
        /// �����û��鵥������
        /// ����ͳһͶ����Ч�ڣ������û��ĸ����ֻ��������Ч����Ͷ�壻�����ѡ��[�����û��鵥������]��������û�����������
        /// Ĭ��Ϊ��
        /// </summary>
        public bool IsUnifiedMLibValidityDate
        {
            get { return base.GetBool("IsUnifiedMLibValidityDate", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibValidityDate", value.ToString()); }
        }
        /// <summary>
        /// �û�ͳһͶ����Ч��
        /// ���ٸ���
        /// </summary>
        public int UnifiedMLibValidityDate
        {
            get { return base.GetInt("UnifiedMLibValidityDate", 0); }
            set { base.SetExtendedAttribute("UnifiedMLibValidityDate", value.ToString()); }
        }


        /// <summary>
        /// �Ƿ������û�ͳһ��Ͷ������
        /// �Ƿ������û�ͳһ��Ͷ�����������л�Ա��Ͷ����������������û��鵥������
        /// </summary>
        public bool IsUnifiedMLibNum
        {
            get { return base.GetBool("IsUnifiedMLibNum", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibNum", value.ToString()); }
        }
        /// <summary>
        /// �û�ͳһ��Ͷ������ 
        /// </summary>
        public int UnifiedMlibNum
        {
            get { return base.GetInt("UnifiedMlibNum", 0); }
            set { base.SetExtendedAttribute("UnifiedMlibNum", value.ToString()); }
        }

        /// <summary>
        /// �����˷�ʽ
        /// ����ĿΪγ�ȣ�����������Ͷ�巶Χʱ����Ŀ�������ã����û���Ϊγ�ȣ��������û�������е�������
        /// Ĭ��Ϊtrue,����ĿΪγ��
        /// </summary>
        public bool MLibCheckType
        {
            get { return base.GetBool("MLibCheckType", true); }
            set { base.SetExtendedAttribute("MLibCheckType", value.ToString()); }
        }

        /// <summary>
        /// �û�ͳһ��Ͷ��վ�㷶Χ 
        /// </summary>
        public string MLibPublishmentSystemIDs
        {
            get { return base.GetString("MLibPublishmentSystemIDs", string.Empty); }
            set { base.SetExtendedAttribute("MLibPublishmentSystemIDs", value); }
        }


        /// <summary>
        /// �û�ͳһ��Ͷ��վ�㷶Χ 
        /// </summary>
        public DateTime MLibStartTime
        {
            get { return base.GetDateTime("MLibStartTime", DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute("MLibStartTime", value.ToString()); }
        }
        #endregion
    }
}
