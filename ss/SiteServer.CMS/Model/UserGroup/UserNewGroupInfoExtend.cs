using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class UserNewGroupInfoExtend : ExtendedAttributes
    {
        public UserNewGroupInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        } 

        #region 稿件库 

        /// <summary>
        /// 是否使用投稿设置中的投稿范围
        /// 使用则无需设置，不使用则需要重新设置投稿范围
        /// </summary>
        public bool IsUseMLibScope
        {
            get { return base.GetBool("IsUseMLibScope", false); }
            set { base.SetExtendedAttribute("IsUseMLibScope", value.ToString()); }
        } 

        /// <summary>
        /// 稿件 发布者管理员账号
        /// </summary>
        public string  MLibAddUser
        {
            get { return base.GetString("MLibAddUser", string.Empty); }
            set { base.SetExtendedAttribute("MLibAddUser", value); }
        }
         
        /// <summary>
        /// 用户统一投稿有效期
        /// 多少个月
        /// </summary>
        public int  MLibValidityDate
        {
            get { return base.GetInt("MLibValidityDate", 0); }
            set { base.SetExtendedAttribute("MLibValidityDate", value.ToString()); }
        }
         
        /// <summary>
        /// 用户统一可投稿数量 
        /// </summary>
        public int  MlibNum
        {
            get { return base.GetInt("MlibNum", 0); }
            set { base.SetExtendedAttribute("MlibNum", value.ToString()); }
        }

        /// <summary>
        /// 稿件是否需要审核 
        /// </summary>
        public bool IsMLibCheck
        {
            get { return base.GetBool("IsMLibCheck", true); }
            set { base.SetExtendedAttribute("IsMLibCheck", value.ToString()); }
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
        /// 站点ID_栏目ID
        /// </summary>
        public string MLibScope
        {
            get { return base.GetString("MLibScope", string.Empty); }
            set { base.SetExtendedAttribute("MLibScope", value); }
        }
        #endregion
    }
}
