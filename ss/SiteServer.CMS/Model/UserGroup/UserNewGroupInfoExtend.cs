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

        #region ����� 

        /// <summary>
        /// �Ƿ�ʹ��Ͷ�������е�Ͷ�巶Χ
        /// ʹ�����������ã���ʹ������Ҫ��������Ͷ�巶Χ
        /// </summary>
        public bool IsUseMLibScope
        {
            get { return base.GetBool("IsUseMLibScope", false); }
            set { base.SetExtendedAttribute("IsUseMLibScope", value.ToString()); }
        } 

        /// <summary>
        /// ��� �����߹���Ա�˺�
        /// </summary>
        public string  MLibAddUser
        {
            get { return base.GetString("MLibAddUser", string.Empty); }
            set { base.SetExtendedAttribute("MLibAddUser", value); }
        }
         
        /// <summary>
        /// �û�ͳһͶ����Ч��
        /// ���ٸ���
        /// </summary>
        public int  MLibValidityDate
        {
            get { return base.GetInt("MLibValidityDate", 0); }
            set { base.SetExtendedAttribute("MLibValidityDate", value.ToString()); }
        }
         
        /// <summary>
        /// �û�ͳһ��Ͷ������ 
        /// </summary>
        public int  MlibNum
        {
            get { return base.GetInt("MlibNum", 0); }
            set { base.SetExtendedAttribute("MlibNum", value.ToString()); }
        }

        /// <summary>
        /// ����Ƿ���Ҫ��� 
        /// </summary>
        public bool IsMLibCheck
        {
            get { return base.GetBool("IsMLibCheck", true); }
            set { base.SetExtendedAttribute("IsMLibCheck", value.ToString()); }
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
        /// վ��ID_��ĿID
        /// </summary>
        public string MLibScope
        {
            get { return base.GetString("MLibScope", string.Empty); }
            set { base.SetExtendedAttribute("MLibScope", value); }
        }
        #endregion
    }
}
