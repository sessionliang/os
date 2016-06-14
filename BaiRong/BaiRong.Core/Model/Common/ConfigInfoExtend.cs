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
        /// 是否开启自动生成页面功能
        /// 如果不开启，则必须手动生成页面
        /// </summary>
        public bool IsUseAjaxCreatePage
        {
            get { return base.GetBool("IsUseAjaxCreatePage", true); }
            set { base.SetExtendedAttribute("IsUseAjaxCreatePage", value.ToString()); }
        }

        /// <summary>
        /// 是否启用过滤xss过滤
        /// 如果启用，内容在添加的时候，会过滤所有的字段（除了file,image,video）
        /// 如果不启用，则不进行过滤，此时有xss攻击危险
        /// </summary>
        public bool IsFilterXss
        {
            get { return base.GetBool("IsFilterXss", false); }
            set { base.SetExtendedAttribute("IsFilterXss", value.ToString()); }
        }


        /// <summary>
        /// 是否启用SiteServer服务组件生成
        /// 如果启用，在批量点击生成操作的时候，会交给服务组件进行排队处理
        /// 如果不启用，批量生成交给IIS处理
        /// </summary>
        public bool IsSiteServerServiceCreate
        {
            get { return base.GetBool("IsSiteServerServiceCreate", false); }
            set { base.SetExtendedAttribute("IsSiteServerServiceCreate", value.ToString()); }
        }

        /// <summary>
        /// 是否只查看自己添加的内容
        /// 如果是，那么管理员只能查看自己添加的内容
        /// 如果不是，那么管理员可以查看其他管理员天机的内容，默认false
        /// 注意：超级管理与，站点管理员，审核管理员，此设置无效
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
        /// 是否开启时间阈值
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
        /// 是否开启条数阈值
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

        #region 分支机构

        /// <summary>
        /// 分支机构的百度地图AK
        /// add by sofuny at 20160104
        /// </summary>
        public string OrganizationBaiduAK
        {
            get { return base.GetString("OrganizationBaiduAK", string.Empty); }
            set { base.SetExtendedAttribute("OrganizationBaiduAK", value); }
        }


        /// <summary>
        /// 分支机构是否启用跨域
        /// add by sofuny at 20160104
        /// </summary>
        public bool OrganizationIsCrossDomain
        {
            get { return base.GetBool("OrganizationIsCrossDomain", false); }
            set { base.SetExtendedAttribute("OrganizationIsCrossDomain", value.ToString()); }
        }
        #endregion


        #region 稿件库 add by sofuny at 20160108

        /// <summary>
        /// 是否启用用户中心投稿
        /// 启用用户投稿，用户中心才会显示投稿菜单
        /// </summary>
        public bool IsUseMLib
        {
            get { return base.GetBool("IsUseMLib", false); }
            set { base.SetExtendedAttribute("IsUseMLib", value.ToString()); }
        }

        /// <summary>
        /// 是否允许用户组单独设置
        /// 所有会员的稿件将以同一管理员身份发布投递的稿件，如果勾选了[允许用户组单独设置]，则可在用户组重新设置
        /// 默认为否
        /// </summary>
        public bool IsUnifiedMLibAddUser
        {
            get { return base.GetBool("IsUnifiedMLibAddUser", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibAddUser", value.ToString()); }
        }


        /// <summary>
        /// 稿件统一发布者管理员账号
        /// </summary>
        public string UnifiedMLibAddUser
        {
            get { return base.GetString("UnifiedMLibAddUser", AdminManager.Current.UserName); }
            set { base.SetExtendedAttribute("UnifiedMLibAddUser", value); }
        }


        /// <summary>
        /// 允许用户组单独设置
        /// 设置统一投稿有效期，所有用户的稿件将只允许在有效期内投稿；如果勾选了[允许用户组单独设置]，则可在用户组重新设置
        /// 默认为否
        /// </summary>
        public bool IsUnifiedMLibValidityDate
        {
            get { return base.GetBool("IsUnifiedMLibValidityDate", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibValidityDate", value.ToString()); }
        }
        /// <summary>
        /// 用户统一投稿有效期
        /// 多少个月
        /// </summary>
        public int UnifiedMLibValidityDate
        {
            get { return base.GetInt("UnifiedMLibValidityDate", 0); }
            set { base.SetExtendedAttribute("UnifiedMLibValidityDate", value.ToString()); }
        }


        /// <summary>
        /// 是否设置用户统一可投稿数量
        /// 是否设置用户统一可投稿数量，所有会员可投稿数量，否则可在用户组单独设置
        /// </summary>
        public bool IsUnifiedMLibNum
        {
            get { return base.GetBool("IsUnifiedMLibNum", false); }
            set { base.SetExtendedAttribute("IsUnifiedMLibNum", value.ToString()); }
        }
        /// <summary>
        /// 用户统一可投稿数量 
        /// </summary>
        public int UnifiedMlibNum
        {
            get { return base.GetInt("UnifiedMlibNum", 0); }
            set { base.SetExtendedAttribute("UnifiedMlibNum", value.ToString()); }
        }

        /// <summary>
        /// 稿件审核方式
        /// 以栏目为纬度：可以在设置投稿范围时给栏目单独设置；以用户组为纬度：可以在用户组管理中单独设置
        /// 默认为true,以栏目为纬度
        /// </summary>
        public bool MLibCheckType
        {
            get { return base.GetBool("MLibCheckType", true); }
            set { base.SetExtendedAttribute("MLibCheckType", value.ToString()); }
        }

        /// <summary>
        /// 用户统一可投稿站点范围 
        /// </summary>
        public string MLibPublishmentSystemIDs
        {
            get { return base.GetString("MLibPublishmentSystemIDs", string.Empty); }
            set { base.SetExtendedAttribute("MLibPublishmentSystemIDs", value); }
        }


        /// <summary>
        /// 用户统一可投稿站点范围 
        /// </summary>
        public DateTime MLibStartTime
        {
            get { return base.GetDateTime("MLibStartTime", DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute("MLibStartTime", value.ToString()); }
        }
        #endregion
    }
}
