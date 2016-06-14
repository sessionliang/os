using BaiRong.Core;
using System;
using System.Collections;

namespace BaiRong.Model
{
    public class TrialApplyAttribute
    {
        protected TrialApplyAttribute()
        {
        }

        //hidden
        public static string TAID = "TAID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string NodeID = "NodeID";
        public static string ContentID = "ContentID";
        public static string Taxis = "Taxis";
        public static string ApplyStatus = "ApplyStatus";
        public static string IsChecked = "IsChecked";
        public static string CheckAdmin = "CheckAdmin";
        public static string CheckDate = "CheckDate";
        public static string UserName = "UserName";
        public static string IPAddress = "IPAddress";
        public static string Location = "Location";
        public static string AddDate = "AddDate";
        public static string IsReport = "IsReport";
        public static string IsEmail = "IsEmail";
        public static string IsMobile = "IsMobile";

        #region ÉêÇë×Ö¶Î
        public static string Name = "Name";
        public static string Phone = "Phone";
        public static string Ext1 = "Ext1";
        public static string Ext2 = "Ext2";
        public static string Ext3 = "Ext3";
        #endregion

        public static string SettingsXML = "SettingsXML";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
                return arraylist;
            }
        }

        private static ArrayList basicAttributes;
        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    #region ÉêÇë×Ö¶Î
                    basicAttributes.Add(Name.ToLower());
                    basicAttributes.Add(Phone.ToLower());
                    basicAttributes.Add(Ext1.ToLower());
                    basicAttributes.Add(Ext2.ToLower());
                    basicAttributes.Add(Ext3.ToLower());
                    #endregion

                }

                return basicAttributes;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(TAID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(ContentID.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(ApplyStatus.ToLower());
                    hiddenAttributes.Add(IsChecked.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(CheckAdmin.ToLower());
                    hiddenAttributes.Add(CheckDate.ToLower());
                    hiddenAttributes.Add(IsReport.ToLower());
                    hiddenAttributes.Add(IsEmail.ToLower());
                    hiddenAttributes.Add(IsMobile.ToLower());
                    hiddenAttributes.Add(SettingsXML.ToLower());
                }

                return hiddenAttributes;
            }
        }

        /// <summary>
        /// Ä¬ÈÏ×Ö¶Î     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsDefaultAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, TrialApplyAttribute.Name) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialApplyAttribute.Phone))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ¿É×öÍ³¼Æ·ÖÎöµÄÀ©Õ¹×Ö¶Î     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsExtendAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, TrialApplyAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialApplyAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialApplyAttribute.Ext3))
            {
                return true;
            }
            return false;
        }

    }
}