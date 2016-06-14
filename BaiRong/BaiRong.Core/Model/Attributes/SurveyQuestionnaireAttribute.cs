using BaiRong.Core;
using System;
using System.Collections;

namespace BaiRong.Model
{
    public class SurveyQuestionnaireAttribute
    {
        protected SurveyQuestionnaireAttribute()
        {
        }

        //hidden
        public static string SQID = "SQID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string NodeID = "NodeID";
        public static string ContentID = "ContentID";
        public static string TAID = "TAID";
        public static string Taxis = "Taxis";
        public static string SurveyStatus = "SurveyStatus";
        public static string UserName = "UserName";
        public static string AdminName = "AdminName";
        public static string IPAddress = "IPAddress";
        public static string Location = "Location";
        public static string AddDate = "AddDate";
        public static string Reply = "Reply";
        public static string ReplyAdmin = "ReplyAdmin";
        public static string IsEmail = "IsEmail";
        public static string IsMobile = "IsMobile";

        #region 调查问卷字段
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
        public static string Ext10 = "Ext10";
        public static string Ext11 = "Ext11";
        public static string Ext12 = "Ext12";
        public static string Ext13 = "Ext13";
        public static string Ext14 = "Ext14";
        public static string Ext15 = "Ext15";
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
                    #region 调查问卷字段
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
                    basicAttributes.Add(Ext10.ToLower());
                    basicAttributes.Add(Ext11.ToLower());
                    basicAttributes.Add(Ext12.ToLower());
                    basicAttributes.Add(Ext13.ToLower());
                    basicAttributes.Add(Ext14.ToLower());
                    basicAttributes.Add(Ext15.ToLower());
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
                    hiddenAttributes.Add(SQID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(ContentID.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(TAID.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(Reply.ToLower());
                    hiddenAttributes.Add(SurveyStatus.ToLower());
                    hiddenAttributes.Add(Reply.ToLower());
                    hiddenAttributes.Add(ReplyAdmin.ToLower());
                    hiddenAttributes.Add(IsEmail.ToLower());
                    hiddenAttributes.Add(IsMobile.ToLower());
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
            if (StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Description) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.CompositeScore))
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
            if (StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext9) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext10) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext11) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext12) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext13) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext14) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext15))
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
            if (StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.CompositeScore)
            || StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext1) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext2) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext3) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext4) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext5) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext6) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext7) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext8) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext9) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext10) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext11) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext12) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext13) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext14) ||
                StringUtils.EqualsIgnoreCase(attributeName, SurveyQuestionnaireAttribute.Ext15))
            {
                return true;
            }
            return false;
        }
    }
}