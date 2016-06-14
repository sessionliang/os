using System;
using System.Collections;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Model
{
	public class ResumeContentAttribute
	{
        protected ResumeContentAttribute()
		{
		}

		//hidden
		public static string ID = "ID";
        public static string StyleID = "StyleID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string JobContentID = "JobContentID";
        public static string UserName = "UserName";
        public static string IsView = "IsView";
        public static string AddDate = "AddDate";

        //basic
        public const string RealName = "RealName";
        public const string Nationality = "Nationality";
        public const string Gender = "Gender";
        public const string Email = "Email";
        public const string MobilePhone = "MobilePhone";
        public const string HomePhone = "HomePhone";
        public const string LastSchoolName = "LastSchoolName";
        public const string Education = "Education";
        public const string IDCardType = "IDCardType";
        public const string IDCardNo = "IDCardNo";
        public const string Birthday = "Birthday";
        public const string Marriage = "Marriage";
        public const string WorkYear = "WorkYear";
        public const string Profession = "Profession";
        public const string ExpectSalary = "ExpectSalary";
        public const string AvailabelTime = "AvailabelTime";
        public const string Location = "Location";
        public const string ImageUrl = "ImageUrl";
        public const string Summary = "Summary";

        //extend
        public const string Exp_Count = "Exp_Count";
        public const string Exp_FromYear = "Exp_FromYear";
        public const string Exp_FromMonth = "Exp_FromMonth";
        public const string Exp_ToYear = "Exp_ToYear";
        public const string Exp_ToMonth = "Exp_ToMonth";
        public const string Exp_EmployerName = "Exp_EmployerName";
        public const string Exp_Department = "Exp_Department";
        public const string Exp_EmployerPhone = "Exp_EmployerPhone";
        public const string Exp_WorkPlace = "Exp_WorkPlace";
        public const string Exp_PositionTitle = "Exp_PositionTitle";
        public const string Exp_Industry = "Exp_Industry";
        public const string Exp_Summary = "Exp_Summary";
        public const string Exp_Score = "Exp_Score";

        public const string Pro_Count = "Pro_Count";
        public const string Pro_FromYear = "Pro_FromYear";
        public const string Pro_FromMonth = "Pro_FromMonth";
        public const string Pro_ToYear = "Pro_ToYear";
        public const string Pro_ToMonth = "Pro_ToMonth";
        public const string Pro_ProjectName = "Pro_ProjectName";
        public const string Pro_Summary = "Pro_Summary";
        public const string Edu_Count = "Edu_Count";
        public const string Edu_FromYear = "Edu_FromYear";
        public const string Edu_FromMonth = "Edu_FromMonth";
        public const string Edu_ToYear = "Edu_ToYear";
        public const string Edu_ToMonth = "Edu_ToMonth";
        public const string Edu_SchoolName = "Edu_SchoolName";
        public const string Edu_Education = "Edu_Education";
        public const string Edu_Profession = "Edu_Profession";
        public const string Edu_Summary = "Edu_Summary";

        public const string Tra_Count = "Tra_Count";
        public const string Tra_FromYear = "Tra_FromYear";
        public const string Tra_FromMonth = "Tra_FromMonth";
        public const string Tra_ToYear = "Tra_ToYear";
        public const string Tra_ToMonth = "Tra_ToMonth";
        public const string Tra_TrainerName = "Tra_TrainerName";
        public const string Tra_TrainerAddress = "Tra_TrainerAddress";
        public const string Tra_Lesson = "Tra_Lesson";
        public const string Tra_Centification = "Tra_Centification";
        public const string Tra_Summary = "Tra_Summary";

        public const string Lan_Count = "Lan_Count";
        public const string Lan_Language = "Lan_Language";
        public const string Lan_Level = "Lan_Level";

        public const string Ski_Count = "Ski_Count";
        public const string Ski_SkillName = "Ski_SkillName";
        public const string Ski_UsedTimes = "Ski_UsedTimes";
        public const string Ski_Ability = "Ski_Ability";

        public const string Cer_Count = "Cer_Count";
        public const string Cer_CertificationName = "Cer_CertificationName";
        public const string Cer_EffectiveDate = "Cer_EffectiveDate";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
                arraylist.AddRange(ExtendAttributes);
                return arraylist;
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
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(JobContentID.ToLower());
                    hiddenAttributes.Add(StyleID.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(IsView.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                }

                return hiddenAttributes;
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
                    basicAttributes.Add(RealName.ToLower());
                    basicAttributes.Add(Nationality.ToLower());
                    basicAttributes.Add(Gender.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(MobilePhone.ToLower());
                    basicAttributes.Add(HomePhone.ToLower());
                    basicAttributes.Add(LastSchoolName.ToLower());
                    basicAttributes.Add(Education.ToLower());
                    basicAttributes.Add(IDCardType.ToLower());
                    basicAttributes.Add(IDCardNo.ToLower());
                    basicAttributes.Add(Birthday.ToLower());
                    basicAttributes.Add(Marriage.ToLower());
                    basicAttributes.Add(WorkYear.ToLower());
                    basicAttributes.Add(Profession.ToLower());
                    basicAttributes.Add(ExpectSalary.ToLower());
                    basicAttributes.Add(AvailabelTime.ToLower());
                    basicAttributes.Add(Location.ToLower());
                    basicAttributes.Add(ImageUrl.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }

        private static ArrayList extendAttributes;
        public static ArrayList ExtendAttributes
        {
            get
            {
                if (extendAttributes == null)
                {
                    extendAttributes = new ArrayList();

                    extendAttributes.Add(Exp_Count.ToLower());
                    extendAttributes.Add(Exp_FromYear.ToLower());
                    extendAttributes.Add(Exp_FromMonth.ToLower());
                    extendAttributes.Add(Exp_ToYear.ToLower());
                    extendAttributes.Add(Exp_ToMonth.ToLower());
                    extendAttributes.Add(Exp_EmployerName.ToLower());
                    extendAttributes.Add(Exp_Department.ToLower());
                    extendAttributes.Add(Exp_EmployerPhone.ToLower());
                    extendAttributes.Add(Exp_WorkPlace.ToLower());
                    extendAttributes.Add(Exp_PositionTitle.ToLower());
                    extendAttributes.Add(Exp_Industry.ToLower());
                    extendAttributes.Add(Exp_Summary.ToLower());
                    extendAttributes.Add(Exp_Score.ToLower());

                    extendAttributes.Add(Pro_Count.ToLower());
                    extendAttributes.Add(Pro_FromYear.ToLower());
                    extendAttributes.Add(Pro_FromMonth.ToLower());
                    extendAttributes.Add(Pro_ToYear.ToLower());
                    extendAttributes.Add(Pro_ToMonth.ToLower());
                    extendAttributes.Add(Pro_ProjectName.ToLower());
                    extendAttributes.Add(Pro_Summary.ToLower());
                    extendAttributes.Add(Edu_Count.ToLower());
                    extendAttributes.Add(Edu_FromYear.ToLower());
                    extendAttributes.Add(Edu_FromMonth.ToLower());
                    extendAttributes.Add(Edu_ToYear.ToLower());
                    extendAttributes.Add(Edu_ToMonth.ToLower());
                    extendAttributes.Add(Edu_SchoolName.ToLower());
                    extendAttributes.Add(Edu_Education.ToLower());
                    extendAttributes.Add(Edu_Profession.ToLower());
                    extendAttributes.Add(Edu_Summary.ToLower());

                    extendAttributes.Add(Tra_Count.ToLower());
                    extendAttributes.Add(Tra_FromYear.ToLower());
                    extendAttributes.Add(Tra_FromMonth.ToLower());
                    extendAttributes.Add(Tra_ToYear.ToLower());
                    extendAttributes.Add(Tra_ToMonth.ToLower());
                    extendAttributes.Add(Tra_TrainerName.ToLower());
                    extendAttributes.Add(Tra_TrainerAddress.ToLower());
                    extendAttributes.Add(Tra_Lesson.ToLower());
                    extendAttributes.Add(Tra_Centification.ToLower());
                    extendAttributes.Add(Tra_Summary.ToLower());

                    extendAttributes.Add(Lan_Count.ToLower());
                    extendAttributes.Add(Lan_Language.ToLower());
                    extendAttributes.Add(Lan_Level.ToLower());

                    extendAttributes.Add(Ski_Count.ToLower());
                    extendAttributes.Add(Ski_SkillName.ToLower());
                    extendAttributes.Add(Ski_UsedTimes.ToLower());
                    extendAttributes.Add(Ski_Ability.ToLower());

                    extendAttributes.Add(Cer_Count.ToLower());
                    extendAttributes.Add(Cer_CertificationName.ToLower());
                    extendAttributes.Add(Cer_EffectiveDate.ToLower());
                }

                return extendAttributes;
            }
        }

        public static string GetAttributeName(string attributeName, int index)
        {
            return attributeName + "_" + index;
        }

        public static string GetAttributeValue(string attributeValue, int index)
        {
            StringCollection collection = TranslateUtils.StringCollectionToStringCollection(attributeValue, '&');
            if (index <= collection.Count)
            {
                return collection[index - 1];
            }
            return string.Empty;
        }
	}
}