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


        //��������ʵ��
        public const string Title = "Title";
        public const string IsTop = "IsTop";
        public const string AddDate = "AddDate";

        //������
        public static string GetFormatStringAttributeName(string attributeName)
        {
            return attributeName + "FormatString";
        }
        public static string GetExtendAttributeName(string attributeName)
        {
            return attributeName + "_Extend";
        }
        public const string Check_IsAdmin = "Check_IsAdmin";              //������Ƿ�Ϊ����Ա
        public const string Check_UserName = "Check_UserName";            //�����
        public const string Check_CheckDate = "Check_CheckDate";          //���ʱ��
        public const string Check_Reasons = "Check_Reasons";              //���ԭ��
        public const string Check_IsTask = "Check_IsTask";                //��ʱ���
        public const string Check_TaskID = "Check_TaskID";                //��ʱ�������ID
        public const string UnCheck_IsTask = "UnCheck_IsTask";                //��ʱ�¼�
        public const string UnCheck_TaskID = "UnCheck_TaskID";                //��ʱ�¼�����ID

        public const string CheckTaskDate = "CheckTaskDate";                    //���ʱ��
        public const string UnCheckTaskDate = "UnCheckTaskDate";                //�¼�ʱ��

        public const string TranslateContentType = "TranslateContentType";    //ת����������

        public const string MemberName = "MemberName";    //��Ա�˺� by 20160121 sofuny �µ�Ͷ����������  ���������������ص�������ͬʱ����Ͷ�����Ĳݸ��bairong_MLibDraftContent���ݸ���е��ֶ������ݱ���ֶ�һ��

        #region by 20160308 sofuny ���������ֶ�

        public const string Trial_BeginDate = "Trial_BeginDate";     // ���ÿ�ʼʱ��
        public const string Trial_EndDate = "Trial_EndDate";    // ���ý���ʱ�� 

        public const string Survey_BeginDate = "Survey_BeginDate";      // ���ÿ�ʼʱ��
        public const string Survey_EndDate = "Survey_EndDate";    // ���ý���ʱ�� 
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
