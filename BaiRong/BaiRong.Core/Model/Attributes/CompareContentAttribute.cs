using BaiRong.Core;
using System;
using System.Collections;

namespace BaiRong.Model
{
    public class CompareContentAttribute
    {
        protected CompareContentAttribute()
        {
        }

        //hidden
        public static string CCID = "CCID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string NodeID = "NodeID";
        public static string ContentID = "ContentID";
        public static string Taxis = "Taxis";
        public static string CompareStatus = "CompareStatus";
        public static string UserName = "UserName";
        public static string IPAddress = "IPAddress";
        public static string Location = "Location";
        public static string AddDate = "AddDate";
        public static string AdminName = "AdminName"; 

        #region 比较功能字段
        public static string Description = "Description";
        public static string CompositeScore1 = "CompositeScore1";
        public static string CompositeScore2 = "CompositeScore2";
        public static string Ext1 = "Ext1";
        public static string Ext2 = "Ext2";
        public static string Ext3 = "Ext3";
        public static string Ext4 = "Ext4";
        public static string Ext5 = "Ext5";
        public static string Ext6 = "Ext6";
        public static string Ext7 = "Ext7";
        public static string Ext8 = "Ext8";
        public static string Ext9 = "Ext9";
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
                    #region 比较功能字段
                    basicAttributes.Add(Description.ToLower());
                    basicAttributes.Add(CompositeScore1.ToLower());
                    basicAttributes.Add(CompositeScore2.ToLower());
                    basicAttributes.Add(Ext1.ToLower());
                    basicAttributes.Add(Ext2.ToLower());
                    basicAttributes.Add(Ext3.ToLower());
                    basicAttributes.Add(Ext4.ToLower());
                    basicAttributes.Add(Ext5.ToLower());
                    basicAttributes.Add(Ext6.ToLower());
                    basicAttributes.Add(Ext7.ToLower());
                    basicAttributes.Add(Ext8.ToLower());
                    basicAttributes.Add(Ext9.ToLower());
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
                    hiddenAttributes.Add(CCID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(ContentID.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(CompareStatus.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(AdminName.ToLower()); 
                    hiddenAttributes.Add(SettingsXML.ToLower());
                }

                return hiddenAttributes;
            }
        }

        /// <summary>
        /// 可做统计分析的扩展字段     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsDefaultAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Description) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.CompositeScore1) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.CompositeScore2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 可做统计分析的扩展字段     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsExtendAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext9))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 可做统计分析的扩展字段     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsAnalysisAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.CompositeScore1) || StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.CompositeScore2) || StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, CompareContentAttribute.Ext9))
            {
                return true;
            }
            return false;
        }
    }
}