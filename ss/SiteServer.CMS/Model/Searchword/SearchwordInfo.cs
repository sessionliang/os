using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model
{
    public class SearchwordAttribute
    {
        protected SearchwordAttribute()
        { }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string Searchword = "Searchword";
        public const string SearchResultCount = "SearchResultCount";
        public const string SearchCount = "SearchCount";
        public const string AddDate = "AddDate";
        public const string IsEnabled = "IsEnabled";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ID.ToLower());
                arrayList.Add(PublishmentSystemID.ToLower());
                arrayList.Add(Searchword.ToLower());
                arrayList.Add(SearchResultCount.ToLower());
                arrayList.Add(SearchCount.ToLower());
                arrayList.Add(AddDate.ToLower());
                arrayList.Add(IsEnabled.ToLower());
                return arrayList;
            }
        }
    }

    public class SearchwordInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_Searchword";
        public const string CACHE_UPDATE_TIME = "SearchwordResultUpdateTime";
        public SearchwordInfo()
        { }

        public int ID
        {
            get { return base.GetInt(SearchwordAttribute.ID, 0); }
            set { base.SetExtendedAttribute(SearchwordAttribute.ID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SearchwordAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SearchwordAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string Searchword
        {
            get { return base.GetString(SearchwordAttribute.Searchword, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordAttribute.Searchword, value); }
        }

        public int SearchResultCount
        {
            get { return base.GetInt(SearchwordAttribute.SearchResultCount, 0); }
            set { base.SetExtendedAttribute(SearchwordAttribute.SearchResultCount, value.ToString()); }
        }

        public int SearchCount
        {
            get { return base.GetInt(SearchwordAttribute.SearchCount, 0); }
            set { base.SetExtendedAttribute(SearchwordAttribute.SearchCount, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(SearchwordAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SearchwordAttribute.AddDate, value.ToString()); }
        }

        public bool IsEnabled
        {
            get { return base.GetBool(SearchwordAttribute.IsEnabled, true); }
            set { base.SetExtendedAttribute(SearchwordAttribute.IsEnabled, value.ToString()); }
        }
    }
}
