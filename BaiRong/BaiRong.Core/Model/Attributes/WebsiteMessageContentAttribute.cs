using System;
using System.Collections;

namespace BaiRong.Model
{
    public class WebsiteMessageContentAttribute
    {
        protected WebsiteMessageContentAttribute()
        {
        }

        //hidden
        public static string ID = "ID";
        public static string WebsiteMessageID = "WebsiteMessageID";
        public static string Taxis = "Taxis";
        public static string IsChecked = "IsChecked";
        public static string UserName = "UserName";
        public static string IPAddress = "IPAddress";
        public static string Location = "Location";
        public static string AddDate = "AddDate";
        public static string Reply = "Reply";
        public static string ClassifyID = "ClassifyID";

        #region ÁôÑÔ×Ö¶Î
        public static string Name = "Name";
        public static string Phone = "Phone";
        public static string Email = "Email";
        public static string Question = "Question";
        public static string Description = "Description";
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
                    #region ÁôÑÔ×Ö¶Î
                    basicAttributes.Add(Name.ToLower());
                    basicAttributes.Add(Phone.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(Question.ToLower());
                    basicAttributes.Add(Description.ToLower());
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
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(WebsiteMessageID.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(IsChecked.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(Reply.ToLower());
                    hiddenAttributes.Add(ClassifyID.ToLower());
                    hiddenAttributes.Add(SettingsXML.ToLower());
                }

                return hiddenAttributes;
            }
        }
    }
}