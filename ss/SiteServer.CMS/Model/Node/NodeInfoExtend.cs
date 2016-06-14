using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Model
{
    public class NodeInfoExtend : ExtendedAttributes
    {
        public NodeInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        //�Ƿ���������Ŀ
        public bool IsChannelAddable
        {
            get { return base.GetBool("IsChannelAddable", true); }
            set { base.SetExtendedAttribute("IsChannelAddable", value.ToString()); }
        }

        //�Ƿ�����������
        public bool IsContentAddable
        {
            get { return base.GetBool("IsContentAddable", true); }
            set { base.SetExtendedAttribute("IsContentAddable", value.ToString()); }
        }

        /// <summary>  
        /// by 20160224 sofuny �����Ƿ������۹�����
        /// </summary>
        public bool IsUseEvaluation
        {
            get { return base.GetBool("IsUseEvaluation", false); }
            set { base.SetExtendedAttribute("IsUseEvaluation", value.ToString()); }
        }

        /// <summary>  
        /// by 20160303 sofuny �����Ƿ������ù�����
        /// </summary>
        public bool IsUseTrial
        {
            get { return base.GetBool("IsUseTrial", false); }
            set { base.SetExtendedAttribute("IsUseTrial", value.ToString()); }
        }

        /// <summary>  
        /// by 20160309 sofuny �����Ƿ��������ʾ���
        /// </summary>
        public bool IsUseSurvey
        {
            get { return base.GetBool("IsUseSurvey", false); }
            set { base.SetExtendedAttribute("IsUseSurvey", value.ToString()); }
        }

        /// <summary>  
        /// by 20160316 sofuny �����Ƿ����ȽϹ���
        /// </summary>
        public bool IsUseCompare
        {
            get { return base.GetBool("IsUseCompare", false); }
            set { base.SetExtendedAttribute("IsUseCompare", value.ToString()); }
        }

        public bool IsChannelCreatable
        {
            get { return base.GetBool("IsChannelCreatable", true); }
            set { base.SetExtendedAttribute("IsChannelCreatable", value.ToString()); }
        }

        public bool IsContentCreatable
        {
            get { return base.GetBool("IsContentCreatable", true); }
            set { base.SetExtendedAttribute("IsContentCreatable", value.ToString()); }
        }

        public bool IsCreateChannelIfContentChanged
        {
            get { return base.GetBool("IsCreateChannelIfContentChanged", true); }
            set { base.SetExtendedAttribute("IsCreateChannelIfContentChanged", value.ToString()); }
        }

        public string CreateChannelIDsIfContentChanged
        {
            get { return base.GetExtendedAttribute("CreateChannelIDsIfContentChanged"); }
            set { base.SetExtendedAttribute("CreateChannelIDsIfContentChanged", value); }
        }

        public string CreateIncludeFilesIfContentChanged
        {
            get { return base.GetExtendedAttribute("CreateIncludeFilesIfContentChanged"); }
            set { base.SetExtendedAttribute("CreateIncludeFilesIfContentChanged", value); }
        }

        public ERestrictionType RestrictionTypeOfChannel
        {
            get { return ERestrictionTypeUtils.GetEnumType(base.GetExtendedAttribute("RestrictionTypeOfChannel")); }
            set { base.SetExtendedAttribute("RestrictionTypeOfChannel", ERestrictionTypeUtils.GetValue(value)); }
        }

        public ERestrictionType RestrictionTypeOfContent
        {
            get { return ERestrictionTypeUtils.GetEnumType(base.GetExtendedAttribute("RestrictionTypeOfContent")); }
            set { base.SetExtendedAttribute("RestrictionTypeOfContent", ERestrictionTypeUtils.GetValue(value)); }
        }

        public string ContentAttributesOfDisplay
        {
            get { return base.GetExtendedAttribute("ContentAttributesOfDisplay"); }
            set { base.SetExtendedAttribute("ContentAttributesOfDisplay", value); }
        }

        public ECrossSiteTransType TransType
        {
            get { return ECrossSiteTransTypeUtils.GetEnumType(base.GetExtendedAttribute("TransType")); }
            set { base.SetExtendedAttribute("TransType", ECrossSiteTransTypeUtils.GetValue(value)); }
        }

        public int TransPublishmentSystemID
        {
            get { return TranslateUtils.ToInt(base.GetExtendedAttribute("TransPublishmentSystemID")); }
            set { base.SetExtendedAttribute("TransPublishmentSystemID", value.ToString()); }
        }

        public string TransNodeIDs
        {
            get { return base.GetExtendedAttribute("TransNodeIDs"); }
            set { base.SetExtendedAttribute("TransNodeIDs", value); }
        }

        public string TransNodeNames
        {
            get { return base.GetExtendedAttribute("TransNodeNames"); }
            set { base.SetExtendedAttribute("TransNodeNames", value); }
        }

        public bool TransIsAutomatic
        {
            get { return base.GetBool("TransIsAutomatic", false); }
            set { base.SetExtendedAttribute("TransIsAutomatic", value.ToString()); }
        }

        //����ת���������ͣ����� ���õ�ַ ��������
        public ETranslateContentType TransDoneType
        {
            get { return ETranslateContentTypeUtils.GetEnumType(base.GetExtendedAttribute("TransDoneType")); }
            set { base.SetExtendedAttribute("TransDoneType", ETranslateContentTypeUtils.GetValue(value)); }
        }

        //�Ƿ���Ҫɾ��Ԥ������
        public bool IsPreviewContentToDelete
        {
            get { return base.GetBool("IsPreviewContentToDelete", false); }
            set { base.SetExtendedAttribute("IsPreviewContentToDelete", value.ToString()); }
        }

        /****************����ǩ������********************/

        public bool IsSignin
        {
            get { return base.GetBool("IsSignin", false); }
            set { base.SetExtendedAttribute("IsSignin", value.ToString()); }
        }

        public bool IsSigninGroup
        {
            get { return base.GetBool("IsSigninGroup", true); }
            set { base.SetExtendedAttribute("IsSigninGroup", value.ToString()); }
        }

        public string SigninUserGroupCollection
        {
            get { return base.GetExtendedAttribute("SigninUserGroupCollection"); }
            set { base.SetExtendedAttribute("SigninUserGroupCollection", value); }
        }

        public string SigninUserNameCollection
        {
            get { return base.GetExtendedAttribute("SigninUserNameCollection"); }
            set { base.SetExtendedAttribute("SigninUserNameCollection", value); }
        }

        public int SigninPriority
        {
            get { return TranslateUtils.ToInt(base.GetExtendedAttribute("SigninPriority")); }
            set { base.SetExtendedAttribute("SigninPriority", value.ToString()); }
        }

        public string SigninEndDate
        {
            get { return base.GetExtendedAttribute("SigninEndDate"); }
            set { base.SetExtendedAttribute("SigninEndDate", value); }
        }

        #region B2C

        public int SpecCount
        {
            get { return base.GetInt("SpecCount", 0); }
            set { base.SetExtendedAttribute("SpecCount", value.ToString()); }
        }

        public int FilterCount
        {
            get { return base.GetInt("FilterCount", 0); }
            set { base.SetExtendedAttribute("FilterCount", value.ToString()); }
        }

        public bool IsBrandSpecified
        {
            get { return base.GetBool("IsBrandSpecified", false); }
            set { base.SetExtendedAttribute("IsBrandSpecified", value.ToString()); }
        }

        public int BrandNodeID
        {
            get { return base.GetInt("BrandNodeID", 0); }
            set { base.SetExtendedAttribute("BrandNodeID", value.ToString()); }
        }

        #endregion

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
