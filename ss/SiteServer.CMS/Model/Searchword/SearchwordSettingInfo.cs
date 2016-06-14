using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model
{
    public class SearchwordSettingAttribute
    {
        protected SearchwordSettingAttribute()
        { }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string IsAllow = "IsAllow";
        public const string InNode = "InNode";
        public const string NotInNode = "NotInNode";
        public const string SearchResultCountLimit = "SearchResultCountLimit";
        public const string SearchCountLimit = "SearchCountLimit";
        public const string SearchOutputLimit = "SearchOutputLimit";
        public const string SearchSort = "SearchSort";

        #region 模板
        public const string IsTemplate = "IsTemplate";
        public const string StyleTemplate = "StyleTemplate";
        public const string ScriptTemplate = "ScriptTemplate";
        public const string ContentTemplate = "ContentTemplate";
        #endregion

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ID.ToLower());
                arrayList.Add(PublishmentSystemID.ToLower());
                arrayList.Add(IsAllow.ToLower());
                arrayList.Add(InNode.ToLower());
                arrayList.Add(NotInNode.ToLower());
                arrayList.Add(SearchResultCountLimit.ToLower());
                arrayList.Add(SearchCountLimit.ToLower());
                arrayList.Add(SearchOutputLimit.ToLower());
                arrayList.Add(SearchSort.ToLower());

                #region 模板
                arrayList.Add(IsTemplate.ToLower());
                arrayList.Add(StyleTemplate.ToLower());
                arrayList.Add(ScriptTemplate.ToLower());
                arrayList.Add(ContentTemplate.ToLower());
                #endregion
                return arrayList;
            }
        }
    }

    public class SearchwordSettingInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_SearchwordSetting";
        public SearchwordSettingInfo()
        { }

        public int ID
        {
            get { return base.GetInt(SearchwordSettingAttribute.ID, 0); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.ID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SearchwordSettingAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.PublishmentSystemID, value.ToString()); }
        }

        public bool IsAllow
        {
            get { return base.GetBool(SearchwordSettingAttribute.IsAllow, true); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.IsAllow, value.ToString()); }
        }

        public string InNode
        {
            get { return base.GetString(SearchwordSettingAttribute.InNode, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.InNode, value); }
        }

        public string NotInNode
        {
            get { return base.GetString(SearchwordSettingAttribute.NotInNode, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.NotInNode, value); }
        }

        public int SearchResultCountLimit
        {
            get { return base.GetInt(SearchwordSettingAttribute.SearchResultCountLimit, 0); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.SearchResultCountLimit, value.ToString()); }
        }

        public int SearchCountLimit
        {
            get { return base.GetInt(SearchwordSettingAttribute.SearchCountLimit, 0); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.SearchCountLimit, value.ToString()); }
        }

        public int SearchOutputLimit
        {
            get { return base.GetInt(SearchwordSettingAttribute.SearchOutputLimit, 0); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.SearchOutputLimit, value.ToString()); }
        }

        public string SearchSort
        {
            get { return base.GetString(SearchwordSettingAttribute.SearchSort, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.SearchSort, value); }
        }

        #region 模板
        public bool IsTemplate
        {
            get { return base.GetBool(SearchwordSettingAttribute.IsTemplate, false); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.IsTemplate, value.ToString()); }
        }
        public string StyleTemplate
        {
            get { return base.GetString(SearchwordSettingAttribute.StyleTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.StyleTemplate, value); }
        }
        public string ScriptTemplate
        {
            get { return base.GetString(SearchwordSettingAttribute.ScriptTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.ScriptTemplate, value); }
        }
        public string ContentTemplate
        {
            get { return base.GetString(SearchwordSettingAttribute.ContentTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SearchwordSettingAttribute.ContentTemplate, value); }
        }
        #endregion
    }
}
