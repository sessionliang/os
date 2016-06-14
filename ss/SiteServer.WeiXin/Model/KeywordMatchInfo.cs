using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace SiteServer.WeiXin.Model
{
    public class KeywordMatchAttribute
    {
        protected KeywordMatchAttribute()
        {
        }

        public const string MatchID = "MatchID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string Keyword = "Keyword";
        public const string KeywordID = "KeywordID";
        public const string IsDisabled = "IsDisabled";
        public const string KeywordType = "KeywordType";
        public const string MatchType = "MatchType";


        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(MatchID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(Keyword);
                    allAttributes.Add(IsDisabled);
                    allAttributes.Add(KeywordID);
                    allAttributes.Add(MatchType);
                    allAttributes.Add(KeywordType); ;
                }

                return allAttributes;
            }
        }
    }

    public class KeywordMatchInfo
    {
        private int matchID;
        private int publishmentSystemID;
        private string keyword;
        private int keywordID;
        private bool isDisabled;
        private EKeywordType keywordType;
        private EMatchType matchType;

        public KeywordMatchInfo()
        {
            this.matchID = 0;
            this.publishmentSystemID = 0;
            this.keyword = string.Empty;
            this.keywordID = 0;
            this.isDisabled = false;
            this.keywordType = EKeywordType.Text;
            this.matchType = EMatchType.Exact;
        }

        public KeywordMatchInfo(int matchID, int publishmentSystemID, string keyword, int keywordID, bool isDisabled, EKeywordType keywordType, EMatchType matchType)
        {
            this.matchID = matchID;
            this.publishmentSystemID = publishmentSystemID;
            this.keyword = keyword;
            this.keywordID = keywordID;
            this.isDisabled = isDisabled;
            this.keywordType = keywordType;
            this.matchType = matchType;
        }

        public KeywordMatchInfo(object dataItem)
        {
            if (dataItem != null)
            {
                foreach (string name in this.AllAttributes)
                {
                    object value = TranslateUtils.Eval(dataItem, name);
                    if (value != null)
                    {
                        this.SetValueInternal(name, value);
                    }
                }
            }
        }
        public KeywordMatchInfo(NameValueCollection form, bool isFilterSqlAndXss)
        {
            if (form != null)
            {
                foreach (string name in this.AllAttributes)
                {
                    string value = form[name];
                    if (value != null)
                    {
                        if (isFilterSqlAndXss)
                        {
                            value = PageUtils.FilterSqlAndXss(value);
                        }
                        this.SetValueInternal(name, value);
                    }
                }
            }
        }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection attributes = new NameValueCollection();
            foreach (string attributeName in this.AllAttributes)
            {
                object value = this.GetType().GetProperty(attributeName).GetValue(this, null);
                if (value != null)
                {
                    attributes.Add(attributeName, value.ToString());
                }
            }
            return attributes;
        }
        

        public object GetValue(string attributeName)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {   
                    object nameVlaue = this.GetType().GetProperty(name).GetValue(this, null);
      
                    if (attributeName == "KeywordType")
                    {
                        return EKeywordTypeUtils.GetEnumType(nameVlaue.ToString());
                    }
                    if (attributeName == "MatchType")
                    {
                        return EMatchTypeUtils.GetEnumType(nameVlaue.ToString());
                    }
                    return nameVlaue;

                }
            }
            return null;
        }

        public void SetValue(string attributeName, object value)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {
                    try
                    {
                        this.SetValueInternal(name, value);
                    }
                    catch { }

                    break;
                }
            }
        }

        private void SetValueInternal(string name, object value)
        {
            try
            {
                this.GetType().GetProperty(name).SetValue(this, value, null);
            }
            catch
            {
                if (StringUtils.ContainsIgnoreCase(name, "Is"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToBool(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "Date"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToDateTime(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "ID") || StringUtils.EndsWithIgnoreCase(name, "Num") || StringUtils.EndsWithIgnoreCase(name, "Count"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToInt(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "Amount"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToDecimal(value.ToString()), null);
                }
            }
        }

        protected List<string> AllAttributes
        {
            get
            {
                return KeywordMatchAttribute.AllAttributes;
            }
        }

        public int MatchID
        {
            get { return matchID; }
            set { matchID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
        }

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public EKeywordType KeywordType
        {
            get { return keywordType; }
            set { keywordType = value; }
        }

        public EMatchType MatchType
        {
            get { return matchType; }
            set { matchType = value; }
        }
    }
}
