using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections;

namespace BaiRong.Model
{
    public class UserConfigInfoExtend : ExtendedAttributes
    {
        public UserConfigInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }

        /****************邮件设置********************/
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
            get { return base.GetString("MailServerUserName", "admin@siteserver.cn"); }
            set { base.SetExtendedAttribute("MailServerUserName", value); }
        }

        public string MailServerPassword
        {
            get { return base.GetString("MailServerPassword", "brtech88"); }
            set { base.SetExtendedAttribute("MailServerPassword", value); }
        }

        /****************用户中心注册设置********************/

        public bool IsRegisterAllowed
        {
            get { return base.GetBool("IsRegisterAllowed", true); }
            set { base.SetExtendedAttribute("IsRegisterAllowed", value.ToString()); }
        }

        public int RegisterUserNameMinLength
        {
            get { return base.GetInt("RegisterUserNameMinLength", 0); }
            set { base.SetExtendedAttribute("RegisterUserNameMinLength", value.ToString()); }
        }

        public EUserPasswordRestriction RegisterPasswordRestriction
        {
            get { return EUserPasswordRestrictionUtils.GetEnumType(base.GetString("RegisterPasswordRestriction", EUserPasswordRestrictionUtils.GetValue(EUserPasswordRestriction.None))); }
            set { base.SetExtendedAttribute("RegisterPasswordRestriction", EUserPasswordRestrictionUtils.GetValue(value)); }
        }

        public bool IsEmailDuplicated
        {
            get { return base.GetBool("IsEmailDuplicated", false); }
            set { base.SetExtendedAttribute("IsEmailDuplicated", value.ToString()); }
        }

        public string ReservedUserNames
        {
            get { return base.GetString("ReservedUserNames", "admin,administrator"); }
            set { base.SetExtendedAttribute("ReservedUserNames", value); }
        }

        public EUserVerifyType RegisterVerifyType
        {
            get { return EUserVerifyTypeUtils.GetEnumType(base.GetString("RegisterVerifyType", EUserVerifyTypeUtils.GetValue(EUserVerifyType.None))); }
            set { base.SetExtendedAttribute("RegisterVerifyType", EUserVerifyTypeUtils.GetValue(value)); }
        }

        public string RegisterWelcome
        {
            get { return base.GetString("RegisterWelcome", "恭喜，您已经成功注册为用户"); }
            set { base.SetExtendedAttribute("RegisterWelcome", value); }
        }

        public string RegisterVerifyMailContent
        {
            get
            {
                return base.GetString("RegisterVerifyMailContent", @"
您好，[UserName] ：

您刚刚在 [SiteUrl] 注册了用户，要完成您的注册，请访问下面的链接：

<a href=""[VerifyUrl]"" target=""_blank"">[VerifyUrl]</a>

感谢您使用我们的服务, 如果您需要了解更多信息, 请登录我们的网站: [SiteUrl]

谢谢！");
            }
            set { base.SetExtendedAttribute("RegisterVerifyMailContent", value); }
        }

        public int RegisterMinHoursOfIPAddress
        {
            get
            {
                return base.GetInt("RegisterMinHoursOfIPAddress", 0);
            }
            set
            {
                base.SetExtendedAttribute("RegisterMinHoursOfIPAddress", value.ToString());
            }
        }

        public EUserWelcomeType RegisterWelcomeType
        {
            get { return EUserWelcomeTypeUtils.GetEnumType(base.GetString("RegisterWelcomeType", EUserWelcomeTypeUtils.GetValue(EUserWelcomeType.None))); }
            set { base.SetExtendedAttribute("RegisterWelcomeType", EUserWelcomeTypeUtils.GetValue(value)); }
        }

        public string RegisterWelcomeTitle
        {
            get
            {
                return base.GetString("RegisterWelcomeTitle", string.Empty);
            }
            set
            {
                base.SetExtendedAttribute("RegisterWelcomeTitle", value);
            }
        }

        public string RegisterWelcomeContent
        {
            get
            {
                return base.GetString("RegisterWelcomeContent", string.Empty);
            }
            set
            {
                base.SetExtendedAttribute("RegisterWelcomeContent", value);
            }
        }

        public bool RegisterAuditType
        {
            get
            {
                return base.GetBool("RegisterAuditType", false);
            }
            set
            {
                base.SetExtendedAttribute("RegisterAuditType", value.ToString());
            }
        }

        /****************用户中心消息设置********************/

        public bool IsMessage
        {
            get { return base.GetBool("IsMessage", false); }
            set { base.SetExtendedAttribute("IsMessage", value.ToString()); }
        }

        public string MessageTitle
        {
            get { return base.GetString("MessageTitle", "通知"); }
            set { base.SetExtendedAttribute("MessageTitle", value); }
        }

        public string MessageContent
        {
            get { return base.GetString("MessageContent", string.Empty); }
            set { base.SetExtendedAttribute("MessageContent", value); }
        }

        /****************用户中心显示设置********************/

        public string SystemName
        {
            get { return base.GetString("SystemName", "用户中心"); }
            set { base.SetExtendedAttribute("SystemName", value); }
        }

        public bool IsLogo
        {
            get { return base.GetBool("IsLogo", false); }
            set { base.SetExtendedAttribute("IsLogo", value.ToString()); }
        }

        public string LogoUrl
        {
            get { return base.GetString("LogoUrl", string.Empty); }
            set { base.SetExtendedAttribute("LogoUrl", value); }
        }

        /****************文件上传设置********************/

        public string UploadDirectoryName
        {
            get { return base.GetString("UploadDirectoryName", "upload"); }
            set { base.SetExtendedAttribute("UploadDirectoryName", value); }
        }

        public string UploadDateFormatString
        {
            get { return base.GetString("UploadDateFormatString", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)); }
            set { base.SetExtendedAttribute("UploadDateFormatString", value); }
        }

        public bool IsUploadChangeFileName
        {
            get { return base.GetBool("IsUploadChangeFileName", true); }
            set { base.SetExtendedAttribute("IsUploadChangeFileName", value.ToString()); }
        }

        public int UploadMonthMaxSize
        {
            get { return base.GetInt("UploadMonthMaxSize", 122880); }  //120M
            set { base.SetExtendedAttribute("UploadMonthMaxSize", value.ToString()); }
        }

        public string UploadImageTypeCollection
        {
            get { return base.GetString("UploadImageTypeCollection", "gif|jpg|jpeg|bmp|png|swf|flv"); }
            set { base.SetExtendedAttribute("UploadImageTypeCollection", value); }
        }

        public int UploadImageTypeMaxSize
        {
            get { return base.GetInt("UploadImageTypeMaxSize", 2048); }
            set { base.SetExtendedAttribute("UploadImageTypeMaxSize", value.ToString()); }
        }

        public string UploadMediaTypeCollection
        {
            get { return base.GetString("UploadMediaTypeCollection", "rm|rmvb|mp3|flv|wav|mid|midi|ra|avi|mpg|mpeg|asf|asx|wma|mov"); }
            set { base.SetExtendedAttribute("UploadMediaTypeCollection", value); }
        }

        public int UploadMediaTypeMaxSize
        {
            get { return base.GetInt("UploadMediaTypeMaxSize", 5120); }
            set { base.SetExtendedAttribute("UploadMediaTypeMaxSize", value.ToString()); }
        }

        public string UploadFileTypeCollection
        {
            get { return base.GetString("UploadFileTypeCollection", "zip|rar|7z|txt|doc|docx|ppt|pptx|xls|xlsx|pdf"); }
            set { base.SetExtendedAttribute("UploadFileTypeCollection", value); }
        }

        public int UploadFileTypeMaxSize
        {
            get { return base.GetInt("UploadFileTypeMaxSize", 5120); }
            set { base.SetExtendedAttribute("UploadFileTypeMaxSize", value.ToString()); }
        }

        /****************图片水印设置********************/

        public bool IsWaterMark
        {
            get { return base.GetBool("IsWaterMark", false); }
            set { base.SetExtendedAttribute("IsWaterMark", value.ToString()); }
        }

        public bool IsImageWaterMark
        {
            get { return base.GetBool("IsImageWaterMark", false); }
            set { base.SetExtendedAttribute("IsImageWaterMark", value.ToString()); }
        }

        public int WaterMarkPosition
        {
            get { return base.GetInt("WaterMarkPosition", 9); }
            set { base.SetExtendedAttribute("WaterMarkPosition", value.ToString()); }
        }

        public int WaterMarkTransparency
        {
            get { return base.GetInt("WaterMarkTransparency", 5); }
            set { base.SetExtendedAttribute("WaterMarkTransparency", value.ToString()); }
        }

        public int WaterMarkMinWidth
        {
            get { return base.GetInt("WaterMarkMinWidth", 200); }
            set { base.SetExtendedAttribute("WaterMarkMinWidth", value.ToString()); }
        }

        public int WaterMarkMinHeight
        {
            get { return base.GetInt("WaterMarkMinHeight", 200); }
            set { base.SetExtendedAttribute("WaterMarkMinHeight", value.ToString()); }
        }

        public string WaterMarkFormatString
        {
            get { return base.GetString("WaterMarkFormatString", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkFormatString", value); }
        }

        public string WaterMarkFontName
        {
            get { return base.GetString("WaterMarkFontName", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkFontName", value); }
        }

        public int WaterMarkFontSize
        {
            get { return base.GetInt("WaterMarkFontSize", 12); }
            set { base.SetExtendedAttribute("WaterMarkFontSize", value.ToString()); }
        }

        public string WaterMarkImagePath
        {
            get { return base.GetString("WaterMarkImagePath", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkImagePath", value); }
        }

        /****************个人空间系统********************/

        public bool IsDisableSpace
        {
            get { return base.GetBool("IsDisableSpace", false); }
            set { base.SetExtendedAttribute("IsDisableSpace", value.ToString()); }
        }

        public bool IsDisableBlog
        {
            get { return base.GetBool("IsDisableBlog", false); }
            set { base.SetExtendedAttribute("IsDisableBlog", value.ToString()); }
        }

        public bool IsDisablePhoto
        {
            get { return base.GetBool("IsDisablePhoto", false); }
            set { base.SetExtendedAttribute("IsDisablePhoto", value.ToString()); }
        }

        public bool IsDisableFavorite
        {
            get { return base.GetBool("IsDisableFavorite", false); }
            set { base.SetExtendedAttribute("IsDisableFavorite", value.ToString()); }
        }

        public bool IsDisableFriends
        {
            get { return base.GetBool("IsDisableFriends", false); }
            set { base.SetExtendedAttribute("IsDisableFriends", value.ToString()); }
        }

        /****************管理员设置********************/

        public EPasswordFormat AdminPasswordFormat
        {
            get { return EPasswordFormatUtils.GetEnumType(base.GetString("AdminPasswordFormat", string.Empty)); }
            set { base.SetExtendedAttribute("AdminPasswordFormat", EPasswordFormatUtils.GetValue(value)); }
        }

        public int AdminMinPasswordLength
        {
            get { return base.GetInt("AdminMinPasswordLength", 4); }
            set { base.SetExtendedAttribute("AdminMinPasswordLength", value.ToString()); }
        }

        /****************用户设置********************/

        public EPasswordFormat UserPasswordFormat
        {
            get { return EPasswordFormatUtils.GetEnumType(base.GetString("UserPasswordFormat", string.Empty)); }
            set { base.SetExtendedAttribute("UserPasswordFormat", EPasswordFormatUtils.GetValue(value)); }
        }

        public int UserMinPasswordLength
        {
            get { return base.GetInt("UserMinPasswordLength", 6); }
            set { base.SetExtendedAttribute("UserMinPasswordLength", value.ToString()); }
        }

        /****************短信集成设置********************/

        public string SMSUserName
        {
            get { return base.GetString("SMSUserName", string.Empty); }
            set { base.SetExtendedAttribute("SMSUserName", value); }
        }

        public string SMSMD5String
        {
            get { return base.GetString("SMSMD5String", string.Empty); }
            set { base.SetExtendedAttribute("SMSMD5String", value); }
        }

        /****************用户登录设置********************/

        public string UserLoginValidateFields
        {
            get { return base.GetString("UserLoginValidateFields", "UserName"); }
            set { base.SetExtendedAttribute("UserLoginValidateFields", value); }
        }

        public bool IsRecordIP
        {
            get { return base.GetBool("IsRecordIP", true); }
            set { base.SetExtendedAttribute("IsRecordIP", value.ToString()); }
        }

        public bool IsRecordSource
        {
            get { return base.GetBool("IsRecordSource", true); }
            set { base.SetExtendedAttribute("IsRecordSource", value.ToString()); }
        }

        public bool IsFailToLock
        {
            get { return base.GetBool("IsFailToLock", false); }
            set { base.SetExtendedAttribute("IsFailToLock", value.ToString()); }
        }

        public int LoginFailCount
        {
            get { return base.GetInt("LoginFailCount", 3); }
            set { base.SetExtendedAttribute("LoginFailCount", value.ToString()); }
        }

        public string LockingType
        {
            get { return base.GetString("LockingType", "Forever"); }
            set { base.SetExtendedAttribute("LockingType", value); }
        }

        public int LockingTime
        {
            get { return base.GetInt("LockingTime", 1); }
            set { base.SetExtendedAttribute("LockingTime", value.ToString()); }
        }

        /****************忘记密码设置********************/

        public string FindPasswordMethods
        {
            get { return base.GetString("FindPasswordMethods", "Email,Phone,SecretQuestion"); }
            set { base.SetExtendedAttribute("FindPasswordMethods", value); }
        }

        public string FindPasswordEmailNotice
        {
            get
            {
                return base.GetString("FindPasswordEmailNotice", @"[UserName]，你好。

请点击一下链接，找回密码：
<a href=""[VerifyUrl]""  target=""_blank"">[VerifyUrl]</a>");
            }
            set { base.SetExtendedAttribute("FindPasswordEmailNotice", value); }
        }

        public string FindPasswordEmailNoticeTitle
        {
            get { return base.GetString("FindPasswordEmailNoticeTitle", "找回密码"); }
            set { base.SetExtendedAttribute("FindPasswordEmailNoticeTitle", value); }
        }

        public string FindPasswordPhoneNotice
        {
            get
            {
                return base.GetString("FindPasswordPhoneNotice", @"您的验证码是：[VerifyCode];

如果这不是您本人的操作，请不要将本信息泄露给其他人。");
            }
            set { base.SetExtendedAttribute("FindPasswordPhoneNotice", value); }
        }


        /****************用户积分币规则********************/


        public string CreditNumName
        {
            get { return base.GetString("CreditNumName", "积分"); }
            set { base.SetExtendedAttribute("CreditNumName", value); }
        }

        public string CreditNumUnit
        {
            get { return base.GetString("CreditNumUnit", string.Empty); }
            set { base.SetExtendedAttribute("CreditNumUnit", value); }
        }

        public int CreditNumInitial
        {
            get { return base.GetInt("CreditNumInitial", 0); }
            set { base.SetExtendedAttribute("CreditNumInitial", value.ToString()); }
        }

        public string CashNumName
        {
            get { return base.GetString("CashNumName", "币"); }
            set { base.SetExtendedAttribute("CashNumName", value); }
        }

        public string CashNumUnit
        {
            get { return base.GetString("CashNumUnit", string.Empty); }
            set { base.SetExtendedAttribute("CashNumUnit", value); }
        }

        public int CashNumInitial
        {
            get { return base.GetInt("CashNumInitial", 0); }
            set { base.SetExtendedAttribute("CashNumInitial", value.ToString()); }
        }

        public int CreditMultiplierPostCount
        {
            get { return base.GetInt("CreditMultiplierPostCount", 1); }
            set { base.SetExtendedAttribute("CreditMultiplierPostCount", value.ToString()); }
        }

        public int CreditMultiplierPostDigestCount
        {
            get { return base.GetInt("CreditMultiplierPostDigestCount", 5); }
            set { base.SetExtendedAttribute("CreditMultiplierPostDigestCount", value.ToString()); }
        }

        public int CreditMultiplier
        {
            get { return base.GetInt("CreditMultiplierPrestige", 1); }
            set { base.SetExtendedAttribute("CreditMultiplierPrestige", value.ToString()); }
        }

        public int CashMultiplier
        {
            get { return base.GetInt("CashMultiplier", 1); }
            set { base.SetExtendedAttribute("CashMultiplier", value.ToString()); }
        }

        public bool IsEnable
        {
            get { return base.GetBool("IsEnable", true); }
            set
            {
                base.SetExtendedAttribute("IsEnable", value.ToString());
            }
        }

        /****************用户消息设置********************/
        /// <summary>
        /// 用户系统信息在NewOfDays天内属于最新消息
        /// </summary>
        public int NewOfDays
        {
            get { return base.GetInt("NewOfDays", 1); }
            set
            {
                base.SetExtendedAttribute("NewOfDays", value.ToString());
            }
        }
    }
}
