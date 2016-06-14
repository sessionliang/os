using BaiRong.Core;
using System;
using System.Collections;

namespace BaiRong.Model
{
    public class TrialReportAttribute
    {
        protected TrialReportAttribute()
        {
        }

        //hidden
        public static string TRID = "TRID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string NodeID = "NodeID";
        public static string ContentID = "ContentID";
        public static string TAID = "TAID";
        public static string Taxis = "Taxis";
        public static string ReportStatus = "ReportStatus";
        public static string UserName = "UserName";
        public static string AdminName = "AdminName";
        public static string IPAddress = "IPAddress";
        public static string Location = "Location";
        public static string AddDate = "AddDate";
        public static string Reply = "Reply";

        #region 报告字段
        public static string Description = "Description";
        public static string CompositeScore = "CompositeScore";
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
                    #region 报告字段
                    basicAttributes.Add(Description.ToLower());
                    basicAttributes.Add(CompositeScore.ToLower());
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
                    hiddenAttributes.Add(TRID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(ContentID.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(TAID.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(AdminName.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(Reply.ToLower());
                    hiddenAttributes.Add(ReportStatus.ToLower());
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
            if (StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Description) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.CompositeScore))
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
            if (StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext9))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 可做统计分析的字段     
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool IsAnalysisAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.CompositeScore) || StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, TrialReportAttribute.Ext9))
            {
                return true;
            }
            return false;
        }
    }
}