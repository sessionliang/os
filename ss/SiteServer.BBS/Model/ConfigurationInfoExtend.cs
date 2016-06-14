using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Model
{
    public class ConfigurationInfoExtend : ExtendedAttributes
    {
        public ConfigurationInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        //----------------论坛信息设置------------------------

        public bool IsCloseBBS
        {
            get {
                return base.GetBool("IsCloseBBS", false);
            }
            set {
                base.SetExtendedAttribute("IsCloseBBS", value.ToString());
            }
        }

        public string CloseBBSReason {
            get {
                return base.GetString("CloseBBSReason", string.Empty);
            }
            set {
                base.SetExtendedAttribute("CloseBBSReason", value.ToString());
            }
        }

        public string BBSName
        {
            get {
                return base.GetString("BBSName", "SiteServer BBS 论坛");
            }
            set {
                base.SetExtendedAttribute("BBSName", value.ToString());
            }
        }

        public string SiteName
        {
            get {
                return base.GetString("SiteName","SiteServer");
            }
            set {
                base.SetExtendedAttribute("SiteName", value.ToString());
            }
        }

        public string SiteUrl
        {
            get {
                return base.GetString("SiteUrl", "www.siteserver.cn");
            }
            set {
                base.SetExtendedAttribute("SiteUrl", value.ToString());
            }
        }

        public string AdminEmail
        {
            get {
                return base.GetString("AdminEmail", "admin@admin.com");
            }
            set {
                base.SetExtendedAttribute("AdminEmail", value);
            }
        }

        public string CountCode {
            get {
                return base.GetString("CountCode", string.Empty);
            }
            set {
                base.SetExtendedAttribute("CountCode", value);
            }
        }

        public bool IsLogBBS { 
            get {
                return base.GetBool("IsLogBBS", false);
            }
            set {
                base.SetExtendedAttribute("IsLogBBS", value.ToString());
            }
        }

        //----------------访问控制------------------------

        public int NovitiateByMinute {
            get {
                return base.GetInt("NovitiateByMinute", 0);
            }
            set {
                base.SetExtendedAttribute("NovitiateByMinute", value.ToString());
            }
        }

        public string ForbiddenAccessTime {
            get {
                return base.GetString("ForbiddenAccessTime", string.Empty);
            }
            set {
                base.SetExtendedAttribute("ForbiddenAccessTime", value);
            }
        }

        public string ForbiddenPostTime {
            get {
                return base.GetString("ForbiddenPostTime", string.Empty);
            }
            set {
                base.SetExtendedAttribute("ForbiddenPostTime", value);
            }
        }

        //----------------防灌水设置------------------------

        public int PostInterval
        {
            get { return base.GetInt("PostInterval", 15); }
            set { base.SetExtendedAttribute("PostInterval", value.ToString()); }
        }

        public bool IsVerifyCodeThread
        {
            get { return base.GetBool("IsVerifyCodeThread", false); }
            set { base.SetExtendedAttribute("IsVerifyCodeThread", value.ToString()); }
        }

        public bool IsVerifyCodePost
        {
            get { return base.GetBool("IsVerifyCodePost", false); }
            set { base.SetExtendedAttribute("IsVerifyCodePost", value.ToString()); }
        }

        public int PostVerifyCodeCount
        {
            get { return base.GetInt("PostVerifyCodeCount", 0); }
            set { base.SetExtendedAttribute("PostVerifyCodeCount", value.ToString()); }
        }

        // 以下暂不用
        //----------------显示界面设置-------------------- 

        public string DefaultTheme
        {
            get { return base.GetString("DefaultTheme", "default"); }
            set { base.SetExtendedAttribute("DefaultTheme", value); }
        }

        public string LogoUrl
        {
            get { return base.GetString("LogoUrl", string.Empty); }
            set { base.SetExtendedAttribute("LogoUrl", value); }
        }

        public string BBSFooter
        {
            get { return base.GetString("BBSFooter", "北京百容千域软件技术开发有限公司 版权所有"); }
            set { base.SetExtendedAttribute("BBSFooter", value); }
        }

        //----------------审核设置------------------------

        public bool IsCheckByUserGroup
        {
            get { return base.GetBool("IsCheckByUserGroup", false); }
            set { base.SetExtendedAttribute("IsCheckByUserGroup", value.ToString()); }
        }

        public bool IsCheckQuestion
        {
            get { return base.GetBool("IsCheckQuestion", false); }
            set { base.SetExtendedAttribute("IsCheckQuestion", value.ToString()); }
        }

        public bool IsCheckAnswer
        {
            get { return base.GetBool("IsCheckAnswer", false); }
            set { base.SetExtendedAttribute("IsCheckAnswer", value.ToString()); }
        }

        public bool IsCheckComment
        {
            get { return base.GetBool("IsCheckComment", false); }
            set { base.SetExtendedAttribute("IsCheckComment", value.ToString()); }
        }

        public string CheckQuestionUserGroups
        {
            get { return base.GetString("CheckQuestionUserGroups", string.Empty); }
            set { base.SetExtendedAttribute("CheckQuestionUserGroups", value); }
        }

        public string CheckAnswerUserGroups
        {
            get { return base.GetString("CheckAnswerUserGroups", string.Empty); }
            set { base.SetExtendedAttribute("CheckAnswerUserGroups", value); }
        }

        public string CheckCommentUserGroups
        {
            get { return base.GetString("CheckContentUserGroups", string.Empty); }
            set { base.SetExtendedAttribute("CheckContentUserGroups", value); }
        }

        public bool IsFullScreen
        {
            get
            {
                return base.GetBool("IsFullScreen", true);
            }
            set
            {
                base.SetExtendedAttribute("IsFullScreen", value.ToString());
            }
        }

        public int DisplayColumns
        {
            get
            {
                return base.GetInt("DisplayColumns", 1);
            }
            set
            {
                base.SetExtendedAttribute("DisplayColumns", value.ToString());
            }
        }

        public int ThreadPageNum
        {
            get
            {
                return base.GetInt("ThreadPageNum", 20);
            }
            set
            {
                base.SetExtendedAttribute("ThreadPageNum", value.ToString());
            }
        }

        public int PostPageNum
        {
            get
            {
                return base.GetInt("PostPageNum", 10);
            }
            set
            {
                base.SetExtendedAttribute("PostPageNum", value.ToString());
            }
        }

        //------------------------------监控设置---------------------------------
        public string NotHandleForumIDCollection
        {
            get { return base.GetString("NotHandleForumIDCollection", string.Empty); }
            set { base.SetExtendedAttribute("NotHandleForumIDCollection", value); }
        }  

        //------------------------------生成设置---------------------------------

        public string TemplateDir
        {
            get { return base.GetString("TemplateDir", "default"); }
            set { base.SetExtendedAttribute("TemplateDir", value); }
        }

        public string FilePathRule
        {
            get { return base.GetString("FilePathRule", "/forums/{@ForumID}.aspx"); }
            set { base.SetExtendedAttribute("FilePathRule", value); }
        }

        public bool IsCreateDoubleClick
        {
            get { return base.GetBool("IsCreateDoubleClick", false); }
            set { base.SetExtendedAttribute("IsCreateDoubleClick", value.ToString()); }
        }

        //----------------------------------上传设置--------------------------------


        public bool IsSaveImageInTextEditor
        {
            get { return base.GetBool("IsSaveImageInTextEditor", true); }
            set { base.SetExtendedAttribute("IsSaveImageInTextEditor", value.ToString()); }
        }

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

        //-----------------------------图片水印设置------------------------------------

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

        //-----------------------------用户积分设置------------------------------------

        public string CreditNamePrestige
        {
            get { return base.GetString("CreditNamePrestige", "威望"); }
            set { base.SetExtendedAttribute("CreditNamePrestige", value); }
        }

        public string CreditUnitPrestige
        {
            get { return base.GetString("CreditUnitPrestige", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitPrestige", value); }
        }

        public int CreditInitialPrestige
        {
            get { return base.GetInt("CreditInitialPrestige", 0); }
            set { base.SetExtendedAttribute("CreditInitialPrestige", value.ToString()); }
        }

        public string CreditNameContribution
        {
            get { return base.GetString("CreditNameContribution", "贡献"); }
            set { base.SetExtendedAttribute("CreditNameContribution", value); }
        }

        public string CreditUnitContribution
        {
            get { return base.GetString("CreditUnitContribution", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitContribution", value); }
        }

        public int CreditInitialContribution
        {
            get { return base.GetInt("CreditInitialContribution", 0); }
            set { base.SetExtendedAttribute("CreditInitialContribution", value.ToString()); }
        }

        public string CreditNameCurrency
        {
            get { return base.GetString("CreditNameCurrency", "金钱"); }
            set { base.SetExtendedAttribute("CreditNameCurrency", value); }
        }

        public string CreditUnitCurrency
        {
            get { return base.GetString("CreditUnitCurrency", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitCurrency", value); }
        }

        public int CreditInitialCurrency
        {
            get { return base.GetInt("CreditInitialCurrency", 0); }
            set { base.SetExtendedAttribute("CreditInitialCurrency", value.ToString()); }
        }

        public bool CreditUsingExtCredit1
        {
            get { return base.GetBool("CreditUsingExtCredit1", false); }
            set { base.SetExtendedAttribute("CreditUsingExtCredit1", value.ToString()); }
        }

        public string CreditNameExtCredit1
        {
            get { return base.GetString("CreditNameExtCredit1", string.Empty); }
            set { base.SetExtendedAttribute("CreditNameExtCredit1", value); }
        }

        public string CreditUnitExtCredit1
        {
            get { return base.GetString("CreditUnitExtCredit1", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitExtCredit1", value); }
        }

        public int CreditInitialExtCredit1
        {
            get { return base.GetInt("CreditInitialExtCredit1", 0); }
            set { base.SetExtendedAttribute("CreditInitialExtCredit1", value.ToString()); }
        }

        public bool CreditUsingExtCredit2
        {
            get { return base.GetBool("CreditUsingExtCredit2", false); }
            set { base.SetExtendedAttribute("CreditUsingExtCredit2", value.ToString()); }
        }

        public string CreditNameExtCredit2
        {
            get { return base.GetString("CreditNameExtCredit2", string.Empty); }
            set { base.SetExtendedAttribute("CreditNameExtCredit2", value); }
        }

        public string CreditUnitExtCredit2
        {
            get { return base.GetString("CreditUnitExtCredit2", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitExtCredit2", value); }
        }

        public int CreditInitialExtCredit2
        {
            get { return base.GetInt("CreditInitialExtCredit2", 0); }
            set { base.SetExtendedAttribute("CreditInitialExtCredit2", value.ToString()); }
        }

        public bool CreditUsingExtCredit3
        {
            get { return base.GetBool("CreditUsingExtCredit3", false); }
            set { base.SetExtendedAttribute("CreditUsingExtCredit3", value.ToString()); }
        }

        public string CreditNameExtCredit3
        {
            get { return base.GetString("CreditNameExtCredit3", string.Empty); }
            set { base.SetExtendedAttribute("CreditNameExtCredit3", value); }
        }

        public string CreditUnitExtCredit3
        {
            get { return base.GetString("CreditUnitExtCredit3", string.Empty); }
            set { base.SetExtendedAttribute("CreditUnitExtCredit3", value); }
        }

        public int CreditInitialExtCredit3
        {
            get { return base.GetInt("CreditInitialExtCredit3", 0); }
            set { base.SetExtendedAttribute("CreditInitialExtCredit3", value.ToString()); }
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

        public int CreditMultiplierPrestige
        {
            get { return base.GetInt("CreditMultiplierPrestige", 2); }
            set { base.SetExtendedAttribute("CreditMultiplierPrestige", value.ToString()); }
        }

        public int CreditMultiplierContribution
        {
            get { return base.GetInt("CreditMultiplierContribution", 1); }
            set { base.SetExtendedAttribute("CreditMultiplierContribution", value.ToString()); }
        }

        public int CreditMultiplierCurrency
        {
            get { return base.GetInt("CreditMultiplierCurrency", 1); }
            set { base.SetExtendedAttribute("CreditMultiplierCurrency", value.ToString()); }
        }

        public int CreditMultiplierExtCredit1
        {
            get
            {
                if (this.CreditUsingExtCredit1)
                {
                    return base.GetInt("CreditMultiplierExtCredit1", 1);
                }
                return 0;
            }
            set { base.SetExtendedAttribute("CreditMultiplierExtCredit1", value.ToString()); }
        }

        public int CreditMultiplierExtCredit2
        {
            get
            {
                if (this.CreditUsingExtCredit2)
                {
                    return base.GetInt("CreditMultiplierExtCredit2", 1);
                }
                return 0;
            }
            set { base.SetExtendedAttribute("CreditMultiplierExtCredit2", value.ToString()); }
        }

        public int CreditMultiplierExtCredit3
        {
            get
            {
                if (this.CreditUsingExtCredit3)
                {
                    return base.GetInt("CreditMultiplierExtCredit3", 1);
                }
                return 0;
            }
            set { base.SetExtendedAttribute("CreditMultiplierExtCredit3", value.ToString()); }
        }

        //-----------------------------IP限制-------------------------------------//

        public ERestrictionType RestrictionType
        {
            get { return ERestrictionTypeUtils.GetEnumType(base.GetString("RestrictionType", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionType", ERestrictionTypeUtils.GetValue(value)); }
        }

        public StringCollection RestrictionBlackList
        {
            get { return TranslateUtils.StringCollectionToStringCollection(base.GetString("RestrictionBlackList", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionBlackList", TranslateUtils.ObjectCollectionToString(value)); }
        }

        public StringCollection RestrictionWhiteList
        {
            get { return TranslateUtils.StringCollectionToStringCollection(base.GetString("RestrictionWhiteList", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionWhiteList", TranslateUtils.ObjectCollectionToString(value)); }
        }

        //-----------------------------在线用户-------------------------------------//

        public bool IsOnlineInIndexPage
        {
            get { return base.GetBool("IsOnlineInIndexPage", true); }
            set { base.SetExtendedAttribute("IsOnlineInIndexPage", value.ToString()); }
        }

        public bool IsOnlineUserOnly
        {
            get { return base.GetBool("IsOnlineUserOnly", false); }
            set { base.SetExtendedAttribute("IsOnlineUserOnly", value.ToString()); }
        }

        public int OnlineMaxInIndexPage
        {
            get
            {
                return base.GetInt("OnlineMaxInIndexPage", 300);
            }
            set { base.SetExtendedAttribute("OnlineMaxInIndexPage", value.ToString()); }
        }

        //活动超时时间初始化 单位：分钟
        public int OnlineTimeout
        {
            get
            {
                return base.GetInt("OnlineTimeout", 60);
            }
            set { base.SetExtendedAttribute("OnlineTimeout", value.ToString()); }
        }

        public int OnlineMaxCount
        {
            get
            {
                return base.GetInt("OnlineMaxCount", 0);
            }
            set { base.SetExtendedAttribute("OnlineMaxCount", value.ToString()); }
        }

        public string OnlineMaxDateTime
        {
            get { return base.GetString("OnlineMaxDateTime", string.Empty); }
            set { base.SetExtendedAttribute("OnlineMaxDateTime", value); }
        }

        public int StatTodayPostCount
        {
            get
            {
                return base.GetInt("StatTodayPostCount", 0);
            }
            set { base.SetExtendedAttribute("StatTodayPostCount", value.ToString()); }
        }

        public int StatYesterdayPostCount
        {
            get
            {
                return base.GetInt("StatYesterdayPostCount", 0);
            }
            set { base.SetExtendedAttribute("StatYesterdayPostCount", value.ToString()); }
        }

        public int StatPostCount
        {
            get
            {
                return base.GetInt("StatPostCount", 0);
            }
            set { base.SetExtendedAttribute("StatPostCount", value.ToString()); }
        }

        public int StatUserCount
        {
            get
            {
                return base.GetInt("StatUserCount", 0);
            }
            set { base.SetExtendedAttribute("StatUserCount", value.ToString()); }
        }

        public string StatDateTime
        {
            get { return base.GetString("StatDateTime", string.Empty); }
            set { base.SetExtendedAttribute("StatDateTime", value); }
        }

        public string StatUserNameRecently
        {
            get { return base.GetString("StatUserNameRecently", string.Empty); }
            set { base.SetExtendedAttribute("StatUserNameRecently", value); }
        }
    }
}
