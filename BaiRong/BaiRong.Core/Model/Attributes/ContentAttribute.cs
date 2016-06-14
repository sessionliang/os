using System;
using System.Collections;

namespace BaiRong.Model
{
	public class ContentAttribute
	{
        protected ContentAttribute()
		{
		}
		
		public const string ID = "ID";
        public const string NodeID = "NodeID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string AddUserName = "AddUserName";
        public const string LastEditUserName = "LastEditUserName";
        public const string LastEditDate = "LastEditDate";
        public const string Taxis = "Taxis";
        public const string ContentGroupNameCollection = "ContentGroupNameCollection";
        public const string Tags = "Tags";
        public const string SourceID = "SourceID";
        public const string ReferenceID = "ReferenceID";
        public const string IsChecked = "IsChecked";
        public const string CheckedLevel = "CheckedLevel";
        public const string Comments = "Comments";
        public const string Photos = "Photos";
        public const string Teleplays = "Teleplays";
        public const string Hits = "Hits";
        public const string HitsByDay = "HitsByDay";
        public const string HitsByWeek = "HitsByWeek";
        public const string HitsByMonth = "HitsByMonth";
        public const string LastHitsDate = "LastHitsDate";
        public static string SettingsXML = "SettingsXML";


        //具体类中实现
        public const string Title = "Title";
        public const string IsTop = "IsTop";
        public const string AddDate = "AddDate";

        //不存在
        public static string GetFormatStringAttributeName(string attributeName)
        {
            return attributeName + "FormatString";
        }
        public static string GetExtendAttributeName(string attributeName)
        {
            return attributeName + "_Extend";
        }
        public const string Check_IsAdmin = "Check_IsAdmin";              //审核者是否为管理员
        public const string Check_UserName = "Check_UserName";            //审核者
        public const string Check_CheckDate = "Check_CheckDate";          //审核时间
        public const string Check_Reasons = "Check_Reasons";              //审核原因
        public const string Check_IsTask = "Check_IsTask";                //定时审核
        public const string Check_TaskID = "Check_TaskID";                //定时审核任务ID
        public const string UnCheck_IsTask = "UnCheck_IsTask";                //定时下架
        public const string UnCheck_TaskID = "UnCheck_TaskID";                //定时下架任务ID

        public const string CheckTaskDate = "CheckTaskDate";                    //审核时间
        public const string UnCheckTaskDate = "UnCheckTaskDate";                //下架时间

        public const string TranslateContentType = "TranslateContentType";    //转移内容类型

        public const string MemberName = "MemberName";    //会员账号 by 20160121 sofuny 新的投稿管理功能添加  如果更新与内容相关的字体请同时更新投稿管理的草稿表bairong_MLibDraftContent，草稿表中的字段与内容表的字段一致

        #region by 20160308 sofuny 功能属性字段

        public const string Trial_BeginDate = "Trial_BeginDate";     // 试用开始时间
        public const string Trial_EndDate = "Trial_EndDate";    // 试用结束时间 

        public const string Survey_BeginDate = "Survey_BeginDate";      // 试用开始时间
        public const string Survey_EndDate = "Survey_EndDate";    // 试用结束时间 
        #endregion

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(AddUserName.ToLower());
                    hiddenAttributes.Add(LastEditUserName.ToLower());
                    hiddenAttributes.Add(LastEditDate.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(ContentGroupNameCollection.ToLower());
                    hiddenAttributes.Add(Tags.ToLower());
                    hiddenAttributes.Add(SourceID.ToLower());
                    hiddenAttributes.Add(ReferenceID.ToLower());
                    hiddenAttributes.Add(IsChecked.ToLower());
                    hiddenAttributes.Add(CheckedLevel.ToLower());
                    hiddenAttributes.Add(Comments.ToLower());
                    hiddenAttributes.Add(Photos.ToLower());
                    hiddenAttributes.Add(Teleplays.ToLower());
                    hiddenAttributes.Add(Hits.ToLower());
                    hiddenAttributes.Add(HitsByDay.ToLower());
                    hiddenAttributes.Add(HitsByWeek.ToLower());
                    hiddenAttributes.Add(HitsByMonth.ToLower());
                    hiddenAttributes.Add(LastHitsDate.ToLower());
                    hiddenAttributes.Add(SettingsXML.ToLower());
                    hiddenAttributes.Add(MemberName.ToLower());
                }

                return hiddenAttributes;
            }
        }

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(ContentAttribute.HiddenAttributes);
                arraylist.AddRange(SystemAttributes);
                return arraylist;
            }
        }

        private static ArrayList systemAttributes;
        public static ArrayList SystemAttributes
        {
            get
            {
                if (systemAttributes == null)
                {
                    systemAttributes = new ArrayList();
                    systemAttributes.Add(Title.ToLower());
                    systemAttributes.Add(IsTop.ToLower());
                    systemAttributes.Add(AddDate.ToLower());
                }

                return systemAttributes;
            }
        }

        private static ArrayList excludeAttributes;
        public static ArrayList ExcludeAttributes
        {
            get
            {
                if (excludeAttributes == null)
                {
                    excludeAttributes = new ArrayList();
                    excludeAttributes.Add(IsTop.ToLower());
                }

                return excludeAttributes;
            }
        }
	}
}
