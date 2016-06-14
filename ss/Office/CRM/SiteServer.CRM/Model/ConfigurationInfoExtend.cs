using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

using System.Collections.Generic;

namespace SiteServer.CRM.Model
{
    public class ConfigurationInfoExtend : ExtendedAttributes
    {
        public ConfigurationInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }

        /*******************************CRM***************************************/

        public List<string> CRMRequestTypeCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToStringList(base.GetExtendedAttribute("CRMRequestTypeCollection"));
            }
            set
            {
                base.SetExtendedAttribute("CRMRequestTypeCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }

        public List<int> CRMDepartmentIDCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToIntList(base.GetExtendedAttribute("CRMDepartmentIDCollection"));
            }
            set
            {
                base.SetExtendedAttribute("CRMDepartmentIDCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }

        public List<int> CRMRequestDepartmentIDCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToIntList(base.GetExtendedAttribute("CRMRequestDepartmentIDCollection"));
            }
            set
            {
                base.SetExtendedAttribute("CRMRequestDepartmentIDCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }

        /*******************************PROJECT***********************************/

        public List<string> ProjectTypeCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToStringList(base.GetExtendedAttribute("ProjectTypeCollection"));
            }
            set
            {
                base.SetExtendedAttribute("ProjectTypeCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }

        public int ProjectApplyDateLimit              //∞Ï¿Ì ±œﬁ
        {
            get { return base.GetInt("ProjectApplyDateLimit", 15); }
            set { base.SetExtendedAttribute("ProjectApplyDateLimit", value.ToString()); }
        }

        public int ProjectApplyAlertDate              //‘§æØ
        {
            get { return base.GetInt("ProjectApplyAlertDate", -3); }
            set { base.SetExtendedAttribute("ProjectApplyAlertDate", value.ToString()); }
        }

        public int ProjectApplyYellowAlertDate      //ª∆≈∆
        {
            get { return base.GetInt("ProjectApplyYellowAlertDate", 3); }
            set { base.SetExtendedAttribute("ProjectApplyYellowAlertDate", value.ToString()); }
        }

        public int ProjectApplyRedAlertDate       //∫Ï≈∆
        {
            get { return base.GetInt("ProjectApplyRedAlertDate", 10); }
            set { base.SetExtendedAttribute("ProjectApplyRedAlertDate", value.ToString()); }
        }

        public bool ProjectApplyIsDeleteAllowed   // «∑Ò‘ –Ì…æ≥˝
        {
            get { return base.GetBool("ProjectApplyIsDeleteAllowed", true); }
            set { base.SetExtendedAttribute("ProjectApplyIsDeleteAllowed", value.ToString()); }
        }


        /*******************************PAYMENT***********************************/

        public List<string> PaymentTypeCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToStringList(base.GetExtendedAttribute("PaymentTypeCollection"));
            }
            set
            {
                base.SetExtendedAttribute("PaymentTypeCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }

        public List<string> PaymentCashBackTypeCollection
        {
            get
            {
                return TranslateUtils.StringCollectionToStringList(base.GetExtendedAttribute("PaymentCashBackTypeCollection"));
            }
            set
            {
                base.SetExtendedAttribute("PaymentCashBackTypeCollection", TranslateUtils.ObjectCollectionToString(value));
            }
        }


    }
}
