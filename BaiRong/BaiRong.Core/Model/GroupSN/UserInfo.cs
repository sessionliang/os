using System;

using System.Collections.Specialized;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Model
{
    public class UserInfo : ExtendedAttributes
    {
        public UserInfo()
        {
            this.GroupSN = string.Empty;
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.PasswordFormat = EPasswordFormat.Encrypted;
            this.PasswordSalt = string.Empty;
            this.CreateDate = DateTime.Now;
            this.CreateIPAddress = string.Empty;
            this.LastActivityDate = DateTime.Now;
            this.IsChecked = false;
            this.IsLockedOut = false;
            this.IsTemporary = false;
            this.DisplayName = string.Empty;
            this.Email = string.Empty;
            this.Mobile = string.Empty;
            this.OnlineSeconds = 0;
            this.AvatarLarge = string.Empty;
            this.AvatarMiddle = string.Empty;
            this.AvatarSmall = string.Empty;
            this.Signature = string.Empty;

            this.NewGroupID = 0;
            this.MLibNum = 0;
            this.MLibValidityDate = DateUtils.SqlMinValue;//默认为系统最小日期
        }

        public UserInfo(object dataItem)
            : base(dataItem)
        {
        }

        public int UserID
        {
            get { return base.GetInt(UserAttribute.UserID, 0); }
            set { base.SetExtendedAttribute(UserAttribute.UserID, value.ToString()); }
        }

        public string GroupSN
        {
            get { return base.GetExtendedAttribute(UserAttribute.GroupSN); }
            set { base.SetExtendedAttribute(UserAttribute.GroupSN, value); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(UserAttribute.UserName); }
            set { base.SetExtendedAttribute(UserAttribute.UserName, value); }
        }

        public string Password
        {
            get { return base.GetExtendedAttribute(UserAttribute.Password); }
            set { base.SetExtendedAttribute(UserAttribute.Password, value); }
        }

        public EPasswordFormat PasswordFormat
        {
            get { return EPasswordFormatUtils.GetEnumType(base.GetExtendedAttribute(UserAttribute.PasswordFormat)); }
            set { base.SetExtendedAttribute(UserAttribute.PasswordFormat, EPasswordFormatUtils.GetValue(value)); }
        }

        public string PasswordSalt
        {
            get { return base.GetExtendedAttribute(UserAttribute.PasswordSalt); }
            set { base.SetExtendedAttribute(UserAttribute.PasswordSalt, value); }
        }

        public int GroupID
        {
            get { return base.GetInt(UserAttribute.GroupID, 0); }
            set { base.SetExtendedAttribute(UserAttribute.GroupID, value.ToString()); }
        }

        public int Credits
        {
            get { return base.GetInt(UserAttribute.Credits, 0); }
            set { base.SetExtendedAttribute(UserAttribute.Credits, value.ToString()); }
        }

        public int LevelID
        {
            get { return base.GetInt(UserAttribute.LevelID, 0); }
            set { base.SetExtendedAttribute(UserAttribute.LevelID, value.ToString()); }
        }

        public int Cash
        {
            get { return base.GetInt(UserAttribute.Cash, 0); }
            set { base.SetExtendedAttribute(UserAttribute.Cash, value.ToString()); }
        }

        public DateTime CreateDate
        {
            get { return base.GetDateTime(UserAttribute.CreateDate, DateTime.Now); }
            set { base.SetExtendedAttribute(UserAttribute.CreateDate, value.ToString()); }
        }

        public string CreateIPAddress
        {
            get { return base.GetString(UserAttribute.CreateIPAddress, string.Empty); }
            set { base.SetExtendedAttribute(UserAttribute.CreateIPAddress, value); }
        }

        public DateTime LastActivityDate
        {
            get { return base.GetDateTime(UserAttribute.LastActivityDate, DateTime.Now); }
            set { base.SetExtendedAttribute(UserAttribute.LastActivityDate, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(UserAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(UserAttribute.IsChecked, value.ToString()); }
        }

        public bool IsLockedOut
        {
            get { return base.GetBool(UserAttribute.IsLockedOut, false); }
            set { base.SetExtendedAttribute(UserAttribute.IsLockedOut, value.ToString()); }
        }

        public bool IsTemporary
        {
            get { return base.GetBool(UserAttribute.IsTemporary, false); }
            set { base.SetExtendedAttribute(UserAttribute.IsTemporary, value.ToString()); }
        }

        public string DisplayName
        {
            get
            {
                string displayName = base.GetExtendedAttribute(UserAttribute.DisplayName);
                return (string.IsNullOrEmpty(displayName)) ? this.UserName : displayName;

            }
            set { base.SetExtendedAttribute(UserAttribute.DisplayName, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(UserAttribute.Email); }
            set { base.SetExtendedAttribute(UserAttribute.Email, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(UserAttribute.Mobile); }
            set { base.SetExtendedAttribute(UserAttribute.Mobile, value); }
        }

        public int OnlineSeconds
        {
            get { return base.GetInt(UserAttribute.OnlineSeconds, 0); }
            set { base.SetExtendedAttribute(UserAttribute.OnlineSeconds, value.ToString()); }
        }

        public string AvatarLarge
        {
            get
            {
                string url = base.GetString(UserAttribute.AvatarLarge, string.Empty);
                if (string.IsNullOrEmpty(url))
                {
                    this.SetAvatarUrl();
                    url = base.GetString(UserAttribute.AvatarLarge, string.Empty);
                }
                return url;
            }
            set { base.SetExtendedAttribute(UserAttribute.AvatarLarge, value); }
        }

        public string AvatarMiddle
        {
            get
            {
                string url = base.GetString(UserAttribute.AvatarMiddle, string.Empty);
                if (string.IsNullOrEmpty(url))
                {
                    this.SetAvatarUrl();
                    url = base.GetString(UserAttribute.AvatarMiddle, string.Empty);
                }
                return url;
            }
            set { base.SetExtendedAttribute(UserAttribute.AvatarMiddle, value); }
        }

        public string AvatarSmall
        {
            get
            {
                string url = base.GetString(UserAttribute.AvatarSmall, string.Empty);
                if (string.IsNullOrEmpty(url))
                {
                    this.SetAvatarUrl();
                    url = base.GetString(UserAttribute.AvatarSmall, string.Empty);
                }
                return url;
            }
            set { base.SetExtendedAttribute(UserAttribute.AvatarSmall, value); }
        }

        private void SetAvatarUrl()
        {
            int r = StringUtils.GetRandomInt(1, 100);
            this.AvatarLarge = string.Format("~/sitefiles/bairong/icons/avatars/atavar_large_{0}.jpg", r);
            this.AvatarMiddle = string.Format("~/sitefiles/bairong/icons/avatars/atavar_middle_{0}.jpg", r);
            this.AvatarSmall = string.Format("~/sitefiles/bairong/icons/avatars/atavar_small_{0}.jpg", r);
            if (this.UserID > 0 && !string.IsNullOrEmpty(this.UserName))
            {
                BaiRongDataProvider.UserDAO.Update(this);
            }
        }

        public string Signature
        {
            get { return base.GetExtendedAttribute(UserAttribute.Signature); }
            set { base.SetExtendedAttribute(UserAttribute.Signature, value); }
        }

        public int LoginNum
        {
            get { return base.GetInt(UserAttribute.LoginNum, 0); }
            set { base.SetExtendedAttribute(UserAttribute.LoginNum, value.ToString()); }
        }


        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            get { return base.GetString(UserAttribute.Gender, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Gender, value); }
        }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get { return base.GetDateTime(UserAttribute.Birthday, DateTime.Now); }
            set { base.SetExtendedAttribute(UserAttribute.Birthday, value.ToString("yyyy-MM-dd hh:mm:ss")); }
        }

        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType
        {
            get { return base.GetString(UserAttribute.BloodType, ""); }
            set { base.SetExtendedAttribute(UserAttribute.BloodType, value); }
        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus
        {
            get { return base.GetString(UserAttribute.MaritalStatus, ""); }
            set { base.SetExtendedAttribute(UserAttribute.MaritalStatus, value); }
        }

        /// <summary>
        /// 教育程度
        /// </summary>
        public string Education
        {
            get { return base.GetString(UserAttribute.Education, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Education, value); }
        }

        /// <summary>
        /// 毕业院校
        /// </summary>
        public string Graduation
        {
            get { return base.GetString(UserAttribute.Graduation, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Graduation, value); }
        }

        /// <summary>
        /// 行业
        /// </summary>
        public string Profession
        {
            get { return base.GetString(UserAttribute.Profession, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Profession, value); }
        }

        /// <summary>
        /// qq
        /// </summary>
        public string QQ
        {
            get { return base.GetString(UserAttribute.QQ, ""); }
            set { base.SetExtendedAttribute(UserAttribute.QQ, value); }
        }

        /// <summary>
        /// 微博
        /// </summary>
        public string WeiBo
        {
            get { return base.GetString(UserAttribute.WeiBo, ""); }
            set { base.SetExtendedAttribute(UserAttribute.WeiBo, value); }
        }

        /// <summary>
        /// 微信
        /// </summary>
        public string WeiXin
        {
            get { return base.GetString(UserAttribute.WeiXin, ""); }
            set { base.SetExtendedAttribute(UserAttribute.WeiXin, value); }
        }

        /// <summary>
        /// 兴趣爱好
        /// </summary>
        public string Interests
        {
            get { return base.GetString(UserAttribute.Interests, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Interests, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return base.GetString(UserAttribute.Department, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Department, value); }
        }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            get { return base.GetString(UserAttribute.Position, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Position, value); }
        }

        /// <summary>
        /// 公司
        /// </summary>
        public string Organization
        {
            get { return base.GetString(UserAttribute.Organization, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Organization, value); }
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return base.GetString(UserAttribute.Address, ""); }
            set { base.SetExtendedAttribute(UserAttribute.Address, value); }
        }

        /// <summary>
        /// 密保问题(扩展字段)
        /// </summary>
        public string SCQU
        {
            get { return TranslateUtils.DecryptStringByTranslate(base.GetString("SCQU", "")); }
            set { base.SetExtendedAttribute("SCQU", TranslateUtils.EncryptStringByTranslate(value)); }
        }

        /// <summary>
        /// 是否绑定邮箱(扩展字段)
        /// </summary>
        public bool IsBindEmail
        {
            get { return base.GetBool("IsBindEmail", false); }
            set { base.SetExtendedAttribute("IsBindEmail", value.ToString()); }
        }


        //public string BindedEmail
        //{
        //    get { return base.GetString("BindedEmail", string.Empty); }
        //    set { base.SetExtendedAttribute("BindedEmail", value); }
        //}

        /// <summary>
        /// 是否绑定手机(扩展字段)
        /// </summary>
        public bool IsBindPhone
        {
            get { return base.GetBool("IsBindPhone", false); }
            set { base.SetExtendedAttribute("IsBindPhone", value.ToString()); }
        }

        //public string BindedPhone
        //{
        //    get { return base.GetString("BindedPhone", string.Empty); }
        //    set { base.SetExtendedAttribute("BindedPhone", value); }
        //}

        /// <summary>
        /// 是否设置密保(扩展字段)
        /// </summary>
        public bool IsSetSCQU
        {
            get { return base.GetBool("IsSetSCQU", false); }
            set { base.SetExtendedAttribute("IsSetSCQU", value.ToString()); }
        }

        /// <summary>
        /// 所在地(扩展字段)
        /// </summary>
        public string Location
        {
            get { return base.GetString("Location", string.Empty); }
            set { base.SetExtendedAttribute("Location", value); }
        }

        /// <summary>
        /// 登录失败次数(扩展字段)
        /// </summary>
        public int LoginFailCounter
        {
            get { return base.GetInt("LoginFailCounter", 0); }
            set { base.SetExtendedAttribute("LoginFailCounter", value.ToString()); }
        }

        /// <summary>
        /// 本次锁定时间(扩展字段)
        /// </summary>
        public DateTime LockedTime
        {
            get { return base.GetDateTime("LockedTime", DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute("LockedTime", value.ToString()); }
        }

        /// <summary>
        /// by 20160119 增加新的用户组功能
        /// </summary>
        public int NewGroupID
        {
            get { return base.GetInt(UserAttribute.NewGroupID, 0); }
            set { base.SetExtendedAttribute(UserAttribute.NewGroupID, value.ToString()); }
        }

        /// <summary>
        /// by 20160119 增加新投稿管理功能
        /// </summary>
        public int MLibNum
        {
            get { return base.GetInt(UserAttribute.MLibNum, 0); }
            set { base.SetExtendedAttribute(UserAttribute.MLibNum, value.ToString()); }
        }

        /// <summary>
        ///  by 20160119 增加新投稿管理功能
        /// </summary>
        public DateTime MLibValidityDate
        {
            get { return base.GetDateTime("MLibValidityDate", DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute("MLibValidityDate", value.ToString()); }
        }
        protected override ArrayList GetDefaultAttributesNames()
        {
            return UserAttribute.UserAttributes;
        }
    }
}
