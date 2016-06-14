using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace BaiRong.Model
{
    public class UserDownloadAttribute
    {
        protected UserDownloadAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string CreateUserName = "CreateUserName";
        public const string Taxis = "Taxis";
        public const string Downloads = "Downloads";

        //basic
        public const string FileName = "FileName";
        public const string FileUrl = "FileUrl";
        public const string Summary = "Summary";
        public const string AddDate = "AddDate";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
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
                    hiddenAttributes.Add(CreateUserName.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
                    hiddenAttributes.Add(Downloads.ToLower());
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
                    basicAttributes.Add(FileName.ToLower());
                    basicAttributes.Add(FileUrl.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class UserDownloadInfo : ExtendedAttributes
    {
        public const string TableName = "bairong_UserDownload";

        public UserDownloadInfo()
        {
            this.ID = 0;
            this.CreateUserName = string.Empty;
            this.Taxis = 0;
            this.Downloads = 0;
            this.FileName = string.Empty;
            this.FileUrl = string.Empty;
            this.Summary = string.Empty;
            this.AddDate = DateTime.Now;
        }

        public UserDownloadInfo(object dataItem)
            : base(dataItem)
		{
		}

        public UserDownloadInfo(int id, string createUserName, int taxis, int downloads)
        {
            this.ID = id;
            this.CreateUserName = createUserName;
            this.Taxis = taxis;
            this.Downloads = downloads;
        }

        public int ID
        {
            get { return base.GetInt(UserDownloadAttribute.ID, 0); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.ID, value.ToString()); }
        }

        public string CreateUserName
        {
            get { return base.GetExtendedAttribute(UserDownloadAttribute.CreateUserName); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.CreateUserName, value); }
        }

        public int Taxis
        {
            get { return base.GetInt(UserDownloadAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.Taxis, value.ToString()); }
        }

        public int Downloads
        {
            get { return base.GetInt(UserDownloadAttribute.Downloads, 0); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.Downloads, value.ToString()); }
        }

        public string FileName
        {
            get { return base.GetExtendedAttribute(UserDownloadAttribute.FileName); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.FileName, value); }
        }

        public string FileUrl
        {
            get { return base.GetExtendedAttribute(UserDownloadAttribute.FileUrl); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.FileUrl, value); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(UserDownloadAttribute.Summary); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.Summary, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(UserDownloadAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(UserDownloadAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return UserDownloadAttribute.AllAttributes;
        }
    }
}
